using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

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
}
