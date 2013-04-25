using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ServiiMetro.Core.Helpers
{
    /// <summary>
    /// Base for property changed classes
    /// </summary>
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        protected void SetField<T>(ref T field, T value, Expression<Func<T>> propertyExpression)
        {
             SetField(ref field, value, propertyExpression, null);
        }

        protected void SetField<T>(ref T field, T value, string propertyName)
        {
            SetField(ref field, value, propertyName, null);
        }

        protected void SetField<T>(ref T field, T value, string propertyName , Action callOnChange)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                RaisePropertyChanged(propertyName);
                if (callOnChange != null) callOnChange();
            }
        }

       
        protected void SetField<T>(ref T field, T value, Expression<Func<T>> propertyExpression,  Action callOnChange)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                RaisePropertyChanged(ExtractPropertyName(propertyExpression));
                if (callOnChange != null) callOnChange();
            }
        }

        private static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException("Expression must be a MemberExpression.", "propertyExpression");
            return memberExpression.Member.Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }

   
}