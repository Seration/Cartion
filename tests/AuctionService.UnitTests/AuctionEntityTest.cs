using AuctionService.Entities;

namespace AuctionService.UnitTests;

public class AuctionEntityTest
{
    [Fact]
    public void HasReservePrice_ReservePriceGtZero_True()
    {
        var auction = new Auction { Id = Guid.NewGuid(), ReservePrice = 10 };

        var result = auction.HasReservePrýce();

        Assert.True(result);
    }
}