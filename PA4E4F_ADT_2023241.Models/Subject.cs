using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PA4E4F_ADT_2023241.Models
{
    public class Subject : IModelWithID
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name {  get; set; }
        [Required]
        public int TeacherId { get; set; }
        public virtual ICollection<Student> EnrolledStudents {  get; set; }
        public virtual Teacher SubjectTeacher { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }
    }
}
