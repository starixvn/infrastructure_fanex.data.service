namespace Fanex.Data.Repository.Tests.Models;

public class CustomerWithUpperProperties
{
    public string ID { get; set; }

    public string COMPANYNAME { get; set; }

    public string CONTRACTNAME { get; set; }

    public string CONTRACTITLE { get; set; }

    public string ADDRESS { get; set; }

    public string CITY { get; set; }

    public string REGION { get; set; } = string.Empty;

    public string POSTALCODE { get; set; }

    public string COUNTRY { get; set; }

    public string PHONE { get; set; }

    public string FAX { get; set; }
}