namespace CarWebsiteBackend.Extensions;

public static class DateTimeExtension
{
    private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static int ToUnixTimeSeconds(this DateTime dateTime)
    {
        return (int)(dateTime.ToUniversalTime() - UnixEpoch).TotalSeconds;
    }
}