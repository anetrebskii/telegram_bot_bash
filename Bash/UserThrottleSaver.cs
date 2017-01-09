using System;
using System.Collections.Generic;
using System.Threading;

namespace Bash
{
	public class UserThrottleSaver
	{
		private readonly TimeSpan SAVE_INTERVAL = TimeSpan.FromSeconds(10);

		private object _syncUsers = new object();
		private Dictionary<int, UserInfo> usersToSave = new Dictionary<int, UserInfo>();
		private Timer _timerForSavingUsers;

		public UserThrottleSaver()
		{
			_timerForSavingUsers = new Timer(save, null, SAVE_INTERVAL, SAVE_INTERVAL);
		}

		public void AddToSaving(UserInfo user)
		{
			lock (_syncUsers)
			{
				usersToSave[user.Id] = user;	
			}
		}

		private void save(object state)
		{
			_timerForSavingUsers.Change(Timeout.Infinite, Timeout.Infinite);
			Dictionary<int, UserInfo> users;
			lock (_syncUsers)
			{
				users = usersToSave;
				usersToSave = new Dictionary<int, UserInfo>();
			}

			//TODO: Save to DB
			_timerForSavingUsers.Change(SAVE_INTERVAL, SAVE_INTERVAL);
		}
	}

}
