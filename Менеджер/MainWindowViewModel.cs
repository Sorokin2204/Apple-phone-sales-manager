using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Менеджер
{
    public enum TabControl
    {
        Product,
        Order,
        Provider
    }

    public class MainWindowViewModel : ViewModelsBase
    {
        private ProductView product;
        private ProviderView provider;
        private OrderView order;

        public MainWindowViewModel()
        {
            product = new ProductView();
            provider = new ProviderView();
            order = new OrderView();
            TabControl = TabControl.Product;
        }

        #region Property
        private object tablePanel;
        public object TablePanel
        {
            get
            {
                return tablePanel;
            }
            set
            {
                tablePanel = value;
                OnPropertyChanged(nameof(TablePanel));
            }
        }

        private TabControl tabControl; 
        public TabControl TabControl
        {
            get { return tabControl; }
            set
            {
                tabControl = value;
                SwitchControl(tabControl);
                OnPropertyChanged("TabControl");
                OnPropertyChanged("ProductSwitch");
                OnPropertyChanged("OrderSwitch");
                OnPropertyChanged("ProviderSwitch");
            }
        }

        public bool ProductSwitch
        {
            get { return TabControl == TabControl.Product; }
            set { TabControl = value ? TabControl.Product : TabControl; }
        }

        public bool OrderSwitch
        {
            get { return TabControl == TabControl.Order; }
            set { TabControl = value ? TabControl.Order : TabControl; }
        }

        public bool ProviderSwitch
        {
            get { return TabControl == TabControl.Provider; }
            set { TabControl = value ? TabControl.Provider : TabControl; }
        }
        #endregion

        #region Method
        void SwitchControl (TabControl tabControl)
        {
            switch (tabControl)
            {
                case TabControl.Product:
                   
                    TablePanel = product;
                   break;
                case TabControl.Order:
               
                    TablePanel = order;
                    break;
                case TabControl.Provider:
            
                    TablePanel = provider;
                    break;
            }
        }
        #endregion
    }
}
