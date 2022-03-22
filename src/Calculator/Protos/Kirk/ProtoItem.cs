using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Media.Imaging;

namespace Calculator.Protos.Kirk
{
    [DataContract]
    public class ProtoItem
    {
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        [DataMember(Name = "key_name", IsRequired = true)]
        public string Key { get; set; }

        [DataMember(Name = "tier", IsRequired = true)]
        public int Tier { get; set; }

        [DataMember(Name = "stack_size", IsRequired = true)]
        public int Stack { get; set; }

        public double Count{ get; set; }
        public BitmapImage Icon { get; set; }
        public ProtoItem Clone()
        {
            return new ProtoItem()
            {
                Name = this.Name,
                Key = this.Key,
                Tier = this.Tier,
                Stack = this.Stack,
                Icon = this.Icon,
                Count = this.Count
            };
        }
    }
}
