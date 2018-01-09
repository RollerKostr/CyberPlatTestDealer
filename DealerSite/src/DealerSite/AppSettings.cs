using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using CyberPlatGate.Contracts.Configurations;

namespace DealerSite
{
    public static class AppSettings
    {
        public static string SD => config<string>("SD");
        public static string AP => config<string>("AP");
        public static string OP => config<string>("OP");
        public static PayTool PAY_TOOL => (PayTool) config<int>("PAY_TOOL");
        public static string TERM_ID => config<string>("TERM_ID", true);
        public static bool NO_ROUTE => config<bool>("NO_ROUTE");

        public static string CheckUrl => config<string>("CheckUrl");
        public static string PayUrl => config<string>("PayUrl");
        public static string StatusUrl => config<string>("StatusUrl");
        public static int TimeoutSec => config<int>("TimeoutSec");

        public static string SecretKeyPath => ensureFullFilePath(config<string>("SecretKeyPath"));
        public static string PublicKeyPath => ensureFullFilePath(config<string>("PublicKeyPath"));
        public static string SecretKeyPassword => config<string>("SecretKeyPassword");
        public static string PublicKeySerial => config<string>("PublicKeySerial");


        private static string ensureFullFilePath(string relativePath)
        {
            return !Path.IsPathRooted(relativePath)
                ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath)
                : relativePath;
        }

        private static T config<T>(string name, bool allowEmpty = false)
        {
            var value = ConfigurationManager.AppSettings[name];

            if (!allowEmpty && string.IsNullOrEmpty(value))
                throw new Exception($"Could not find setting '{name}'.");

            return (T) Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}