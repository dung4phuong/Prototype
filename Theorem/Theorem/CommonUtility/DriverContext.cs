using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using System;
using System.Globalization;
using System.IO;

namespace  Theorem
{
	public class DriverContext
	{


		public DriverContext(string name, string fullName, string browser)
		{
			this.TestName = name;
			this.TestFullName = fullName;
			this.Browser = browser;
		}

		public string Browser { get; set; }

		private string TestName { get; }
		private string TestFullName { get; }

		private IWebDriver Driver { get; set; }

		public string CurrentDirectory { get; set; }


		public IWebDriver InitDriver()
		{
			CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

			int maximumTry = 5;
			int i = 0;
			var isDriverCreated = false;
			while (i < maximumTry && !isDriverCreated)
			{
				try
				{
					switch (BaseConfiguration.Browser)
					{
						case BrowserType.Chrome:
							Driver = new ChromeDriver();
							isDriverCreated = true;
							break;
						case BrowserType.InternetExplorer:
							Driver = new InternetExplorerDriver();
							isDriverCreated = true;
							break;
						case BrowserType.Firefox:
							Driver = new FirefoxDriver();
							isDriverCreated = true;
							break;
						default:
							throw new NotSupportedException(
								String.Format(CultureInfo.CurrentCulture, $"Driver {BaseConfiguration.Browser} is not supported"));
					}

				}
				catch (Exception e)
				{
					if (i++ >= maximumTry) throw e;
				}
			}
			
			TurnOnDefaultWaiter(Driver); // Turn on waiters
			return Driver;
		}

		public static void TurnOnDefaultWaiter(IWebDriver driver)
		{
			driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60); //BaseConfiguration.LongTimeout;
			driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(5); //BaseConfiguration.ShortTimeout;
			driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(100); //BaseConfiguration.ImplicitlyWaitMilliseconds;
		}

		public static void TurnOffImplicitWait(IWebDriver driver)
		{
			driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
		}
	}
}
