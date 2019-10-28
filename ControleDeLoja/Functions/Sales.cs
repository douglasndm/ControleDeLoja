using ControleDeLoja.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace ControleDeLoja.Functions
{
    public class Sales
    {
        public static List<Sale> GetAllSales()
        {
            List<Sale> sales = new List<Sale>();

            SQLiteConnection connection = Database.ConnectionDB();
            try
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "SELECT * FROM Sales";

                var result = command.ExecuteReader();

                while (result.Read())
                {
                    short idTemp = result.GetInt16(0);
                    int ClientIdTemp = Convert.ToInt32(result["ClientId"]);
                    short UserIdTemp = result["UserId"].GetType() != typeof(DBNull) ? Convert.ToInt16(result["UserId"]) : Convert.ToInt16(0);
                    DateTime dateTimeTemp = result["SaleDate"].GetType() != typeof(DBNull) ? DateTime.Parse(result["SaleDate"].ToString()) : DateTime.MinValue;
                    Decimal totalPriceDecimal = result["TotalPrice"].GetType() != typeof(DBNull) ? Convert.ToDecimal(result["TotalPrice"]) : 0;
                    string formaDePagamento = result["FormaPagamento"].GetType() != typeof(DBNull) ? result["FormaPagamento"].ToString() : "";

                    sales.Add(new Sale()
                    {
                        Id = idTemp,
                        ClientId = ClientIdTemp,
                        UserId = UserIdTemp,
                        SaleDate = dateTimeTemp,
                        TotalPrice = totalPriceDecimal,
                        FormaPagamento = formaDePagamento
                    });
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return sales;
        }

        public static List<Sale> GetAllSalesByDate(DateTime from, DateTime to)
        {
            List<Sale> sales = new List<Sale>();

            foreach (var item in GetAllSales())
            {
                if(item.SaleDate > from && item.SaleDate < to)
                {
                    sales.Add(item);
                }
            }

            return sales;
        }

        public static List<Sale> GetAllSalesByAClient(short clientId)
        {
            List<Sale> sales = new List<Sale>();

            SQLiteConnection connection = Database.ConnectionDB();
            try
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "SELECT * FROM Sales WHERE ClientId = @id";
                command.Parameters.AddWithValue("@id", clientId);

                var result = command.ExecuteReader();

                while(result.Read())
                {
                    short idTemp = result.GetInt16(0);
                    short UserIdTemp = result["UserId"].GetType() != typeof(DBNull) ? Convert.ToInt16(result["UserId"]) : Convert.ToInt16(0);
                    DateTime dateTimeTemp = result["SaleDate"].GetType() != typeof(DBNull) ? DateTime.Parse(result["SaleDate"].ToString()) : DateTime.MinValue;
                    Decimal totalPriceDecimal = result["TotalPrice"].GetType() != typeof(DBNull) ? Convert.ToDecimal(result["TotalPrice"]) : 0;
                    string formaDePagamento = result["FormaPagamento"].GetType() != typeof(DBNull) ? result["FormaPagamento"].ToString() : "";

                    sales.Add(new Sale()
                    {
                        Id = idTemp,
                        ClientId = clientId,
                        UserId = UserIdTemp,
                        SaleDate = dateTimeTemp,
                        TotalPrice = totalPriceDecimal,
                        FormaPagamento = formaDePagamento
                    });
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return sales;
        }

        public static void MakeASale(Sale sale)
        {
            SQLiteConnection connection = Database.ConnectionDB();

            try
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "INSERT INTO Sales (ClientId, SaleDate, TotalPrice, FormaPagamento, UserId) VALUES (@clientId, @saleDate, @totalPrice, @formaDePagamento, @userId)";
                command.Parameters.AddWithValue("@clientId", sale.ClientId);
                command.Parameters.AddWithValue("@saleDate", sale.SaleDate);
                command.Parameters.AddWithValue("@totalPrice", sale.TotalPrice);
                command.Parameters.AddWithValue("@formaDePagamento", sale.FormaPagamento);
                command.Parameters.AddWithValue("@userId", sale.UserId);

                command.ExecuteNonQuery();

                //ESSE É UM COMANDO AUXILIAR QUE PEGAR O ID DA SALE QUE ACABOU DE SER REGISTRADA NO BANCO DE DADOS E ADICIONA A REFERENCIA DO OBJETO SALE
                //PARA DEIXAR O OBJETO COMPLETO E DEIXANDO POSSIVEL O USO DELE POR DEMAIS CLASSES
                // A classe SQLiteConnection tem a propriedade LastInsertRowId que retorna a ultima id, auto increment que foi registrada no banco usando essa instancia de conexão
                sale.Id = Convert.ToInt32(connection.LastInsertRowId);
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
