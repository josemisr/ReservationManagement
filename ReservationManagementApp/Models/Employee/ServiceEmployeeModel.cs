using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationManagementApp.Models.ServiceEmployeeModel
{
    public class ServiceEmployeeModel
    {
        public ServicesEmployees ServiceEmployee { get; set; }
        public IEnumerable<ServicesEmployees> ServicesEmployeesList { get; set; }
       
    }
}
