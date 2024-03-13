namespace NewTiceAI.Helpers
{
    public static class PGDataHelper
    {
        public static DateTime FormatDate(DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }
    }
}
