﻿using ReservationManagementApp.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationManagementApp.Models.ServiceEmployeeModel
{
    public class ServiceEmployeeModel
    {
        public ServiceEmployeeDto ServiceEmployee { get; set; }
        public IEnumerable<ServiceEmployeeDto> ServicesEmployeesList { get; set; }
       
    }
}
