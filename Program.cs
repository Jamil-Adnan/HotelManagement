using HotelManagement.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HotelManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Skapar en Configuration Builder som kan hämta enskilda värden från appsettings.json.
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            //Hämtar vår connection string inuti appsettings.json med ConfigurationBuilder objektet
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            //Med vår connection string skapar vi en DbContextOption, alltså en inställning för vår databas.
            var contextOptions = new DbContextOptionsBuilder<HotelDbContext>()
             .UseSqlServer(connectionString)
             .Options;

            // Skapar ett objekt av ApplicationDbContext genom att skicka in våra inställningar som innehåller connection stringen.
            using var dbContext = new HotelDbContext(contextOptions);

            var optionsBuilder = new DbContextOptionsBuilder<HotelDbContext>();
            optionsBuilder.UseSqlServer("Your_Connection_String_Here");

            HotelDbContext _dbContext = new HotelDbContext(optionsBuilder.Options);
            RoomService roomService = new RoomService(dbContext);
            GuestService guestService = new GuestService(dbContext);
            BookingService bookingService = new BookingService(dbContext);
            PaymentService paymentService = new PaymentService(dbContext);
            Menu menu = new Menu(roomService, guestService, bookingService, paymentService);

            menu.ShowMainMenu();
            dbContext.Database.Migrate();

        }
    }
}
