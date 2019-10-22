using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Менеджер
{
    class ProviderDialogViewModel : ViewModelsBase, IPhoneMask
    {
        public readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter;


        public void Click_AddButton(object sender, RoutedEventArgs e)
        {
            AddProvider();
        }

        public void AddProvider()
        {
            try
            {
                connection = new SqlConnection(connectionString);
                command = new SqlCommand("sp_AddProvider", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                adapter = new SqlDataAdapter(command);
                command.Parameters.AddWithValue("@Title", Title);
                command.Parameters.AddWithValue("@Address", Address);
                command.Parameters.AddWithValue("@Telephone", Phone);
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                adapter.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }

        #region Property
        private string title = string.Empty;
        public string Title
        {
            get { return title; }
            set
            {
                if (title == value) return;
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string address = string.Empty;
        public string Address
        {
            get { return address; }
            set
            {
                if (address == value) return;
                address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

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
