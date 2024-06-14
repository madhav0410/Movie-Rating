using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAceess.Models
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserEmail { get; set; }    
        public string Movie { get; set; }
        public int MovieRating { get; set; }

        [ForeignKey("UserEmail")]
        public User User { get; set; }

        [ForeignKey("Movie")]
        public Movie movie { get; set; }
    }
}
