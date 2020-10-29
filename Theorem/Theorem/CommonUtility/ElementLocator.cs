
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using ServiceStack;

namespace  Theorem
{
	public class ElementLocator
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ElementLocator"/> class.
		/// </summary>
		/// <example>How we define locator: <code>
		/// private readonly ElementLocator searchTextbox = new ElementLocator(Locator.Id, "SearchTextBoxId");
		/// </code> </example>
		/// <param name="kind">The locator type.</param>
		/// <param name="value">The locator value.</param>
		public ElementLocator(Locator kind, string value)
		{
			this.Kind = kind;
			this.Value = value;
		}

		/// <summary>
		/// Gets or sets the kind of element locator.
		/// </summary>
		/// <value>
		/// Kind of element locator (Id, Xpath, ...).
		/// </value>
		public Locator Kind { get; set; }

		/// <summary>
		/// Gets or sets the element locator value.
		/// </summary>
		/// <value>
		/// The the element locator value.
		/// </value>
		public string Value { get; set; }

		/// <summary>
		/// Formats the generic element locator definition and create the instance
		/// </summary>
		/// <param name="parameters">The parameters.</param>
		/// <returns>
		/// New element locator with value changed by injected parameters
		/// </returns>
		/// <example>How we can replace parts of defined locator: <code>
		/// private readonly ElementLocator menuLink = new ElementLocator(Locator.XPath, "//*[@title='{0}' and @ms.title='{1}']");
		/// var element = this.Driver.GetElement(this.menuLink.Format("info","news"));
		/// </code></example>
		public ElementLocator Format(params object[] parameters)
		{
			return new ElementLocator(this.Kind, string.Format(CultureInfo.CurrentCulture, this.Value, parameters));
		}

		/*
         * XPath only functions
         *
         * Set of functionality that work only for Locator Type XPath. Any Element sent with a different kind will be
         * converted to xpath. Still it's recommended to use only with XPath type.
         *
         */

		/// <summary>
		/// Initializes a new instance of the <see cref="ElementLocator"/> class and gives Xpath type by default.
		/// </summary>
		/// <example>How we define locator: <code>
		/// private readonly ElementLocator searchTextbox = new ElementLocator( "//input[@id='SearchTextBoxId']");
		/// </code> </example>       
		/// <param name="value">The locator value.</param>
		public ElementLocator(string value)
		{
			this.Kind = Locator.XPath;
			this.Value = value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ElementLocator"/> class and gives Xpath type by default.
		/// </summary>
		/// <example>How we define locator: <code>
		/// private readonly ElementLocator searchTextbox = new ElementLocator( "//input[@id='SearchTextBoxId']");
		/// </code> </example> 
		/// <param name="value">The locator value.</param>
		public static ElementLocator New(string value)
		{
			return new ElementLocator(Locator.XPath, value);
		}
		/// <summary>
		/// Initializes a new instance of ElementLocator class using Selenium By
		/// </summary>
		/// <example>How we define locator: <code>
		/// private readonly ElementLocator searchTextbox = new ElementLocator( "//input[@id='SearchTextBoxId']");
		/// </code> </example> 
		/// <param name="value">The locator value.</param>
		public static ElementLocator New(By by)
		{
			Locator locatorType;
			Enum.TryParse(Regex.Match(by.ToString(), @"(?<=By\.).*?(?=:)").Value.Trim(), out locatorType);
			return new ElementLocator(locatorType, Regex.Match(by.ToString(), @"(?<=:).*$").Value.Trim());
		}
		/// <summary>
		/// Return true if the ElementLocator is of type XPath.
		/// </summary>        
		/// <returns>
		/// The <see cref="bool"/>.
		/// </returns>
		private bool IsXpathType()
		{
			return this.Kind == Locator.XPath ? true : false;
		}

		/// <summary>
		/// The verify type is xpath, else tries to convert the given element descrition to XPath
		/// </summary>       
		/// <returns>
		/// The <see cref="string"/>.
		/// </returns>
		private string VerifyTypeIsXpath()
		{
			return IsXpathType() ? this.Value : ConvertToXpath();
		}

		/// <summary>
		/// The verify type is xpath, else tries to convert the given element descrition to XPath
		/// </summary>
		/// <param name="elementLocator">
		/// The element locator.
		/// </param>
		/// <returns>
		/// The <see cref="string"/>.
		/// </returns>
		private string VerifyTypeIsXpath(ElementLocator elementLocator)
		{
			return IsXpathType() ? elementLocator.Value : ConvertToXpath();
		}

		/// <summary>
		/// Converts given ElementLocator value to XPath
		/// </summary>    
		/// <returns>
		/// The <see cref="string"/>.
		/// </returns>
		private string ConvertToXpath()
		{
			switch (this.Kind)
			{
				case Locator.Id:
					return $"//*[@id='{this.Value}']";
				case Locator.ClassName:
					return $"//*[@class='{this.Value}']";
				case Locator.CssSelector:
					return $"//*[contains(@class,'{this.Value.ReplaceAll(".", " ").Trim()}')]";
				case Locator.LinkText:
					return $"//a[text()='{this.Value}']";
				case Locator.Name:
					return $"//*[@name='{this.Value}']";
				case Locator.PartialLinkText:
					return $"//a[contains(normalize-space(.),'{this.Value}']";
				case Locator.TagName:
					return $"//{this.Value}";
				case Locator.XPath:
					return this.Value;
			}

			throw new Exception($"Unsupported Locator Type: {this.Kind}");
		}

		/// <summary>
		/// The replace given values in the string describing the XPath: <code>
		/// ElementLocator element= ElementLocator.New("//input[@id='{0}'");
		/// element.ReplaceValue("SearchTextBoxId");
		/// </code>
		/// </summary>
		/// <param name="elementLocator">
		/// The element locator.
		/// </param>
		/// <param name="replaceValues">
		/// The replace values.
		/// </param>
		/// <returns>
		/// The <see cref="ElementLocator"/>.
		/// </returns>
		public ElementLocator ReplaceValue(params object[] replaceValues)
		{
			string value = VerifyTypeIsXpath();
			return new ElementLocator(string.Format(value, replaceValues));
		}


		/// <summary>
		/// Formats the XPath value to return the selected index(starting with 1,2,...) value <code>
		/// ElementLocator element= ElementLocator.New("//input");
		/// element.Index(1)
		/// </code>
		/// </summary>
		/// <param name="elementLocator">
		/// The element locator.
		/// </param>
		/// <param name="index">
		/// The index.
		/// </param>
		/// <returns>
		/// The <see cref="ElementLocator"/>.
		/// </returns>
		public ElementLocator Index(int index)
		{
			string value = VerifyTypeIsXpath();
			return new ElementLocator($"({value})[{index}]");
		}

		/// <summary>
		/// Appends the XPath value of given ElementLocator to the current ElementLocator  <code>
		/// ElementLocator baseElement= ElementLocator.New("//table[@id='anId']");
		/// ElementLocator newElement= ElementLocator.New("//tr/td");
		/// baseElement.Append(newElement)
		/// </code>
		/// </summary>
		/// <param name="elementLocator">
		/// The element locator.
		/// </param>
		/// <param name="index">
		/// The index.
		/// </param>
		/// <returns>
		/// The <see cref="ElementLocator"/>.
		/// </returns>
		public ElementLocator Append(ElementLocator elementLocatorToAppend)
		{
			string value = VerifyTypeIsXpath();
			string valueToAppend = VerifyTypeIsXpath(elementLocatorToAppend);
			return new ElementLocator(value.EndsWith("/") || valueToAppend.StartsWith("[") || valueToAppend.StartsWith("/")
				? $"{value}{valueToAppend}"
				: $"{value}/{valueToAppend}");
		}

		/// <summary>
		/// Appends the XPath value of given ElementLocator to the current ElementLocator  <code>
		/// ElementLocator baseElement= ElementLocator.New("//table[@id='anId']");        
		/// baseElement.Append("//tr/td")
		/// </summary>
		/// <param name="elementLocator">
		/// The element locator.
		/// </param>
		/// <param name="valueToAppend">
		/// The value to append.
		/// </param>
		/// <returns>
		/// The <see cref="ElementLocator"/>.
		/// </returns>
		public ElementLocator Append(string valueToAppend)
		{
			string value = VerifyTypeIsXpath();
			return new ElementLocator($"{value}{valueToAppend}");
		}

		/// <summary>
		/// Override toString value to show the type and value in the Element Locator object
		/// </summary>
		/// <returns>
		/// The <see cref="string"/>.
		/// </returns>
		public override string ToString()
		{
			return $"Type: {this.Kind}  with value: {this.Value}";
		}
	}
}
