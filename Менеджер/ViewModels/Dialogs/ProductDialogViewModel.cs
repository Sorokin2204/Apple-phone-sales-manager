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
    class ProductDialogViewModel : ViewModelsBase , IPartNoMask
    {

        #region Property
        public int PartNoLength { get; set; }

        private string partNo;

        public string PartNo
        {
            get => partNo;
            set
            {
                if (value == partNo) return;
                partNo = value;
                ((IPartNoMask)this).PartNoMask(ref partNo, PartNoLength);
                OnPropertyChanged(nameof(PartNo));
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

        public void PreviewTextInput_PartNo(object obj, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9A-Z]+");
            e.Handled = regex.IsMatch(e.Text);
        }
#endregion
    }
}
