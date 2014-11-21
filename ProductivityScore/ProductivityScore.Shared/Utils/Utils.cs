using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ProductivityScore.Utils
{
    /// <summary>
    /// This is the boilerplate implementation of the <code>INotifyPropertyChanged</code>
    /// </summary>
    public class Data : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Notify listeners of a change in a property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }


        /// <summary>
        /// Set a field and notify of the change.
        /// </summary>
        /// <typeparam name="T">The type of the field</typeparam>
        /// <param name="field">A reference to the changing field</param>
        /// <param name="value">The value to be assigned to the field</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns><code>False</code> if there was no change</returns>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            // Do not trigger event if no actual change is applied
            if (EqualityComparer<T>.Default.Equals(field, value)) 
                return false;

            // Apply change
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }


    static class XAMLHelper
    {
        /// <summary>
        /// Gets all the children of this element in a flat list
        /// </summary>
        /// <param name="parent">The element whose children to get</param>
        /// <returns>A flat list of all the children</returns>
        public static List<Control> AllChildren(DependencyObject parent)
        {
            var list = new List<Control>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is Control)
                    list.Add(child as Control);
                list.AddRange(AllChildren(child));
            }
            return list;
        }


        /// <summary>
        /// Get the first parent with the given name
        /// </summary>
        /// <param name="ob">The element to start the search from</param>
        /// <param name="parentName">The target parent name</param>
        /// <returns>The parent element</returns>
        public static FrameworkElement GetParent(DependencyObject ob, String parentName)
        {
            DependencyObject parent = ob;
            while (parent != null)
            {
                if ((parent as FrameworkElement).Name == parentName)
                    return parent as FrameworkElement;
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }


        /// <summary>
        /// Finds the first parent with a DataContext value of type T
        /// </summary>
        /// <typeparam name="T">The type of the data context</typeparam>
        /// <param name="ob">The object to start the search from</param>
        /// <returns>The DataContext value found, or null</returns>
        public static T GetDataContext<T>(Object ob)
        {
            DependencyObject scan = ob as DependencyObject;
            while (scan != null)
            {
                if (scan is FrameworkElement)
                {
                    FrameworkElement element = scan as FrameworkElement;
                    if (element.DataContext is T)
                        return (T)element.DataContext;
                }
                scan = VisualTreeHelper.GetParent(scan);
            }
            return default(T);
        }
    }

}
