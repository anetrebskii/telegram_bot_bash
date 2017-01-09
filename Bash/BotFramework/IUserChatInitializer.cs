using System;
namespace Bash
{
	public interface IUserChatInitializer
	{
		IState CreateInitialState(long userId);
	}
}
