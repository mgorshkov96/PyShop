using Billing.Data;
using Billing.Models;

namespace Billing.Repositories
{
	public class UserCodeRepository : IUserRepository
	{		
		private CodeStorage _codeStorage;

		public UserCodeRepository(CodeStorage codeStorage) 
		{
			_codeStorage = codeStorage;
		}

		public List<User> Users => _codeStorage.Users;

		public User? GetUser(string name)
		{
			var user = _codeStorage.Users.FirstOrDefault(x => x.Name == name);
			return user;
		}

		public int GetSumRating()
		{
			int result = 0;

			foreach (var user in Users)
			{
				result += user.Rating;
			}

			return result;
		}
	}
}
