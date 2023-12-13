using Billing;
using System.Text;

namespace Billing.Models
{
	public class Coin
	{
		public long Id { get; set; }		
		public User Owner { get; set; }		
		public Dictionary<DateTime, string> History { get; set; }
		public int HistoryAmount { get => History.Count; }

		public Coin(long id, User owner)
		{
			Id = id;
			Owner = owner;
			History = new Dictionary<DateTime, string>();
			History.Add(DateTime.Now, Owner.Name);
		}
		
		public string GetHistory()
		{
			var result = new string[HistoryAmount];
			int i = 0;

			foreach (var item in History)
			{
				result[i] = ($"{item.Key.ToString("dd.MM.yyyy HH:mm:ss")} - {item.Value}");	
				i++;
			}

			return string.Join( "; ", result );
		}
	}
}
