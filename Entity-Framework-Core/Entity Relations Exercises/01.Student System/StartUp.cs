using P01_StudentSystem.Data;
using P01_StudentSystem.Data.Models;

namespace _01.Student_System;

internal class StartUp
{
    static void Main(string[] args)
    {
        var context = new StudentSystemContext();

        context.Students.Add(new Student { Name = "Elmir", RegisteredOn = DateTime.Now});
        context.SaveChanges();
    }
}
