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
        public void UpdateUserRating(string email, string title,  int rating);
        public Rating GetUserRating(string email, string title);
    }
}
