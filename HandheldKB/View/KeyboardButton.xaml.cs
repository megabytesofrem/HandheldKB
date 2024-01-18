using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HandheldKB.View
{
    public partial class KeyboardButton : UserControl
    {
        public static readonly DependencyProperty KeyLabelProperty = DependencyProperty.Register(
            "KeyLabel", typeof(string), typeof(KeyboardButton));

        public static readonly DependencyProperty ShiftKeyLabelProperty = DependencyProperty.Register(
            "ShiftKeyLabel", typeof(string), typeof(KeyboardButton));

        public string KeyLabel
        {
            get { return (string)GetValue(KeyLabelProperty); }
            set { SetValue(KeyLabelProperty, value); }
        }

        public string ShiftKeyLabel
        {
            get { return (string)GetValue(ShiftKeyLabelProperty); }
            set { SetValue(ShiftKeyLabelProperty, value); }
        }

        public KeyboardButton()
        {
            InitializeComponent();
            Loaded += KeyboardButton_Loaded;
        }

        private void KeyboardButton_Loaded(object sender, RoutedEventArgs e)
        {
            var keyText = FindVisualChild<TextBlock>(this, "KeyText");
            var shiftKeyText = FindVisualChild<TextBlock>(this, "ShiftKeyText");

            if (keyText != null)
            {
                keyText.Text = KeyLabel;
            }

            if (shiftKeyText != null)
            {
                // Very cursed code, because we need to escape the curly braces in the XAML
                if (ShiftKeyLabel == "%lbrace%")
                {
                    shiftKeyText.Text = "{";
                }
                else
                {
                    shiftKeyText.Text = ShiftKeyLabel;
                }
            }


        }

        private static T FindVisualChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is T dependencyObject && (child as FrameworkElement)?.Name == childName)
                {
                    return dependencyObject;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child, childName);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }

            return null;
        }
    }
}
