using UniversalHelmod.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalHelmod.Databases.Models
{
    public class Logistic : Item
    {
        public int Rate { get; set; }

        public string Transport { get; set; }
    }
}
