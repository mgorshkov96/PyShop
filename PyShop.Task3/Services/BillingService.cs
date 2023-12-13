using Grpc.Core;
using Billing;
using Billing.Models;
using Billing.Data;
using Google.Protobuf.WellKnownTypes;
using Google.Protobuf;
using System.Transactions;
using Billing.Repositories;

namespace Billing.Services
{
	public class BillingService : Billing.BillingBase
	{
		private IUserRepository _userRepository;
		private ICoinRepository _coinRepository;

		public BillingService(IUserRepository userRepository, ICoinRepository coinRepository)
		{
			_userRepository = userRepository;
			_coinRepository = coinRepository;
		}

		public override async Task ListUsers(None request, IServerStreamWriter<UserProfile> responseStream, ServerCallContext context)
		{
			foreach (var user in _userRepository.Users)
			{
				await responseStream.WriteAsync(new UserProfile { Name = user.Name, Amount = user.Amount });				
			}
		}

		public override Task<Response> CoinsEmission(EmissionAmount request, ServerCallContext context)
		{
			if(request.Amount < _userRepository.Users.Count)
			{
				return Task.FromResult(new Response
				{
					Status = Response.Types.Status.Failed,
					Comment = "Not enough coins"
				});
			}
			
			List<SumsOfEmissions> sumsOfEmissions = new List<SumsOfEmissions>();
			sumsOfEmissions.Capacity = _userRepository.Users.Count;

			int sumRating = _userRepository.GetSumRating();						

			long currentAmount = request.Amount - _userRepository.Users.Count;
			long amountCounter = 0;

			foreach (var user in _userRepository.Users)
			{
				double amount = ((double)user.Rating / sumRating) * currentAmount;
				long roundedAmount = (long)(amount);

				var sum = new SumsOfEmissions(user, roundedAmount + 1, amount - roundedAmount);
				sumsOfEmissions.Add(sum);
				amountCounter += roundedAmount;
			}			

			while (currentAmount - amountCounter > 0)
			{
				double remainder = 0;
				SumsOfEmissions sum = new SumsOfEmissions(null, 0, 0);

				foreach (var item in sumsOfEmissions)
				{
					if (item.RemainderOfRating > remainder && !sum.isAdd)
					{						
						remainder = item.RemainderOfRating;
						sum = item;
					}
				}

				sum.EmissionSum++;
				sum.isAdd = true;

				amountCounter++;
			}				

			foreach (var item in sumsOfEmissions)
			{
				_coinRepository.CreateCoins(item.EmissionSum, item.User);
				item.User.AddCoins(item.EmissionSum);				
			}

			return Task.FromResult(new Response
			{
				Status = Response.Types.Status.Ok,
				Comment = "Complete"
			});
		}

		public override Task<Response> MoveCoins(MoveCoinsTransaction request, ServerCallContext context)
		{
			var response = new Response();

			var srcUser = _userRepository.GetUser(request.SrcUser);

			if (srcUser == null)
			{
				return Task.FromResult(new Response
				{
					Status = Response.Types.Status.Failed,
					Comment = "src_user not found"
				});
			}

			if(srcUser.Amount < request.Amount)
			{
				return Task.FromResult(new Response
				{
					Status = Response.Types.Status.Failed,
					Comment = "the amount of src_user's money is less than the amount in the request"
				});
			}

			var dstUser = _userRepository.GetUser(request.DstUser);

			if (dstUser == null)
			{
				return Task.FromResult(new Response
				{
					Status = Response.Types.Status.Failed,
					Comment = "dst_user not found"
				});
			}

			_coinRepository.MoveCoins(request.Amount, srcUser, dstUser);
			srcUser.SubtractCoins(request.Amount);
			dstUser.AddCoins(request.Amount);

			return Task.FromResult(new Response
			{
				Status = Response.Types.Status.Ok,
				Comment = "Complete"
			});
		}

		public override Task<Coin> LongestHistoryCoin(None request, ServerCallContext context)
		{
			int historyAmount = -1;
			Models.Coin resultCoin = null;

			foreach(var coin in _coinRepository.Coins)
			{
				if(coin.HistoryAmount > historyAmount)
				{
					historyAmount = coin.HistoryAmount;
					resultCoin = coin;
				}
			}
			
			return Task.FromResult(new Coin
			{
				Id = resultCoin.Id,
				History = resultCoin.GetHistory()
			});
		}
	}
}