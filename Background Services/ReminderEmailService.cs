using System;
using System.Net;
using System.Net.Mail;
using CarWebsiteBackend.Interfaces;
using CarWebsiteBackend.DTOs;
using Azure.Core;

namespace CarWebsiteBackend.Background_Services;
// Assuming you have a configuration file (appsettings.json) to store SMTP settings
public class ReminderEmailService : BackgroundService
{
    private readonly ITestDriveInterface testdriveStore;

    public ReminderEmailService(ITestDriveInterface testdriveStore)
    {
        this.testdriveStore = testdriveStore;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Logic to retrieve upcoming test drives from the database and send reminder emails

            // Delay for a specific interval before running the task again
            await Task.Delay(TimeSpan.FromHours(13), stoppingToken);
            SendReminderEmail();
        }
    }

    private async void SendReminderEmail()
    {
        var timeNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        int oneday = 86400;
        try
        {
            var testdrives = await testdriveStore.GetAllTestDrives();
            foreach (var testdrive in testdrives)
            {

                if (testdrive.Time - timeNow <= oneday && testdrive.Time > timeNow)
                {
                    var account = testdrive.Account;
                    var car = testdrive.Car;

                    DateTimeOffset utcDateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(testdrive.Time).ToUniversalTime();
                    DateTimeOffset gmtDateTimeOffset = utcDateTimeOffset.AddHours(3);
                    string gmtDateString = gmtDateTimeOffset.ToString("yyyy-MM-dd");
                    string gmtTimeString = gmtDateTimeOffset.ToString("HH:mm tt");

                    Email.Email.sendEmail(account.email, "Test Drive Reminder",
                        HTMLContent.HTMLContent.TestdriveReminderEmail(account.firstname, gmtDateString, gmtTimeString, car.name));

                }
            }
        }
        catch { }
    }
}
