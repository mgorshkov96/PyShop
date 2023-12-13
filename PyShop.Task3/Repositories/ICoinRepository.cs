using Billing.Models;

namespace Billing.Repositories
{
	public interface ICoinRepository
	{
		List<Models.Coin> Coins { get; }
		void MoveCoins(long amount, User srcUser, User dstUser);
		void CreateCoins(long amount, User owner);
	}
}
