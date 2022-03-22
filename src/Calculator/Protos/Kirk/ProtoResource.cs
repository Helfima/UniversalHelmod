using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Media.Imaging;

namespace Calculator.Protos.Kirk
{
    [DataContract]
    public class ProtoResource
    {
        [DataMember(Name = "key_name", IsRequired = true)]
        public string Key { get; set; }

        [DataMember(Name = "category", IsRequired = true)]
        public string Category { get; set; }

        public BitmapImage Icon { get; set; }
        public ProtoResource Clone()
        {
            return new ProtoResource()
            {
                Key = this.Key,
                Category = this.Category,
                Icon = this.Icon
            };
        }
    }
}
