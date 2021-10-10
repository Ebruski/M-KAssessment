using Microsoft.Extensions.Configuration;

namespace M_KShared.Extensions
{
    public static class AppSettingsManager
    {
        public static IConfiguration Configuration { get; set; }

        public static string Fetch(string fetchingKey)
        {
            string stringValue = "";

            try
            {
                stringValue = Configuration[$"AppSettings:{fetchingKey}"] ?? "";
            }
            catch
            {
                // ignored
            }
            return stringValue;
        }

        public static string SmartSms(string fetchingKey)
        {
            string stringValue = "";

            try
            {
                stringValue = Configuration[$"SmartSmsAppSettings:{fetchingKey}"] ?? "";
            }
            catch
            {
                // ignored
            }
            return stringValue;
        }
    }
}
