using System;

namespace Api.Models
{
    public class EmployeeShiftDto
    {
        public int Id { get; set; }
        public int IdEmployee { get; set; }
        public DateTime WorkDay { get; set; }
        public int InitHour { get; set; }
        public int EndHour { get; set; }    
    }
}
