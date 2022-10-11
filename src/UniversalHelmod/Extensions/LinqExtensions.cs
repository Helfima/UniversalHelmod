using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace UniversalHelmod.Extensions
{
    public static class LinqExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> col)
        {
            if(col == null) return new ObservableCollection<T>();
            return new ObservableCollection<T>(col);
        }
    }
}
