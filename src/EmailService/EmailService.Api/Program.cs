using EmailService.Api.Abstractions;
using EmailService.Api.Contracts;
using EmailService.Api.Models;
using EmailService.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GmailOptions>(
    builder.Configuration.GetSection(GmailOptions.GmailOptionsKey));

builder.Services.AddScoped<IMailService, GmailService>();

builder.Services.AddEndpointsApiExplorer(); // Required for Minimal APIs
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo // No need for the long prefix now
    {
        Title = "Email Service API",
        Version = "v1"
    });
});



#region  Gome Back
// builder.Services.AddQuartz(q => q.UseMicrosoftDependencyInjectionJobFactory());
// builder.Services.AddQuartzHostedService(opt => opt.WaitForJobsToComplete = true);

// builder.Services.AddHealthChecks()
//     .AddRabbitMQ(async sp =>
//                 {
//                     return await (new ConnectionFactory
//                     {
//                         Uri = new Uri(builder.Configuration.GetSection("MessageBroker:Host").Value!),
//                         UserName = builder.Configuration.GetSection("MessageBroker:Username").Value!,
//                         Password = builder.Configuration.GetSection("MessageBroker:Password").Value!
//                     })
//                         .CreateConnectionAsync();

//                 },
//                     name: "RabbitMQ",
//                     tags: new[] { "messagebroker", "critical" }
//                 );

#endregion

var app = builder.Build();

#region  Gome Back 

// app.MapHealthChecks("/health", new HealthCheckOptions
// {
//     ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
// });

// // E. API Endpoint to Schedule Emails
// app.MapPost("/api/emails/schedule", async (ScheduleEmailRequest request, ISchedulerFactory schedulerFactory) =>
// {
//     var scheduler = await schedulerFactory.GetScheduler();

//     var job = JobBuilder.Create<SendEmailJob>()
//         .WithIdentity($"Job-{Guid.NewGuid()}")
//         .UsingJobData("EmailList", string.Join(",", request.Emails))
//         .UsingJobData("Subject", request.Subject)
//         .UsingJobData("Body", request.Body)
//         .Build();

//     var trigger = TriggerBuilder.Create()
//         .WithCronSchedule(request.CronExpression) 
//         .StartNow()
//         .Build();

//     await scheduler.ScheduleJob(job, trigger);
//     return Results.Ok("Job Scheduled");
// });
#endregion

app.MapPost("/email", async ([FromBody] SendEmailRequest request, [FromServices] IMailService mailService) =>
 {
     await mailService.SendEmailAsync(request);
     return Results.Ok("Email Sent Successfully");

 });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "EmailService v1");
        // To serve Swagger at the app's root (http://localhost:5000/), set RoutePrefix to empty
        options.RoutePrefix = string.Empty; 
    });
}

app.Run();
