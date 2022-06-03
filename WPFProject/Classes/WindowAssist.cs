using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFProject.Classes
{
    static class WindowAssist
    {
        private static IEnumerable<T> FindVisualChilds<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield return (T)Enumerable.Empty<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);
                if (ithChild == null) continue;
                if (ithChild is T t) yield return t;
                foreach (T childOfChild in FindVisualChilds<T>(ithChild)) yield return childOfChild;
            }
        }

        public static bool isAllTextBoxesFilled(DependencyObject depObj)
        {
            bool q = true;
            foreach (TextBox textbox in WindowAssist.FindVisualChilds<TextBox>(depObj))
            {
                if (!textbox.Text.Any())
                {   
                    q = false; break;
                }
            }
            return q;
        }

        public static bool isAllComboboxFilled(DependencyObject depObj)
        {
            bool q = true;
            foreach (ComboBox combobox in WindowAssist.FindVisualChilds<ComboBox>(depObj))
            {
                if (!combobox.Text.Any())
                {   
                    q = false; break;
                }
            }
            return q;
        }
    }
}
