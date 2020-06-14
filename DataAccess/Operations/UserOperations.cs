﻿using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Operations
{
    public  class UserOperations
    {
        private ReservationManagementDbContext db = new ReservationManagementDbContext();
        public List<Users> getAllUsers()
        {
            List<Users> users = db.Users.ToList();
            return users;
        }
        public Users CreateUser(Users user)
        {
            db.Add(user);
            db.SaveChangesAsync();
            return user;
        }
        public Users DeleteUser(Users user)
        {
            db.Remove(user);
            db.SaveChangesAsync();
            return user;
        }

        public Users UpdateUser(Users user)
        {
            db.Update(user);
            db.SaveChangesAsync();
            return user;
        }

        public Users ValidateUserLogin(string email, string password)
        {
            Users user = db.Users.Where(elem=> elem.Email == email && elem.Password == password).Include(e => e.IdRoleNavigation).FirstOrDefault();
            db.SaveChangesAsync();
            return user;
        }
        public Users GetByPk(int id)
        {
            var user = db.Users.Find(id);
            return user;
        }
    }
}