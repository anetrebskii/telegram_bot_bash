using System;
namespace Bash
{
	public class NewStateEventArgs : EventArgs
	{
		public IState NewState
		{
			get;
			private set;
		}

		public NewStateEventArgs(IState newState)
		{
			NewState = newState;
		}
	}
}
