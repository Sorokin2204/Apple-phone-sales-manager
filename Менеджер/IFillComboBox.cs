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
        public ObservableCollection<T> FillComboBox<T>(string commandText,  int modelID , int colorID , int memoryID,  string connectionString)
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



            if (typeof(T) == typeof(Model))
            {
                adapter.Fill(dataSet);
                if(commandText == "sp_Models_Products")
                {
                    (collection as ObservableCollection<Model>).Add(new Model()
                    {
                        ModelID = 0,
                        Title = ""
                    });
                }
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
                command.Parameters.AddWithValue("@ModelID", modelID);
                command.Parameters.AddWithValue("@ColorID", colorID);
                adapter.Fill(dataSet);
                if(modelID == 0)
                {
                    (collection as ObservableCollection<Memory>).Add(new Memory()
                    {
                        MemoryID = 0,
                        Size = ""
                    });
                }
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
                command.Parameters.AddWithValue("@ModelID", modelID);
                command.Parameters.AddWithValue("@MemoryID", memoryID);
                adapter.Fill(dataSet);
                if(modelID == 0)
                {
                    (collection as ObservableCollection<Color>).Add(new Color()
                    {
                        ColorID = 0,
                        Title = ""
                    });
                }
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    (collection as ObservableCollection<Color>).Add(new Color()
                    {
                        ColorID = Convert.ToInt32(dr[0].ToString()),
                        Title = dr[1].ToString()
                    });
                }
            }

            if (typeof(T) == typeof(Provider))
            {
                adapter.Fill(dataSet);
                (collection as ObservableCollection<Provider>).Add(new Provider()
                {
                    ProviderID = 0,
                    Title = ""
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

            dataSet = null;
            adapter.Dispose();
            connection.Close();
            connection.Dispose();

            return collection;
            
        }
    }
}
