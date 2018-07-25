using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Moq;
using Persistence;
using PriceCalculator.Extensions;
using PriceCalculator.Repositories;
using Xunit;

namespace PriceCalculator.Tests.Repositories
{
    public class TillRepositoryTests
    {
        private readonly List<CheckoutItem> _basicTestSetAsCheckoutItems = new List<CheckoutItem>();
        private readonly string[] _basicTestSetAsStringArray = {"milk", "bread", "beans", "apples"};
        private readonly decimal _basicTestSetBasketValueBeforeOffers;

        private readonly List<CheckoutItem> _dbItems = new List<CheckoutItem>
        {
            new CheckoutItem {Cost = 1m, Name = "apples", Unit = SaleUnit.PerBag},
            new CheckoutItem {Cost = 2m, Name = "beans", Unit = SaleUnit.PerItem},
            new CheckoutItem {Cost = 4m, Name = "bread", Unit = SaleUnit.PerItem},
            new CheckoutItem {Cost = 8m, Name = "milk", Unit = SaleUnit.PerItem},
            new CheckoutItem {Cost = 16m, Name = "jamie dodgers", Unit = SaleUnit.PerItem}
        };

        private readonly string _nonExistantItem = "hobnobs";
        private readonly DateTime _nowTimestamp = new DateTime(2018, 09, 15, 2, 21, 15);
        private string[] _emptyArray;

        public TillRepositoryTests()
        {
            _basicTestSetAsCheckoutItems.Add(_dbItems.First(o => o.Name == "milk"));
            _basicTestSetAsCheckoutItems.Add(_dbItems.First(o => o.Name == "bread"));
            _basicTestSetAsCheckoutItems.Add(_dbItems.First(o => o.Name == "beans"));
            _basicTestSetAsCheckoutItems.Add(_dbItems.First(o => o.Name == "apples"));
            _basicTestSetBasketValueBeforeOffers = _basicTestSetAsCheckoutItems.Sum(o => o.Cost);
        }

        // Happy Path
        [Fact]
        public void
            When_a_list_of_shopping_items_that_are_all_good_are_passed_in__Then_they_are_converted_to_checkout_items()
        {
            // Arrange 
            var mockDb = new Mock<IDatabase>();
            mockDb.Setup(o => o.CheckoutItems).Returns(_dbItems);

            var mockDateRepo = new Mock<IDateRepository>();
            mockDateRepo.Setup(o => o.UtcNow()).Returns(_nowTimestamp);

            var sut = new TillRepository(mockDb.Object, mockDateRepo.Object);

            // Act 
            var result = sut.ProcessCheckoutItems(_basicTestSetAsStringArray);

            // Assert
            Assert.Equal(_basicTestSetAsCheckoutItems, result.CheckoutItems);
        }

        [Fact]
        public void When_an_empty_list_of_shopping_items_are_passed_in__Then_the_converted_list_is_empty()
        {
            // Arrange 
            var mockDb = new Mock<IDatabase>();
            mockDb.Setup(o => o.CheckoutItems).Returns(_dbItems);

            var mockDateRepo = new Mock<IDateRepository>();
            mockDateRepo.Setup(o => o.UtcNow()).Returns(_nowTimestamp);

            var sut = new TillRepository(mockDb.Object, mockDateRepo.Object);

            // Act 
            var result = sut.ProcessCheckoutItems(_emptyArray);

            // Assert
            Assert.Equal(result.CheckoutItems.Count, actual: 0);
            Assert.Equal(result.Errors.Count, actual: 0);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public void
            When_a_list_of_shopping_items_are_passed_in_with_a_non_existant_item__Then_they_are_converted_to_checkout_items_and_an_error_raised(
                int amountOfNonExistantItems)
        {
            // Arrange 
            var testSetWithNonExistantItem = _basicTestSetAsStringArray;
            for (var i = 0; i < amountOfNonExistantItems; i++)
                testSetWithNonExistantItem = testSetWithNonExistantItem.Add(_nonExistantItem + " - " + i);

            var mockDb = new Mock<IDatabase>();
            mockDb.Setup(o => o.CheckoutItems).Returns(_dbItems);

            var mockDateRepo = new Mock<IDateRepository>();
            mockDateRepo.Setup(o => o.UtcNow()).Returns(_nowTimestamp);

            var sut = new TillRepository(mockDb.Object, mockDateRepo.Object);

            // Act 
            var result = sut.ProcessCheckoutItems(testSetWithNonExistantItem);

            // Assert
            Assert.Equal(_basicTestSetAsCheckoutItems, result.CheckoutItems);
            Assert.Equal(result.Errors.Count, actual: amountOfNonExistantItems);

            for (var i = 0; i < result.Errors.Count(); i++)
            {
                var errorMessage = result.Errors.First(o => o.Value == _nonExistantItem + " - " + i);
                Assert.Equal("Item not found in DB", errorMessage.Key);
                Assert.Equal(_nonExistantItem + " - " + i, errorMessage.Value);
            }
        }

        [Fact]
        public void When_a_basket_is_submitted__The_total_is_correct_before_offers_applied()
        {
            // Arrange 
            var mockDb = new Mock<IDatabase>();
            mockDb.Setup(o => o.CheckoutItems).Returns(_dbItems);

            var mockDateRepo = new Mock<IDateRepository>();
            mockDateRepo.Setup(o => o.UtcNow()).Returns(_nowTimestamp);

            var sut = new TillRepository(mockDb.Object, mockDateRepo.Object);

            // Act 
            var result = sut.ProcessCheckoutItems(_basicTestSetAsStringArray);

            // Assert
            Assert.Equal(_basicTestSetBasketValueBeforeOffers, result.Total);
        }
    }
}