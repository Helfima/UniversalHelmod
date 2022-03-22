using Calculator.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace Calculator.Models
{
    public class Amount : NotifyProperty
    {
        private Item item;
        private double count;
        private double flow;
        private double total;
        private ItemState state = ItemState.Normal;

        private Amount()
        {

        }
        public Amount(Item item, double count)
        {
            this.item = item;
            this.count = count;
        }
        public Item Item
        {
            get { return item; }
        }
        public ItemType ItemType
        {
            get { return item.ItemType; }
        }
        public BitmapImage Icon
        {
            get { return item.Icon; }
        }
        public ItemState State
        {
            get { return state; }
            set { state = value; }
        }

        public double Count
        {
            get { return count; }
            set { count = value; NotifyPropertyChanged(); }
        }
        public double Total
        {
            get { return total; }
            set { total = value; }
        }
        public double Flow
        {
            get { return flow; }
            set { flow = value; }
        }
        public string Name
        {
            get { return item.Name; }
        }
        public string Type
        {
            get { return item.GetType().Name; }
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
