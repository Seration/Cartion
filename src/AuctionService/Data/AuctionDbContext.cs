using AuctionService.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Auction> Auctions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //ef ile calilsaan massTransit ozelligi. RabbitMQ cokunce
        //RabbitMq ya gidecek gitmesi gereken mesaji db ye kayit ediyor
        //ve rabbitmq ayaga kalkana kadar araliklarla onu gondermeye calisiyor
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}
