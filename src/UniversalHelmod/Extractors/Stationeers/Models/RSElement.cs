using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UniversalHelmod.Extensions;

namespace UniversalHelmod.Extractors.Stationeers.Models
{
    public class RSElement
    {
        public string Name { get; set; }
        public string PrefabName { get; set; }
        public RSElement() { }
        public RSElement(string prefabName)
        {
            this.PrefabName = prefabName;
        }
    }
}
