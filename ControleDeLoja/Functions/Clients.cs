using ControleDeLoja.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;

namespace ControleDeLoja.Functions
{
    public class Clients
    {
        public static List<Client> GetAllClients()
        {
            List<Client> clients = new List<Client>();

            SQLiteConnection connection = Database.ConnectionDB();

            try
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "SELECT * FROM Clients";
                var result = command.ExecuteReader();

                while (result.Read())
                {
                    clients.Add(new Client(
                        VerificarSeDadosSaoValidos(result["Id"]) == true ? Convert.ToInt32(result["Id"]) : 0,

                        VerificarSeDadosSaoValidos(result["Name"]) ? result["Name"].ToString() : "",
                        VerificarSeDadosSaoValidos(result["LastName"]) ? result["LastName"].ToString() : "",
                        VerificarSeDadosSaoValidos(result["CPF"]) ? result["CPF"].ToString() : "",
                        VerificarSeDadosSaoValidos(result["RG"]) ? result["RG"].ToString() : "",
                        VerificarSeDadosSaoValidos(result["Phone"]) ? result["Phone"].ToString() : "",
                        VerificarSeDadosSaoValidos(result["Andress"]) ? result["Andress"].ToString() : "",

                        VerificarSeDadosSaoValidos(result["Renda"]) ? Convert.ToDecimal(result["Renda"], CultureInfo.CurrentCulture) : 0,
                        VerificarSeDadosSaoValidos(result["AvailableLimit"]) ? Convert.ToDecimal(result["AvailableLimit"], CultureInfo.CurrentCulture) : 0,

                        (bool)result["Blocked"] ? true : false                        
                    ));
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

            return clients;
        }

        public static Client GetClient(string clientCPF)
        {
            Client client = null;
            SQLiteConnection connection = Database.ConnectionDB();

            try
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(connection);

                command.CommandText = "SELECT * FROM Clients WHERE CPF = @cpf";
                command.Parameters.AddWithValue("@cpf", clientCPF);

                var result = command.ExecuteReader();

                while (result.Read())
                {
                    client = new Client(
                        VerificarSeDadosSaoValidos(result["Id"]) == true ? Convert.ToInt32(result["Id"]) : 0,

                        VerificarSeDadosSaoValidos(result["Name"]) ? result["Name"].ToString() : "",
                        VerificarSeDadosSaoValidos(result["LastName"]) ? result["LastName"].ToString() : "",
                        VerificarSeDadosSaoValidos(result["CPF"]) ? result["CPF"].ToString() : "",
                        VerificarSeDadosSaoValidos(result["RG"]) ? result["RG"].ToString() : "",
                        VerificarSeDadosSaoValidos(result["Phone"]) ? result["Phone"].ToString() : "",
                        VerificarSeDadosSaoValidos(result["Andress"]) ? result["Andress"].ToString() : "",

                        VerificarSeDadosSaoValidos(result["Renda"]) ? Convert.ToDecimal(result["Renda"], CultureInfo.CurrentCulture) : 0,
                        VerificarSeDadosSaoValidos(result["AvailableLimit"]) ? Convert.ToDecimal(result["AvailableLimit"], CultureInfo.CurrentCulture) : 0,

                        (bool)result["Blocked"] ? true : false);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return client;
        }

        public static void UpdateClientLimit(Client client, Decimal newLimite)
        {
            SQLiteConnection connection = Database.ConnectionDB();

            try
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "UPDATE Clients SET AvailableLimit = @limit WHERE Id = @id";
                command.Parameters.AddWithValue("@limit", newLimite);
                command.Parameters.AddWithValue("@id", client.Id);
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

        private static bool VerificarSeDadosSaoValidos(Object obj)
        {
            if (obj.GetType() != typeof(DBNull) && obj.ToString() != null)
                return true;

            return false;            
        }
    }
}
