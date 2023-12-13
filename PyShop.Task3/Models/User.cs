using System.Reflection.Metadata.Ecma335;

namespace Billing.Models
{
	public class User
	{		
		public string Name { get; set; }
		public int Rating { get; set; }		
		public long Amount { get => amount; }

		private long amount;

		public User(string name, int rating)
		{
			Name = name;
			Rating = rating;
		}

		public void AddCoins(long addAmount)
		{
			amount += addAmount;
		}

		public void SubtractCoins(long subtractAmount)
		{
			amount -= subtractAmount;
		}
	}
}
