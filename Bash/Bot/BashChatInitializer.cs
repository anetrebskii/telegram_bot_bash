using System;
using System.Linq;

namespace Bash
{
	public class BashChatInitializer : IUserChatInitializer
	{
		public BashChatInitializer()
		{
		}

		public IState CreateInitialState(long userId)
		{
			using (var context = new BashDbContext())
			{
				var user = context.Users.Find(userId);
				if (user == null)
				{
					user = context.Users.Create();
					user.UserID = userId;
					context.Users.Add(user);
					context.SaveChanges();
				}
			}
			return new BashState();
		}
	}
}
