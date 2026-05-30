using UniversalHelmod.Classes;
using UniversalHelmod.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace UniversalHelmod.Databases.Models
{
    public class Amount : NotifyProperty
    {
        
        private Amount()
        {

        }
        public Amount(Item item, double count)
        {
            this.item = item;
            this.count = count;
        }
        private Item item;
        public Item Item
        {
            get { return item; }
        }
        public string Type
        {
            get { return item?.Type; }
        }
        public string Form
        {
            get { return item?.Form; }
        }
        public BitmapImage Icon
        {
            get { return item?.Icon; }
        }
        private ItemState state = ItemState.Normal;
        public ItemState State
        {
            get { return state; }
            set { state = value; }
        }

        private double count;
        public double Count
        {
            get { return count; }
            set { count = value; NotifyPropertyChanged(); }
        }
        private double total;
        public double Total
        {
            get { return total; }
            set { total = value; }
        }
        private double flow;
        public double Flow
        {
            get { return flow; }
            set { flow = value; }
        }
        public string Name
        {
            get { return item?.Name; }
        }
        public string DisplayName
        {
            get { return item?.DisplayName; }
        }
        private LogisticFlow logisticFlow;
        public LogisticFlow LogisticFlow
        {
            get { return logisticFlow; }
            set { logisticFlow = value; }
        }
        public Amount Clone(double factor = 1)
        {
            var item = new Amount()
            {
                item = this.item,
                count = this.count * factor,
                total = this.count * factor,
                state = this.state
            };
            return item;
        }
    }
}
