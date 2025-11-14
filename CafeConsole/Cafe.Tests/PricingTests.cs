using Cafe.Domain.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Tests
{
    public class PricingTests
    {
        [Fact]
        public void Regular_Apply10_Return10()
        {
            var pricing = new RegularPricing();
            Assert.Equal(10.00m, pricing.Apply(10.00m));
        }

        [Fact]
        public void HappyHour_Apply10_Return8()
        {
            var pricing = new HappyHourPricing();
            Assert.Equal(8.00m, pricing.Apply(10.00m));
        }
    }
}
