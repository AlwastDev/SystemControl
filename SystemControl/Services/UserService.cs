using SystemControl.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SystemControl.Services
{
    static class UserService
    {
        public static UserModel AuthUser { get; private set; }
        public static UserModel Registration(string name, string login, string password)
        {
            return Registration(new UserModel()
            {
                Login = login,
                Password = password,
                Name = name
            });
        }
        public static UserModel Registration(UserModel userModel)
        {
            if (!LoginCheckUniqueness(userModel.Login))
                throw new Exception("This login is already in use!");
            userModel.Id = DateTime.Now.Ticks;
            userModel.Password = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(userModel.Password)));
            XmlService.AddUser(userModel);
            AuthUser = userModel;
            return userModel;
        }

        public static UserModel Authentication(string login, string password)
        {
            return Authentication(new UserModel()
            {
                Login = login,
                Password = password
            });
        }
        public static UserModel Authentication(UserModel userModel)
        {
            userModel.Password = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(userModel.Password)));
            var user = XmlService.GetUserList().FirstOrDefault(p => p.Login == userModel.Login && p.Password == userModel.Password);
            if (user == null)
                throw new Exception("Wrong login or password!");
            AuthUser = user;
            return user;
        }

        public static bool LoginCheckUniqueness(string login)
        {
            return !XmlService.GetUserList().Where(p => p.Login == login).Any();
        }

        public static void SetPassword(string password)
        {
            password = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));
            AuthUser.Password = password;
            XmlService.SetUser(AuthUser);
        }

        public static bool CkeckPassword(string password)
        {
            return Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password))) == AuthUser.Password;
        }
    }
}
