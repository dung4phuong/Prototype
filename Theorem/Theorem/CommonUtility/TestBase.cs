
using System;
using  Theorem;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;


namespace  Theorem
{
    public class StepBase
    {
        protected static AllTestInfo _allTest = AllTestInfo.Instance;


        private static CurrentTest GetCurrentTest()
        {
            return _allTest.GetCurrentTest(GetCurrentTestId());
        }

        private static string GetCurrentTestId()
        {
            return AllTestInfo.CurrentTestID;
        }

        public static TestData GetTestData()
        {
            return GetCurrentTest().TestData;
        }

        protected static IWebDriver Driver => GetTest().Driver;
        protected static TestData TestData => GetTest().TestData;

        protected static SessionId SessionId => GetTest().SessionId;

        private static CurrentTest GetTest()
        {
            return _allTest.GetCurrentTest(AllTestInfo.CurrentTestID);
        }

        public static void Step(string message)
        {
            GetTest().AddStep(message);
            Console.WriteLine(message);
        }
    }
}