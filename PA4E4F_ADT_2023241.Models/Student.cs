﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PA4E4F_ADT_2023241.Models
{
    public class Student : IModelWithID
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Subject> Subjects { get; set; }
        [JsonIgnore]
        public virtual ICollection<Grade> Grades {  get; set; } 
    }
}