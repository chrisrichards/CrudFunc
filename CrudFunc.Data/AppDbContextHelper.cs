using System;
using Microsoft.EntityFrameworkCore;

namespace CrudFunc.Data
{
    public class AppDbContextHelper
    {
        public static AppDbContext CreateContext()
        {
            var connectionString = Environment.GetEnvironmentVariable("SQLAZURECONNSTR");
            if (connectionString == null)
                throw new InvalidOperationException("AppDbContextHelper connection string not found");

            return CreateContext(connectionString);
        }

        public static AppDbContext CreateContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString, builder => { builder.EnableRetryOnFailure(); });
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}