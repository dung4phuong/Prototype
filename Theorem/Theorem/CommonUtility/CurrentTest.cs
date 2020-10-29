

using  Theorem;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace  Theorem
{


    public class CurrentTest
    {
        private string ID;
        private string FullName;
        private string Name;
        private string Description;
        private string Status;
        private string ErrorMessage;
        private string StackTrace;
        private int RetryNumber;
        private string Browser;
        private string Environment;
        public SessionId SessionId;
        private int RunCount = 0;
        private List<string> Steps = new List<string>();
        private TestData testData;
        private IWebDriver driver;

        private DateTime StartTime = DateTime.Now;
        private DateTime EndTime = DateTime.Now;
        private DriverStatus driverStatus { get; set; } = DriverStatus.None;

        private enum DriverStatus
        {
            Required,
            Initialized,
            None
        }

        public void SetDriverRequired()
        {
            this.driverStatus = DriverStatus.Required;
        }

        public void SetDriverNoneIfNotRequired()
        {
            if (this.driverStatus == DriverStatus.Required)
                this.driverStatus = DriverStatus.None;
        }
        public TestData TestData
        {
            get => this.testData ?? (this.testData = new TestData());

            set => this.testData = value;
        }

        public IWebDriver Driver
        {
            get
            {
                if (driverStatus == DriverStatus.Required)
                {
                    SetBrowser();
                    this.driver = new DriverContext(Name, FullName, Browser).InitDriver();
                    this.driverStatus = DriverStatus.Initialized;
                    this.SessionId = BaseConfiguration.Browser == BrowserType.Firefox
                        ? ((RemoteWebDriver)this.driver).SessionId
                        : null;
                }
                return this.driver;
            }

            set => this.driver = value;
        }

        public CurrentTest(string id, string fullName, string name, int retryNumber, string description)
        {
            ID = id;
            FullName = fullName;
            Name = name;
            RetryNumber = retryNumber;
            Description = description;
            SetEnvironment();
        }

        public void IncreaseRunCount() => RunCount++;

        public int GetRunCount() => RunCount;

        public int GetRetryNumber() => RetryNumber;

        public CurrentTest UpdateSTatus(string status)
        {
            Status = status;
            return this;
        }
        public CurrentTest UpdateErrorMessage(string errorMessage)
        {
            ErrorMessage = errorMessage;
            return this;
        }

        public CurrentTest UpdateStackTrace(string stackTRace)
        {
            StackTrace = stackTRace;
            return this;
        }

        public CurrentTest UpdateStartTime(DateTime time)
        {
            StartTime = time;
            return this;
        }

        public CurrentTest UpdateEndtime(DateTime time)
        {
            EndTime = time;
            return this;
        }

        public CurrentTest UpdateTestName(string testName)
        {
            Name = testName;
            return this;
        }


        public string GetName()
        {
            return Name;
        }

        public string GetFullName()
        {
            return FullName;
        }

        public string GetStatus()
        {
            return Status;
        }
        public CurrentTest SetBrowser()
        {
            if (BaseConfiguration.Browser != BrowserType.Firefox)
            {
                Browser = BaseConfiguration.Browser.ToString();
            }
            else
            {
                if (Regex.IsMatch(FullName, "\"(.+)\""))
                {
                    Browser = Regex.Match(FullName, "\"(.+?)\"").Groups[1].Value.Trim();
                }
                else
                {
                    var supportedBrowser = Enum.TryParse(BaseConfiguration.Config.AppSettings.CrossBrowserEnv,
                        out BrowserType browserType);
                    Browser = supportedBrowser ? browserType.ToString() : "Chrome";
                }
            }

            return this;
        }

        public CurrentTest SetEnvironment()
        {
            Environment = BaseConfiguration.Config.AppSettings.Environment;
            return this;
        }

        public string GetBrowser()
        {
            return Browser;
        }

        public SessionId GetSessionId()
        {
            return SessionId;
        }

        public string GetEnvironment()
        {
            return Environment;
        }

        public DateTime GetStartTime()
        {
            return StartTime;
        }

        public DateTime GetEndTestTestTime()
        {
            return EndTime;
        }

        public long GetElapsedTime()
        {
            return (long)(EndTime - StartTime).TotalMilliseconds;
        }

        public object GetDescription()
        {
            return Description;
        }
        public void AddExeption(Exception e)
        {
            UpdateErrorMessage(e.Message);
            UpdateStackTrace(e.StackTrace);
        }
        public string GetException()
        {
            return ErrorMessage;
        }

        public string GetStackTrace()
        {
            return StackTrace;
        }

        public void AddStep(string message)
        {
            Steps.Add(message);
        }
        public List<string> GetSteps()
        {
            return Steps;
        }


        public CurrentTest ClearSteps()
        {
            Steps.Clear();
            return this;
        }

        public CurrentTest SetDriverNull()
        {
            Driver = null;
            return this;
        }

       
    }
}

