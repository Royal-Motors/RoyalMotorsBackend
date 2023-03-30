using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWebsiteBackend.Tests.StorageTests;

public class ImageBlobStorageTests : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
{
    public Task DisposeAsync()
    {
        throw new NotImplementedException();
    }

    public Task InitializeAsync()
    {
        throw new NotImplementedException();
    }
}
