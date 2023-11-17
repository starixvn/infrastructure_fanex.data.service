using Fanex.Data.ProxyClient.Tests.ParameterSets.Query;
using Fanex.Data.SampleData.Models;
using Xunit.Asserts.Compare;

namespace Fanex.Data.ProxyClient.Tests
{
    using Repository;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    [Collection("Fanex Data Client collection")]
    public class MultipleFetchTests
    {
        private readonly DynamicRepository proxy = new DynamicRepository();

        private const string TestedCustomerId = "VINET";

        private const int ExpectedNumberOfOrders = 5;

        [Fact]
        public async Task FetchMultipleAsync_NullCriteria_ThrowArgumentNullException()
        {
            // Arrange.
            var criteria = default(ICriteria);

            // Act.
            var exception = await Record.ExceptionAsync(() => proxy.FetchMultipleAsync<Supplier, Shipper>(criteria));

            // Assert.
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public async Task FetchMultipleAsync_InvalidCriteria_ReturnEmptyLists()
        {
            // Arrange.
            var criteria = new SupplierAndShipperByCustomerIdCriteria
            {
                CustomerId = string.Empty
            };

            // Act.
            var (actualSuppliers, actualShippers) = await proxy.FetchMultipleAsync<Supplier, Shipper>(criteria);

            // Assert.
            Assert.Empty(actualSuppliers);
            Assert.Empty(actualShippers);
        }

        [Fact]
        public async Task FetchMultipleAsync_TwoDataMapping_ReturnCorrectly()
        {
            // Arrange.
            var criteria = new SupplierAndShipperByCustomerIdCriteria
            {
                CustomerId = TestedCustomerId
            };

            // Act.
            var (actualSuppliers, actualShippers) = await proxy.FetchMultipleAsync<Supplier, Shipper>(criteria);

            // Assert.
            Assert.Equal(ExpectedData.ExpectedSuppliers, actualSuppliers);
            Assert.Equal(ExpectedData.ExpectedShippers, actualShippers);
        }

        [Fact]
        public async Task FetchMultipleAsync_ThreeDataMapping_ReturnCorrectly()
        {
            // Arrange.
            var parameterSet = new MultiTablesCriteria();

            // Act.
            var (actualCategories, actualCustomers, actualEmployees) = await proxy.FetchMultipleAsync<Category, Customer, Employee>(parameterSet);

            // Assert.
            Assert.NotEmpty(actualCategories);
            Assert.NotEmpty(actualCustomers);
            Assert.NotEmpty(actualEmployees);
        }

        [Fact]
        public async Task FetchMultipleAsync_FourDataMapping_ReturnCorrectly()
        {
            // Arrange.
            var criteria = new MultiTablesCriteria();

            // Act.
            var (categories, customers, employees, products) = await proxy.FetchMultipleAsync<Category, Customer, Employee, Product>(criteria);

            // Assert.
            Assert.NotEmpty(categories);
            Assert.NotEmpty(customers);
            Assert.NotEmpty(employees);
            Assert.NotEmpty(products);
        }

        [Fact]
        public async Task FetchMultipleAsync_FiveDataMapping_ReturnCorrectly()
        {
            // Arrange.
            var criteria = new MultiTablesCriteria();

            // Act.
            var (categories, customers, employees, products, orders) = await proxy.FetchMultipleAsync<Category, Customer, Employee, Product, Order>(criteria);

            // Assert.
            Assert.NotEmpty(categories);
            Assert.NotEmpty(customers);
            Assert.NotEmpty(employees);
            Assert.NotEmpty(products);
            Assert.NotEmpty(orders);
        }

        [Fact]
        public async Task FetchMultipleAsync_SixDataMapping_ReturnCorrectly()
        {
            // Arrange.
            var criteria = new MultiTablesCriteria();

            // Act.
            var (categories, customers, employees, products, orders, Shippers) = await proxy.FetchMultipleAsync<Category, Customer, Employee, Product, Order, Shipper>(criteria);

            // Assert.
            Assert.NotEmpty(categories);
            Assert.NotEmpty(customers);
            Assert.NotEmpty(employees);
            Assert.NotEmpty(products);
            Assert.NotEmpty(orders);
            Assert.NotEmpty(Shippers);
        }

        [Fact]
        public async Task FetchMultipleAsync_ValidCriteria_GetsCorrectOutputs()
        {
            // Arrange.
            var parameterSet = new SupplierAndShipperByCustomerIdCriteria
            {
                CustomerId = TestedCustomerId
            };

            // Act.
            var (suppliers, shippers) = await proxy.FetchMultipleAsync<Supplier, Shipper>(parameterSet);

            // Assert.
            Assert.Equal(ExpectedNumberOfOrders, parameterSet.NumberOfOrders);
            DeepAssert.Equal(ExpectedData.ExpectedSuppliers, suppliers);
            DeepAssert.Equal(ExpectedData.ExpectedShippers, shippers);
        }
    }
}