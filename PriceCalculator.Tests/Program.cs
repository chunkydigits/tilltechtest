using Xunit;

namespace PriceCalculator.Tests
{
    public class Program
    {
        // NOTE: In this class i would add some basic end to end tests of different scenarios that may occur
        //       Although with console applications it's not as much use as something like a micro service architecture
        //       with lots of different interdependent components that create the data journey. 

        //       I would tend to keep end to end tests to a fairly high level, maybe a couple of happy paths and maybe 
        //       include a couple of higher profile error cases that are likely to be encountered and handled. 

        [Fact]
        public void When_i_submit_a_basket__Then_the_totals_are_correct_and_output_is_formatted_correctly()
        {
        }

        [Fact]
        public void
            When_i_submit_a_basket_with_trigger_items_and_offers__Then_the_totals_are_correct_and_output_is_formatted_correctly_with_details_of_the_offers_applied()
        {
        }
    }
}