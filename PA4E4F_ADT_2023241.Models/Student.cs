﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PA4E4F_ADT_2023241.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int StudentId { get; set; }
        public int TeacherId { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<Grade> Grades {  get; set; } 
    }
}