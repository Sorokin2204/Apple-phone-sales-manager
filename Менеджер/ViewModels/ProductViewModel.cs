﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;

namespace Менеджер
{
    class ProductViewModel : ViewModelsBase , IPartNoMask , IFillComboBox
    {
        public readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter;
        DataSet dataSet;

        public ProductViewModel()
        {
            ProductModels = new ObservableCollection<ProductModel>();
            FillProductModels();
            Models = ((IFillComboBox)this).FillComboBox<Model>("sp_Models_Products", 0,0,0 ,connectionString);
            Memory = ((IFillComboBox)this).FillComboBox<Memory>("sp_Memory",0,0,0, connectionString);
            Colors = ((IFillComboBox)this).FillComboBox<Color>("sp_Colors" ,0,0,0, connectionString);
            Providers = ((IFillComboBox)this).FillComboBox<Provider>("sp_Providers", 0,0,0, connectionString);
        }
        
        public void FillProductModels()
        {
            var count =1;
            try
            {
                connection = new SqlConnection(connectionString);
                command = new SqlCommand("sp_SearchProduct", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                adapter = new SqlDataAdapter(command);
                dataSet = new DataSet();
                connection.Open();
                    command.Parameters.AddWithValue("@Model",SelectedModel);
                command.Parameters.AddWithValue("@Color", SelectedColor);
                command.Parameters.AddWithValue("@SizeMemory", SelectedMemory);
                command.Parameters.AddWithValue("@Provider", SelectedProvider);
                command.Parameters.AddWithValue("@DateSupply", DateSupply);
                command.Parameters.AddWithValue("@PartNo", PartNo);
                adapter.Fill(dataSet);
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    ProductModels.Add(new ProductModel
                    {
                        NumberRow = count++,
                        Model = dr[0].ToString(),
                        SizeMemory = Convert.ToInt32(dr[1]),
                        Color = dr[2].ToString(),
                        QuantityInStock = Convert.ToInt32(dr[3].ToString()),
                        RetailPrice = Convert.ToDouble(dr[4].ToString()),
                        PartNo = dr[5].ToString(),
                        QuantityDelivered = Convert.ToInt32(dr[6].ToString()),
                        Price = Convert.ToDouble(dr[7].ToString()),
                        Cost = Convert.ToDouble(dr[8].ToString()),
                        AmountOfVAT = Convert.ToDouble(dr[9].ToString()),
                        CostWithVAT = Convert.ToDouble(dr[10].ToString()),
                        DateSupply = Convert.ToDateTime(dr[11]).ToShortDateString(),
                        ProviderName = Convert.ToString(dr[12].ToString()),
                        SupplyID = Convert.ToInt32(dr[13].ToString()),
                        ModelID = Convert.ToInt32(dr[14].ToString()),
                        MemoryID = Convert.ToInt32(dr[15].ToString()),
                        ColorID = Convert.ToInt32(dr[16].ToString()),
                        ProviderID = Convert.ToInt32(dr[17].ToString())
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
            ProductModels.Clear();
            FillProductModels();
        }

        #region Dialog
        public ICommand RunDialogAddProductCommand => new AnotherCommandImplementation(AddProductCommand);
        public ICommand RunDialogEditProductCommand => new AnotherCommandImplementation(EditProductCommand);

        private async void AddProductCommand(object o)
        {
            var view = new ProductDialog
            {
                DataContext = new ProductDialogViewModel()
            };
           
            var result = await DialogHost.Show(view, "RootDialog");
            if ((bool)result == true)
            {
                ProductModels.Clear();
                FillProductModels();
            }
        }

        private async void EditProductCommand(object o)
        {
            var view = new ProductDialog
            {
                DataContext = new ProductDialogViewModel(SelectedProductModel.ModelID, SelectedProductModel.ColorID, SelectedProductModel.MemoryID,  SelectedProductModel.ProviderID, SelectedProductModel.SupplyID , SelectedProductModel.PartNo, SelectedProductModel.QuantityDelivered, SelectedProductModel.Price, SelectedProductModel.RetailPrice)
            };

            var result = await DialogHost.Show(view, "RootDialog");
            if ((bool)result == true)
            {
                ProductModels.Clear();
                FillProductModels();
            }
        }


        #endregion

        #region Property
        public ObservableCollection<ProductModel> ProductModels { get; set; }

        public ProductModel SelectedProductModel { get; set; }

        public ObservableCollection<Model> Models { get; set; }

        public ObservableCollection<Provider> Providers { get; set; }

        public ObservableCollection<Color> Colors { get; set; }

        public ObservableCollection<Memory> Memory { get; set; }

        public int PartNoLength { get; set; }

        private string partNo = string.Empty;
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
        private DataTable productTable;
        public DataTable ProductTable
        {
            get => productTable;
            set
            {
                if (value == productTable) return;
                productTable = value;
                OnPropertyChanged(nameof(ProductTable));
            }
        }
        private string selectedModel = string.Empty;
        public string SelectedModel
        {
            get { return selectedModel; }
            set
            {
                if (selectedModel == value) return;
                selectedModel = value;
                OnPropertyChanged(nameof(SelectedModel));
            }
        }

        private string selectedColor = string.Empty;
        public string SelectedColor
        {
            get { return selectedColor; }
            set
            {
                if (selectedColor == value) return;
                selectedColor = value;
                OnPropertyChanged(nameof(SelectedColor));
            }
        }

        private string selectedProvider = string.Empty;
        public string SelectedProvider
        {
            get { return selectedProvider; }
            set
            {
                if (selectedProvider == value) return;
                selectedProvider = value;
                OnPropertyChanged(nameof(SelectedProvider));
            }
        }

        private string selectedMemory = string.Empty;
        public string SelectedMemory
        {
            get { return selectedMemory; }
            set
            {
                if (selectedMemory == value) return;
                selectedMemory = value;
                OnPropertyChanged(nameof(SelectedMemory));
            }
        }

        private string dateSupply = string.Empty;
        public string DateSupply
        {
            get { return dateSupply; }
            set
            {
                if (dateSupply == value) return;
                dateSupply = value;
                OnPropertyChanged(nameof(DateSupply));
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
        #endregion
    }
}

//private void UpdateProductModels(object sender, NotifyCollectionChangedEventArgs e)
//{
//    foreach (var x in e.NewItems)
//    {
//        // do something
//    }

//    foreach (var y in e.OldItems)
//    {
//        //do something
//    }
//    if (e.Action == NotifyCollectionChangedAction.Move)
//    {
//        //do something
//    }
//}

//var reader = command.ExecuteReader();
//while (reader.Read())
//{
//    Console.WriteLine(reader.GetString(0) + reader.GetInt32(1) + reader.GetInt32(2));
//productModels.Add(new ProductModel
//{

//    Model = reader.GetString(0),
//    SizeMemory = reader.GetInt32(1),
//    Color = reader.GetString(2),
//    QuantityInStock = reader.GetInt32(3),
//    RetailPrice = reader.GetDouble(4),
//    PartNo = reader.GetString(5),
//    QuantityDelivered = reader.GetInt32(6),
//    Price = reader.GetDouble(7),
//    Cost = reader.GetDouble(8),
//    АmountOfVAT = reader.GetDouble(9),
//    СostWithVAT = reader.GetDouble(10),
//    DateSupply = reader.GetDateTime(11),
//    ProviderName = reader.GetString(12),
//});

//reader.Close();



//dataSet = null;
//adapter.Dispose();
//connection.Close();
//connection.Dispose();
//finally
//{
//    dataSet = null;
//    adapter.Dispose();
//    connection.Close();
//    connection.Dispose();
//}


//private ObservableCollection<ProductModel> CreateData()
//{
//    return new ObservableCollection<ProductModel>
//    {
//        new ProductModel
//        {
//             Model = "12",
//                SizeMemory = 12,
//                Color = "124",
//                QuantityInStock = 421,
//                RetailPrice = 123.2,
//                PartNo = "1234567/9",
//                QuantityDelivered = 123,
//                Price = 3,
//                Cost = 43,
//                АmountOfVAT = 4,
//                СostWithVAT = 4,
//                DateSupply = Convert.ToDateTime( "02.03.2001"),
//                ProviderName = "1234f",
//        }
//    };

//}

//public void PreviewTextInpurt_PartNo(object obj, TextCompositionEventArgs e)
//{
//    e.Handled = true;
//    Console.WriteLine("X." + Regex.IsMatch(e.Text, @"^[0-9]{1,8}(?:\.)"));
//    Console.WriteLine("X.X" + Regex.IsMatch(e.Text, @"^[0-9]{1,8}(?:\.[0-9])"));
//    Console.WriteLine("X.XX" + Regex.IsMatch(e.Text, @"^[0-9]{1,8}(?:\.[0-9][0-9])"));

//    //if (Regex.IsMatch(e.Text, @"^[0-9]{1,8}(?:\.)") || Regex.IsMatch(e.Text, @"^[0-9]{1,8}(?:\.[0-9])") || Regex.IsMatch(e.Text, @"^[0-9]{1,8}(?:\.[0-9][0-9])"))


//    //    e.Handled = false;
//}


