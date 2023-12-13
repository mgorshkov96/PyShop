using Billing.Models;

namespace Billing.Repositories
{
	public interface IUserRepository
	{
		List<User> Users { get; }
		User? GetUser(string name);
		int GetSumRating();
	}
}
