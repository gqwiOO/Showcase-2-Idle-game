namespace Mechanics.DayNight
{
    public class DayIncomeResult : IDayResultData
    {
        public float StartBalance { get; set; }
        public float EndBalance { get; set; }
        public float Earned { get; set; }
        public float Spent { get; set; }

        public float BalanceDelta => EndBalance - StartBalance;
    }
}
