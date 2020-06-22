using AccountServiceApi.Models;
using DataAccess.Models;
using System.Collections.Generic;

namespace AccountServiceApi.IServicesApi
{
    public interface IAccountService
    {
        public string Login(UserLogin userLogin);
        public string Register(Users user);
        public List<UserSimpleDto> GetUsers();
    }
}
