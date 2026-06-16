namespace FantasyEkstraklasaCoach.Application.Abstractions;

public interface IFileStorageService
{
    Task<string> SaveAsync(Stream stream, string fileName, CancellationToken ct = default);
}
