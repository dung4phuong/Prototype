
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;

namespace  Theorem
{
    public static class BaseConfiguration
    {
        public const int RetryCount = 2;
        public const int DefaultTimeOut = 40;
        public static Config Config { get; }

        static BaseConfiguration()
        {
            Config = PopulateData();
        }

        private static Config PopulateData()
        {
            var json = File.ReadAllText(Path.Combine(
                Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) ?? throw new InvalidOperationException(),
                "appsettings.json"));
            var config = JsonConvert.DeserializeObject<Config>(json);
            return config;

        }

        public static BrowserType Browser
        {
            get
            {
                BrowserType br;
                var isSupported = Enum.TryParse(Config.AppSettings.Browser, out br);
                return isSupported ? br : BrowserType.None;
            }
        }

        public static TimeSpan MediumTimeout =>
            TimeSpan.FromSeconds(Convert.ToInt32(Config.Timeouts.MediumTimeout, CultureInfo.CurrentCulture));

        public static TimeSpan LongTimeout =>
            TimeSpan.FromSeconds(Convert.ToInt32(Config.Timeouts.LongTimeout, CultureInfo.CurrentCulture));

        public static TimeSpan ShortTimeout =>
            TimeSpan.FromSeconds(Convert.ToInt32(Config.Timeouts.ShortTimeout, CultureInfo.CurrentCulture));


        public static TimeSpan ImplicitlyWaitMilliseconds =>
            TimeSpan.FromMilliseconds(Convert.ToDouble(Config.Timeouts.ImplicitWaitMilliseconds,
                CultureInfo.CurrentCulture));

        public static string FileToUploadPath =>
            Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles"), "PDF");

        public static string DownloadFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        public static string LocalScreenShotFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        public static string RemoteScreenShotFolder => Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.FullName, "Output");
        public static bool SeleniumScreenShotEnabled => true;
        public static bool DesktopScreenShotEnabled => false;
    }
    public class Config
    {
        [JsonProperty(PropertyName = "AppSettings")]
        public AppConfig AppSettings { get; set; }
        [JsonProperty(PropertyName = "Timeouts")]
        public Timeouts Timeouts { get; set; }
        [JsonProperty(PropertyName = "DriverCapabilities")]
        public DriverCaps DriverCaps { get; set; }
        [JsonProperty(PropertyName = "Url")]
        public Url Url { get; set; }
        [JsonProperty(PropertyName = "Passwords")]
        public Passwords Passwords { get; set; }
        [JsonProperty(PropertyName = "Emails")]
        public Emails Emails { get; set; }
        public const int RetryCount = 1;

    }
    public class DriverCaps
    {
        [JsonProperty(PropertyName = "EnableVNC")]
        public bool EnableVNC { get; set; }
        [JsonProperty(PropertyName = "EnableVideo")]
        public bool EnableVideo { get; set; }
        [JsonProperty(PropertyName = "UseCurrentDirectory")]
        public bool UseCurrentDirectory { get; set; }
        [JsonProperty(PropertyName = "DownloadFolder")]
        public string DownloadFolder { get; set; }
        [JsonProperty(PropertyName = "ScreenShotFolder")]
        public string ScreenShotFolder { get; set; }
        [JsonProperty(PropertyName = "PageSourceFolder")]
        public string PageSourceFolder { get; set; }
        [JsonProperty(PropertyName = "BuildId")]
        public string BuildId { get; set; }
        [JsonProperty(PropertyName = "EnableEventFiringWebDriver")]
        public bool EnableEventFiringWebDriver { get; set; }
    }
    public class Passwords
    {
        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }
    }
    public class Url
    {
        [JsonProperty(PropertyName = "PortalUrl")]
        public string PortalUrl { get; set; }
    }
    public class Emails
    {
        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }
    }
    public class Timeouts
    {
        [JsonProperty(PropertyName = "LongTimeout")]
        public string LongTimeout { get; set; }
        [JsonProperty(PropertyName = "MediumTimeout")]
        public string MediumTimeout { get; set; }
        [JsonProperty(PropertyName = "MiniTimeout")]
        public string MiniTimeout { get; set; }
        [JsonProperty(PropertyName = "ShortTimeout")]
        public string ShortTimeout { get; set; }
        [JsonProperty(PropertyName = "ImplicitWaitMilliseconds")]
        public string ImplicitWaitMilliseconds { get; set; }
    }
    public class AppConfig
    {
        [JsonProperty(PropertyName = "Environment")]
        public string Environment { get; set; }
        [JsonProperty(PropertyName = "Browser")]
        public string Browser { get; set; }
        [JsonProperty(PropertyName = "RunId")]
        public string RunId { get; set; }
        [JsonProperty(PropertyName = "CrossBrowserEnv")]
        public string CrossBrowserEnv { get; set; }
        [JsonProperty(PropertyName = "ClientId")]
        public string ClientId { get; set; }
    }
    public enum BrowserType
    {
        Firefox,
        InternetExplorer,
        Chrome,
        Edge,
        Safari,
        None
    }

}
