using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PA4E4F_ADT_2023241.Models
{
    public class Teacher : IModelWithID
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Subject> TaughtSubjects { get; set; }
        public virtual ICollection<Grade> GivenGrades { get; set; }
    }
}
