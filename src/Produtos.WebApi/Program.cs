using MediatR;
using Produtos.CrossCutting.IoC;
using Produtos.WebApi.Configurations;
using Produtos.WebApi.Middlewares;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerConfiguration();
builder.Services.AddAuthenticationConfiguration(builder.Configuration);

builder.Services.AddMediatR(typeof(Program));
builder.Services.ApplyApiConfigurations(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwaggerSetup();

app.UseMiddleware(typeof(ErrorHandlingMiddleware));

app.MapControllers();

app.Run();
