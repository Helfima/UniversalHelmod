using UniversalHelmod.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace UniversalHelmod.Sheets.Views
{
    public class IngredientTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            var state = ((dynamic)item).State;
            if (element != null && state != null)
            {
                var name = $"IngredientTemplate_{state}";
                var template = element.TryFindResource(name) as DataTemplate;
                if(template == null)
                {
                    template = element.TryFindResource("IngredientTemplate_Default") as DataTemplate;
                }
                return template;
            }
            return null;
        }
    }
}
