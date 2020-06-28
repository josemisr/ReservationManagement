using System;

namespace FunctionsEmployee.Models
{
    public class ReservationSimpleDto
    {
        public int Id { get; set; }
        public int IdService { get; set; }
        public int IdEmployee { get; set; }
        public int IdUser { get; set; }
        public DateTime Date { get; set; }
    }
}
