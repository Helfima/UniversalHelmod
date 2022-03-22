using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Media.Imaging;

namespace Calculator.Protos.Kirk
{
    [DataContract]
    public class ProtoBelt
    {
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        [DataMember(Name = "key_name", IsRequired = true)]
        public string Key { get; set; }

        [DataMember(Name = "rate", IsRequired = true)]
        public int Rate { get; set; }

        public BitmapImage Icon { get; set; }
        public ProtoBelt Clone()
        {
            return new ProtoBelt()
            {
                Name = this.Name,
                Key = this.Key,
                Rate = this.Rate,
                Icon = this.Icon,
            };
        }
    }
}
