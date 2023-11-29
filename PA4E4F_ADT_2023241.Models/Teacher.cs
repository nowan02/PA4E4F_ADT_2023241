using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PA4E4F_ADT_2023241.Models
{
    public class Teacher : IDbModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Subject> TaughtSubjects { get; set; }
        [JsonIgnore]
        public virtual ICollection<Grade> GivenGrades { get; set; }
    }
}
