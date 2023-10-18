using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z1_forms.model
{
    public class FormData
    {
        [Required( ErrorMessage="Povinné pole")]
        public string name { get; set; }
        [Required( ErrorMessage="Povinné pole")]
        public string surname { get; set; }
        [Required( ErrorMessage="Povinné pole")]
        public decimal age { get; set; }
        public string? degreeAfter { get; set; }
        public string? degreeBefore { get; set; }
        public string maritalStatus { get; set; }
        [Required( ErrorMessage="Povinné pole")]
        public string streetName { get; set; }
        [Required( ErrorMessage="Povinné pole")]
        public string houseNum { get; set; }
        [Required( ErrorMessage="Povinné pole")]
        public string city { get; set; }
        [StringLength(5,MinimumLength =5, ErrorMessage ="PSČ má mať 5 číslic" )]
        public string postCode { get; set; }
        [Required( ErrorMessage="Povinné pole")]
        public string country { get; set; }
        public bool tax { get; set; }
        public List<Child> kids { get; set; }
        public DateTime date { get; set; }

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
