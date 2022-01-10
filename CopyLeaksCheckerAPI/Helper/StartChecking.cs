using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Copyleaks.SDK.V3.API;
using Copyleaks.SDK.V3.API.Models.Requests;
using Copyleaks.SDK.V3.API.Models.Requests.Properties;
using Copyleaks.SDK.V3.API.Models.Types;
using Polly;

namespace CopyLeaksCheckerAPI.Helper
{
    public class StartChecking
    {
        public const string WebhooksHost = "https://localhost:44310/Webhooks/";

        private HttpClient Client { get; set; }
        private CopyleaksIdentityApi IdentityClient { get; set; }
        private CopyleaksScansApi EducationAPIClient { get; set; }
        
        public async Task<string> SubmitEducationFileScanAsync(string authToken)
        {
            // A unique scan ID for the scan
            // In case this scan ID already exists for this user Copyleaks API will return HTTP 409 Conflict result
            string scanId = Guid.NewGuid().ToString();
            string scannedText = "Hellow world";
            // Submit a file for scan in https://api.copyleaks.com
            await EducationAPIClient.SubmitFileAsync(scanId, new FileDocument
            {
                Base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(scannedText)),
                Filename = "text.txt",
                PropertiesSection = GetScanPropertiesByProduct(scanId, eProduct.Education)
            },
            authToken).ConfigureAwait(false);

            return scanId;
        }

        private ScanProperties GetScanPropertiesByProduct(string scanId, eProduct product)
        {
            ScanProperties scanProperties;
            if (product == eProduct.Businesses)
                scanProperties = new BusinessesScanProperties();
            else
            {
                scanProperties = new EducationScanProperties();
                ((EducationScanProperties)scanProperties).ReportSection.Create = true;
            }

            // The action to perform
            // Possible values:
            // 1. checkCredits - return the number of credits that will be consumed by the scan.
            //                   The Result of the request will be returned to the 'Completion' callback
            // 2. Scan - Scan the submitted text
            //           The Result of the request will be returned to the 'Completion' callback
            // 3. Index - Upload the submitted text to Copyleaks internal database to be compared against feture scans
            //            The Result of the request will be returned to the 'Completion' callback
            scanProperties.Action = eSubmitAction.Index;
            scanProperties.Webhooks = new Webhooks
            {
                // Copyleaks API will POST the scan results to the 'completed' callback
                // See 'CompletedProcess' method for more details
                Status = new Uri($"{WebhooksHost}/{scanId}/{{status}}")
            };

            // Sandbox mode does not take any credits
            scanProperties.Sandbox = true;

            return scanProperties;
        }
    }
}
