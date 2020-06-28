namespace FunctionsEmployee.Models
{
    public class ServiceEmployeeDto
    {
        public int Id { get; set; }
        public int IdService { get; set; }
        public int IdEmployee { get; set; }

        public EmployeeDto IdEmployeeNavigation { get; set; }
        public ServiceDto IdServiceNavigation { get; set; }
    }
}
