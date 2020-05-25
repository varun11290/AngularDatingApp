using System;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

//We register this service in StartUp.cs file
namespace DatingApp.API.Repo
{
    public class AuthRepo : IAuthRepo
    {
        private DataContext _context;

        public AuthRepo(DataContext context)
        {
            _context = context;
        }
        public async Task<User> Login(string userName, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == userName);

            if (user == null)
                return null;
            //Here we check if the password is matching or not by gnrating the hashcode with the store key and then compare it
            if (!VarifyHashPassword(password, user.Password, user.PasswordKey))
                return null;

            return user;
        }

        private bool VarifyHashPassword(string password, byte[] userPasswordHash, byte[] userPasswordKey)
        {
            using (var hashMap = new System.Security.Cryptography.HMACSHA512(userPasswordKey))
            {
                var computedPasswordHash = hashMap.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedPasswordHash.Length; i++)
                {
                    if (userPasswordHash[i] != computedPasswordHash[i])
                        return false;
                }
            }
            return true;

        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordKey;
            //This method will generate the hash key and hash value for password
            CreatePasswordHash(password, out passwordHash, out passwordKey);

            user.Password = passwordHash;
            user.PasswordKey = passwordKey;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordKey)
        {
            //This method is used to gnerate the has password and hash key
            //hash key is randomly generated key which will be used to varify the user aginst the password hash
            //We used using becuse HMACSHA512 has implemented the IDisposed interface and implemented the finalise mathed to dispose the 
            //object
            using (var hashMap = new System.Security.Cryptography.HMACSHA512())
            {
                passwordKey = hashMap.Key;
                passwordHash = hashMap.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExist(string userName)
        {
            if (await _context.Users.AnyAsync(u => u.Name == userName))
                return true;

            return false;
        }
    }
}