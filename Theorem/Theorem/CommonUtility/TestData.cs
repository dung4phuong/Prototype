
using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace  Theorem
{
    public class TestData
    {
        
       
        


        public TestData()
        {
           
        }

        public string SearchDialogEnteredAddress { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Initials { get; set; }
        public string CurrentToastMessage { get; set; }
        public string Message { get; set; }
        public string NewStageName { get; set; }
        public string TestContextTestName { get; set; }
        public IList TestContextCategory { get; set; }
        public TestStatus TestContextOutcomeStatus { get; set; }
        public string Url { get; set; }
        public bool IsFirstRun { get; set; } = false;
        public string DateTimes { get; set; }
        public string primaryColor { get; set; }
        public string ClientCode { get; set; }
       
    }
}
