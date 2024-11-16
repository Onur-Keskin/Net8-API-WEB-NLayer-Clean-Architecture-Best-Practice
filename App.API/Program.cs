using App.Repositories.Extensions;
using App.Services;
using App.Services.Extensions;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => 
{
    options.Filters.Add<FluentValidationFilter>();
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; //referans tipler için nullable kontrolünü devredýþý býrak
});
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter=true);//.Net' in kendi filter yapýsýný disable etmek
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRepositories(builder.Configuration).AddServices(builder.Configuration);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
