using Api.Models;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.IServicesApi
{
    public interface IEmployeeShiftService
    {
        List<EmployeeShiftDto> GetEmployeesShifts();
        EmployeeShiftDto GetEmployeeShiftById(int id);
        EmployeeShiftDto AddEmployeeShift(EmployeeShiftDto employeeShiftDto);
        EmployeeShiftDto UpdateEmployeeShift(EmployeeShiftDto employeeShiftDto);
        EmployeeShiftDto RemoveEmployeeShift(int id);
    }
}
