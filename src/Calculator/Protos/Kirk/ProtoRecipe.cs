using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Media.Imaging;

namespace Calculator.Protos.Kirk
{
    [DataContract]
    class ProtoRecipe
    {
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        [DataMember(Name = "key_name", IsRequired = true)]
        public string Key { get; set; }

        [DataMember(Name = "category", IsRequired = true)]
        public string Category { get; set; }

        [DataMember(Name = "time", IsRequired = true)]
        public int Time { get; set; }

        [DataMember(Name = "ingredients", IsRequired = true)]
        public List<object> ArrayIngredients { get; set; }

        [DataMember(Name = "product", IsRequired = true)]
        public object Product { get; set; }

        [DataMember(Name = "byproduct", IsRequired = false)]
        public object ByProduct { get; set; }
        public BitmapImage Icon { get; set; }
        public List<ProtoItem> Ingredients { get; set; }
        public List<ProtoItem> Products { get; set; }
    }
}
