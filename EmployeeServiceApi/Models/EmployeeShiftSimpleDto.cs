using System;

namespace EmployeeServiceApi.Models
{
    public class EmployeeShiftSimpleDto
    {
        public int Id { get; set; }
        public int IdEmployee { get; set; }
        public DateTime WorkDay { get; set; }
        public int InitHour { get; set; }
        public int EndHour { get; set; }
    }
}
