using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Media.Imaging;

namespace Calculator.Protos.Kirk
{
    [DataContract]
    public class ProtoBuilding
    {
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        [DataMember(Name = "key_name", IsRequired = true)]
        public string Key { get; set; }

        [DataMember(Name = "category", IsRequired = true)]
        public string Category { get; set; }

        [DataMember(Name = "power", IsRequired = true)]
        public int Power { get; set; }

        [DataMember(Name = "max", IsRequired = true)]
        public int Max { get; set; }

        public BitmapImage Icon { get; set; }
        public ProtoBuilding Clone()
        {
            return new ProtoBuilding()
            {
                Name = this.Name,
                Key = this.Key,
                Category = this.Category,
                Power = this.Power,
                Max = this.Max,
                Icon = this.Icon
            };
        }
    }
}
