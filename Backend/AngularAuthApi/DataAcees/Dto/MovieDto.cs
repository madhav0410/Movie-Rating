using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAceess.Dto
{
    public class MovieDto
    {
        public string Title { get; set; }
        public string[] Cast { get; set; }
        public string Director { get; set; }
        public string Plot { get; set; }
        public string ReleaseDate { get; set; }
        public string[] Genre { get; set; }
        public string Poster { get; set; }
        public string Trailer { get; set; }

    }
}
