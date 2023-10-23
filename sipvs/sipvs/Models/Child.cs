using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Z1_forms.model
{
    //[XmlRoot(Namespace = "http://www.taxbonusform.com", ElementName = "Child")]
    public class Child
    {
        //[XmlElement(Namespace = "http://www.taxbonusform.com")]
        [Required(ErrorMessage = "Povinné pole")]
        public string fullname { get; set; }
        //[XmlElement(Namespace = "http://www.taxbonusform.com")]
        [Required(ErrorMessage = "Povinné pole")]
        public decimal? age { get; set; }

        public Child() { }

        public Child(string fullname, decimal age)
        {
            this.fullname = fullname;
            this.age = age;
        }

    }
}
