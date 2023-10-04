
using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data;
using P01_HospitalDatabase.Data.Models;

var context = new HospitalContext();
bool dataIsRead = false;


while (!dataIsRead)
{
    Console.WriteLine("Press 1 for a list of all patients");
    Console.WriteLine("Press 2 to search for a patient by full name (seperate the names by space)");

    var readKey = Console.ReadKey().Key;

    if (readKey == ConsoleKey.D1)
    {
        Console.Clear();

        var patientInfo = context.Patients
            .AsNoTracking()
            .Include(p => p.Diagnoses)
            .ToArray();

        foreach (var p in patientInfo)
        {
            PrintPatientInfo(p);
            //add prescriptions and visitations
        }
    }

    else if (readKey == ConsoleKey.D2)
    {
        while (!dataIsRead)
        {
            Console.Clear();

            var patientName = Console.ReadLine()
                .ToLower();
            var patientInfo = context.Patients
                .AsNoTracking()
                .Include(p => p.Diagnoses)
                .FirstOrDefault(p => p.FirstName.ToLower() + " " + p.LastName.ToLower() == patientName);

            if (patientInfo != null)
            {
                PrintPatientInfo(patientInfo);
                dataIsRead = true;
            }

            else
            {
                Console.WriteLine("Patient Doesn't exist.Please enter a valid patient name");
                Thread.Sleep(2000);
            }
        }
        
    }

    dataIsRead = true;
}

static void PrintPatientInfo(Patient? p)
{
    Console.WriteLine($"Name: {p.FirstName} {p.LastName}");
    Console.WriteLine($"Email: {p.Email}");
    Console.WriteLine($"Adress: {p.Address}");

    if (p.HasInsurance)
    {
        Console.WriteLine($"Insurance: Yes");
    }

    else
    {
        Console.WriteLine($"Insurance: No");
    }

    Console.WriteLine("Patient Diagnoses:");


    foreach (var d in p.Diagnoses)
    {
        Console.WriteLine($"Name:{d.Name}");

        if (d.Comments == "")
        {
            Console.WriteLine("Comments: No diagnose comments");
        }

        else
        {
            Console.WriteLine($"Comments:{d.Comments}");
        }

    }

    Console.WriteLine("-------------");
}