using Api.Models;
using DataAccess.Models;
using System.Collections.Generic;

namespace Api.IServicesApi
{
    public interface IAccountService
    {
        public string Login(UserLogin userLogin);
        public string Register(Users user);
        public List<UserSimpleDto> GetUsers();
    }
}
