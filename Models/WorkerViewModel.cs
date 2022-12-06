using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MvcTest.Models
{
    public class WorkerViewModel
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

        public IFormFile Photo { get; set; }

    }
}
