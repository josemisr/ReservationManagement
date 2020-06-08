using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationManagementApp.Models.ServiceEmployeeModel
{
    public class ServiceEmployeeModel
    {
        public ServicesEmployees ServicesEmployee { get; set; }
        public IEnumerable<ServicesEmployees> ServicesEmployees { get; set; }
       
    }
}
