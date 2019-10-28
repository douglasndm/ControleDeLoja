using ControleDeLoja.Models;
using ControleDeLoja.Functions;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace ControleDeLoja.Classes
{
    public static class Products
    {
        public static List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            SQLiteConnection connection = Database.ConnectionDB();

            try
            {
                connection.Open();

                SQLiteCommand sqLiteCommand = new SQLiteCommand(connection);
                sqLiteCommand.CommandText = "SELECT * FROM Products";

                SQLiteDataReader result = sqLiteCommand.ExecuteReader();

                while (result.Read())
                {
                    String ProductName = null, ProductDescription = null, ProductEAN = null;
                    short ProductQnt = 0;
                    decimal ProductPrice = 0; ;

                    if (result["Name"].ToString() != "" && result["Name"].GetType() != typeof(DBNull))
                        ProductName = result["Name"].ToString();

                    if (result["EAN"].ToString() != "" && result["EAN"].GetType() != typeof(DBNull))
                        ProductEAN = result["EAN"].ToString();

                    if (result["Description"].ToString() != "" && result["Description"].GetType() != typeof(DBNull))
                        ProductDescription = result["Description"].ToString();

                    if (result["Qnt"].ToString() != "" && result["Qnt"].GetType() != typeof(DBNull))
                        ProductQnt = Convert.ToInt16(result["Qnt"]);

                    if (result["Price"].ToString() != "" && result["Price"].GetType() != typeof(DBNull))
                        ProductPrice = Convert.ToDecimal(result["Price"], CultureInfo.CurrentCulture);

                    products.Add(new Product()
                    {
                        Id = Convert.ToInt16(result["Id"]),
                        EAN = ProductEAN,
                        Name = ProductName,
                        Description = ProductDescription,
                        Count = ProductQnt,
                        Price = ProductPrice
                    });
                }
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return products;
        }

        public static Product GetProduct(short productId)
        {
            Product product = null;

            SQLiteConnection connection = Database.ConnectionDB();

            try
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "SELECT * FROM Products WHERE Id = @id";
                command.Parameters.AddWithValue("@id", productId);

                SQLiteDataReader result = command.ExecuteReader();
                result.Read();

                decimal totalPrice = ((result["Price"].GetType() != typeof(DBNull) ? Convert.ToDecimal(result["Price"]) : 0) * (result["Qnt"].GetType() != typeof(DBNull) ? Convert.ToInt16(result["Qnt"]) : Convert.ToInt16(0)));

                product = new Product()
                {
                    Id = productId,
                    EAN = result["EAN"].GetType() != typeof(DBNull) ? result["EAN"].ToString() : "",
                    Name = result["Name"].GetType() != typeof(DBNull) ? result["Name"].ToString() : "",
                    Description = result["Description"].GetType() != typeof(DBNull) ? result["Description"].ToString() : "",
                    Count = result["Qnt"].GetType() != typeof(DBNull) ? Convert.ToInt16(result["Qnt"]) : Convert.ToInt16(0),
                    Price = result["Price"].GetType() != typeof(DBNull) ? Convert.ToDecimal(result["Price"]) : 0,
                    TotalPrice = Convert.ToDecimal(totalPrice.ToString("F2"))
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return product;
        }

        /// <summary>
        /// ATENÇÃO: PARA ESSE METODO FUNCIONAR CORRETAMENTE É NECESSARIO UMA INSTANCIA DO PRODUTO COM JÁ OS DADOS DA VENDA.
        /// POR EXEMPLO: SE O PRODUTO VENDEU 10 PEÇAS O COUNT DO PRODUTO DEVE TER 10 E NÃO A QUANTIDADE DO BANCO DE DADOS
        /// </summary>
        /// <param name="product"></param>
        /// <param name="sale"></param>
        public static void SellProduct(Product product, Sale sale)
        {
            SQLiteConnection connection = Database.ConnectionDB();

            try
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "INSERT INTO Products_Sold (Product_Id, Product_Count, Product_Price, Sale_Id) VALUES (@productId, @productCount, @productPrice, @saleId)";
                command.Parameters.AddWithValue("@productId", product.Id);
                command.Parameters.AddWithValue("@productCount", product.Count);
                command.Parameters.AddWithValue("@productPrice", product.Price);
                command.Parameters.AddWithValue("@saleId", sale.Id);

                command.ExecuteNonQuery();

                // O SEGUNDO PARAMETRO PEGA A QUANTIDADE DO PRODUTO DISPONIVEL NO BANCO DE DADOS E SUBTRAI COM A NOVA QUANTIDADE
                // DE PRODUTO VENDIDO
                AlterarQuantidadeEstoque(GetProduct(product.Id), Convert.ToInt16(GetProduct(product.Id).Count - product.Count));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private static void AlterarQuantidadeEstoque(Product product, short NovaQuantidade)
        {
            if ((product.Count - NovaQuantidade) < 0)
                throw new Exception("Estoque vai ficar negativo se reduzir nesta quantidade");

            SQLiteConnection connection = Database.ConnectionDB();

            try
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "UPDATE Products SET Qnt = @qnt WHERE Id = @id";
                command.Parameters.AddWithValue("@qnt", NovaQuantidade);
                command.Parameters.AddWithValue("@id", product.Id);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

    }
}
