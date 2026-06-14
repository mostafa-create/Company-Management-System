using Demo.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Demp.PL.ViewModels
{
    public class EmployeeViewModel
    {
        // here we should put the staff that gonna required to be rendered inside the view
        public int Id { get; set; }
        [Required(ErrorMessage = "Name Is Required")]
        [MaxLength(50, ErrorMessage = "Max length is 50 chars")]
        [MinLength(5, ErrorMessage = "Min length is 5 chars")]
        public string Name { get; set; }
        [Range(22, 35, ErrorMessage = "Age must be in range from 22 to 35")]
        public int? Age { get; set; }
        [RegularExpression("^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
            ErrorMessage = "Address must be like 123-street-city-country")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Phone]
        public string Phone { get; set; }
        public DateTime HireDate { get; set; }
        public string ImageName { get; set; }
        public IFormFile image { get; set; }
        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        //FK Optional => OnDelete : Restrict
        //FK Required => OnDelete : Cascade
        [InverseProperty("Employees")]
        public Department Department { get; set; }
    }
}
