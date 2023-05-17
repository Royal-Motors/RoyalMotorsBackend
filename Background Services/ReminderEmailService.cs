using System;
using System.Net;
using System.Net.Mail;
using CarWebsiteBackend.Interfaces;
using CarWebsiteBackend.DTOs;
using Azure.Core;
using static System.Net.Mime.MediaTypeNames;

namespace CarWebsiteBackend.Background_Services;

public class ReminderEmailService : BackgroundService
{
    private readonly ITestDriveInterface testdriveStore;
    private readonly IImageInterface imageStore;

    public ReminderEmailService(ITestDriveInterface testdriveStore, IImageInterface imageStore)
    {
        this.testdriveStore = testdriveStore;
        this.imageStore = imageStore;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
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
                    try
                    {
                        var image = await imageStore.DownloadImage($"{testdrive.Car.name.Replace(" ", "_")}_2");
                        Email.Email.sendEmail(account.email, "Test Drive Reminder",
                        HTMLContent.HTMLContent.TestdriveReminderEmail(account.firstname, gmtDateString, gmtTimeString, car.name), image.Content);
                    }
                    catch { }
                }
            }
        }
        catch { }
    }
}