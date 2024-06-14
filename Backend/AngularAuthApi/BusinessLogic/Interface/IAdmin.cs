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
        public bool AddMovie(Movie model);
        public bool UpdateMovie(Movie model);
        public bool DeleteMovie(int id);
        
    }
}
