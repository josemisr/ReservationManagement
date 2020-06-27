using ReservationManagementApp.Models.Dto;
using System.Collections.Generic;

namespace ReservationManagementApp.Models.ServiceEmployeeModel
{
    public class ServiceEmployeeModel
    {
        public ServiceEmployeeDto ServiceEmployee { get; set; }
        public IEnumerable<ServiceEmployeeDto> ServicesEmployeesList { get; set; }

    }
}
