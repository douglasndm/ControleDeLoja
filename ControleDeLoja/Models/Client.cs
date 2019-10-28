using System.Collections.ObjectModel;

namespace ControleDeLoja.Models
{
    public class Client
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string LastName { get; private set; }
        public string CPF { get; private set; }
        public string RG { get; private set; }
        public string Phone { get; private set; }
        public string Andress { get; private set; }
        public decimal Renda { get; set; }
        public decimal Limit { get; private set; }
        public bool Blocked { get; private set; }

        public Client(int Id, string Name, string LastName, string CPF, string RG, string Phone, string Andress, decimal Renda, decimal Limit, bool Blocked)
        {
            this.Id = Id;
            this.Name = Name;
            this.LastName = LastName;
            this.CPF = CPF;
            this.RG = RG;
            this.Phone = Phone;
            this.Andress = Andress;
            this.Renda = Renda;
            this.Limit = Limit;
            this.Blocked = Blocked;
        }
    }
}
