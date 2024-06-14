using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAceess.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string[] Cast {  get; set; }
        public string Director { get; set; }
        public string Plot { get; set; }
        public string ReleaseDate { get; set; }
        public string[] Genre { get; set; }
        public string Poster { get; set; }
        public string Trailer { get; set; }
    }
}
