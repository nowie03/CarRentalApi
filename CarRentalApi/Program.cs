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

            builder.Services.AddErrorFilter<GraphQLErrorFilter>();

            builder.Services.AddScoped<UserRepository, UserRepository>();
            builder.Services.AddScoped<CarRepository, CarRepository>();
            builder.Services.AddScoped<BookingRepository, BookingRepository>();
            builder.Services.AddScoped<CommentRepository, CommentRepository>();



            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost3000", builder => builder.WithOrigins("http://localhost:3000").AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials());
            });


            var app = builder.Build();

           

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseWebSockets();

            app.MapGraphQL();

            app.UseCors("AllowLocalhost3000");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}