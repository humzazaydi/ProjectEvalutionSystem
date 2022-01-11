using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Copyleaks.SDK.V3.API;
using Copyleaks.SDK.V3.API.Models.Requests;
using Copyleaks.SDK.V3.API.Models.Types;
using CopyLeaksCheckerAPI.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Polly;

namespace CopyLeaksCheckerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CopyLeaksController : ControllerBase
    {
        private HttpClient Client { get; set; }
        private CopyleaksIdentityApi IdentityClient { get; set; }
        private CopyleaksScansApi EducationAPIClient { get; set; }
        private IHostingEnvironment _hostingEnvironment;

        public const string USER_EMAIL = "syedmohammadhumzazaydi@gmail.com";
        public const string USER_KEY = "c7f6ee91-47d9-483c-9d8c-7b77f905a0bf";

        public CopyLeaksController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,

            };

            Client = new HttpClient(handler);
            IdentityClient = new CopyleaksIdentityApi(Client);
            EducationAPIClient = new CopyleaksScansApi(eProduct.Education.ToString().ToLower(), Client);
            _hostingEnvironment = new HostingEnvironment();
        }

        [HttpGet]
        public async Task<JsonResult> loginAsync()
        {
            var LoginResposne = await IdentityClient.LoginAsync(USER_EMAIL, USER_KEY).ConfigureAwait(false);

            return new JsonResult(LoginResposne);
        }

        [HttpGet("SubmitFileToCheck")]
        public async Task SUBMIT_FILE_TEST([FromQuery] string fileName, string filePath)
        {
            var LoginResposne = await IdentityClient.LoginAsync(USER_EMAIL, USER_KEY).ConfigureAwait(false);
            var authToken = LoginResposne.Token;

            //var EducationBalance = await EducationAPIClient.CreditBalanceAsync(authToken).ConfigureAwait(false);

            var scanId = await new StartChecking().SubmitEducationFileScanAsync(authToken,
                Guid.NewGuid().ToString(),
                filePath).ConfigureAwait(false);

            var progress = await Policy.HandleResult<uint>((result) => result != 100)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(3))
                .ExecuteAsync(() => EducationAPIClient.ProgressAsync(scanId, authToken)).ConfigureAwait(false);

            Console.WriteLine(progress);

            //downloads
            var pdfReport = await EducationAPIClient.DownloadPdfReportAsync(scanId, authToken).ConfigureAwait(false);
            var sources = await EducationAPIClient.DownloadSourceReportAsync(scanId, authToken).ConfigureAwait(false);
            var results = await EducationAPIClient.ResultAsync(scanId, authToken).ConfigureAwait(false);

            //sandbox scan always have at least one internet result.
            var downloadedResult = await EducationAPIClient.DownloadResultAsync(scanId, results.Results.Internet[0].Id, authToken).ConfigureAwait(false);

            //await EducationAPIClient.DeleteAsync(new DeleteRequest
            //{
            //    Scans = new DeleteScanItem[] { new DeleteScanItem { Id = scanId } },
            //    Purge = true
            //}, authToken).ConfigureAwait(false);
        }

        public async Task USER_USAGE_TEST(DateTime starDateTime, DateTime endDateTime)
        {
            var LoginResposne = await IdentityClient.LoginAsync(USER_EMAIL, USER_KEY).ConfigureAwait(false);
            var authToken = LoginResposne.Token;

            DateTime start = starDateTime;
            start.ToString("dd-MM-yyyy");
            DateTime end = endDateTime;
            end.ToString("dd-MM-yyyy");

            using (var stream = new MemoryStream())
            {
                await EducationAPIClient.GetUserUsageAsync(start, end, stream, authToken).ConfigureAwait(false);

                using (var sr = new StreamReader(stream))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    var csv = await sr.ReadToEndAsync();
                    Console.WriteLine(csv);
                }
            }
        }
    }
}
