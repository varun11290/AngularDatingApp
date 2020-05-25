using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Repo
{
    public interface IAuthRepo
    {
        Task<User> Register(User user,string password);

        Task<User> Login(string userName,string password);

        Task<bool> UserExist(string userName);
    }
}