using System;
using System.Collections.Generic;

namespace RetailWebApi.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string DeptName { get; set; } = string.Empty;
    }

    public class Skill
    {
        public int Id { get; set; }
        public string SkillName { get; set; } = string.Empty;
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Salary { get; set; }
        public bool Permanent { get; set; }
        public Department? Department { get; set; }
        public List<Skill> Skills { get; set; } = new();
        public DateTime DateOfBirth { get; set; }
    }
}