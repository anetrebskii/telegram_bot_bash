using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bash
{
	public class User
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

		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public long UserID
		{
			get;
			set;
		}

		[NotMapped]
		public bool HasRead
		{
			get { return MaxReadQuoteId != 0; }
		}

		public User Clone()
		{
			return (User)MemberwiseClone();
		}
	}
}
