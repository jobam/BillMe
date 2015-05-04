using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace BillMe.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public String Telephone { get; set; }
        public String Address { get; set; }
        public String Mail { get; set; }
        public String Date { get; set; }
        public String Client { get; set; }
        public List<Product> Products { get; set; }
    }
}