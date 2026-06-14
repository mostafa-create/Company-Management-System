using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
	public class Employee
	{
		// here we put the staff that related to the DB
		public int Id { get; set; }
		[Required]
		[MaxLength(50)]
		public string Name { get; set; }
		public int? Age { get; set; }
		public string Address { get; set; }
		public decimal Salary {  get; set; }
		public bool IsActive { get; set; }
		public string EmailAddress { get; set; }
		
		public string Phone { get; set; }
		public DateTime HireDate { get; set; }
		public DateTime CreationDate { get; set; } = DateTime.Now;
		public string ImageName { get; set; }
		[ForeignKey("Department")]

		public int? DepartmentId { get; set; }
		//FK Optional => OnDelete : Restrict
		//FK Required => OnDelete : Cascade
		[InverseProperty("Employees")]
        public Department Department { get; set; }
	}
}
