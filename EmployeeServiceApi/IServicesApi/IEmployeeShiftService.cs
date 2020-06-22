using EmployeeServiceApi.Models;
using System.Collections.Generic;

namespace EmployeeServiceApi.IServicesApi
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
