

using CarWebsiteBackend.Interfaces;

namespace CarWebsiteBackend
{
    public class MyBackgroundService : BackgroundService
    {
        /*
        private readonly IAccountInterface accountInterface;
        public MyBackgroundService(IAccountInterface accountInterface)
        {
            this.accountInterface = accountInterface;
        }
        */

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Background service: clearing unverified accounts");
                //await accountInterface.DeleteUnverifiedAccounts();
                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }
    }
}
