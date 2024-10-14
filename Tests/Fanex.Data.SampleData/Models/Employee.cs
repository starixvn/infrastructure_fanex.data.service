namespace Fanex.Data.SampleData.Models;

using System;

public class Employee : Entity
{
    public int Id { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public string Title { get; set; }

    public string TitleOfCourtesy { get; set; }

    public DateTime BirthDate { get; set; }

    public DateTime HireDate { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string Region { get; set; } = string.Empty;

    public string PostalCode { get; set; }

    public string Country { get; set; }

    public string HomePhone { get; set; }

    public string Extension { get; set; }

    public string Notes { get; set; }

    public string PhotoPath { get; set; }
}