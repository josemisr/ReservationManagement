using Api.Models;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.IServicesApi
{
    public interface IAccountService
    {
        public string Login(UserLogin userLogin);
        public string Register(Users user);
        public List<Users> GetUsers();
    }
}
