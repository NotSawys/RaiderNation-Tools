using System;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using Keys = OpenQA.Selenium.Keys;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualBasic;
using System.IO;
using System.Linq;
using RestSharp;

namespace Cabbo_Generator
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public static Random rand = new Random();
        public static Random giorno = new Random();
        public static Random mese = new Random();
        public static Random anno = new Random();
        bool Generating = false;
        public Form1()
        {
            InitializeComponent();
        }

        public static int GetRandomNumber(int min, int cap)
        {
            return rand.Next(min, cap);
        }

        public string[] GetRandomProxy()
        {
            try
            {
                TextBox textboxona = new TextBox();
                textboxona.Text = File.ReadAllLines("proxies.txt").ToString();
                return Strings.Split(textboxona.Lines[GetRandomNumber(0, textboxona.Lines.Length)], ":");
            }
            catch (Exception)
            {
            }
            return new string[] { };
        }

        void GetEmail()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            IWebDriver driver = new ChromeDriver(service);
            driver.Navigate().GoToUrl("https://temp-mail.org/en/");
            Thread.Sleep(7000);
            driver.FindElement(By.Id("click-to-copy")).Click();
            driver.Close();
        }

        void Gen()
        {
            while (!Generating)
            {
                try
                {
                    GetEmail();
                    ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                    service.HideCommandPromptWindow = true;
                    IWebDriver driver = new ChromeDriver(service);
                    driver.Navigate().GoToUrl("https://www.discord.com/register");
                    string username = "";
                    string password = "#CabboGenOnTop#";
                    string day = giorno.Next(1, 28).ToString();
                    string month = giorno.Next(1, 12).ToString();
                    string year = anno.Next(1990, 2000).ToString();
                    if (metroCheckBox2.Checked)
                    {
                        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                        var stringChars = new char[8];
                        var random = new Random();

                        for (int i = 0; i < 4; i++)
                        {
                            stringChars[i] = chars[random.Next(chars.Length)];
                        }

                        var finalString = new String(stringChars);
                        username = finalString + " | Cabbo Flushed";
                    }
                    else
                    {
                        username = metroTextBox2.Text;
                    }
                    driver.FindElement(By.Name("email")).SendKeys(Keys.Control + "v");
                    driver.FindElement(By.Name("username")).SendKeys(username);
                    driver.FindElement(By.Name("password")).SendKeys(password);
                    driver.FindElement(By.ClassName("css-gvi9bl-control")).Click();
                    Actions action = new Actions(driver);
                    action.SendKeys(day);
                    action.SendKeys(Keys.Enter);
                    action.SendKeys(month);
                    action.SendKeys(Keys.Enter);
                    action.SendKeys(year);
                    action.SendKeys(Keys.Enter);
                    action.Perform();
                    try
                    {
                        driver.FindElement(By.ClassName("input-3ITkQf")).Click();
                    }
                    catch (Exception)
                    {
                    }
                    string tokenhere = "";
                    string content = "{\"token\":" + tokenhere + ",\"captcha_key\":" + metroTextBox4.Text + "}";
                    driver.FindElement(By.ClassName("button-3k0cO7")).Click();
                    /*var client = new RestClient($"http://2captcha.com/in.php?key={metroTextBox4.Text}&method=hcaptcha&sitekey=10000000-ffff-ffff-ffff-000000000001&pageurl=https://discord.com/register");
                    var request = new RestRequest(Method.POST);
                    IRestResponse response = client.Execute(request);*/
                    var client = new RestClient("https://discord.com/api/v9/auth/verify");
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", content);
                    IRestResponse response = client.Execute(request);
                    if (File.Exists(Environment.CurrentDirectory + "/accounts.txt"))
                    {
                        File.WriteAllText(Environment.CurrentDirectory + "/accounts.txt", Clipboard.GetText() + ":" + username + ":" + password + Environment.NewLine);
                    }
                    else
                    {
                        File.Create(Environment.CurrentDirectory + "/accounts.txt");
                        File.WriteAllText(Environment.CurrentDirectory + "/accounts.txt", Clipboard.GetText() + ":" + username + ":" + password + Environment.NewLine);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if(metroButton1.Text == "Start")
            {
                try
                {
                    Thread thread = new Thread(new ThreadStart(Gen));
                    thread.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                metroButton1.Text = "Stop";
            }
            else if (metroButton1.Text == "Stop")
            {
                Generating = false;
                metroButton1.Text = "Start";
            }
        }
    }
}
