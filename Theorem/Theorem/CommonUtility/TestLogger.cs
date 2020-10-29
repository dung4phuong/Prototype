using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Globalization;
using LogManager = NLog.LogManager;

namespace  Theorem
{
    /// <summary>
    /// Class for test logger
    /// </summary>
    public class TestLogger
    {
        public static string ElasticSearchUrl = "http://automationpratice.com";
        public static Uri ElasticSearch = new Uri(ElasticSearchUrl);

        /// <summary>
        /// The logger
        /// </summary>
        private readonly NLog.Logger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The seri log object use to send logs to ElasticSearch
        /// </summary>
        //private Serilog.Core.Logger seriLog = GetLogger();
        private Serilog.Core.Logger _seriLog = null;

        /// <summary>
        /// The get seri logger used to send logs to ElasticSearch
        /// </summary>
        /// <returns>
        /// The <see cref="Logger"/>.
        /// </returns>
        public static Serilog.Core.Logger GetLogger()
        {
            var Log = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(ElasticSearch)
                {
                    MinimumLogEventLevel = LogEventLevel.Verbose,
                    AutoRegisterTemplate = true
                })
                .Enrich.WithProperty("RunId", "")
                .Enrich.WithProperty("Environment", BaseConfiguration.Config.AppSettings.Environment)
                .CreateLogger();

            return Log;
        }
        /// <summary>
        /// Add extra fields to every log posted in ElasticSearch
        /// </summary>
        /// <returns>
        /// The <see cref="ILogger"/>.
        /// </returns>
        private ILogger getEnrichedLog(string testName)
        {
            string browser = BaseConfiguration.Browser != BrowserType.Firefox
                ? BaseConfiguration.Config.AppSettings.Browser
                : BaseConfiguration.Config.AppSettings.CrossBrowserEnv;

            return this._seriLog.ForContext("TestName", testName)
                .ForContext("Browser", browser);
        }

        /// <summary>
        /// Logs the test starting.
        /// </summary>
        /// <param name="driverContext">The driver context.</param>
        public void LogTestStarting(CurrentTest currentTest)
        {
            this.log.Info("*************************************************************************************");
            this.Info("START: {0} starts at {1}.", currentTest.GetName(), currentTest.GetStartTime());
        }

        /// <summary>
        /// Logs the test ending.
        /// </summary>
        /// <param name="driverContext">The driver context.</param>
        public void LogTestEnding(CurrentTest currentTest)
        {
            this.Info($"END: { currentTest.GetName()} ends at {currentTest.GetEndTestTestTime()} after {currentTest.GetElapsedTime()} sec.");
            this.log.Info("*************************************************************************************");
        }

        /// <summary>
        /// Information the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public void Info(string message, params object[] args)
        {
            this.Info(CultureInfo.CurrentCulture, message, args);
        }

        public void Info(CultureInfo culture, string message, params object[] args)
        {
            this.log.Info(culture, message, args);
        }
        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public void Warn(string message, params object[] args)
        {
            this.log.Warn(CultureInfo.CurrentCulture, message, args);
        }


        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public void Error(string message, params object[] args)
        {
            this.Error(CultureInfo.CurrentCulture, message, args);
        }

        public void Error(CultureInfo culture, string message, params object[] args)
        {
            this.log.Error(culture, message, args);
        }

        /// <summary>
        /// Information the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public void Debug(string message, params object[] args)
        {
            this.log.Debug(CultureInfo.CurrentCulture, message, args);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="e">The e.</param>
        public void LogError(Exception e)
        {
            this.Error("Error occurred: {0}", e);
            throw e;
        }

        /// <summary>
        /// Log Trace
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public void Trace(string message, params object[] args)
        {
            this.log.Trace(CultureInfo.CurrentCulture, message, args);
        }
    }
}
