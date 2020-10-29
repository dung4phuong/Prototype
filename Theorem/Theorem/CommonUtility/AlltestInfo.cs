using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace  Theorem
{
    public class AllTestInfo
    {
        public static string CurrentTestID
        {
            get => testIdAsync.Value;
            set => testIdAsync.Value = value;
        }

        private static AsyncLocal<string> testIdAsync = new AsyncLocal<string>();

        private AllTestInfo()
        {
            _allTest = new ConcurrentDictionary<string, CurrentTest>();
        }

        public static AllTestInfo Instance => Nested.instance;

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly AllTestInfo instance
                = new AllTestInfo();
        }

        private readonly ConcurrentDictionary<string, CurrentTest> _allTest;


        public bool isCurrentTestInDic(string ID)
        {
            return _allTest.ContainsKey(ID);
        }


        public CurrentTest GetCurrentTest(string testId)
        {
            return _allTest[testId];
        }

        public CurrentTest AddTest(string id, string fullName, string name, int retryNumber, string description)
        {
            return _allTest[id] = new CurrentTest(id, fullName, name, retryNumber, description);
        }

    }

}
