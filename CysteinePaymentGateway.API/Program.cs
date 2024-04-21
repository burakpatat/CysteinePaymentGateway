using CysteinePaymentGateway.API.Hubs;
using CysteinePaymentGateway.VPOS;
using CysteinePaymentGateway.VPOS.Interfaces;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(crf =>
{
    crf.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader().AllowCredentials().AllowAnyMethod().SetIsOriginAllowed(policy => true);
    });
});

builder.Services.AddSignalR();

builder.Services.AddScoped<ICysteineVirtualPOSService, CysteineVirtualPOSClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<PaymentHub>("/pay-hub");

app.Run();
