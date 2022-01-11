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
using Microsoft.AspNetCore.Hosting.Server.Features;
using Polly;

namespace CopyLeaksCheckerAPI.Helper
{
    public class StartChecking
    {
        public const string WebhooksHost = "https://localhost:44310/api/Webhooks/";
        private CopyleaksScansApi EducationAPIClient = new CopyleaksScansApi(eProduct.Education);
        
        public async Task<string> SubmitEducationFileScanAsync(string authToken,string scanId, string path)
        {
            string scannedText = ConvertFileToString(path);

            await EducationAPIClient.SubmitFileAsync(scanId, new FileDocument
            {
                Base64 = scannedText,
                Filename = "Test.docx",
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

        public string ConvertFileToString(string path)
        {
            try
            {
                var bytes = File.ReadAllBytes(@path);
                return Convert.ToBase64String(bytes);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
