using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace Менеджер
{
    interface IFillComboBox
    {
        public ObservableCollection<T> FillComboBox<T>(string commandText, string connectionString)
        {
            ObservableCollection<T> collection = new ObservableCollection<T>();
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(commandText, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet dataSet = new DataSet();
            connection.Open();
            adapter.Fill(dataSet);

            if (typeof(T) == typeof(Provider))
            {
                (collection as ObservableCollection<Provider>).Add(new Provider()
                {
                    ProviderID = 0,
                    Title = null
                });
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    (collection as ObservableCollection<Provider>).Add(new Provider()
                    {
                        ProviderID = Convert.ToInt32(dr[0].ToString()),
                        Title = dr[1].ToString()
                    });
                }
            }

            if (typeof(T) == typeof(Model))
            {
                (collection as ObservableCollection<Model>).Add(new Model()
                {
                    ModelID = 0,
                    Title = null
                });
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    (collection as ObservableCollection<Model>).Add(new Model()
                    {
                        ModelID = Convert.ToInt32(dr[0].ToString()),
                        Title = dr[1].ToString()
                    });
                }
            }

            if (typeof(T) == typeof(Memory))
            {
                (collection as ObservableCollection<Memory>).Add(new Memory()
                {
                    MemoryID = 0,
                    Size = null
                });
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    (collection as ObservableCollection<Memory>).Add(new Memory()
                    {
                        MemoryID = Convert.ToInt32(dr[0].ToString()),
                        Size = Convert.ToString(dr[1])
                    });
                }
            }

            if (typeof(T) == typeof(Color))
            {
                (collection as ObservableCollection<Color>).Add(new Color()
                {
                    ColorID = 0,
                    Title = null
                });
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    (collection as ObservableCollection<Color>).Add(new Color()
                    {
                        ColorID = Convert.ToInt32(dr[0].ToString()),
                        Title = dr[1].ToString()
                    });
                }
            }

            dataSet = null;
            adapter.Dispose();
            connection.Close();
            connection.Dispose();

            return collection;
            
        }
    }
}
