using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace wpfBudgetSys.View.UserControls
{
    public partial class LabeledTextBox : UserControl
    {
        public LabeledTextBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty InputLabelProperty =
            DependencyProperty.Register(
                nameof(InputLabel),
                typeof(string),
                typeof(LabeledTextBox),
                new PropertyMetadata(string.Empty));

        public string InputLabel
        {
            get => (string)GetValue(InputLabelProperty);
            set => SetValue(InputLabelProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(LabeledTextBox),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register(
                nameof(ErrorMessage),
                typeof(string),
                typeof(LabeledTextBox),
                new PropertyMetadata(string.Empty));

        public string ErrorMessage
        {
            get => (string)GetValue(ErrorMessageProperty);
            set => SetValue(ErrorMessageProperty, value);
        }

        public static readonly DependencyProperty IsNumericOnlyProperty =
            DependencyProperty.Register(
                nameof(IsNumericOnly),
                typeof(bool),
                typeof(LabeledTextBox),
                new PropertyMetadata(false));

        public bool IsNumericOnly
        {
            get => (bool)GetValue(IsNumericOnlyProperty);
            set => SetValue(IsNumericOnlyProperty, value);
        }

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(
                nameof(IsReadOnly),
                typeof(bool),
                typeof(LabeledTextBox),
                new PropertyMetadata(false));

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumericOnly || sender is not TextBox textBox)
                return;

            Regex regex = new Regex(@"^[0-9]*\.?[0-9]*$");
            string futureText = textBox.Text.Insert(textBox.CaretIndex, e.Text);
            e.Handled = !regex.IsMatch(futureText);
        }

        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (IsNumericOnly)
            {
                if (e.DataObject.GetDataPresent(typeof(string)))
                {
                    string pastedText = (string)e.DataObject.GetData(typeof(string));
                    Regex regex = new Regex(@"^[0-9]*\.?[0-9]*$");
                    if (!regex.IsMatch(pastedText))
                        e.CancelCommand();
                }
                else
                {
                    e.CancelCommand();
                }
            }
        }
    }
}