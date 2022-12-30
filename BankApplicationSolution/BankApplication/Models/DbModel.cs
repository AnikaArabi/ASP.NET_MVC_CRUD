using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BankApplication.Models
{
    public enum Branch { Dhanmondi = 1, Banani, Motijhil, Uttara, Mirpur }
    public class Bank
    {
        [Display(Name = "Bank Id")]
        public int BankId { get; set; }
        [Required, StringLength(50), Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Required, EnumDataType(typeof(Branch))]
        public Branch Branch { get; set; }
        [Required, Display(Name = "Establish Date"), Column(TypeName = "date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EstablishDate { get; set; }
        [Required, Display(Name = "Contact Number")]
        public int ContactNumber { get; set; }
        [Required, Display(Name = "Routing Number")]
        public int RoutingNumber { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();


    }
    public class Employee
    {
        [Display(Name = "Employee Id")]
        public int EmployeeId { get; set; }
        [Required, StringLength(50), Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        [Required, StringLength(50), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, StringLength(200)]
        public string Picture { get; set; }
        [Required, Display(Name = "Start Date"), Column(TypeName = "date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StratDate { get; set; }
        [Display(Name = "End Date"), Column(TypeName = "date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Working?")]
        public bool IsWorking { get; set; }
        [Required]
        public int BankId { get; set; }
        [ForeignKey("BankId")]
        public Bank Bank { get; set; }
    }
    public class BankDbContext : DbContext
    {
        public BankDbContext()
        {
            Database.SetInitializer(new BankDbInitializer());
        }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
    public class BankDbInitializer : DropCreateDatabaseIfModelChanges<BankDbContext>
    {
        protected override void Seed(BankDbContext db)
        {
            Bank b = new Bank { BankName = "AB Bank", Branch = Branch.Banani, ContactNumber = 01988778786, EstablishDate = DateTime.Now.AddDays(-9 * 30), RoutingNumber = 3434343 };
            b.Employees.Add(new Employee { EmployeeName = "Anika Arabi", Email = "anika@gmail.com", StratDate = DateTime.Now.AddDays(-6 * 30), Picture = "1.jpg", IsWorking = true });
            b.Employees.Add(new Employee { EmployeeName = "Nuzat Tabassum", Email = "nuzat@gmail.com", StratDate = DateTime.Now.AddDays(-6 * 30), Picture = "3.jpg", IsWorking = true });
            db.Banks.Add(b);
            db.SaveChanges();
        }
    }
}