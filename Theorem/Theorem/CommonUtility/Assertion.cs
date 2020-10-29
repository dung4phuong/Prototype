using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using System.IO;
using System.IO.Compression;
using System.Linq;
using  Theorem;
using System.Text.RegularExpressions;

namespace  Theorem
{
    class Assertion
        {
            //Properties
            public static string ElementsNotFound { get; set; }
        public static object WaiterHelper { get; private set; }

        public static bool IsJsAlert(IWebDriver driver, string alert)
            {
                try
                {
                    return driver.SwitchTo().Alert().Text.Contains(alert);

                }
                catch (Exception)
                {
                    return false;
                }
            }

            private static string GetTagName(IWebDriver driver, By by)
            {
                return driver.FindElement(by).TagName;
            }

            //Asserts
            public static bool IsElementEnabled(IWebDriver driver, By by)
            {
                try
                {
                    return driver.FindElement(by).Enabled;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public static bool IsElementDisplay(IWebDriver driver, By by)
            {
                //DriverContext.TurnOffImplicitWait(driver);
                try
                {
                    var element = driver.FindElement(by);
                    //DriverContext.TurnOnDefaultWaiter(driver);//turn on the implictitly wait
                    return element.Displayed;
                }
                catch (Exception)
                {
                    //DriverContext.TurnOnDefaultWaiter(driver);//turn on the implictitly wait
                    return false;
                }
            }
            public static bool IsElementDisplay(IWebDriver driver, ElementLocator el)
            {
                return IsElementDisplay(driver, el.ToBy());
            }

            public static bool IsElementDisplay(IWebDriver driver, IWebElement el)
            {
                DriverContext.TurnOffImplicitWait(driver);
                try
                {
                    DriverContext.TurnOnDefaultWaiter(driver);//turn on the implictitly wait
                    return el.Displayed;
                }
                catch (Exception)
                {
                    DriverContext.TurnOnDefaultWaiter(driver);//turn on the implictitly wait
                    return false;
                }
            }

            public static bool IsElementLoaded(IWebDriver driver, By by)
            {
                DriverContext.TurnOffImplicitWait(driver);
                try
                {
                    driver.FindElement(by);
                    DriverContext.TurnOnDefaultWaiter(driver);//turn on the implictitly wait
                    return true;
                }
                catch (Exception)
                {
                    DriverContext.TurnOnDefaultWaiter(driver);//turn on the implictitly wait
                    return false;
                }
            }

            public static bool IsElementLoaded(IWebDriver driver, ElementLocator locator)
            {
                return IsElementLoaded(driver, locator.ToBy());
            }


            public static bool IsElementLoaded(IWebDriver driver, By by, int index)
            {
                DriverContext.TurnOffImplicitWait(driver);
                try
                {
                    var a = driver.FindElements(by)[index];
                    DriverContext.TurnOnDefaultWaiter(driver);//turn on the implictitly wait
                    return true;
                }
                catch (Exception)
                {
                    DriverContext.TurnOnDefaultWaiter(driver);//turn on the implictitly wait
                    return false;
                }
            }


            public static bool IsAttributeValueContains(IWebDriver driver, By by, AttributeName attr, string value)
            {
                try
                {
                    return driver.FindElement(by).GetAttribute(attr.ToString().ToLower()).ToLower().Contains(value.ToLower());
                }
                catch (Exception)
                {
                    return false;
                }
            }

            
            public static bool IsAttributeValueContains(IWebDriver driver, IWebElement element, AttributeName attr, string value)
            {
                try
                {
                    return element.GetAttribute(attr.ToString().ToLower()).ToLower().Contains(value.ToLower());
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public static bool IsAttributeValueMatch(IWebDriver driver, By by, AttributeName attr, string value)
            {
                try
                {
                    return driver.FindElement(by).GetAttribute(attr.ToString().ToLower()).Equals(value);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public static bool IsElementMatchesRegEx(IWebDriver driver, By by, string text)
            {
                try
                {
                    Regex regex = new Regex(text, RegexOptions.IgnoreCase);
                    return driver.FindElements(@by).Any(x => regex.IsMatch(x.Text));
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public static bool IsElementContainsText(IWebDriver driver, By by, string text)
            {
                try
                {
                    return driver.FindElements(@by).Any(x => x.Text.Contains(text));
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public static bool IsElementContainsText(IWebDriver driver, ElementLocator el, string text)
            {
                return IsElementContainsText(driver, el.ToBy(), text);
            }

            public static bool IsElementContainsText(IWebDriver driver, IWebElement el, string text)
            {
                try
                {
                    return el.Text.Contains(text);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public static bool IsListOneMatchListTwo(IEnumerable<string> list1, IEnumerable<string> list2)
            {
                foreach (var item in list1)
                {
                    if (list2.Any(x => x.Contains(item))) continue;
                    ElementsNotFound = item;
                    return false;
                }
                return true;
            }

            public static bool IsMessageAppear(IWebDriver driver, string msg)
            {
                bool exist = IsElementContainsText(driver, By.TagName("html"), msg);
                
                return exist;
            }

            public static bool IsCurrentUrlEqual(IWebDriver driver, string url)
            {
                try
                {
                    var currenturl = driver.Url;
                    if (currenturl.Contains(url)) return true;
                }
                catch (Exception)
                {
                    return false;
                }
                return false;
            }
            public static bool IsCurrentUrlMatch(IWebDriver driver, string urlPater)
            {
                try
                {
                    return Regex.IsMatch(driver.Url, urlPater);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public static bool IsSpinner(IWebDriver driver)
            {
                By Spinner = By.CssSelector(".fa.fa-circle-o-notch.fa-spin.fa-4x.fa-fw.primary-color-text");
                bool isSpinner = IsElementDisplay(driver, Spinner);
                return isSpinner;
            }
        public enum AttributeName
        {
            Class,
            Value,
            Name,
            Id,
            Src,
            Alt,
            Type,
            Title,
            OnClick,
            Style,
            Disabled,
            Href,
            placeholder
        }
    }
    }


