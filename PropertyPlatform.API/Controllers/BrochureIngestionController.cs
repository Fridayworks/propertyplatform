using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyPlatform.Core.Entities;
using PropertyPlatform.Core.Interfaces;
using System.Security.Claims;

namespace PropertyPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BrochureIngestionController : ControllerBase
    {
        private readonly IBrochureIngestionService _brochureService;

        public BrochureIngestionController(IBrochureIngestionService brochureService)
        {
            _brochureService = brochureService;
        }

        public class IngestBrochureRequest
        {
            public string BrochureUrl { get; set; } = string.Empty;
            public Guid ProjectId { get; set; }
        }

        public class IngestPdfRequest
        {
            public string PdfUrl { get; set; } = string.Empty;
            public Guid ProjectId { get; set; }
        }

        public class IngestImageRequest
        {
            public string ImageUrl { get; set; } = string.Empty;
            public Guid ProjectId { get; set; }
        }

        public class IngestUrlRequest
        {
            public string Url { get; set; } = string.Empty;
            public Guid ProjectId { get; set; }
        }

        public class BulkMediaRequest
        {
            public Guid ProjectId { get; set; }
            public List<string> MediaUrls { get; set; } = new();
        }

        [HttpPost("brochure")]
        public async Task<IActionResult> IngestBrochure([FromBody] IngestBrochureRequest request)
        {
            var result = await _brochureService.IngestBrochureAsync(request.BrochureUrl, request.ProjectId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("pdf")]
        public async Task<IActionResult> IngestPdf([FromBody] IngestPdfRequest request)
        {
            var result = await _brochureService.IngestPdfAsync(request.PdfUrl, request.ProjectId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("image")]
        public async Task<IActionResult> IngestImage([FromBody] IngestImageRequest request)
        {
            var result = await _brochureService.IngestImageAsync(request.ImageUrl, request.ProjectId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("url")]
        public async Task<IActionResult> IngestUrl([FromBody] IngestUrlRequest request)
        {
            var result = await _brochureService.IngestUrlAsync(request.Url, request.ProjectId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("extract-text/pdf")]
        public async Task<IActionResult> ExtractTextFromPdf([FromBody] IngestPdfRequest request)
        {
            var result = await _brochureService.ExtractTextFromPdfAsync(request.PdfUrl);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("extract-text/image")]
        public async Task<IActionResult> ExtractTextFromImage([FromBody] IngestImageRequest request)
        {
            var result = await _brochureService.ExtractTextFromImageAsync(request.ImageUrl);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> ProcessBulkMedia([FromBody] BulkMediaRequest request)
        {
            var result = await _brochureService.ProcessBulkProjectMediaAsync(request.ProjectId, request.MediaUrls);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }
    }
}