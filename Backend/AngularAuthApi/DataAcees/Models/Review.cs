using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAceess.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public string User {  get; set; }
        public string Movie { get; set; }
        public string MovieReview {  get; set; }
        

        [ForeignKey("User")]
        public User user { get; set; }

        [ForeignKey("Movie")]
        public Movie movie { get; set; }
    }
}
