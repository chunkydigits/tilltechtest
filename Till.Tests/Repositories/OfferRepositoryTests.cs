using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Moq;
using Persistence;
using Till.Extensions;
using Till.Repositories;
using Xunit;

namespace Till.Tests.Repositories
{
    public class OfferRepositoryTests
    {
        private readonly DateTime _nowTimestamp = new DateTime(2018, 09, 15, 2, 21, 15);
        private readonly string _nonExistantItem = "hobnobs";
        private readonly List<CheckoutItem> _dbItems = new List<CheckoutItem>{
            new CheckoutItem{Cost = 1m, Name = "apples", Unit = SaleUnit.PerBag},
            new CheckoutItem{Cost = 2m, Name = "beans", Unit = SaleUnit.PerItem},
            new CheckoutItem{Cost = 4m, Name = "bread", Unit = SaleUnit.PerItem},
            new CheckoutItem{Cost = 8m, Name = "milk", Unit = SaleUnit.PerItem},
            new CheckoutItem{Cost = 16m, Name = "jamie dodgers", Unit = SaleUnit.PerItem}
        };

        private List<CheckoutItem> _basicTestSetAsCheckoutItems = new List<CheckoutItem>();
        private string[] _basicTestSetAsStringArray = new[] { "milk", "bread", "beans", "apples" };
        private decimal _basicTestSetBasketValueBeforeOffers;
        private string[] _emptyArray;

        public OfferRepositoryTests()
        {
            _basicTestSetAsCheckoutItems.Add(_dbItems.First(o => o.Name == "milk"));
            _basicTestSetAsCheckoutItems.Add(_dbItems.First(o => o.Name == "bread"));
            _basicTestSetAsCheckoutItems.Add(_dbItems.First(o => o.Name == "beans"));
            _basicTestSetAsCheckoutItems.Add(_dbItems.First(o => o.Name == "apples"));
            _basicTestSetBasketValueBeforeOffers = _basicTestSetAsCheckoutItems.Sum(o => o.Cost);
        }

        [Fact]
        public void When_a_basket_is_submitted__Then_the_total_is_correct_when_single_offer_applied()
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


        [Fact]
        public void When_a_basket_is_submitted_with_two_qualifying_items__Then_total_is_correct_with_both_percentage_reduction_offers_applied() { }

        [Fact]
        public void When_a_basket_is_submitted_with_qualifying_trigger_items__The_total_is_correct_when_multiple_different_offers_apply() { }

        [Fact]
        public void When_a_basket_is_submitted_with_no_trigger_items__And_offers_apply__Then_no_offers_are_applied() { }

        [Fact]
        public void When_a_basket_is_submitted_with_a_trigger_item_that_does_not_meet_multibuy_offer_criteria__And_a_multibuy_offer_applies__Then_no_offers_are_applied() { }

        [Fact]
        public void When_a_basket_is_submitted_with_trigger_items_that_meet_multibuy_offer_criteria__And_a_multibuy_offer_applies__Then_the_correct_multibuy_offer_is_applied() { }
    }
}
