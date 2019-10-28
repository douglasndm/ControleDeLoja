using ControleDeLoja.Models.Users;
using ControleDeLoja.Functions.Users;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace ControleDeLoja.Functions.Users
{
    public static class Administrators
    {
        public static Administrator GetAdministrator(short UserId)
        {
            Administrator administrator = null;
            SQLiteConnection connection = Database.ConnectionDB();

            try
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "SELECT * FROM Administrators WHERE User_Id = @id";
                command.Parameters.AddWithValue("@id", UserId);

                var result = command.ExecuteReader();

                if (result.StepCount > 0)
                {
                    result.Read();

                    User userTemp = Users.GetUser(UserId);

                    administrator = new Administrator()
                    {
                        UserId = userTemp.UserId,
                        Name = userTemp.Name,
                        LastName = userTemp.LastName,
                        Email = userTemp.Email,
                        UserName = userTemp.UserName,
                        Password = userTemp.Password,

                        // Administrator exclusive
                        Id = result.GetInt16(0),
                        AcessLevel = result.GetInt16(2)
                    };
                }
                else
                {
                    return null;
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

            return administrator;
        }

        public static List<Administrator> GetAdministrators()
        {
            List<Administrator> administrators = new List<Administrator>();

            SQLiteConnection connection = Database.ConnectionDB();

            try
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "SELECT * FROM Administrators";
                var result = command.ExecuteReader();

                if (result.StepCount <= 0)
                    return administrators;

                while(result.Read())
                {
                    var userTemp = Users.GetUser(Convert.ToInt16(result["User_Id"]));

                    administrators.Add(new Administrator()
                    {
                        Id = Convert.ToInt16(result["Id"]),
                        Name = userTemp.Name,
                        LastName = userTemp.LastName,
                        Email = userTemp.Email,
                        UserName = userTemp.UserName,
                        Password = userTemp.Password,

                        AcessLevel = Convert.ToInt16(result["AcessLevel"]),
                        UserId = Convert.ToInt16(result["User_Id"])
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
            return administrators;
        }

    }
}
