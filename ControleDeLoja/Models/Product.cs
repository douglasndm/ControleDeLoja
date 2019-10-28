using System;
using System.ComponentModel;

namespace ControleDeLoja.Models
{
    public class Product : ICloneable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private int count;
        private decimal totalPrice;

        public short Id { get; set; }
        public string EAN { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int Count
        {
            get
            {
                return this.count;
            }
            set
            {
                if (this.count != value)
                {
                    this.count = value;
                    OnPropertyChanged("Count");
                }
            }
        }

        public decimal TotalPrice
        {
            get { return totalPrice; }
            set
            {
                if (totalPrice != value)
                    totalPrice = value;
                OnPropertyChanged("TotalPrice");
            }
        }

        // CLONA A CLASSE PARA UM NOVA INSTANCIA
        // UTILIZADO NA PARTE DO CAIXA QUE É NECESSÁRIO TRANSFERIR UM PRODUTO DE UMA LISTA DE PRODUTOS DISPONIVEIS PARA A LISTA DE COMPRAS E MESMO ASSIM MANTER AS DUAS INSTANCIAS SEPARADAS
        public object Clone()
        {
            return new Product()
            {
                Id = this.Id,
                EAN = this.EAN,
                Name = this.Name,
                Description = this.Description,
                Price = this.Price,
                Count = this.Count,
            };
        }
    }
}
