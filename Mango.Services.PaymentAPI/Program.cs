using Mango.PaymentProcessor;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using Mango.Services.PaymentAPI.Schedule;
using Mango.MessageBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IProcessPayment, ProcessPayment>();
builder.Services.AddSingleton<IJobFactory, SingletonJobFactory>();
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

builder.Services.AddSingleton<PaymentMessageReceivedTask>();
builder.Services.AddSingleton(new JobSchedule(
    jobType: typeof(PaymentMessageReceivedTask),
    cronExpression: "0 0/1 * 1/1 * ? *"));
builder.Services.AddHostedService<QuartzHostedService>();

builder.Services.AddSingleton<IMessageBus, AzureServiceMessageBus>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
