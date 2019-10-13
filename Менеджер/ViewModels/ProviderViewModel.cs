using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Менеджер
{
    class ProviderViewModel : ViewModelsBase , IPhoneMask
    {
        #region Dialog
        public ICommand RunDialogAddProviderCommand => new AnotherCommandImplementation(AddProviderCommand);

        private async void AddProviderCommand(object o)
        {
            var view = new ProviderDialogView
            {
                DataContext = new ProviderDialogViewModel()
            };

            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }
     

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }
        #endregion

        #region Property
        public int PhoneLength { get; set; }

        private string phone;

        public string Phone
        {
            get => phone;
            set
            {
                if (value == phone) return;
                phone = value;
                ((IPhoneMask)this).PhoneMask(ref phone, PhoneLength);
                OnPropertyChanged(nameof(Phone));
            }
        }
        #endregion

        #region Method
        public void OnLeftButtonClicked(object obj, KeyEventArgs e)
        {

            if (e.Key == Key.Left)
            {
                (obj as TextBox).Focus();
                e.Handled = true;
                (obj as TextBox).SelectionStart = (obj as TextBox).Text.Length;
            }
        }

        public void OnLeftMouseClicked(object obj, MouseButtonEventArgs e)
        {
            (obj as TextBox).Focus();
            e.Handled = true;
            (obj as TextBox).SelectionStart = (obj as TextBox).Text.Length;
        }

        public void PreviewTextInput_Phone(object obj, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion
    }
}
