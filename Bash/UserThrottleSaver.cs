using System;
using System.Collections.Generic;
using System.Threading;

namespace Bash
{
	public class UserThrottleSaver
	{
		private readonly TimeSpan SAVE_INTERVAL = TimeSpan.FromSeconds(10);

		private object _syncUsers = new object();
		private Dictionary<long, User> usersToSave = new Dictionary<long, User>();
		private Timer _timerForSavingUsers;

		public UserThrottleSaver()
		{
			_timerForSavingUsers = new Timer(save, null, SAVE_INTERVAL, SAVE_INTERVAL);
		}

		public void AddToSaving(User user)
		{
			lock (_syncUsers)
			{
				usersToSave[user.UserID] = user;	
			}
		}

		private void save(object state)
		{
			_timerForSavingUsers.Change(Timeout.Infinite, Timeout.Infinite);
			Dictionary<long, User> users;
			lock (_syncUsers)
			{
				users = usersToSave;
				usersToSave = new Dictionary<long, User>();
			}

			using (var context = new BashDbContext())
			{
				foreach (var user in users.Values)
				{
					context.Users.Attach(user);
					context.Entry(user).Property(u => u.MinReadQuoteId).IsModified = true;
					context.Entry(user).Property(u => u.MaxReadQuoteId).IsModified = true;
				}
				context.SaveChanges();
			}
			_timerForSavingUsers.Change(SAVE_INTERVAL, SAVE_INTERVAL);
		}
	}

}
