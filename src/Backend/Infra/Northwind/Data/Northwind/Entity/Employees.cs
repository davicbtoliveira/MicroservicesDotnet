﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Northwind.Data.Northwind.Entity;

public partial class Employees
{
    public int EmployeeID { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public string Title { get; set; }

    public string TitleOfCourtesy { get; set; }

    public DateTime? BirthDate { get; set; }

    public DateTime? HireDate { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string Region { get; set; }

    public string PostalCode { get; set; }

    public string Country { get; set; }

    public string HomePhone { get; set; }

    public string Extension { get; set; }

    public byte[] Photo { get; set; }

    public string Notes { get; set; }

    public int? ReportsTo { get; set; }

    public string PhotoPath { get; set; }

    public string PhotoFile { get; set; }

    public int? Situacao { get; set; }

    public string Descricao { get; set; }

    public virtual ICollection<EmployeesDocuments> EmployeesDocuments { get; set; } = new List<EmployeesDocuments>();

    public virtual ICollection<Employees> InverseReportsToNavigation { get; set; } = new List<Employees>();

    public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();

    public virtual Employees ReportsToNavigation { get; set; }

    public virtual ICollection<Territories> Territory { get; set; } = new List<Territories>();
}