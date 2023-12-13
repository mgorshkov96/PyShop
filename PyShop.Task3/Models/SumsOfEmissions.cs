namespace Billing.Models
{
    public class SumsOfEmissions
    {
        public User User { get; set; }
        public long EmissionSum { get; set; }
        public double RemainderOfRating { get; set; }
        public bool isAdd { get; set; } = false;

        public SumsOfEmissions(User user, long emissionSum, double remainderOfRating)
        {
            User = user;
            EmissionSum = emissionSum;
            RemainderOfRating = remainderOfRating;
        }
    }
}
