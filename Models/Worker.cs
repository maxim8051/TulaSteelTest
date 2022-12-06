using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcTest.Models
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Worker> Workers { get; set; }
    }

    public class Worker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите имя!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите фамилию!")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите отчество!")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите телефон!")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите отдел!")]
        public int DepartmentID { get; set; }
        public Department Department { get; set; }

        public string PathPhoto { get; set; }

        public string FullName { get; set; }

    }
}
