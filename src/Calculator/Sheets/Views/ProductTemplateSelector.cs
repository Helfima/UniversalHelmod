using Calculator.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Calculator.Sheets.Views
{
    public class ProductTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            var state = ((dynamic)item).State;
            if (element != null && state != null)
            {
                var name = $"ProductTemplate_{state}";
                var template = element.TryFindResource(name) as DataTemplate;
                if (template == null)
                {
                    template = element.TryFindResource("ProductTemplate_Default") as DataTemplate;
                }
                return template;
            }
            return null;
        }
    }
}
