using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z1_forms.model
{
    public class FormData
    {
        public string name { get; set; }
        public string surname { get; set; }
        public decimal age { get; set; }
        public string degreeAfter { get; set; }
        public string degreeBefore { get; set; }
        public string maritalStatus { get; set; }
        public string streetName { get; set; }
        public string houseNum { get; set; }
        public string city { get; set; }
        public string postCode { get; set; }
        public string country { get; set; }
        public bool tax { get; set; }
        public List<Child> kids { get; set; }
        public DateTime date { get; set; }

    }

 
}
