using ControleDeLoja.Functions.Exceptions;
using ControleDeLoja.Models.Users;

namespace ControleDeLoja.Functions
{
    public class Login
    {
        public static User Logon(string emailOrUserName, string password)
        {
            User user = Users.Users.GetUser(emailOrUserName);

            if (CheckPassword(password, user))
                return user;
            else
                throw new WrongPasswordException("Senha incorreta");
        }

        private static bool CheckPassword(string password, User user)
        {
            if (user.Password == MD5Hash.Hash(password))
                return true;
            else
                return false;
        }
    }
}
