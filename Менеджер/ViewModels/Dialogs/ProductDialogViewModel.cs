using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.Data;
using System.Windows;

namespace Менеджер
{
    class ProductDialogViewModel : ViewModelsBase , IPartNoMask , IFillComboBox
    {
        public readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter;

        public ProductDialogViewModel()
        {
            ButtonContent = "ДОБАВИТЬ";
            Models = ((IFillComboBox)this).FillComboBox<Model>("sp_Models_Products", 0,0,0, connectionString);
            Memory = ((IFillComboBox)this).FillComboBox<Memory>("sp_Memory",0,0,0, connectionString);
            Colors = ((IFillComboBox)this).FillComboBox<Color>("sp_Colors",0,0,0, connectionString);
            Providers = ((IFillComboBox)this).FillComboBox<Provider>("sp_Providers",0,0,0, connectionString);
        }

        public ProductDialogViewModel(int modelID, int colorID, int memoryID, int providerID , int supplyID,string partNo ,int quantityDelivered, double price, double retailPrice) : this()
        {
            ButtonContent = "РЕДАКТИРОВАТЬ";
            SelectedModelID = modelID;
            SelectedColorID = colorID;
            SelectedMemoryID = memoryID;
            SelectedProviderID = providerID;
            SelectedSupplyID = supplyID;
            PartNo = partNo;
            QuantityDelivered =  quantityDelivered.ToString();
            Price = price.ToString().Replace(",", ".");
            RetailPrice = retailPrice.ToString().Replace(",", ".");
        }


        public void Click_AddButton(object sender, RoutedEventArgs e)
        {
            AddProduct();
        }

        public void AddProduct()
        {
            try
            {
                connection = new SqlConnection(connectionString);
                command = new SqlCommand("sp_AddProduct", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                adapter = new SqlDataAdapter(command);
              
                command.Parameters.AddWithValue("@ModelID", SelectedModelID);
                command.Parameters.AddWithValue("@ColorID", SelectedColorID);
                command.Parameters.AddWithValue("@MemoryID", SelectedMemoryID);
                command.Parameters.AddWithValue("@ProviderID", SelectedProviderID);
                command.Parameters.AddWithValue("@PartNo", PartNo);
                command.Parameters.AddWithValue("@QuantityDelivered", QuantityDelivered);
                command.Parameters.AddWithValue("@Price", Price);
                command.Parameters.AddWithValue("@RetailPrice", RetailPrice);
                command.Parameters.AddWithValue("@SupplyID", SelectedSupplyID);
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

        public ObservableCollection<Model> Models { get; set; }

        public ObservableCollection<Provider> Providers { get; set; }

        public ObservableCollection<Color> Colors { get; set; }

        public ObservableCollection<Memory> Memory { get; set; }

        public int PartNoLength { get; set; }

        private int selectedModelID;
        public int SelectedModelID
        {
            get { return selectedModelID; }
            set
            {
                if (selectedModelID == value) return;
                selectedModelID = value;
                OnPropertyChanged(nameof(SelectedModelID));
            }
        }

        private int selectedColorID;
        public int SelectedColorID
        {
            get { return selectedColorID; }
            set
            {
                if (selectedColorID == value) return;
                selectedColorID = value;
                OnPropertyChanged(nameof(SelectedColorID));
            }
        }

        private int selectedMemoryID ;
        public int SelectedMemoryID
        {
            get { return selectedMemoryID; }
            set
            {
                if (selectedMemoryID == value) return;
                selectedMemoryID = value;
                OnPropertyChanged(nameof(SelectedMemoryID));
            }
        }

        private int selectedSupplyID = 0;
        public int SelectedSupplyID
        {
            get { return selectedSupplyID; }
            set
            {
                if (selectedSupplyID == value) return;
                selectedSupplyID = value;
                OnPropertyChanged(nameof(SelectedSupplyID));
            }
        }

        private int selectedProviderID ;
        public int SelectedProviderID
        {
            get { return selectedProviderID; }
            set
            {
                if (selectedProviderID == value) return;
                selectedProviderID = value;
                OnPropertyChanged(nameof(SelectedProviderID));
            }
        }

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

        private string quantityDelivered = string.Empty;
        public string QuantityDelivered
        {
            get { return quantityDelivered; }
            set
            {
                if (quantityDelivered == value) return;
                quantityDelivered = value;
                OnPropertyChanged(nameof(QuantityDelivered));
            }
        }

        private string price = string.Empty;
        public string Price
        {
            get { return price; }
            set
            {
                if (price == value) return;
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        private string retailPrice  = string.Empty;
        public string RetailPrice
        {
            get { return retailPrice; }
            set
            {
                if (retailPrice == value) return;
                retailPrice = value;
                OnPropertyChanged(nameof(RetailPrice));
            }
        }

        private string buttonContent = string.Empty;
        public string ButtonContent
        {
            get { return buttonContent; }
            set
            {
                if (buttonContent == value) return;
                buttonContent = value;
                OnPropertyChanged(nameof(ButtonContent));
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


//public void PreviewTextInput_RetailPrice(object obj, TextCompositionEventArgs e)
//{
//    /*|[.][0-9]{1,2}*/
//    Console.WriteLine("X." + Regex.IsMatch(e.Text, @"^[0-9]{1,8}(?:\.)"));
//    Console.WriteLine("X.X" + Regex.IsMatch(e.Text, @"^[0-9]{1,8}(?:\.[0-9])"));
//    Console.WriteLine("X.XX" + Regex.IsMatch(e.Text, @"^[0-9]{1,8}(?:\.[0-9][0-9])"));
//    //Regex regex = new Regex(@"[0-9]+(\.[0-9][0-9]?)?");
//    //e.Handled = !regex.IsMatch(e.Text);
//}
//private bool IsValid(DependencyObject obj)
//{
//    return !Validation.GetHasError(obj) && LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>().All(IsValid);
//}
//Trace.WriteLine(IsValid(sender as DependencyObject));
//e.Handled =  IsValid(sender as DependencyObject);
//e.Handled = IsValid(sender as DependencyObject);