using System;

namespace ControleDeLoja.Models
{
    // ESSE CLASSE É O MODELO DO BANCO DE DADOS QUE SERVE PARA
    // GUARDA CADA ITEM DE UMA COMPRA DE FORMA EXCLUSIVA
    // COM SEU PREÇO NO ATO DA COMPRA E A QUANTIDADE QUE FOI COMPRADA
    // ALEM DE TER UMA REFERENCIA PARA A VENDA QUE ESSE PRODUTO FOI COMPRADO
    class ProductSold
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductCount { get; set; }
        public Decimal ProductPrice { get; set; }
        public Sale Sale { get; set; }
    }
}
