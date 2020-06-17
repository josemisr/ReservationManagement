using ReservationManagementApp.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationManagementApp.Models.EmployeeShiftModel
{
    public class EmployeeShiftModel
    {
        public EmployeeShiftDto EmployeeShift { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<EmployeeShiftDto> EmployeeShiftsList { get; set; }


    }
}
