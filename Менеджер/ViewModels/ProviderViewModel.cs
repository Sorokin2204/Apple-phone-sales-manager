using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
        public readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter;
        DataSet dataSet;

        public ProviderViewModel()
        {
            ProviderModels = new ObservableCollection<ProviderModel>();
            FillProviderModels();
        }

        public void FillProviderModels()
        {
            var count = 1;
            try
            {
                connection = new SqlConnection(connectionString);
                command = new SqlCommand("sp_SearchProvider", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                adapter = new SqlDataAdapter(command);
                dataSet = new DataSet();
                connection.Open();
                command.Parameters.AddWithValue("@Title", Title);
                command.Parameters.AddWithValue("@Telephone", Phone);
                adapter.Fill(dataSet);
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    ProviderModels.Add(new ProviderModel
                    {
                        NumberRow = count++,
                        Title = dr[0].ToString(),
                        Address = dr[1].ToString(),
                        Telephone = dr[2].ToString(),
                    });
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                dataSet = null;
                adapter.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }

        public void Click()
        {
            ProviderModels.Clear();
            FillProviderModels();
        }

        #region Dialog
        public ICommand RunDialogAddProviderCommand => new AnotherCommandImplementation(AddProviderCommand);

        private async void AddProviderCommand(object o)
        {
            var view = new ProviderDialogView
            {
                DataContext = new ProviderDialogViewModel()
            };
            
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
            if ((bool)result == true)
            {
                ProviderModels.Clear();
                FillProviderModels();
            }
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }
     

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }
        #endregion

        #region Property

        public ObservableCollection<ProviderModel> ProviderModels { get; set; }

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

        public int PhoneLength { get; set; }

        private string phone =string.Empty;

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
