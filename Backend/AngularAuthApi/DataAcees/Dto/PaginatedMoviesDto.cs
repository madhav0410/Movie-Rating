using DataAceess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAceess.Dto
{
    public class PaginatedMoviesDto
    {
        public int TotalPages { get; set; }
        public List<Movie> PaginatedList { get; set; }
    }
}
