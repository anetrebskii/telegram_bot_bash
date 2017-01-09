using System;
namespace Bash
{
	public class UserInfo
	{
		public int MaxReadQuoteId
		{
			get;
			set;
		}

		public int MinReadQuoteId
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public UserInfo()
		{
			
		}

		public bool HasRead
		{
			get { return MaxReadQuoteId != 0; }
		}

		public UserInfo Clone()
		{
			return (UserInfo)MemberwiseClone();
		}
	}
}
