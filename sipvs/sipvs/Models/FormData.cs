using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using sipvs.Models;

namespace Z1_forms.model
{
    [XmlRoot(Namespace = "http://www.taxbonusform.com", ElementName = "FormData")]
    public class FormData
    {

        [XmlElement(Namespace = "http://www.taxbonusform.com")]
        [Required( ErrorMessage="Povinné pole")]
        public string name { get; set; }
        [XmlElement(Namespace = "http://www.taxbonusform.com")]
        [Required( ErrorMessage="Povinné pole")]
        public string surname { get; set; }
        [XmlElement(Namespace = "http://www.taxbonusform.com")]
        [Required( ErrorMessage="Povinné pole")]
        public decimal? age { get; set; }
        [XmlElement(Namespace = "http://www.taxbonusform.com")]
        public string? degreeAfter { get; set; }
        [XmlElement(Namespace = "http://www.taxbonusform.com")]
        public string? degreeBefore { get; set; }
        [XmlElement(Namespace = "http://www.taxbonusform.com")]
        public string maritalStatus { get; set; }

        [XmlElement(Namespace = "http://www.taxbonusform.com")]
        [Required( ErrorMessage="Povinné pole")]
        public string streetName { get; set; }
        [XmlElement(Namespace = "http://www.taxbonusform.com")]
        [Required( ErrorMessage="Povinné pole")]
        public string houseNum { get; set; }
        [XmlElement(Namespace = "http://www.taxbonusform.com")]
        [Required( ErrorMessage="Povinné pole")]
        public string city { get; set; }
        [XmlElement(Namespace = "http://www.taxbonusform.com")]
        [StringLength(5,MinimumLength =5, ErrorMessage ="PSČ má mať 5 číslic" )]
        [Required(ErrorMessage = "Povinné pole")]
        public string postCode { get; set; }
        [XmlElement(Namespace = "http://www.taxbonusform.com")]
        [Required( ErrorMessage="Povinné pole")]
        public string country { get; set; }
        [XmlElement(Namespace = "http://www.taxbonusform.com")]
        public bool tax { get; set; }
       
        public List<Child> kids { get; set; }
        public sipvs.Models.DateFormat dateFormat { get; set; }
//        [XmlElement( Namespace = "http://www.taxbonusform.com")]
//        [Required(ErrorMessage = "Povinné pole")]
//        public DateTime? date { get; set; }
//>>>>>>> detached

        public void fillOutEmptyData()
        {
            this.name ??= "";
            this.surname ??= "";
            this.degreeBefore ??= "";
            this.degreeAfter ??= "";
            this.maritalStatus ??= "";
            this.streetName ??= "";
            this.houseNum ??= "";
            this.city ??= "";
            this.country ??= "";
        }
    }

 
}
