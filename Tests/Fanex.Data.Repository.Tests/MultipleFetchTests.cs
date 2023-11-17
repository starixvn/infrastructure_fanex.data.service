namespace Fanex.Data.Repository.Tests
{
    using Fanex.Data.SampleData.Models;
    using ParameterSets.Criteria;
    using SampleData;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    [Collection(nameof(FanexDataConfigurationManagerCollection))]
    public class MultipleFetchTests : IClassFixture<DynamicRepositoryFixture>
    {
        private const string TestedCustomerId = "VINET";
        private const int ExpectedNumberOfOrders = 5;

        private readonly IDynamicRepository repository;

        public MultipleFetchTests(DynamicRepositoryFixture fixture)
            => repository = fixture.Repository;

        [Fact]
        public void FetchMultiple_NullCriteria_ThrowArgumentNullException()
        {
            // Arrange.
            var criteria = default(ICriteria);

            // Act.
            var exception = Record.Exception(() => repository.FetchMultiple<Supplier, Shipper>(criteria));

            // Assert.
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void FetchMultiple_InvalidCriteria_ReturnEmptyLists()
        {
            // Arrange.
            var criteria = new SupplierAndShipperByCustomerIdCriteria
            {
                CustomerId = string.Empty
            };

            // Act.
            var (actualSuppliers, actualShippers) = repository.FetchMultiple<Supplier, Shipper>(criteria);

            // Assert.
            Assert.Empty(actualSuppliers);
            Assert.Empty(actualShippers);
        }

        [Fact]
        public void FetchMultiple_ValidCriteria_ReturnExpectedLists()
        {
            // Arrange.
            var criteria = new SupplierAndShipperByCustomerIdCriteria
            {
                CustomerId = TestedCustomerId
            };

            // Act.
            var (actualSuppliers, actualShippers) = repository.FetchMultiple<Supplier, Shipper>(criteria);

            // Assert.
            Assert.Equal(ExpectedData.ExpectedSuppliers, actualSuppliers);
            Assert.Equal(ExpectedData.ExpectedShippers, actualShippers);
        }

        [Fact]
        public void FetchMultiple_ValidCriteriaAndThreeMappingTypes_ReturnExpectedLists()
        {
            // Arrange.
            var parameterSet = new MultiTablesCriteria();

            // Act.
            var (actualCategories, actualCustomers, actualEmployees) = repository.FetchMultiple<Category, Customer, Employee>(parameterSet);

            // Assert.
            Assert.NotEmpty(actualCategories);
            Assert.NotEmpty(actualCustomers);
            Assert.NotEmpty(actualEmployees);
        }

        [Fact]
        public void FetchMultiple_ValidCriteria_ReturningMoreThan3Lists()
        {
            // Arrange.
            var parameterSet = new MultiTablesCriteria();
            var mappingTypes = new[]
            {
                typeof(Category),
                typeof(Customer),
                typeof(Employee),
                typeof(Product)
            };

            // Act.
            var results = repository.FetchMultiple(parameterSet, mappingTypes);

            // Assert.
            const int expectedNumberOfResult = 4;

            var firstResult = results[0];
            var secondResult = results[1];
            var thirdResult = results[2];
            var forthResult = results[3];

            Assert.Equal(expectedNumberOfResult, results.Length);
            Assert.NotEmpty(firstResult);
            Assert.NotEmpty(secondResult);
            Assert.NotEmpty(thirdResult);
            Assert.NotEmpty(forthResult);
        }

        [Fact]
        public void FetchMultiple_ValidCriteria_GetsCorrectOutputs()
        {
            // Arrange.
            var parameterSet = new SupplierAndShipperByCustomerIdCriteria
            {
                CustomerId = TestedCustomerId
            };

            // Act.
            repository.FetchMultiple<Supplier, Shipper>(parameterSet);

            // Assert.
            Assert.Equal(ExpectedNumberOfOrders, parameterSet.NumberOfOrders);
        }

        [Fact]
        public void FetchMultiple_NumberOfMappingTypesIsMoreThanNumberOfDataTables_ReturnsEmptyListForMissingTables()
        {
            // Arrange.
            var criteria = new SupplierAndShipperByCustomerIdCriteria
            {
                CustomerId = TestedCustomerId
            };

            // Act.
            var (_, _, actualProducts) = repository.FetchMultiple<Supplier, Shipper, Product>(criteria);

            // Assert.
            Assert.Empty(actualProducts);
        }

        [Fact]
        public void FetchMultiple__NumberOfMappingTypesIsLessThanNumberOfDataTables_OnlyReturnDataMappingWithType()
        {
            // Arrange.
            var criteria = new SupplierAndShipperByCustomerIdCriteria
            {
                CustomerId = TestedCustomerId
            };

            // Act.
            var actualResult = repository.FetchMultiple(criteria, new[] { typeof(Supplier) });

            // Assert.
            Assert.Single(actualResult);
            Assert.Equal(ExpectedData.ExpectedSuppliers, actualResult[0]);
        }

        [Fact]
        public void FetchMultiple_NullMappingTypes_ThrowArgumentException()
        {
            // Arrange.
            var criteria = new SupplierAndShipperByCustomerIdCriteria
            {
                CustomerId = TestedCustomerId
            };
            var mappingTypes = default(Type[]);

            // Act.
            var exception = Record.Exception(() => repository.FetchMultiple(criteria, mappingTypes));

            // Assert.
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void FetchMultiple_EmptyMappingTypes_ThrowArgumentException()
        {
            // Arrange.
            var criteria = new SupplierAndShipperByCustomerIdCriteria
            {
                CustomerId = TestedCustomerId
            };
            var mappingTypes = Type.EmptyTypes;

            // Act.
            var exception = Record.Exception(() => repository.FetchMultiple(criteria, mappingTypes));

            // Assert.
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void FetchMultiple_MappingTypesContainsNull_ThrowArgumentException()
        {
            // Arrange.
            var criteria = new SupplierAndShipperByCustomerIdCriteria
            {
                CustomerId = TestedCustomerId
            };
            var mappingTypes = new Type[] { null };

            // Act.
            var exception = Record.Exception(() => repository.FetchMultiple(criteria, mappingTypes));

            // Assert.
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public async Task FetchMultipleAsync_NullCriteria_ThrowArgumentNullException()
        {
            // Arrange.
            var criteria = default(ICriteria);

            // Act.
            var exception = await Record.ExceptionAsync(() => repository.FetchMultipleAsync<Supplier, Shipper>(criteria));

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
            var (actualSuppliers, actualShippers) = await repository.FetchMultipleAsync<Supplier, Shipper>(criteria);

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
            var (actualSuppliers, actualShippers) = await repository.FetchMultipleAsync<Supplier, Shipper>(criteria);

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
            var (actualCategories, actualCustomers, actualEmployees) = await repository
                .FetchMultipleAsync<Category, Customer, Employee>(parameterSet)
                ;

            // Assert.
            Assert.NotEmpty(actualCategories);
            Assert.NotEmpty(actualCustomers);
            Assert.NotEmpty(actualEmployees);
        }

        [Fact]
        public async Task FetchMultipleAsync_MoreThanThreeDataMapping_ReturnCorrectly()
        {
            // Arrange.
            var parameterSet = new MultiTablesCriteria();
            var mappingTypes = new[]
            {
                typeof(Category),
                typeof(Customer),
                typeof(Employee),
                typeof(Product)
            };

            // Act.
            var results = await repository.FetchMultipleAsync(parameterSet, mappingTypes);

            // Assert.
            const int expectedNumberOfResult = 4;

            var firstResult = results[0];
            var secondResult = results[1];
            var thirdResult = results[2];
            var forthResult = results[3];

            Assert.Equal(expectedNumberOfResult, results.Length);
            Assert.NotEmpty(firstResult);
            Assert.NotEmpty(secondResult);
            Assert.NotEmpty(thirdResult);
            Assert.NotEmpty(forthResult);
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
            await repository.FetchMultipleAsync<Supplier, Shipper>(parameterSet);

            // Assert.
            Assert.Equal(ExpectedNumberOfOrders, parameterSet.NumberOfOrders);
        }
    }
}