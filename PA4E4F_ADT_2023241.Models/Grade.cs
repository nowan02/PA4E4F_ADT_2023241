using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace PA4E4F_ADT_2023241.Models
{
    public class Grade : IDbModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int SubjectId { get; set; }
        public int StudentId {  get; set; }
        public int TeacherId { get; set; }
        [Range(0, 5)]
        public int FinalGrade { get; set; }
        [JsonIgnore]
        public virtual Student Student { get; set; }
        [JsonIgnore]
        public virtual Teacher Teacher { get; set;}
        [JsonIgnore]
        public virtual Subject Subject { get; set; }
    }
}
