using FantasyEkstraklasaCoach.Application.Abstractions;

namespace FantasyEkstraklasaCoach.IntegrationTests.Infrastructure;

public class FakeFileStorageService : IFileStorageService
{
    public Task<string> SaveAsync(Stream stream, string fileName, CancellationToken ct = default)
    {
        var ext = Path.GetExtension(fileName);
        var url = $"https://fake-storage.test/{Guid.NewGuid()}{ext}";
        return Task.FromResult(url);
    }
}
