using PropertyPlatform.Core.Entities;

namespace PropertyPlatform.Core.Interfaces
{
    public interface IBrochureIngestionService
    {
        Task<BrochureIngestionResult> IngestBrochureAsync(string brochureUrl, Guid projectId);
        Task<BrochureIngestionResult> IngestPdfAsync(string pdfUrl, Guid projectId);
        Task<BrochureIngestionResult> IngestImageAsync(string imageUrl, Guid projectId);
        Task<BrochureIngestionResult> IngestUrlAsync(string url, Guid projectId);
        Task<BrochureIngestionResult> ExtractTextFromPdfAsync(string pdfUrl);
        Task<BrochureIngestionResult> ExtractTextFromImageAsync(string imageUrl);
        Task<BrochureIngestionResult> ProcessBulkProjectMediaAsync(Guid projectId, List<string> mediaUrls);
    }

    public class BrochureIngestionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> ExtractedTexts { get; set; } = new();
        public List<ProjectMedia> ProcessedMedia { get; set; } = new();
        public Exception? Error { get; set; }
    }
}