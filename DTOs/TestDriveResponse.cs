using Microsoft.Identity.Client;

namespace CarWebsiteBackend.DTOs;

public record TestDriveResponse
{
    public TestDriveResponse(string email, string carname, int time, int testdriveId)
    {
        this.email = email;
        this.carname = carname;
        this.time = time;
        this.testdriveId = testdriveId;
    }
    public string email { get; set; }
    public string carname { get; set; }
    public int time { get; set; }
    public int testdriveId { get; set;}
}
