using OpenQA.Selenium;
using OpenQA.Selenium.IE;
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

        private void SearchGoogle(IWebDriver ie)
        {
            ie.Url = "https://www.google.co.jp";
            IWebElement element = ie.FindElement(By.Name("q"));
            element.SendKeys("Cheese!");
            element.Submit();
        }
    }
}
