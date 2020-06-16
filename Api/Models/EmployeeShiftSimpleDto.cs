using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
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
