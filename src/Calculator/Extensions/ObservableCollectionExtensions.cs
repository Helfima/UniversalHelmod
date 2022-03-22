using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Calculator.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void MoveStep<T>(this ObservableCollection<T> list, T item, int step)
        {
            if (item != null)
            {
                var oldIndex = list.IndexOf(item);
                if (oldIndex > -1)
                {
                    if (step > 0)
                    {
                        list.RemoveAt(oldIndex);
                        if (oldIndex + step < list.Count)
                        {
                            list.Insert(oldIndex + step, item);
                        }
                        else
                        {
                            list.Add(item);
                        }
                    }
                    if (step < 0)
                    {
                        if (oldIndex + step >= 0)
                        {
                            list.Insert(oldIndex + step, item);
                        }
                        else
                        {
                            list.Insert(0, item);
                        }
                        list.RemoveAt(oldIndex + 1);
                    }
                }
            }
        }
    }
}
