using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Manager.Domains.Guilda_e_User
{
    public interface IUserDAL
    {

        public Task<List<string>> GetAllUsers();

        public Task<bool> AddNewUser(ulong UserId);

    }
}
