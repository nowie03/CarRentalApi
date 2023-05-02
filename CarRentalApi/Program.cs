using CarRentalApi.DAL;
using CarRentalApi.Data;
using CarRentalApi.GraphQL;
using HotChocolate.Execution.Processing;
using Microsoft.EntityFrameworkCore;
using System;

namespace CarRentalApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
          

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


            builder.Services
            .AddGraphQLServer()
          .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = builder.Environment.IsDevelopment())
          .AddQueryType<Query>().
          AddMutationType<Mutation>()
          .AddSubscriptionType<Subscription>().
          AddInMemorySubscriptions();

            builder.Services.AddScoped<UserRepository, UserRepository>();
            builder.Services.AddScoped<CarRepository, CarRepository>();
            builder.Services.AddScoped<BookingRepository, BookingRepository>();
            builder.Services.AddScoped<CommentRepository, CommentRepository>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            /*  if (app.Environment.IsDevelopment())
              {
                  app.UseSwagger();
                  app.UseSwaggerUI();
              }*/

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.MapGraphQL();


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}