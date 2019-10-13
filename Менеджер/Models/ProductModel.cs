using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Менеджер
{
    public class ProductModel : ViewModelsBase 
    {
        public int NumberRow { get; set; }
        public string Model { get; set; }
        public int SizeMemory { get; set; }
        public string Color { get; set; }
        public int QuantityInStock { get; set; }
        public double RetailPrice { get; set; }
        public string PartNo { get; set; }
        public int QuantityDelivered { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }
        public double AmountOfVAT { get; set; }
        public double CostWithVAT { get; set; }
        public string DateSupply { get; set; }
        public string ProviderName { get; set; }
    }
}
