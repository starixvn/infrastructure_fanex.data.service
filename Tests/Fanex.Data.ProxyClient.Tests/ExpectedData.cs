using Fanex.Data.SampleData.Models;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Fanex.Data.ProxyClient.Tests;

using System;
using System.Collections.Generic;
using System.Linq;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "StringLiteralTypo")]
public static class ExpectedData
{
    private static Customer expectedCustomer;
    private static OrderDetails expectedOrderDetails;

    private static List<OrderedProduct> expectedOrderedProducts;
    private static List<Order> expectedOrders;
    private static List<Employee> expectedEmployees;
    private static List<Product> expectedProducts;
    private static List<Shipper> expectedShippers;
    private static List<Supplier> expectedSuppliers;
    private static List<Category> expectedCategories;
    private static List<MostExpensiveProduct> expectedMostExpensiveProducts;

    public static Customer ExpectedCustomer
    {
        get
        {
            expectedCustomer ??= GenerateSampleOfCustomer();

            return expectedCustomer;
        }
    }

    public static OrderDetails ExpectedOrderDetails
    {
        get
        {
            expectedOrderDetails ??= GenerateSampleOfOrderDetails();

            return expectedOrderDetails;
        }
    }

    public static List<OrderedProduct> ExpectedOrderedProducts
    {
        get
        {
            expectedOrderedProducts ??= GenerateSampleOfOrderedProducts();

            return expectedOrderedProducts;
        }
    }

    public static List<Order> ExpectedOrders
    {
        get
        {
            expectedOrders ??= GenerateSampleOfOrders();

            return expectedOrders;
        }
    }

    public static List<Employee> ExpectedEmployees
    {
        get
        {
            expectedEmployees ??= GenerateSampleOfEmployees();

            return expectedEmployees;
        }
    }

    public static List<Product> ExpectedProducts
    {
        get
        {
            expectedProducts ??= GenerateSampleOfProducts();

            return expectedProducts;
        }
    }

    public static List<Shipper> ExpectedShippers
    {
        get
        {
            expectedShippers ??= GenerateSampleOfShippers();

            return expectedShippers;
        }
    }

    public static List<Supplier> ExpectedSuppliers
    {
        get
        {
            expectedSuppliers ??= GenerateSampleOfSuppliers();

            return expectedSuppliers;
        }
    }

    public static List<Category> ExpectedCategories
    {
        get
        {
            expectedCategories ??= GenerateSampleOfCategories();

            return expectedCategories;
        }
    }

    public static List<MostExpensiveProduct> ExpectedMostExpensiveProducts
    {
        get
        {
            expectedMostExpensiveProducts ??= GenerateSampleOfMostExpensiveProducts();

            return expectedMostExpensiveProducts;
        }
    }

    private static Customer GenerateSampleOfCustomer()
        => new()
        {
            Id = "VINET",
            CompanyName = "Vins et alcools Chevalier",
            ContactName = "Paul Henriot",
            ContactTitle = "Accounting Manager",
            Address = "59 rue de l'Abbaye",
            City = "Reims",
            PostalCode = "51100",
            Country = "France",
            Phone = "26.47.15.10",
            Fax = "26.47.15.11"
        };

    private static OrderDetails GenerateSampleOfOrderDetails()
        => new()
        {
            Order = ExpectedOrders.First(order => order.Id == 10248),
            OrderedProducts = ExpectedOrderedProducts
        };

    private static List<OrderedProduct> GenerateSampleOfOrderedProducts()
        =>
        [
            new()
            {
                UnitPrice = 14.00m,
                Quantity = 12,
                Discount = 0m,
                Product = ExpectedProducts.First(product => product.Id == 11)
            },

            new()
            {
                UnitPrice = 9.80m,
                Quantity = 10,
                Discount = 0m,
                Product = ExpectedProducts.First(product => product.Id == 42)
            },

            new()
            {
                UnitPrice = 34.80m,
                Quantity = 5,
                Discount = 0m,
                Product = ExpectedProducts.First(product => product.Id == 72)
            }
        ];

    private static List<Order> GenerateSampleOfOrders()
        =>
        [
            new()
            {
                Id = 10248,
                OrderDate = Convert.ToDateTime("1996-07-04 00:00:00.000", CultureInfo.InvariantCulture),
                RequiredDate = Convert.ToDateTime("1996-08-01 00:00:00.000", CultureInfo.InvariantCulture),
                ShippedDate = Convert.ToDateTime("1996-07-16 00:00:00.000", CultureInfo.InvariantCulture),
                Freight = 32.38m,
                ShipName = "Vins et alcools Chevalier",
                ShipAddress = "59 rue de l'Abbaye",
                ShipCity = "Reims",
                ShipPostalCode = "51100",
                ShipCountry = "France",
                Customer = ExpectedCustomer,
                Employee = ExpectedEmployees.First(employee => employee.Id == 5),
                Shipper = ExpectedShippers.First(shipper => shipper.Id == 3)
            },

            new()
            {
                Id = 10274,
                OrderDate = Convert.ToDateTime("1996-08-06 00:00:00.000", CultureInfo.InvariantCulture),
                RequiredDate = Convert.ToDateTime("1996-09-03 00:00:00.000", CultureInfo.InvariantCulture),
                ShippedDate = Convert.ToDateTime("1996-08-16 00:00:00.000", CultureInfo.InvariantCulture),
                Freight = 6.01m,
                ShipName = "Vins et alcools Chevalier",
                ShipAddress = "59 rue de l'Abbaye",
                ShipCity = "Reims",
                ShipPostalCode = "51100",
                ShipCountry = "France",
                Customer = ExpectedCustomer,
                Employee = ExpectedEmployees.First(employee => employee.Id == 6),
                Shipper = ExpectedShippers.First(shipper => shipper.Id == 1)
            },

            new()
            {
                Id = 10295,
                OrderDate = Convert.ToDateTime("1996-09-02 00:00:00.000", CultureInfo.InvariantCulture),
                RequiredDate = Convert.ToDateTime("1996-09-30 00:00:00.000", CultureInfo.InvariantCulture),
                ShippedDate = Convert.ToDateTime("1996-09-10 00:00:00.000", CultureInfo.InvariantCulture),
                Freight = 1.15m,
                ShipName = "Vins et alcools Chevalier",
                ShipAddress = "59 rue de l'Abbaye",
                ShipCity = "Reims",
                ShipPostalCode = "51100",
                ShipCountry = "France",
                Customer = ExpectedCustomer,
                Employee = ExpectedEmployees.First(employee => employee.Id == 2),
                Shipper = ExpectedShippers.First(shipper => shipper.Id == 2)
            },

            new()
            {
                Id = 10737,
                OrderDate = Convert.ToDateTime("1997-11-11 00:00:00.000", CultureInfo.InvariantCulture),
                RequiredDate = Convert.ToDateTime("1997-12-09 00:00:00.000", CultureInfo.InvariantCulture),
                ShippedDate = Convert.ToDateTime("1997-11-18 00:00:00.000", CultureInfo.InvariantCulture),
                Freight = 7.79m,
                ShipName = "Vins et alcools Chevalier",
                ShipAddress = "59 rue de l'Abbaye",
                ShipCity = "Reims",
                ShipPostalCode = "51100",
                ShipCountry = "France",
                Customer = ExpectedCustomer,
                Employee = ExpectedEmployees.First(employee => employee.Id == 2),
                Shipper = ExpectedShippers.First(shipper => shipper.Id == 2)
            },

            new()
            {
                Id = 10739,
                OrderDate = Convert.ToDateTime("1997-11-12 00:00:00.000", CultureInfo.InvariantCulture),
                RequiredDate = Convert.ToDateTime("1997-12-10 00:00:00.000", CultureInfo.InvariantCulture),
                ShippedDate = Convert.ToDateTime("1997-11-17 00:00:00.000", CultureInfo.InvariantCulture),
                Freight = 11.08m,
                ShipName = "Vins et alcools Chevalier",
                ShipAddress = "59 rue de l'Abbaye",
                ShipCity = "Reims",
                ShipPostalCode = "51100",
                ShipCountry = "France",
                Customer = ExpectedCustomer,
                Employee = ExpectedEmployees.First(employee => employee.Id == 3),
                Shipper = ExpectedShippers.First(shipper => shipper.Id == 3)
            }
        ];

#pragma warning disable S103 // Lines should not be too long

    private static List<Employee> GenerateSampleOfEmployees()
        =>
        [
            new()
            {
                Id = 2,
                FirstName = "Andrew",
                LastName = "Fuller",
                Title = "Vice President, Sales",
                TitleOfCourtesy = "Dr.",
                BirthDate = Convert.ToDateTime("1952-02-19 00:00:00.000", CultureInfo.InvariantCulture),
                HireDate = Convert.ToDateTime("1992-08-14 00:00:00.000", CultureInfo.InvariantCulture),
                Address = "908 W. Capital Way",
                City = "Tacoma",
                Region = "WA",
                PostalCode = "98401",
                Country = "USA",
                HomePhone = "(206) 555-9482",
                Extension = "3457",
                PhotoPath = "http://accweb/emmployees/fuller.bmp",
                Notes =
                    "Andrew received his BTS commercial IN 1974 AND a Ph.D. IN international marketing from the University of Dallas IN 1981.  He is fluent IN French AND Italian AND reads German.  He joined the company as a sales representative, was promoted to sales manager IN January 1992 AND to vice president of sales IN March 1993.  Andrew is a member of the Sales Management Roundtable, the Seattle Chamber of Commerce, AND the Pacific Rim Importers Association."
            },

            new()
            {
                Id = 3,
                FirstName = "Janet",
                LastName = "Leverling",
                Title = "Sales Representative",
                TitleOfCourtesy = "Ms.",
                BirthDate = Convert.ToDateTime("1963-08-30 00:00:00.000", CultureInfo.InvariantCulture),
                HireDate = Convert.ToDateTime("1992-04-01 00:00:00.000", CultureInfo.InvariantCulture),
                Address = "722 Moss Bay Blvd.",
                City = "Kirkland",
                Region = "WA",
                PostalCode = "98033",
                Country = "USA",
                HomePhone = "(206) 555-3412",
                Extension = "3355",
                PhotoPath = "http://accweb/emmployees/leverling.bmp",
                Notes =
                    "Janet has a BS degree IN chemistry from Boston College (1984).  She has also completed a certificate program IN food retailing management.  Janet was hired as a sales associate IN 1991 AND promoted to sales representative IN February 1992."
            },

            new()
            {
                Id = 5,
                FirstName = "Steven",
                LastName = "Buchanan",
                Title = "Sales Manager",
                TitleOfCourtesy = "Mr.",
                BirthDate = Convert.ToDateTime("1955-03-04 00:00:00.000", CultureInfo.InvariantCulture),
                HireDate = Convert.ToDateTime("1993-10-17 00:00:00.000", CultureInfo.InvariantCulture),
                Address = "14 Garrett Hill",
                City = "London",
                PostalCode = "SW1 8JR",
                Country = "UK",
                HomePhone = "(71) 555-4848",
                Extension = "3453",
                PhotoPath = "http://accweb/emmployees/buchanan.bmp",
                Notes =
                    "Steven Buchanan graduated from St. Andrews University, Scotland, with a BSC degree IN 1976.  Upon joining the company as a sales representative IN 1992, he spent 6 months IN an orientation program at the Seattle office AND then returned to his permanent post IN London.  He was promoted to sales manager IN March 1993.  Mr. Buchanan has completed the courses \"Successful Telemarketing\" AND \"International Sales Management.\"  He is fluent IN French."
            },

            new()
            {
                Id = 6,
                FirstName = "Michael",
                LastName = "Suyama",
                Title = "Sales Representative",
                TitleOfCourtesy = "Mr.",
                BirthDate = Convert.ToDateTime("1963-07-02 00:00:00.000", CultureInfo.InvariantCulture),
                HireDate = Convert.ToDateTime("1993-10-17 00:00:00.000", CultureInfo.InvariantCulture),
                Address = "Coventry House Miner Rd.",
                City = "London",
                PostalCode = "EC2 7JR",
                Country = "UK",
                HomePhone = "(71) 555-7773",
                Extension = "428",
                PhotoPath = "http://accweb/emmployees/davolio.bmp",
                Notes =
                    "Michael is a graduate of Sussex University (MA, economics, 1983) AND the University of California at Los Angeles (MBA, marketing, 1986).  He has also taken the courses \"Multi-Cultural Selling\" AND \"Time Management for the Sales Professional.\"  He is fluent IN Japanese AND can read AND write French, Portuguese, AND Spanish."
            }
        ];

#pragma warning restore S103 // Lines should not be too long

    private static List<Product> GenerateSampleOfProducts()
        =>
        [
            new()
            {
                Id = 11,
                ProductName = "Queso Cabrales",
                QuantityPerUnit = "1 kg pkg.",
                UnitPrice = 21.00m,
                UnitsInStock = 22,
                UnitsOnOrder = 30,
                ReorderLevel = 30,
                Discontinued = false,
                Supplier = ExpectedSuppliers.First(supplier => supplier.Id == 5),
                Category = ExpectedCategories.First(category => category.Id == 4)
            },

            new()
            {
                Id = 42,
                ProductName = "Singaporean Hokkien Fried Mee",
                QuantityPerUnit = "32 - 1 kg pkgs.",
                UnitPrice = 14.00m,
                UnitsInStock = 26,
                UnitsOnOrder = 0,
                ReorderLevel = 0,
                Discontinued = true,
                Supplier = ExpectedSuppliers.First(supplier => supplier.Id == 20),
                Category = ExpectedCategories.First(category => category.Id == 5)
            },

            new()
            {
                Id = 72,
                ProductName = "Mozzarella di Giovanni",
                QuantityPerUnit = "24 - 200 g pkgs.",
                UnitPrice = 34.80m,
                UnitsInStock = 14,
                UnitsOnOrder = 0,
                ReorderLevel = 0,
                Discontinued = false,
                Supplier = ExpectedSuppliers.First(supplier => supplier.Id == 14),
                Category = ExpectedCategories.First(category => category.Id == 4)
            }
        ];

    private static List<Shipper> GenerateSampleOfShippers()
        =>
        [
            new()
            {
                Id = 1,
                CompanyName = "Speedy Express",
                Phone = "(503) 555-9831"
            },

            new()
            {
                Id = 2,
                CompanyName = "United Package",
                Phone = "(503) 555-3199"
            },

            new()
            {
                Id = 3,
                CompanyName = "Federal Shipping",
                Phone = "(503) 555-9931"
            }
        ];

    private static List<Supplier> GenerateSampleOfSuppliers()
        =>
        [
            new()
            {
                Id = 5,
                CompanyName = "Cooperativa de Quesos 'Las Cabras'",
                ContactName = "Antonio del Valle Saavedra",
                ContactTitle = "Export Administrator",
                Address = "Calle del Rosal 4",
                City = "Oviedo",
                Region = "Asturias",
                PostalCode = "33007",
                Country = "Spain",
                Phone = "(98) 598 76 54"
            },

            new()
            {
                Id = 6,
                CompanyName = "Mayumi's",
                ContactName = "Mayumi Ohno",
                ContactTitle = "Marketing Representative",
                Address = "92 Setsuko Chuo-ku",
                City = "Osaka",
                PostalCode = "545",
                Country = "Japan",
                Phone = "(06) 431-7877"
            },

            new()
            {
                Id = 14,
                CompanyName = "Formaggi Fortini s.r.l.",
                ContactName = "Elio Rossi",
                ContactTitle = "Sales Representative",
                Address = "Viale Dante, 75",
                City = "Ravenna",
                PostalCode = "48100",
                Country = "Italy",
                Phone = "(0544) 60323",
                Fax = "(0544) 60603"
            },

            new()
            {
                Id = 15,
                CompanyName = "Norske Meierier",
                ContactName = "Beate Vileid",
                ContactTitle = "Marketing Manager",
                Address = "Hatlevegen 5",
                City = "Sandvika",
                PostalCode = "1320",
                Country = "Norway",
                Phone = "(0)2-953010"
            },

            new()
            {
                Id = 17,
                CompanyName = "Svensk Sjöföda AB",
                ContactName = "Michael Björn",
                ContactTitle = "Sales Representative",
                Address = "Brovallavägen 231",
                City = "Stockholm",
                PostalCode = "S-123 45",
                Country = "Sweden",
                Phone = "08-123 45 67"
            },

            new()
            {
                Id = 19,
                CompanyName = "New England Seafood Cannery",
                ContactName = "Robb Merchant",
                ContactTitle = "Wholesale Account Agent",
                Address = "Order Processing Dept. 2100 Paul Revere Blvd.",
                City = "Boston",
                Region = "MA",
                PostalCode = "02134",
                Country = "USA",
                Phone = "(617) 555-3267",
                Fax = "(617) 555-3389"
            },

            new()
            {
                Id = 20,
                CompanyName = "Leka Trading",
                ContactName = "Chandra Leka",
                ContactTitle = "Owner",
                Address = "471 Serangoon Loop, Suite #402",
                City = "Singapore",
                PostalCode = "0512",
                Country = "Singapore",
                Phone = "555-8787"
            },

            new()
            {
                Id = 24,
                CompanyName = "G'day, Mate",
                ContactName = "Wendy Mackenzie",
                ContactTitle = "Sales Representative",
                Address = "170 Prince Edward Parade Hunter's Hill",
                City = "Sydney",
                Region = "NSW",
                PostalCode = "2042",
                Country = "Australia",
                Phone = "(02) 555-5914",
                Fax = "(02) 555-4873"
            },

            new()
            {
                Id = 26,
                CompanyName = "Pasta Buttini s.r.l.",
                ContactName = "Giovanni Giudici",
                ContactTitle = "Order Administrator",
                Address = "Via dei Gelsomini, 153",
                City = "Salerno",
                PostalCode = "84100",
                Country = "Italy",
                Phone = "(089) 6547665",
                Fax = "(089) 6547667"
            }
        ];

    private static List<Category> GenerateSampleOfCategories()
        =>
        [
            new()
            {
                Id = 4,
                CategoryName = "Dairy Products",
                Description = "Cheeses"
            },

            new()
            {
                Id = 5,
                CategoryName = "Grains/Cereals",
                Description = "Breads, crackers, pasta, AND cereal"
            }
        ];

    private static List<MostExpensiveProduct> GenerateSampleOfMostExpensiveProducts()
        =>
        [
            new()
            {
                ProductName = "Côte de Blaye",
                UnitPrice = 263.50m
            },

            new()
            {
                ProductName = "Thüringer Rostbratwurst",
                UnitPrice = 123.79m
            },

            new()
            {
                ProductName = "Mishi Kobe Niku",
                UnitPrice = 97.00m
            },

            new()
            {
                ProductName = "Sir Rodney's Marmalade",
                UnitPrice = 81.00m
            },

            new()
            {
                ProductName = "Carnarvon Tigers",
                UnitPrice = 62.50m
            },

            new()
            {
                ProductName = "Raclette Courdavault",
                UnitPrice = 55.00m
            },

            new()
            {
                ProductName = "Manjimup Dried Apples",
                UnitPrice = 53.00m
            },

            new()
            {
                ProductName = "Tarte au sucre",
                UnitPrice = 49.30m
            },

            new()
            {
                ProductName = "Ipoh Coffee",
                UnitPrice = 46.00m
            },

            new()
            {
                ProductName = "Rössle Sauerkraut",
                UnitPrice = 45.60m
            }
        ];
}