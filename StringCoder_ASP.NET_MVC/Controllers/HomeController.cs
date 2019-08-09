using StringCoder_ASP.NET_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StringCoder_ASP.NET_MVC.Controllers
{
    public class HomeController : Controller
    {
        readonly char[] alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        readonly char[] alphabetUpper = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        readonly string[] charsToRemove = new string[] { ",", ".", ";", ":", "@", "!", "#", "$", "%", "*", "(", ")", "[", "]", "^", "?", "|", "+", "-", "&", @"\", "/", "'", "\"" };
        private static int key = 0;
        int Xkey, Ytext;
        string current = null;
        string prev = null;

        private static List<string> ciphers = new List<string>() { "Caesar cipher", "Vigenere cipher", "HashFunc" };

        [HttpPost]
        public ActionResult EncodeText(FormData formData, FormCollection form)
        {
            // Caesar cipher encode
            if (Request.Form["Ciphers"] == "Caesar cipher" && formData.tbkey != "")
            {
                Request.Form["Your decoded text:"] = "  Your decoded text:";
                Request.Form["Decode"] = "Decode";
                key = Convert.ToInt32(formData.tbkey);
                if (formData.EncodedText != "")
                {
                    formData.EncodedText = "";
                }
                try
                {
                    foreach (var item in charsToRemove)
                    {
                        formData.YourText = formData.YourText.Replace(item, string.Empty);
                    }

                    char[] text = formData.YourText.ToCharArray().Where(s => !char.IsWhiteSpace(s)).ToArray();

                    for (int i = 0; i < text.Length; i++)
                    {
                        for (int j = 0; j <= alphabet.Length && j <= alphabetUpper.Length; j++)
                        {
                            if (text[i].ToString() == alphabet[j].ToString() || text[i].ToString() == alphabetUpper[j].ToString())
                            {
                                char.ToLower(text[i]);

                                formData.EncodedText += Convert.ToChar(alphabet[(j + key) % alphabet.Length]);
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Exception = ex.Message;
                }
            }
            return View(formData);
        }
        [HttpPost]
        public ActionResult DecodeText(FormData formData, FormCollection form)
        {
            // Caesar cipher decode
            if (Request.Form["Ciphers"] == "Caesar cipher")
            {
                if (formData.DecodedText != "")
                {
                    formData.DecodedText = "";
                }
                try
                {
                    int temp = 0;
                    char[] text = formData.EncodedText.ToCharArray();
                    for (int i = 0; i < text.Length; i++)
                    {
                        for (int j = 0; j <= alphabet.Length; j++)
                        {
                            if (text[i].ToString() == alphabet[j].ToString())
                            {
                                temp = j - key;

                                if (temp < 0)
                                    temp += alphabet.Length;
                                if (temp >= alphabet.Length)
                                    temp -= alphabet.Length;

                                formData.DecodedText += alphabet[temp];
                                break;
                            }
                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Exception = ex.Message;
                }
            }
            return View(formData);
        }
        public ActionResult Index()
        {
            SelectList items = new SelectList(ciphers);
            ViewBag.Ciphers = items;
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormData formData, FormCollection formCollection)
        {
            SelectList items = new SelectList(ciphers);
            ViewBag.Ciphers = items;

            // Caesar cipher encode
            if (Request.Form["Ciphers"] == "Caesar cipher" && formData.tbkey != "")
            {
                //Request.Form["Your decoded text:"] = "  Your decoded text:";
                //Request.Form["Decode"] = "Decode";
                key = Convert.ToInt32(formData.tbkey);
                if (formData.EncodedText != "")
                {
                    formData.EncodedText = "";
                }
                try
                {
                    foreach (var item in charsToRemove)
                    {
                        formData.YourText = formData.YourText.Replace(item, string.Empty);
                    }

                    char[] text = formData.YourText.ToCharArray().Where(s => !char.IsWhiteSpace(s)).ToArray();

                    for (int i = 0; i < text.Length; i++)
                    {
                        for (int j = 0; j <= alphabet.Length && j <= alphabetUpper.Length; j++)
                        {
                            if (text[i].ToString() == alphabet[j].ToString() || text[i].ToString() == alphabetUpper[j].ToString())
                            {
                                char.ToLower(text[i]);

                                formData.EncodedText += Convert.ToChar(alphabet[(j + key) % alphabet.Length]);
                                ViewBag.EncText = formData.EncodedText;
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Exception = ex.Message;
                }
            }
            return View(formData);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}