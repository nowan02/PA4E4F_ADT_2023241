using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace PA4E4F_ADT_2023241.Models
{
    public class Subject : IDbModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int TeacherId { get; set; }
        [JsonIgnore]
        public virtual Teacher SubjectTeacher { get; set; }
        [JsonIgnore]
        public virtual ICollection<Student> EnrolledStudents {  get; set; }
        [JsonIgnore]
        public virtual ICollection<Grade> Grades { get; set; }
    }
}
