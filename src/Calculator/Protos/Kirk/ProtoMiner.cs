using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Media.Imaging;

namespace Calculator.Protos.Kirk
{
    [DataContract]
    public class ProtoMiner
    {
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        [DataMember(Name = "key_name", IsRequired = true)]
        public string Key { get; set; }

        [DataMember(Name = "category", IsRequired = true)]
        public string Category { get; set; }

        [DataMember(Name = "power", IsRequired = true)]
        public int Power { get; set; }

        [DataMember(Name = "base_rate", IsRequired = true)]
        public int Rate { get; set; }

        public BitmapImage Icon { get; set; }
        public ProtoMiner Clone()
        {
            return new ProtoMiner()
            {
                Name = this.Name,
                Key = this.Key,
                Category = this.Category,
                Power = this.Power,
                Rate = this.Rate,
                Icon = this.Icon
            };
        }
    }
}
