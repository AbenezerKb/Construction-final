namespace ERP.Helpers
{
    public class Utils
    {
        public static bool isValidDateRange(DateTime startDate, DateTime endDate)
        {
            return (startDate >= endDate || endDate <= startDate) ? false : true;

        }
    }

}