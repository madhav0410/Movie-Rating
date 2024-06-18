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
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string[] Cast {  get; set; }
        [Required]
        public string Director { get; set; }
        [Required]
        public string Plot { get; set; }
        [Required]
        public string ReleaseDate { get; set; }
        [Required]
        public string[] Genre { get; set; }
        [Required]
        public string Poster { get; set; }
        [Required]
        public string Trailer { get; set; }
    }
}
