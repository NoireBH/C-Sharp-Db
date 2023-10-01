using P01_StudentSystem.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models;

public class Homework
{
    [Required]
    public int HomeworkId { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR")]
    public string Content { get; set; }

    [Required]
    public ContentType ContentType { get; set; }

    [Required]

    public DateTime SubmissionTime { get; set; }

    [Required]
    public int StudentId { get; set; }
    public virtual Student Student { get; set; }

    [Required]
    public int CourseId { get; set; }

    public virtual Course Course { get; set; }



}
