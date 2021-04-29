using System;
using System.Collections.Generic;
using System.Text;

namespace SampleData
{
    [Serializable]
    public class Supplier
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
    }
}
