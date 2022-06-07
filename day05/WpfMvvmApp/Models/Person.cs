﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMvvmApp.Helpers;

namespace WpfMvvmApp.Models
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        private string email;
        public string Email
        {
            get => email;       //람다
            set 
            {
                if (!Commons.IsValidEmail(value))
                {
                    throw new Exception("invalid Email");
                }
                else
                {
                    email = value;
                }
            }
        }
        public DateTime date;
        public DateTime Date 
        { 
            get { return date; }
            set
            {
                var result = Commons.CalcAge(value);
                if(result > 135 || result < 0)
                {
                    throw new Exception("Invalid Date");
                }
                else
                {
                    date = value;
                }
            }
        }
        public bool IsBirthDate
        {
            get
            {
                return DateTime.Now.Month == date.Month && DateTime.Now.Day == date.Day;
            }
        }
        public bool IsAdult 
        {
            get
            {
                return Commons.CalcAge(date) > 18;
            }
        }

        public Person(string firstName, string lastName, string email, DateTime date)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Date = date;
        }
    }
}
