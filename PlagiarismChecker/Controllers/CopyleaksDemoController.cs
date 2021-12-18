using Copyleaks.SDK.Demo.Models.Requests;
using Copyleaks.SDK.Demo.Models.Responses;
using Copyleaks.SDK.V3.API;
using Copyleaks.SDK.V3.API.Exceptions;
using Copyleaks.SDK.V3.API.Models.Callbacks;
using Copyleaks.SDK.V3.API.Models.Requests;
using Copyleaks.SDK.V3.API.Models.Requests.Properties;
using Copyleaks.SDK.V3.API.Models.Responses;
using Copyleaks.SDK.V3.API.Models.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlagiarismChecker.Helpers;
using SampleWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PlagiarismChecker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CopyleaksDemoController : ControllerBase
    {
        private HttpClient httpClient { get; set; }

        public CopyleaksDemoController()
        {
            httpClient = new HttpClient();
        }
        /// <summary>
        /// Example of how to send a Login request to Copyleaks API
        /// A successful request wil return a Login response containing the login token
        /// </summary>
        /// <param name="loginModel">A model with the Email and Apikey to login with</param>
        /// <returns>Action result with a Token</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var response = new Copyleaks.SDK.Demo.Models.Responses.LoginResponse();
            try
            {
                // Use CopyleaksIdentityApi to aquire a login Token from 
                Guid temp;
                var validOrEmptyKey = Guid.TryParse(loginModel.Key, out temp) ? loginModel.Key : Guid.Empty.ToString();

                // Request an API token from https://id.copyleaks.com/
                var identity = CopyleaksSDKHttpClients.IdentityClient();
                var loginResponse = await identity.LoginAsync(loginModel.Email, validOrEmptyKey).ConfigureAwait(false);

                var educationApi = CopyleaksSDKHttpClients.APIClient(CopyleaksSDKHttpClients.PRODUCT_EDUCATION);
                var businessesApi = CopyleaksSDKHttpClients.APIClient(CopyleaksSDKHttpClients.PRODUCT_BUSINESSES);
                var educationCredits = await educationApi.CreditBalanceAsync(loginResponse.Token).ConfigureAwait(false);
                var businessesCredits = await businessesApi.CreditBalanceAsync(loginResponse.Token).ConfigureAwait(false);

                var submitResponse = new SubmitResponse()
                {
                    Token = loginResponse.Token,
                    EducationCredits = educationCredits,
                    BusinessesCredits = businessesCredits
                };

                return StatusCode(StatusCodes.Status200OK, submitResponse);
            }
            catch (CopyleaksHttpException cfe)
            {
                response.ErrorMessage = cfe.Message;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }

        [HttpPost]
        [Route("/submit")]
        public async Task<IActionResult> Submit(SubmitModel submitModel)
        {
            // A unique scan ID for the scan
            // In case this scan ID already exists for this user Copyleaks API will return HTTP 409 Conflict result
            string scanId = Guid.NewGuid().ToString();
            var response = new SubmitResponse()
            {
                ScanId = scanId,
                Token = submitModel.Token
            };

            try
            {
                using (var api = new CopyleaksScansApi(submitModel.Product))
                {
                    // Submit a file for scan in https://api.copyleaks.com
                    await api.SubmitFileAsync(scanId, new FileDocument
                    {
                        // The text to scan in base64 format
                        Base64 = TextToBase64(submitModel.Text),
                        // The file name is it will appear in the scan result
                        Filename = "text.txt",
                        PropertiesSection = GetScanPropertiesByProduct(scanId, submitModel)
                    },
                    submitModel.Token).ConfigureAwait(false);
                }
                var checkResult = new CheckResultsResponse
                {
                    ScanId = scanId
                };
                return StatusCode(StatusCodes.Status200OK, checkResult);
            }
            catch (CopyleaksHttpException cfe)
            {
                response.ErrorMessage = cfe.Message;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }

        private ScanProperties GetScanPropertiesByProduct(string scanId, SubmitModel submitModel)
        {
            ScanProperties scanProperties;
            if (submitModel.Product == eProduct.Businesses)
                scanProperties = new BusinessesScanProperties();
            else
                scanProperties = new EducationScanProperties();

            // The action to perform
            // Possible values:
            // 1. checkCredits - return the number of credits that will be consumed by the scan.
            //                   The Result of the request will be returned to the 'Completion' callback
            // 2. Scan - Scan the submitted text
            //           The Result of the request will be returned to the 'Completion' callback
            // 3. Index - Upload the submitted text to Copyleaks internal database to be compared against feture scans
            //            The Result of the request will be returned to the 'Completion' callback
            scanProperties.Action = eSubmitAction.Scan;
            scanProperties.Webhooks = new Webhooks
            {
                // Copyleaks API will POST the scan results to the 'completed' callback
                // See 'CompletedProcess' method for more details
                Status = new Uri($"{submitModel.WebHookHost}/{scanId}/{{status}}")
            };
            // Sandbox mode does not take any credits
            scanProperties.Sandbox = submitModel.Sandbox;

            return scanProperties;
        }

        [HttpPost]
        [Route("/{scanId}/checkResult")]
        public IActionResult CheckResult(string scanId)
        {
            var response = new CheckResultsResponse()
            {
                ScanId = scanId,
                CompletedCallback = ResultFile.GetResults(scanId)
            };
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Receive the Completed callback from Copyleaks API with the result of the scan
        /// </summary>
        /// <param name="scanId">The unique scanID as provided in the 'submit' request</param>
        /// <param name="scanResults">The scan results</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/{scanId}/completed")]
        public IActionResult CompletedProcess(string scanId, [FromBody] CompletedCallback scanResults)
        {
            // Do something with the scan results
            ResultFile.SaveResults(scanResults, scanId);
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static string TextToBase64(string text) => Convert.ToBase64String(Encoding.UTF8.GetBytes(text));

        public static X509Certificate2 CreateCertificate(string certificateFilePath, string certificatePassword = null)
        {
            if (!System.IO.File.Exists(certificateFilePath))
                return null;
            if (certificatePassword != null)
                return new X509Certificate2(certificateFilePath, certificatePassword);
            else
                return new X509Certificate2(certificateFilePath);
        }
    }
}
