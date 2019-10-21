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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Менеджер
{
    class OrderDialogViewModel : ViewModelsBase, IPhoneMask , IFillComboBox
    {
        public readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter;

        public OrderDialogViewModel()
        {
            Models = ((IFillComboBox)this).FillComboBox<Model>("sp_Models_Orders", 0,0,0 ,connectionString);
        }

        public void Click_AddButton(object sender, RoutedEventArgs e)
        {
            AddOrder();
        }

        public void AddOrder()
        {
            try
            {
                connection = new SqlConnection(connectionString);
                command = new SqlCommand("sp_AddOrder", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                adapter = new SqlDataAdapter(command);
                command.Parameters.AddWithValue("@ModelID", SelectedModelID);
                command.Parameters.AddWithValue("@ColorID", SelectedColorID);
                command.Parameters.AddWithValue("@MemoryID", SelectedMemoryID);
                command.Parameters.AddWithValue("@Name", Name);
                command.Parameters.AddWithValue("@Telephone", Phone);
                command.Parameters.AddWithValue("@Address", Address);
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

        private ObservableCollection<Color> сolors;
        public ObservableCollection<Color> Colors
        {
            get { return сolors; }
            set
            {
                if (сolors == value) return;
                сolors = value;
                OnPropertyChanged(nameof(Colors));
            }
        }

        private ObservableCollection<Memory> memory;
        public ObservableCollection<Memory> Memory
        {
            get { return memory; }
            set
            {
                if (memory == value) return;
                memory = value;
                OnPropertyChanged(nameof(Memory));
            }
        }


        public int selectedModelID;
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

        private bool isEnabledColor = false;
        public bool IsEnabledColor
        {
            get { return isEnabledColor; }
            set
            {
                if (isEnabledColor == value) return;
                isEnabledColor = value;
                OnPropertyChanged(nameof(IsEnabledColor));
            }
        }

        private int selectedMemoryID;
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

        private bool isEnabledMemory = false;
        public bool IsEnabledMemory
        {
            get { return isEnabledMemory; }
            set
            {
                if (isEnabledMemory == value) return;
                isEnabledMemory = value;
                OnPropertyChanged(nameof(IsEnabledMemory));
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

        private string name;
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

        private string address;
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


        #endregion

        #region Method

        public void SelectionChanged_ComboBoxModel(object sender, SelectionChangedEventArgs e)
        {
            SelectedColorID = 0;
            SelectedMemoryID = 0;
            Colors = null;
            Memory = null;
            IsEnabledColor = true;
            IsEnabledMemory = false;
            Colors = ((IFillComboBox)this).FillComboBox<Color>("sp_Colors", SelectedModelID, SelectedColorID, SelectedMemoryID, connectionString);
        }

        public void SelectionChanged_ComboBoxColor(object sender, SelectionChangedEventArgs e)
        {
            SelectedMemoryID = 0;
            Memory = null;
            IsEnabledMemory = true;
            Memory = ((IFillComboBox)this).FillComboBox<Memory>("sp_Memory", SelectedModelID, SelectedColorID, SelectedMemoryID, connectionString);
        }

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
        public void PreviewTextInput_Name(object obj, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^А-Яа-я]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion
    }
}