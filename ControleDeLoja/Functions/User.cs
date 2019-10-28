using ControleDeLoja.Functions.Exceptions;
using ControleDeLoja.Models.Users;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;

namespace ControleDeLoja.Functions
{
    public class DeprecrachUser
    {
        /// <summary>
        /// Verifica se existe algum usuário com esse e-mail ou nome de usuário e retorna verdadeiro se encontrar.
        /// </summary>
        public static bool CheckIfUserExist(string emailOrUserName)
        {
            SQLiteConnection connection = Database.ConnectionDB();
            try
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = "SELECT * FROM Users WHERE lower(Email) = lower(@email) OR lower(UserName) = lower(@user)";
                command.Parameters.AddWithValue("@email", emailOrUserName);
                command.Parameters.AddWithValue("@user", emailOrUserName);

                SQLiteDataReader result = command.ExecuteReader();

                if (result.StepCount > 0)
                    return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.HResult);
            }  
            finally
            {
                connection.Close();
            }
            return false;
        }


        //public static IUser GetUser(string emailOrUserName)
        //{
        //    IUser user = null;

        //    User userTemp = GetUserGeneric(emailOrUserName);

        //    SQLiteConnection connection = Database.ConnectionDB();

        //    try
        //    {
        //        connection.Open();

        //        SQLiteCommand command = new SQLiteCommand(connection);
        //        command.CommandText = "SELECT * FROM Administrators WHERE User_Id = @id";
        //        command.Parameters.AddWithValue("@id", userTemp.User_Id);

        //        var result = command.ExecuteReader();

        //        if(result.StepCount > 0)
        //        {
        //            result.Read();

        //            user = new Administrator()
        //            {
        //                User_Id = userTemp.User_Id,
        //                Name = userTemp.Name,
        //                LastName = userTemp.LastName,
        //                Email = userTemp.Email,
        //                UserName = userTemp.UserName,
        //                Password = userTemp.Password,

        //                // Administrator exclusive
        //                Id = result.GetInt16(0),
        //                AcessLevel = result.GetInt16(2)
        //            };
        //        }
        //        else
        //        {
        //            SQLiteCommand command2 = new SQLiteCommand(connection);
        //            command.CommandText = "SELECT * FROM Operators WHERE User_Id = @id";
        //            command.Parameters.AddWithValue("@id", userTemp.User_Id);

        //            var result2 = command2.ExecuteReader();

        //            if(result2.StepCount > 1)
        //            {
        //                result2.Read();

        //                user = new Operator()
        //                {
        //                    User_Id = userTemp.User_Id,
        //                    Name = userTemp.Name,
        //                    LastName = userTemp.LastName,
        //                    Email = userTemp.Email,
        //                    UserName = userTemp.UserName,
        //                    Password = userTemp.Password,

        //                    // Operator exclusive
        //                    Id = result2.GetInt16(0)
        //                };
        //            }
        //            else
        //                throw new Exception("Não foi possível encontrar o tipo de usuário");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }

        //    return user;
        //}


        public static List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            SQLiteConnection connection1 = Database.ConnectionDB();
            SQLiteConnection connection2 = Database.ConnectionDB();
            SQLiteConnection connection3 = Database.ConnectionDB();

            try
            {
                connection1.Open();

                SQLiteCommand command = new SQLiteCommand(connection1);
                command.CommandText = "SELECT * FROM Users";
                var result = command.ExecuteReader();

                while(result.Read())
                {
                    connection2.Open();

                    SQLiteCommand cmd = new SQLiteCommand(connection2);
                    cmd.CommandText = "SELECT * FROM Administrators WHERE User_Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", result["Id"]);
                    var result2 = cmd.ExecuteReader();

                    if (result2.StepCount > 0)
                    {
                        result2.Read();

                        users.Add(new Administrator()
                        {
                            Id = Convert.ToInt16(result["Id"]),
                            Name = result["Name"].ToString(),
                            LastName = result["LastName"].ToString(),
                            Email = result["Email"].ToString(),
                            UserName = result["UserName"].ToString(),
                            Password = result["Password"].ToString(),

                            AcessLevel = Convert.ToInt16(result2["AcessLevel"]),
                            UserId = Convert.ToInt16(result2["User_Id"])
                        });

                        connection2.Close();
                    }
                    else
                    {
                        connection3.Open();

                        SQLiteCommand cmd2 = new SQLiteCommand(connection3);
                        cmd2.CommandText = "SELECT * FROM Operators WHERE User_Id = @Id";
                        cmd2.Parameters.AddWithValue("@Id", result["Id"]);
                        var result3 = cmd2.ExecuteReader();

                        if(result3.StepCount > 0)
                        {
                            result3.Read();

                            users.Add(new Operator()
                            {
                                Id = Convert.ToInt16(result["Id"]),
                                Name = result["Name"].ToString(),
                                LastName = result["LastName"].ToString(),
                                Email = result["Email"].ToString(),
                                UserName = result["UserName"].ToString(),
                                Password = result["Password"].ToString(),

                                UserId = Convert.ToInt16(result3["User_Id"])
                            });
                        }

                        connection3.Close();
                    }
                }                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection1.Close();
                connection2.Close();
                connection3.Close();
            }

            return users;
        }

        //public static List<Operator> GetOperators()
        //{
        //    List<Operator> operators = new List<Operator>();

        //    SQLiteConnection connection = Database.ConnectionDB();

        //    try
        //    {
        //        connection.Open();

        //        SQLiteCommand command = new SQLiteCommand(connection);
        //        command.CommandText = "SELECT * FROM Operators";
        //        var result = command.ExecuteReader();

        //        while(result.Read())
        //        {
        //            Operator operatorTemp = GetUserGeneric(result["Email"].ToString()) as Operator;

        //            operators.Add(new Operator()
        //            {
        //                Id = Convert.ToInt16(result["Id"]),
        //                Name = operatorTemp.Name,
        //                LastName = operatorTemp.LastName,
        //                Email = operatorTemp.Email,
        //                UserName = operatorTemp.UserName,
        //                Password = operatorTemp.Password,

        //                User_Id = Convert.ToInt16(result["User_Id"])
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }

        //    return operators;
        //}

    }

}
