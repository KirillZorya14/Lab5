using App5.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Xml.Linq;

namespace App5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    public class StudentsController : ControllerBase
    {
        private static readonly string XmlPath =  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "students.xml");

        [HttpGet]
        public List<Student> GetStudents(string minGrade, string maxGrade)
{
    if (!float.TryParse(minGrade, NumberStyles.Float, CultureInfo.InvariantCulture, out float min) ||
        !float.TryParse(maxGrade, NumberStyles.Float, CultureInfo.InvariantCulture, out float max) ||
        min < 0 || max > 5 || min > max)
    {
        throw new ArgumentException("Invalid grade range");
    }

    var xdocument = XDocument.Load(XmlPath);

    var res =  xdocument.Root.Elements()
        .Where(student =>
        {
            float avg = float.Parse(student.Element("averageGrade").Value, CultureInfo.InvariantCulture);
            return avg >= min && avg <= max;
        })
        .Select(student => new Student
        {
            FirstName = student.Element("name").Value,
            LastName = student.Element("surname").Value,
            Patronymic = student.Element("patronymic").Value,
            AverageGrade = float.Parse(student.Element("averageGrade").Value, CultureInfo.InvariantCulture)
        })
        .ToList();

            return res;
}
    }
}
