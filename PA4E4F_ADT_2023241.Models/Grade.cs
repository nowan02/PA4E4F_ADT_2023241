using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PA4E4F_ADT_2023241.Models
{
    public class Grade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int StudentId {  get; set; }
        public int TeacherId { get; set; }
        public int FinalGrade { get; set; }
        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set;}
        public virtual Subject Subject { get; set; }
    }
}
