using System;
namespace Bash
{
	public class BashRegistry
	{
		public BashRegistry()
		{
			BashManager = new BashManager();
			BashManager.UpdateMaxQuoteId();

			UserThrottleSaver = new UserThrottleSaver();
		}

		private static BashRegistry _default = new BashRegistry();
		public static BashRegistry Default
		{
			get { return _default; }
		}

		public BashManager BashManager { get; private set; }
		public UserThrottleSaver UserThrottleSaver { get; private set; }

		public void Initialize()
		{
		}
	}
}
