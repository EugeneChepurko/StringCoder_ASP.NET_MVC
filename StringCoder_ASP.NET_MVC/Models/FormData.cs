using System.Web.UI.WebControls;

namespace StringCoder_ASP.NET_MVC.Models
{
    public class FormData
    {
        public string YourText { get; set; }
        public string EncodedText { get; set; }
        public string DecodedText { get; set; }
        public string tbkey { get; set; }
        public Label lbDecText { get; set; }
        public Button Decode { get; set; }
        public DropDownList Ciphers { get; set; }
    }
}