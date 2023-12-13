using Billing.Models;

namespace Billing.Data
{
	public class CodeStorage
	{
		public List<User> Users { get; set; }
		public List<Models.Coin> Coins { get; set; }
		public long CoinCounter { get; set; } = 1;

		public CodeStorage() 
		{
			List<User> users = new List<User>()
			{
				new User("boris", 5000),
				new User("maria", 1000),
				new User("oleg", 800)
			};

			Users = users;
			Coins = new List<Models.Coin>();
		}
	}
}
