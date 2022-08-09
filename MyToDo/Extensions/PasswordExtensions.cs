using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyToDo.Extensions
{
    public class PasswordExtensions
    {  
        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordExtensions), new PropertyMetadata(OnPasswordPropertyChanged));

        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = (PasswordBox)d;
            string password = (string)e.NewValue;

            if(password!=null && passwordBox.Password != password)
            {
                passwordBox.Password = password;
            }
        }
    }

    public class PasswordBehavior : Behavior<PasswordBox>
    { 
        protected override void OnAttached()
        {
            base.OnAttached(); 
            AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;
        }

        private void AssociatedObject_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            string password = PasswordExtensions.GetPassword(passwordBox);

            if(passwordBox.Password != password)
            {
                PasswordExtensions.SetPassword(passwordBox, passwordBox.Password);
            }

        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if(AssociatedObject!=null)
                AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;
        }


    }
}
