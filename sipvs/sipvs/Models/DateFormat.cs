using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace sipvs.Models
{
    [XmlRoot(Namespace = "http://www.taxbonusform.com", ElementName = "date")]
    public class DateFormat
    {
        public DateFormat()
        {
        }

        public DateFormat(string date_input)
        {
            format = "yyyy-MM-dd";
            date = date_input;
        }

        [XmlAttribute(Namespace = "http://www.taxbonusform.com")]
        public string format { get; set; } = "yyyy-MM-dd";

        [XmlElement(Namespace = "http://www.taxbonusform.com")]
        [Required(ErrorMessage = "Povinné pole")]
        public string date { get; set; }
    }
}
