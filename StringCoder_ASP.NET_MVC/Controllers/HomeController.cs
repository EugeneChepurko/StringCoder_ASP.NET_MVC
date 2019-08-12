using StringCoder_ASP.NET_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace StringCoder_ASP.NET_MVC.Controllers
{
    public class HomeController : Controller
    {
        readonly char[] alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        readonly char[] alphabetUpper = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        readonly string[] charsToRemove = new string[] { ",", ".", ";", ":", "@", "!", "#", "$", "%", "*", "(", ")", "[", "]", "^", "?", "|", "+", "-", "&", @"\", "/", "'", "\"" };
        private static int key = 0;
        int Xkey, Ytext;

        private static List<string> ciphers = new List<string>() { "Caesar cipher", "Vigenere cipher" };

        [HttpPost]
        public ActionResult EncodeText(FormData formData, FormCollection form)
        {
            SelectList items = new SelectList(ciphers);

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

            //  Vigenere cipher encode
            if (Request.Form["Ciphers"] == "Vigenere cipher")
            {
                //Request.Form["Your decoded text:"] = "  Your decoded text:";
                //Request.Form["Decode"] = "Decode";
                if (formData.EncodedText != "")
                {
                    formData.EncodedText = "";
                }
                foreach (var item in charsToRemove)
                {
                    formData.YourText = formData.YourText.Replace(item, string.Empty);
                }
                char[] text = formData.YourText.ToCharArray().Where(s => !char.IsWhiteSpace(s)).ToArray();
                char[] key = formData.tbkey.ToCharArray();
                try
                {
                    char[,] Vigenere_Table = new char[26, 26];

                    int temp = 0;
                    for (int i = 0; i < alphabet.Length; i++)
                    {
                        for (int j = 0; j < 26; j++)
                        {
                            temp = j + i;
                            if (temp >= 26)
                            {
                                temp = temp % 26;
                            }
                            Vigenere_Table[i, j] = alphabet[temp];
                        }
                    }

                    for (int t = 0, k = 0; t < text.Length || k < key.Length; t++, k++)
                    {
                        if (t >= text.Length)
                        {
                            break;
                        }
                        if (k == key.Length/*t % key.Length == 0*/)
                        {
                            k = 0;
                            for (int y = 0; y <= alphabet.Length; y++)
                            {
                                if (text[t].ToString() == alphabet[y].ToString())
                                {
                                    Ytext = y;
                                    for (int x = 0; x <= alphabet.Length; x++)
                                    {
                                        if (key[k].ToString() == alphabet[x].ToString())
                                        {
                                            Xkey = x;
                                            formData.EncodedText += Vigenere_Table[Ytext, Xkey].ToString();
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int y = 0; y <= alphabet.Length; y++)
                            {
                                if (text[t].ToString() == alphabet[y].ToString())
                                {
                                    Ytext = y;
                                    for (int x = 0; x <= alphabet.Length; x++)
                                    {
                                        if (key[k].ToString() == alphabet[x].ToString())
                                        {
                                            Xkey = x;
                                            formData.EncodedText += Vigenere_Table[Ytext, Xkey].ToString();
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Exception = ex.Message;
                }
            }        

            ViewBag.Ciphers = items;
            return View(formData);
        }

        [HttpPost]
        public ActionResult DecodeText(FormData formData, FormCollection form)
        {
            SelectList items = new SelectList(ciphers);
            ViewBag.Ciphers = items;
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

            //  Vigenere cipher decode
            if (Request.Form["Ciphers"] == "Vigenere cipher")
            {
                if (formData.DecodedText != "")
                {
                    formData.DecodedText = "";
                }
                char[] text = formData.EncodedText.ToCharArray().Where(s => !char.IsWhiteSpace(s)).ToArray();
                char[] key = formData.tbkey.ToCharArray();
                try
                {
                    char[,] Vigenere_Table = new char[26, 26];

                    int temp = 0;
                    for (int i = 0; i < alphabet.Length; i++)
                    {
                        for (int j = 0; j < 26; j++)
                        {
                            temp = j + i;
                            if (temp >= 26)
                            {
                                temp = temp % 26;
                            }
                            Vigenere_Table[i, j] = alphabet[temp];
                        }
                    }
                    // DECRYPT VIGENERE
                    for (int t = 0, k = 0; t < text.Length || k < key.Length; t++, k++)
                    {
                        if (t >= text.Length)
                        {
                            break;
                        }
                        if (k == key.Length)
                        {
                            k = 0;
                            for (int y = 0; y <= alphabet.Length; y++)
                            {
                                if (key[k].ToString() == alphabet[y].ToString())
                                {
                                    Xkey = y;
                                    for (int x = 0; x <= alphabet.Length; x++)
                                    {
                                        Ytext = x;
                                        if (Vigenere_Table[Ytext, Xkey].ToString() == text[t].ToString())
                                        {
                                            formData.DecodedText += alphabet[x].ToString();
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int y = 0; y <= alphabet.Length; y++)
                            {
                                if (key[k].ToString() == alphabet[y].ToString())
                                {
                                    Xkey = y;
                                    for (int x = 0; x <= alphabet.Length; x++)
                                    {
                                        Ytext = x;
                                        if (Vigenere_Table[Ytext, Xkey].ToString() == text[t].ToString())
                                        {
                                            formData.DecodedText += alphabet[x].ToString();
                                            break;
                                        }
                                    }
                                    break;
                                }
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
        [HttpGet]
        public ActionResult Index()
        {
            SelectList items = new SelectList(ciphers);
            ViewBag.Ciphers = items;
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}