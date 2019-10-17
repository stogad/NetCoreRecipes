﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentValidationDemo.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Forename { get; set; }
        public decimal Discount { get; set; }
        public Address Address { get; set; }
    }
    public class Address
    {
        public string Postcode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
    }
}
