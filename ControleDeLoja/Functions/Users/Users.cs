using ControleDeLoja.Models.Users;
using System;
using System.Data.SQLite;

namespace ControleDeLoja.Functions.Users
{
    public static class Users
    {
        public static User GetUser(short Id)
        {
            User user = null;

            SQLiteConnection connection = Database.ConnectionDB();

            try
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "SELECT * FROM Users WHERE Id = @id";
                command.Parameters.AddWithValue("@id", Id);
                var result = command.ExecuteReader();

                if (result.StepCount > 0)
                {
                    result.Read();

                    Administrator admin = null;

                    try
                    {
                        admin = Administrators.GetAdministrator(Convert.ToInt16(result["Id"]));
                    }
                    catch (Exception)
                    {
                    }


                    if(admin != null)
                    {
                        user = new Administrator()
                        {
                            UserId = Convert.ToInt16(result["Id"]),
                            Name = result["Name"].ToString(),
                            LastName = result["LastName"].ToString(),
                            UserName = result["Email"].ToString(),
                            Email = result["UserName"].ToString(),
                            Password = result["Password"].ToString(),
                        };
                    }
                    else
                    {
                        user = new Operator()
                        {
                            UserId = Convert.ToInt16(result["Id"]),
                            Name = result["Name"].ToString(),
                            LastName = result["LastName"].ToString(),
                            UserName = result["Email"].ToString(),
                            Email = result["UserName"].ToString(),
                            Password = result["Password"].ToString(),
                        };
                    }
                    
                }
                else
                    throw new Exception("Nenhum usuário encontrado com esse ID");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return user;
        }

        public static User GetUser(string EmailOrUserName)
        {
            User user = null;

            SQLiteConnection connection = Database.ConnectionDB();

            try
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(connection);

                command.CommandText = "SELECT * FROM Users WHERE lower(Email) = lower(@email) OR lower(UserName) = lower(@user)";
                command.Parameters.AddWithValue("@email", EmailOrUserName);
                command.Parameters.AddWithValue("@user", EmailOrUserName);

                var result = command.ExecuteReader();

                if (result.StepCount > 0)
                {
                    result.Read();

                    Administrator admin = null;

                    try
                    {
                        admin = Administrators.GetAdministrator(Convert.ToInt16(result["Id"]));
                    }
                    catch (Exception)
                    {
                    }


                    if (admin != null)
                    {
                        user = new Administrator()
                        {
                            UserId = Convert.ToInt16(result["Id"]),
                            Name = result["Name"].ToString(),
                            LastName = result["LastName"].ToString(),
                            UserName = result["Email"].ToString(),
                            Email = result["UserName"].ToString(),
                            Password = result["Password"].ToString(),
                        };
                    }
                    else
                    {
                        user = new Operator()
                        {
                            UserId = Convert.ToInt16(result["Id"]),
                            Name = result["Name"].ToString(),
                            LastName = result["LastName"].ToString(),
                            UserName = result["Email"].ToString(),
                            Email = result["UserName"].ToString(),
                            Password = result["Password"].ToString(),
                        };
                    }
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

            return user;
        }
    }
}
