using FantasyEkstraklasaCoach.Application.Abstractions;
using Microsoft.Extensions.Options;

namespace FantasyEkstraklasaCoach.Infrastructure.Storage;

public class SupabaseStorageService(Supabase.Client client, IOptions<SupabaseOptions> options) : IFileStorageService
{
    private readonly SupabaseOptions _options = options.Value;

    public async Task<string> SaveAsync(Stream stream, string fileName, CancellationToken ct = default)
    {
        var ext = Path.GetExtension(fileName);
        var storedFileName = $"{Guid.NewGuid()}{ext}";

        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms, ct);

        await client.Storage
            .From(_options.BucketName)
            .Upload(ms.ToArray(), storedFileName);

        return client.Storage
            .From(_options.BucketName)
            .GetPublicUrl(storedFileName);
    }
}
