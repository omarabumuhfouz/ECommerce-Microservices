// using Quartz;
// namespace EmailService.Api.Jobs;

// public class SendEmailJob(IEmailSender emailSender) : IJob
// {
//     public async Task Execute(IJobExecutionContext context)
//     {
//         // Retrieve data passed from the API
//         var data = context.MergedJobDataMap;
//         var emails = data.GetString("EmailList")?.Split(',') ?? Array.Empty<string>();
//         var subject = data.GetString("Subject") ?? "No Subject";
//         var body = data.GetString("Body") ?? "";

//         await emailSender.SendEmailAsync(emails, subject, body);
//     }
// }