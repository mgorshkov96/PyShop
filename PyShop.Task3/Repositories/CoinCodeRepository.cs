using Billing.Data;
using Billing.Models;

namespace Billing.Repositories
{
	public class CoinCodeRepository : ICoinRepository
	{
		private CodeStorage _codeStorage;

		public CoinCodeRepository(CodeStorage codeStorage)
		{
			_codeStorage = codeStorage;
		}

		public List<Models.Coin> Coins { get => _codeStorage.Coins; set => _codeStorage.Coins = value; }
		public long CoinNextId { get => _codeStorage.CoinCounter; set => _codeStorage.CoinCounter = value; }

		public void Add(long id, User owner)
		{
			Models.Coin coin = new Models.Coin(id, owner);
			Coins.Add(coin);
		}

		public void MoveCoins(long amount, User srcUser, User dstUser)
		{
			long moveCounter = 0;

			foreach (var coin in Coins)
			{
				if (coin.Owner.Equals(srcUser))
				{					
					coin.History.Add(DateTime.Now, dstUser.Name);					
					coin.Owner = dstUser;
					moveCounter++;
				}

				if(moveCounter == amount)
				{
					break;
				}
			}			
		}

		public void CreateCoins(long amount, User owner)
		{
			for(long i = 0; i < amount; i++)
			{
				var coin = new Models.Coin(CoinNextId, owner);
				Coins.Add(coin);
				CoinNextId++;
			}
		}
	}
}
