namespace DayMemory.Core
{
    public static class Constants
    {
        public const int IdMaxLength = 36;

        public const string AdminRole = "Administrator";

        public const string AdminPolicy = "AdminOnly";

        public static class RequestLimits
        {
            public const long MaxFileSize = 100 * 1024 * 1024;
        }
    }
}