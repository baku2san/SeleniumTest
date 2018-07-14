using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SelenimuTest
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // InternetExplorer
            IWebDriver ie = new InternetExplorerDriver();
            SearchGoogle(ie);

            // 注意
            // http://sig9.hatenablog.com/entry/2015/01/13/010335
            // 「NoSuchElementException」が発生する場合があります。この場合は IE のインターネットオプションからセキュリティタブを選択し、全てのセキュリティゾーンに対して「保護モードを有効にする」へチェックしておきます。
        }

        private void SearchGoogle(IWebDriver driver)
        {
            driver.Url = "https://www.google.co.jp";
            var element = driver.FindElement(By.Name("q"));
            element.SendKeys("Cheese!");        // input text in the text
            element.Submit();
            return;

            // handle alerts
            driver.SwitchTo().Alert().Accept();

            driver.Navigate().GoToUrl("URL");
            driver.Navigate().Back();
            driver.Navigate().Forward();
            driver.Navigate().Refresh();
            driver.Close();
            driver.Quit();
            var currentUrl = driver.Url;
            var pageTitle = driver.Title;
            driver.Manage().Window.Maximize();

            driver.FindElement(By.ClassName("classname")); //classでの指定
            driver.FindElement(By.Id("id")); //idでの指定
            driver.FindElement(By.XPath("xpath")); //xpathでの指定

            var readText = element.Text;
            var attribute = element.GetAttribute("value");
            var isDisplayed = element.Displayed;
            var isEnalbed = element.Enabled;
            var isSelected = element.Selected;
            element.Click();

            // scroll to the element
            var actions = new Actions(driver);
            actions.MoveToElement(element);
            actions.Perform();

            // select dropdown 
            var selectElement = new SelectElement(element);
            selectElement.SelectByIndex(5); // indexで選択
            selectElement.SelectByValue("value"); // valueの値
            selectElement.SelectByText("text"); // 表示テキスト

            // Selenium API
            //            http://www.seleniumqref.com/api/webdriver_gyaku.html

        }
    }
}
