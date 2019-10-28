using System;

namespace ControleDeLoja.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public short UserId { get; set; } // QUEM VENDEU
        public DateTime SaleDate { get; set; }
        public Decimal TotalPrice { get; set; }
        public string FormaPagamento { get; set; }
    }
}
