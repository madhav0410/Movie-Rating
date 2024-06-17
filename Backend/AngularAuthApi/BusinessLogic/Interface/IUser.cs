using DataAceess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public interface IUser
    {
        public Task UpdateUserRating(string email, string title,  int rating);
        public Task<Rating> GetUserRating(string email, string title);
    }
}
