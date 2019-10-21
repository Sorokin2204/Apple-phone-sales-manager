using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Менеджер
{
    class OrderViewModel : ViewModelsBase, IPhoneMask
    {
        public readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter;
        DataSet dataSet;

        public OrderViewModel()
        {
            OrderModels = new ObservableCollection<OrderModel>();
            FillOrderModels();
        }

        public void FillOrderModels()
        {
            var count = 1;
            try
            {
                connection = new SqlConnection(connectionString);
                command = new SqlCommand("sp_SearchOrder", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                adapter = new SqlDataAdapter(command);
                dataSet = new DataSet();
                connection.Open();
                command.Parameters.AddWithValue("@Number", Number);
                command.Parameters.AddWithValue("@Date", Date);
                command.Parameters.AddWithValue("@Name", Name);
                command.Parameters.AddWithValue("@Telephone", Phone);
                adapter.Fill(dataSet);
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    OrderModels.Add(new OrderModel
                    {
                        NumberRow = count++,
                        Date = Convert.ToDateTime(dr[0]).ToShortDateString(),
                        Number = dr[1].ToString(),
                        Model = dr[2].ToString(),
                        Memory = Convert.ToInt32(dr[3].ToString()),
                        Color = dr[4].ToString(),
                        Name = dr[5].ToString(),
                        Telephone = dr[6].ToString(),
                        Address = dr[7].ToString(),
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
            OrderModels.Clear();
            FillOrderModels();
        }

        #region Dialog
        public ICommand RunDialogAddOrderCommand => new AnotherCommandImplementation(AddOrderCommand);

        private async void AddOrderCommand(object o)
        {
            var view = new OrderDialogView
            {
                DataContext = new OrderDialogViewModel()
            };

            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
            if ((bool)result == true)
            {
                OrderModels.Clear();
                FillOrderModels();
            }
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }
        #endregion

        #region Property

        public ObservableCollection<OrderModel> OrderModels { get; set; }

        public ProductModel SelectedProductModel { get; set; }

        #region Search panel
        private string number = string.Empty;
        public string Number
        {
            get { return number; }
            set
            {
                if (number == value) return;
                number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

        private string date = string.Empty;
        public string Date
        {
            get { return date; }
            set
            {
                if (date == value) return;
                date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private string name = string.Empty;
        public string Name
        {
            get { return name; }
            set
            {
                if (name == value) return;
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private int phoneLength;
        public int PhoneLength
        {
            get { return phoneLength; }
            set
            {
                if (phoneLength == value) return;
                phoneLength = value;
                OnPropertyChanged(nameof(PhoneLength));
            }
        }

        private string phone = string.Empty;
        public string Phone
        {
            get => phone;
            set
            {
                if (value == phone) return;
                phone = value;
                ((IPhoneMask)this).PhoneMask(ref phone,PhoneLength);
             OnPropertyChanged(nameof(Phone));
            }
        }
        #endregion

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

        public void PreviewTextInput_Number(object obj, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void PreviewTextInput_Name(object obj, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^А-Яа-я]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion
    }
}

