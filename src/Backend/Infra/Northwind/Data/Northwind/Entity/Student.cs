﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Northwind.Data.Northwind.Entity;

public partial class Student
{
    public int StudentId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Branch { get; set; }

    public virtual ICollection<StudentAdress> StudentAdress { get; set; } = new List<StudentAdress>();
}