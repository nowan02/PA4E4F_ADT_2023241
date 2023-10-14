using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PA4E4F_ADT_2023241.Models
{
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubjectCode { get; set; }
        public string Name {  get; set; }
        public virtual ICollection<Student> EnrolledStudents {  get; set; }
        public virtual Teacher SubjectTeacher { get; set; }

    }
}
