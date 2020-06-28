﻿using System;

namespace FunctionsEmployees.Models
{
    public class UserJWT
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Surname2 { get; set; }
        public string IdCard { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public int IdRole { get; set; }
        public string Rol { get; set; }
        public string Password { get; set; }
    }
}
