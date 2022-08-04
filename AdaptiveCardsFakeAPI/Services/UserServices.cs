using AdaptiveCardsFakeAPI.Data;
using AdaptiveCardsFakeAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaptiveCardsFakeAPI.Services
{
    public interface IUserServices
    {
        public IList<User> getUsers();
        public User getUserByUserName(string userName);
    }
    public class UserServices : IUserServices
    {
        #region variables and constructor
        private readonly AppDBContext _context;
        public UserServices(AppDBContext context)
        {
            _context = context;
        }
        #endregion

        #region GetUsers
        public IList<User> getUsers()
        {
            return  _context.User.Where(u=>u.selected == false).ToList();
        }
        #endregion

        #region GetUserByUserName
        public User getUserByUserName(string userName)
        {
            return _context.User.Where(u => u.userName == userName).FirstOrDefault();
        }
        #endregion
    }
}
