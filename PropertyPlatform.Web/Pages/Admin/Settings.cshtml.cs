using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PropertyPlatform.Web.Pages.Admin
{
    public class SettingsModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public SettingsModel(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [BindProperty]
        public string DatabaseProvider { get; set; } = "PostgreSQL";

        [BindProperty]
        public string PostgreSqlConnection { get; set; } = string.Empty;

        [BindProperty]
        public string SqliteConnection { get; set; } = string.Empty;

        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
            if (!IsCurrentUserAdmin())
            {
                Response.Redirect("/Login");
                return;
            }

            DatabaseProvider = _configuration["DatabaseProvider"] ?? "PostgreSQL";
            PostgreSqlConnection = _configuration.GetConnectionString("PostgreSqlConnection") ?? "";
            SqliteConnection = _configuration.GetConnectionString("SqliteConnection") ?? "";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!IsCurrentUserAdmin()) return Forbid();

            try
            {
                var webSettingsPath = Path.Combine(_env.ContentRootPath, "appsettings.json");

                await UpdateAppSettings(webSettingsPath, DatabaseProvider, PostgreSqlConnection, SqliteConnection);

                var apiSettingsPath = Path.Combine(_env.ContentRootPath, "..", "PropertyPlatform.API", "appsettings.json");
                if (System.IO.File.Exists(apiSettingsPath))
                {
                    await UpdateAppSettings(apiSettingsPath, DatabaseProvider, PostgreSqlConnection, SqliteConnection);
                }

                SuccessMessage = "Settings saved successfully. You may need to restart the application for database provider changes to take effect.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to save settings: {ex.Message}";
            }

            return Page();
        }

        private async Task UpdateAppSettings(string filePath, string provider, string pgConn, string sqliteConn)
        {
            if (!System.IO.File.Exists(filePath)) return;

            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var jsonObj = JsonNode.Parse(json) as JsonObject;

            if (jsonObj != null)
            {
                jsonObj["DatabaseProvider"] = provider;
                
                if (jsonObj["ConnectionStrings"] == null)
                {
                    jsonObj["ConnectionStrings"] = new JsonObject();
                }
                
                // If they update the specific connections, we also update DefaultConnection to match the active provider
                var activeConn = provider.Equals("SQLite", StringComparison.OrdinalIgnoreCase) ? sqliteConn : pgConn;
                
                jsonObj["ConnectionStrings"]!["DefaultConnection"] = activeConn;
                jsonObj["ConnectionStrings"]!["PostgreSqlConnection"] = pgConn;
                jsonObj["ConnectionStrings"]!["SqliteConnection"] = sqliteConn;

                var options = new JsonSerializerOptions { WriteIndented = true };
                await System.IO.File.WriteAllTextAsync(filePath, jsonObj.ToJsonString(options));
            }
        }

        private bool IsCurrentUserAdmin()
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrWhiteSpace(email)) return false;

            var adminEmails = _configuration.GetSection("Admin:Emails").Get<string[]>() ?? [];
            return adminEmails.Any(admin => string.Equals(admin, email, StringComparison.OrdinalIgnoreCase));
        }
    }
}
