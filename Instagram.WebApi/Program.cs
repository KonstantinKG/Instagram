using Instagram.Application;
using Instagram.Domain.Configurations;
using Instagram.Infrastructure;
using Instagram.WebApi;

using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var configuration = new AppConfiguration();
builder.Configuration.Bind(configuration);

builder.Services.AddSingleton(Options.Create(configuration));
builder.Services
    .AddPresentation()
    .AddApplication(configuration)
    .AddInfrastructure(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();