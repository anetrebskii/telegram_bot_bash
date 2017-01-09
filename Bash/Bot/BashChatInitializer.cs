using System;
namespace Bash
{
	public class BashChatInitializer : IUserChatInitializer
	{
		public BashChatInitializer()
		{
		}

		public IState CreateInitialState(long userId)
		{
			return new BashState();
		}
	}
}
