﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// The Customer in a parcel details
    /// </summary>
    public class CustomerInParcel
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return "Customer ID: " + CustomerID + "\nCustomer name: " + Name + "\n";
        }
    }
}