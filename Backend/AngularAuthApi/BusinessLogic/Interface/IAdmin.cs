using DataAceess.Dto;
using DataAceess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public interface IAdmin
    {
        public Task<bool> AddMovie(Movie model);
        public Task<bool> UpdateMovie(Movie model);
        public Task<bool> DeleteMovie(int id);
        
    }
}
