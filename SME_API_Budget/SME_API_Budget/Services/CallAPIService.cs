using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using SME_API_Budget.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
namespace SME_API_Budget.Services
{
    public class CallAPIService : ICallAPIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _FlagDev;
        private readonly string Api_ErrorLog;
        private readonly string Api_SysCode;
        private readonly ILogger<CallAPIService> _logger;

        public CallAPIService(HttpClient httpClient, IConfiguration configuration, ILogger<CallAPIService> logger)
        {
            _httpClient = httpClient;
            _FlagDev = configuration["Devlopment:FlagDev"] ?? throw new ArgumentNullException("FlagDev is missing");
            Api_ErrorLog = configuration["Information:Api_ErrorLog"] ?? throw new ArgumentNullException("Api_ErrorLog is missing");
            Api_SysCode = configuration["Information:Api_SysCode"] ?? throw new ArgumentNullException("Api_SysCode is missing");
            _logger = logger;
        }
        #region return project
        public async Task<ApiResponseReturnProjectModels> GetDataApiAsync_ReturnProject(MapiInformationModels apiModels, string? year, string? projectcode)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            var httpClient = new HttpClient(handler);
            string requestJson = "";
            if (apiModels == null)
                return new ApiResponseReturnProjectModels { StatusCode = 400, Message = "Invalid API parameters" };

            string? apiUrl = _FlagDev == "Y" ? apiModels.Urldevelopment : apiModels.Urlproduction;
            if (string.IsNullOrEmpty(apiUrl))
                return new ApiResponseReturnProjectModels { StatusCode = 400, Message = "API URL is missing" };

            apiUrl = apiUrl
                .Replace("{1}", apiModels.ServiceNameCode ?? "")
                .Replace("{2}", year ?? "");
            requestJson = apiUrl;

            _logger.LogInformation($"Requesting URL: {apiUrl}");

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

                if (!string.IsNullOrEmpty(apiModels.ApiKey))
                    request.Headers.Add("X-Api-Key", apiModels.ApiKey);

                if (!string.IsNullOrEmpty(apiModels.Password))
                {
                    var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiModels.Username}:{apiModels.Password}"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
                }

          

                using var response = await httpClient.SendAsync(request);
                string responseData = await response.Content.ReadAsStringAsync();

                Dictionary<string, APIResponseReturnProjectModels>? resultData = null;
                if (response.IsSuccessStatusCode)
                {
                    resultData = JsonSerializer.Deserialize<Dictionary<string, APIResponseReturnProjectModels>>(responseData);
                }

                return new ApiResponseReturnProjectModels
                {
                    StatusCode = (int)response.StatusCode,
                    Message = response.IsSuccessStatusCode ? "Success" : $"API Error: {response.ReasonPhrase}",
                    Data = resultData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"SSL error: {ex.Message} | Inner: {ex.InnerException?.Message}");
                var errorLog = new ErrorLogModels
                {
                    Message = "Function " + apiModels.ServiceNameTh + " " + ex.Message,
                    StackTrace = ex.StackTrace,
                    Source = ex.Source,
                    TargetSite = ex.TargetSite?.ToString(),
                    ErrorDate = DateTime.Now,
                    UserName = apiModels.Username,
                    Path = apiModels.Urlproduction,
                    HttpMethod = apiModels.MethodType,
                    RequestData = requestJson,
                    InnerException = ex.InnerException?.ToString(),
                    SystemCode = Api_SysCode,
                    CreatedBy = "system",
                    HttpCode = "500"
                };
                await RecErrorLogApiAsync(apiModels, errorLog);
                throw new Exception("Error in GetData: " + ex.Message + " | Inner Exception: " + ex.InnerException?.Message);
            }
        }

        #endregion return project
        public async Task<ApiResponseReturnAreaModels> GetDataApiAsync_ReturnArea(MapiInformationModels apiModels, string? year, string? projectcode)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            var httpClient = new HttpClient(handler);
           
            string requestJson = "";
            if (apiModels == null)
                return new ApiResponseReturnAreaModels { StatusCode = 400, Message = "Invalid API parameters" };

            // ✅ เลือก URL ตาม _FlagDev
            string? apiUrl = _FlagDev == "Y" ? apiModels.Urldevelopment : apiModels.Urlproduction;

            if (string.IsNullOrEmpty(apiUrl))
                return new ApiResponseReturnAreaModels { StatusCode = 400, Message = "API URL is missing" };

            // ✅ Replace ค่าใน URL (ใช้ ?? เพื่อป้องกัน null)
            apiUrl = apiUrl
                .Replace("{1}", apiModels.ServiceNameCode ?? "")
                .Replace("{2}", year ?? "")
                .Replace("{3}", projectcode ?? "");
            requestJson = apiUrl;
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}");

                switch (apiModels.AuthorizationType) 
                {
                    case "Bearer":
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiModels.ApiKey);
                        break;

                        case "Basic":
                        // ✅ ใส่ Basic Authentication ถ้ามี Username & Password
                        if (!string.IsNullOrEmpty(apiModels.Password))
                        {
                            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiModels.Username}:{apiModels.Password}"));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
                        }
                        break;
                        case "ApiKey":
                        // ✅ ใส่ API Key ถ้ามี
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Add("X-Api-Key", apiModels.ApiKey);

                        break;
                }



                // ✅ Call API
                using var response = await httpClient.SendAsync(request);
                string responseData = await response.Content.ReadAsStringAsync();
                var resultx = JsonSerializer.Deserialize<APIResponseDataReturnPAreaModels>(responseData);
                return new ApiResponseReturnAreaModels
                {
                    StatusCode = (int)response.StatusCode,
                    Message = response.IsSuccessStatusCode ? "Success" : $"API Error: {response.ReasonPhrase}",
                    Data = resultx,
                };
            }
            catch (Exception ex)
            {
                var errorLog = new ErrorLogModels
                {
                    Message = "Function " + apiModels.ServiceNameTh + " " + ex.Message,
                    StackTrace = ex.StackTrace,
                    Source = ex.Source,
                    TargetSite = ex.TargetSite?.ToString(),
                    ErrorDate = DateTime.Now,
                    UserName = apiModels.Username, // ดึงจาก context หรือ session
                    Path = apiModels.Urlproduction,
                    HttpMethod = apiModels.MethodType,
                    RequestData = requestJson, // serialize เป็น JSON
                    InnerException = ex.InnerException?.ToString(),
                    SystemCode = Api_SysCode,
                    CreatedBy = "system"

                };
                await RecErrorLogApiAsync(apiModels, errorLog);
                throw new Exception("Error in GetData: " + ex.Message + " | Inner Exception: " + ex.InnerException?.Message);
            }
        }
        public async Task<ApiResponseReturnPOutputModels> GetDataApiAsync_ReturnPOutput(
            MapiInformationModels apiModels, string? year, string? projectcode)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            var httpClient = new HttpClient(handler);
            if (apiModels == null)
                return new ApiResponseReturnPOutputModels { StatusCode = 400, Message = "Invalid API parameters" };

            string apiUrl = _FlagDev == "Y" ? apiModels.Urldevelopment : apiModels.Urlproduction;
            if (string.IsNullOrEmpty(apiUrl))
                return new ApiResponseReturnPOutputModels { StatusCode = 400, Message = "API URL is missing" };

            // Replace placeholders ifมี
            apiUrl = apiUrl
                .Replace("{1}", apiModels.ServiceNameCode ?? "")
                .Replace("{2}", year ?? "")
                .Replace("{3}", projectcode ?? "");

            string requestUrl = apiUrl;  // เก็บไว้ log

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

                // Auth ถ้ามี
                switch (apiModels.AuthorizationType)
                {
                    case "Bearer":
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiModels.ApiKey);
                        break;
                    case "Basic":
                        if (!string.IsNullOrEmpty(apiModels.Password))
                        {
                            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiModels.Username}:{apiModels.Password}"));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
                        }
                        break;
                    case "ApiKey":
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Add("X-Api-Key", apiModels.ApiKey);
                        break;
                }

                using var response = await httpClient.SendAsync(request);
                string responseData = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Requested URL: " + requestUrl);
                Console.WriteLine("Response StatusCode: " + response.StatusCode);
                Console.WriteLine("Raw JSON Response (or other): " + responseData);

                // ถ้าเจอ status code ไม่ใช่ success
                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponseReturnPOutputModels
                    {
                        StatusCode = (int)response.StatusCode,
                        Message = $"API returned error: {response.ReasonPhrase} | StatusCode: {(int)response.StatusCode}",
                        Data = null
                    };
                }

                // Deserialize ตามที่ API ส่ง
                // สมมติ API ส่ง JSON ที่เข้ากับ Dictionary<string, APIResponseDataReturnPOutputModels>
                Dictionary<string, APIResponseDataReturnPOutputModels>? resultxx = null;
                try
                {
                    resultxx = JsonSerializer.Deserialize<Dictionary<string, APIResponseDataReturnPOutputModels>>(responseData);
                }
                catch (JsonException jex)
                {
                    // ถ้า parse JSON ไม่ได้
                    return new ApiResponseReturnPOutputModels
                    {
                        StatusCode = 500,
                        Message = "Response JSON parsing error: " + jex.Message,
                        Data = null
                    };
                }

                return new ApiResponseReturnPOutputModels
                {
                    StatusCode = (int)response.StatusCode,
                    Message = "Success",
                    Data = resultxx
                };
            }
            catch (Exception ex)
            {
               _logger.LogInformation($"Function {apiModels.ServiceNameTh} Error: {ex.Message}");
                // Log error
                var errorLog = new ErrorLogModels
                {
                    Message = $"Function {apiModels.ServiceNameTh} Error: {ex.Message}",
                    StackTrace = ex.StackTrace,
                    Source = ex.Source,
                    TargetSite = ex.TargetSite?.ToString(),
                    ErrorDate = DateTime.Now,
                    UserName = apiModels.Username,
                    Path = requestUrl,
                    HttpMethod = "GET",
                    RequestData = requestUrl,
                    InnerException = ex.InnerException?.ToString(),
                    SystemCode = Api_SysCode,
                    CreatedBy = "system"
                };
                await RecErrorLogApiAsync(apiModels, errorLog);

                return new ApiResponseReturnPOutputModels
                {
                    StatusCode = 500,
                    Message = "Exception thrown: " + ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseReturnPOutComeModels> GetDataApiAsync_ReturnPOutCome(MapiInformationModels apiModels, string? year, string? projectcode)
        {

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            var httpClient = new HttpClient(handler);

            string requestJson = "";
            if (apiModels == null)
                return new ApiResponseReturnPOutComeModels { StatusCode = 400, Message = "Invalid API parameters" };

            string? apiUrl = _FlagDev == "Y" ? apiModels.Urldevelopment : apiModels.Urlproduction;

            if (string.IsNullOrEmpty(apiUrl))
                return new ApiResponseReturnPOutComeModels { StatusCode = 400, Message = "API URL is missing" };

            apiUrl = apiUrl
                .Replace("{1}", apiModels.ServiceNameCode ?? "")
                .Replace("{2}", year ?? "")
                .Replace("{3}", projectcode ?? "");
            requestJson = apiUrl;

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

                switch (apiModels.AuthorizationType)
                {
                    case "Bearer":
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiModels.ApiKey);
                        break;

                    case "Basic":
                        // ✅ ใส่ Basic Authentication ถ้ามี Username & Password
                        if (!string.IsNullOrEmpty(apiModels.Password))
                        {
                            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiModels.Username}:{apiModels.Password}"));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
                        }
                        break;
                    case "ApiKey":
                        // ✅ ใส่ API Key ถ้ามี
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Add("X-Api-Key", apiModels.ApiKey);

                        break;
                }


                using var response = await httpClient.SendAsync(request);
                string responseData = await response.Content.ReadAsStringAsync();
                //    Console.WriteLine("Raw JSON Response: " + responseData);
                var resultxx = JsonSerializer.Deserialize<Dictionary<string, APIResponseDataReturnPOutComeModels>>(responseData);
                //  var resultxx2 = JsonSerializer.Deserialize<Dictionary<string, object>>(responseData);

                return new ApiResponseReturnPOutComeModels
                {
                    StatusCode = (int)response.StatusCode,
                    Message = response.IsSuccessStatusCode ? "Success" : $"API Error: {response.ReasonPhrase}",
                    Data = resultxx,
                };
            }
            catch (Exception ex)
            {
                var errorLog = new ErrorLogModels
                {
                    Message = "Function " + apiModels.ServiceNameTh + " " + ex.Message,
                    StackTrace = ex.StackTrace,
                    Source = ex.Source,
                    TargetSite = ex.TargetSite?.ToString(),
                    ErrorDate = DateTime.Now,
                    UserName = apiModels.Username, // ดึงจาก context หรือ session
                    Path = apiModels.Urlproduction,
                    HttpMethod = apiModels.MethodType,
                    RequestData = requestJson, // serialize เป็น JSON
                    InnerException = ex.InnerException?.ToString(),
                    SystemCode = Api_SysCode,
                    CreatedBy = "system"

                };
                await RecErrorLogApiAsync(apiModels, errorLog);
                throw new Exception("Error in GetData: " + ex.Message + " | Inner Exception: " + ex.InnerException?.Message);
            }
        }

        // Return_P_Expected 
        public async Task<ApiResponseReturnPExpectedModels> GetDataApiAsync_ReturnExpected(MapiInformationModels apiModels, string? year, string? projectcode)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            var httpClient = new HttpClient(handler);

            string requestJson = "";
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            if (apiModels == null)
                return new ApiResponseReturnPExpectedModels { StatusCode = 400, Message = "Invalid API parameters" };

            string? apiUrl = _FlagDev == "Y" ? apiModels.Urldevelopment : apiModels.Urlproduction;

            if (string.IsNullOrEmpty(apiUrl))
                return new ApiResponseReturnPExpectedModels { StatusCode = 400, Message = "API URL is missing" };

            apiUrl = apiUrl
                .Replace("{1}", apiModels.ServiceNameCode ?? "")
                .Replace("{2}", year ?? "")
                .Replace("{3}", projectcode ?? "");
            requestJson = apiUrl;

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

                switch (apiModels.AuthorizationType)
                {
                    case "Bearer":
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiModels.ApiKey);
                        break;

                    case "Basic":
                        // ✅ ใส่ Basic Authentication ถ้ามี Username & Password
                        if (!string.IsNullOrEmpty(apiModels.Password))
                        {
                            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiModels.Username}:{apiModels.Password}"));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
                        }
                        break;
                    case "ApiKey":
                        // ✅ ใส่ API Key ถ้ามี
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Add("X-Api-Key", apiModels.ApiKey);

                        break;
                }


                using var response = await httpClient.SendAsync(request);
                string responseData = await response.Content.ReadAsStringAsync();
                //    Console.WriteLine("Raw JSON Response: " + responseData);
                var resultxx = JsonSerializer.Deserialize<Dictionary<string, ReturnPExpectedModels>>(responseData,options);
                var result = JsonSerializer.Deserialize<Dictionary<string, ReturnPExpectedModels>>(responseData, options);
                //  var resultxx2 = JsonSerializer.Deserialize<Dictionary<string, object>>(responseData);

                return new ApiResponseReturnPExpectedModels
                {
                    StatusCode = (int)response.StatusCode,
                    Message = response.IsSuccessStatusCode ? "Success" : $"API Error: {response.ReasonPhrase}",
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                var errorLog = new ErrorLogModels
                {
                    Message = "Function " + apiModels.ServiceNameTh + " " + ex.Message,
                    StackTrace = ex.StackTrace,
                    Source = ex.Source,
                    TargetSite = ex.TargetSite?.ToString(),
                    ErrorDate = DateTime.Now,
                    UserName = apiModels.Username, // ดึงจาก context หรือ session
                    Path = apiModels.Urlproduction,
                    HttpMethod = apiModels.MethodType,
                    RequestData = requestJson, // serialize เป็น JSON
                    InnerException = ex.InnerException?.ToString(),
                    SystemCode = Api_SysCode,
                    CreatedBy = "system"

                };
                await RecErrorLogApiAsync(apiModels, errorLog);
                throw new Exception("Error in GetData: " + ex.Message + " | Inner Exception: " + ex.InnerException?.Message);
            }
        }

        // 

        public async Task<ApiResponseReturnPActivityModels> GetDataApiAsync_ReturnPActivity(MapiInformationModels apiModels, string? year, string? projectcode)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            var httpClient = new HttpClient(handler);
            string requestJson = "";
            if (apiModels == null)
                return new ApiResponseReturnPActivityModels { StatusCode = 400, Message = "Invalid API parameters" };

            // ✅ เลือก URL ตาม _FlagDev
            string? apiUrl = _FlagDev == "Y" ? apiModels.Urldevelopment : apiModels.Urlproduction;

            if (string.IsNullOrEmpty(apiUrl))
                return new ApiResponseReturnPActivityModels { StatusCode = 400, Message = "API URL is missing" };

            // ✅ Replace ค่าใน URL (ใช้ ?? เพื่อป้องกัน null)
            apiUrl = apiUrl
                .Replace("{1}", apiModels.ServiceNameCode ?? "")
                .Replace("{2}", year ?? "")
                .Replace("{3}", projectcode ?? "");
            requestJson = apiUrl;
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}");

                switch (apiModels.AuthorizationType)
                {
                    case "Bearer":
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiModels.ApiKey);
                        break;

                    case "Basic":
                        // ✅ ใส่ Basic Authentication ถ้ามี Username & Password
                        if (!string.IsNullOrEmpty(apiModels.Password))
                        {
                            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiModels.Username}:{apiModels.Password}"));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
                        }
                        break;
                    case "ApiKey":
                        // ✅ ใส่ API Key ถ้ามี
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Add("X-Api-Key", apiModels.ApiKey);

                        break;
                }


                // ✅ Call API
                using var response = await httpClient.SendAsync(request);
                string responseData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var resultx = JsonSerializer.Deserialize<Dictionary<string, ActivityDataModel>>(responseData, options);
              

                return new ApiResponseReturnPActivityModels
                {
                    StatusCode = (int)response.StatusCode,
                    Message = response.IsSuccessStatusCode ? "Success" : $"API Error: {response.ReasonPhrase}",
                    Data = resultx,
                };
            }
            catch (Exception ex)
            {
                var errorLog = new ErrorLogModels
                {
                    Message = "Function " + apiModels.ServiceNameTh + " " + ex.Message,
                    StackTrace = ex.StackTrace,
                    Source = ex.Source,
                    TargetSite = ex.TargetSite?.ToString(),
                    ErrorDate = DateTime.Now,
                    UserName = apiModels.Username, // ดึงจาก context หรือ session
                    Path = apiModels.Urlproduction,
                    HttpMethod = apiModels.MethodType,
                    RequestData = requestJson, // serialize เป็น JSON
                    InnerException = ex.InnerException?.ToString(),
                    SystemCode = Api_SysCode,
                    CreatedBy = "system"

                };
                await RecErrorLogApiAsync(apiModels, errorLog);
                throw new Exception("Error in GetData: " + ex.Message + " | Inner Exception: " + ex.InnerException?.Message);
            }
        }
        public async Task<ApiResponseReturnPPlanBdgModels> GetDataApiAsync_ReturnPPlanBdg(MapiInformationModels apiModels, string? year, string? projectcode)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            var httpClient = new HttpClient(handler);
            string requestJson = "";
            if (apiModels == null)
                return new ApiResponseReturnPPlanBdgModels { StatusCode = 400, Message = "Invalid API parameters" };

            // ✅ เลือก URL ตาม _FlagDev
            string? apiUrl = _FlagDev == "Y" ? apiModels.Urldevelopment : apiModels.Urlproduction;

            if (string.IsNullOrEmpty(apiUrl))
                return new ApiResponseReturnPPlanBdgModels { StatusCode = 400, Message = "API URL is missing" };

            // ✅ Replace ค่าใน URL (ใช้ ?? เพื่อป้องกัน null)
            apiUrl = apiUrl
                .Replace("{1}", apiModels.ServiceNameCode ?? "")
                .Replace("{2}", year ?? "")
                .Replace("{3}", projectcode ?? "");
            requestJson = apiUrl;
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}");

                switch (apiModels.AuthorizationType)
                {
                    case "Bearer":
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiModels.ApiKey);
                        break;

                    case "Basic":
                        // ✅ ใส่ Basic Authentication ถ้ามี Username & Password
                        if (!string.IsNullOrEmpty(apiModels.Password))
                        {
                            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiModels.Username}:{apiModels.Password}"));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
                        }
                        break;
                    case "ApiKey":
                        // ✅ ใส่ API Key ถ้ามี
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Add("X-Api-Key", apiModels.ApiKey);

                        break;
                }


                // ✅ Call API
                using var response = await httpClient.SendAsync(request);
                string responseData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Check for {"SUCCESS":"N","ERROR_LOG":"No Data"}
                using var doc = JsonDocument.Parse(responseData);
                var root = doc.RootElement;
                // Replace this block inside GetDataApiAsync_ReturnPPlanBdg:
                if (root.TryGetProperty("SUCCESS", out var successProp) &&
                    successProp.GetString() == "N" &&
                    root.TryGetProperty("ERROR_LOG", out var errorLogProp) &&
                    errorLogProp.GetString() == "No Data")
                {
                    return new ApiResponseReturnPPlanBdgModels
                    {
                        StatusCode = (int)response.StatusCode,
                        Message = "No Data",
                        Data = null
                    };
                }

                var resultx = JsonSerializer.Deserialize<Dictionary<string, PlanBdgDataModel>>(responseData, options);


                return new ApiResponseReturnPPlanBdgModels
                {
                    StatusCode = (int)response.StatusCode,
                    Message = response.IsSuccessStatusCode ? "Success" : $"API Error: {response.ReasonPhrase}",
                    Data = resultx,
                };
            }
            catch (Exception ex)
            {
                var errorLog = new ErrorLogModels
                {
                    Message = "Function " + apiModels.ServiceNameTh + " " + ex.Message,
                    StackTrace = ex.StackTrace,
                    Source = ex.Source,
                    TargetSite = ex.TargetSite?.ToString(),
                    ErrorDate = DateTime.Now,
                    UserName = apiModels.Username, // ดึงจาก context หรือ session
                    Path = apiModels.Urlproduction,
                    HttpMethod = apiModels.MethodType,
                    RequestData = requestJson, // serialize เป็น JSON
                    InnerException = ex.InnerException?.ToString(),
                    SystemCode = Api_SysCode,
                    CreatedBy = "system"

                };
                await RecErrorLogApiAsync(apiModels, errorLog);
                throw new Exception("Error in GetData: " + ex.Message + " | Inner Exception: " + ex.InnerException?.Message);
            }
        }
        public async Task<ApiResponseReturnPRiskModels> GetDataApiAsync_ReturnPRisk(MapiInformationModels apiModels, string? year, string? projectcode)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            var httpClient = new HttpClient(handler);
            string requestJson = "";
            if (apiModels == null)
                return new ApiResponseReturnPRiskModels { StatusCode = 400, Message = "Invalid API parameters" };

            string? apiUrl = _FlagDev == "Y" ? apiModels.Urldevelopment : apiModels.Urlproduction;

            if (string.IsNullOrEmpty(apiUrl))
                return new ApiResponseReturnPRiskModels { StatusCode = 400, Message = "API URL is missing" };

            apiUrl = apiUrl
                .Replace("{1}", apiModels.ServiceNameCode ?? "")
                .Replace("{2}", year ?? "")
                .Replace("{3}", projectcode ?? "");
            requestJson = apiUrl;

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

                switch (apiModels.AuthorizationType)
                {
                    case "Bearer":
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiModels.ApiKey);
                        break;

                    case "Basic":
                        // ✅ ใส่ Basic Authentication ถ้ามี Username & Password
                        if (!string.IsNullOrEmpty(apiModels.Password))
                        {
                            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiModels.Username}:{apiModels.Password}"));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
                        }
                        break;
                    case "ApiKey":
                        // ✅ ใส่ API Key ถ้ามี
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Add("X-Api-Key", apiModels.ApiKey);

                        break;
                }


                using var response = await httpClient.SendAsync(request);
                string responseData = await response.Content.ReadAsStringAsync();
                //    Console.WriteLine("Raw JSON Response: " + responseData);
                var resultxx = JsonSerializer.Deserialize<Dictionary<string, APIResponseDataReturnPRiskModels>>(responseData);
                //var resultxx = JsonSerializer.Deserialize<Dictionary<string, APIResponseReturnProjectModels>>(responseData);
                var resultxx2 = JsonSerializer.Deserialize<Dictionary<string, object>>(responseData);
                //  var resultxx2 = JsonSerializer.Deserialize<Dictionary<string, object>>(responseData);

                return new ApiResponseReturnPRiskModels
                {
                    StatusCode = (int)response.StatusCode,
                    Message = response.IsSuccessStatusCode ? "Success" : $"API Error: {response.ReasonPhrase}",
                    Data = resultxx,
                };
            }
            catch (Exception ex)
            {
                var errorLog = new ErrorLogModels
                {
                    Message = "Function " + apiModels.ServiceNameTh + " " + ex.Message,
                    StackTrace = ex.StackTrace,
                    Source = ex.Source,
                    TargetSite = ex.TargetSite?.ToString(),
                    ErrorDate = DateTime.Now,
                    UserName = apiModels.Username, // ดึงจาก context หรือ session
                    Path = apiModels.Urlproduction,
                    HttpMethod = apiModels.MethodType,
                    RequestData = requestJson, // serialize เป็น JSON
                    InnerException = ex.InnerException?.ToString(),
                    SystemCode = Api_SysCode,
                    CreatedBy = "system"

                };
                await RecErrorLogApiAsync(apiModels, errorLog);
                throw new Exception("Error in GetData: " + ex.Message + " | Inner Exception: " + ex.InnerException?.Message);
            }
        }
        public async Task<ApiResponseReturnPPayModels> GetDataApiAsync_ReturnPPay(MapiInformationModels apiModels, string? year, string? projectcode)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            var httpClient = new HttpClient(handler);
            string requestJson = "";
            if (apiModels == null)
                return new ApiResponseReturnPPayModels { StatusCode = 400, Message = "Invalid API parameters" };

            // ✅ เลือก URL ตาม _FlagDev
            string? apiUrl = _FlagDev == "Y" ? apiModels.Urldevelopment : apiModels.Urlproduction;

            if (string.IsNullOrEmpty(apiUrl))
                return new ApiResponseReturnPPayModels { StatusCode = 400, Message = "API URL is missing" };

            // ✅ Replace ค่าใน URL (ใช้ ?? เพื่อป้องกัน null)
            apiUrl = apiUrl
                .Replace("{1}", apiModels.ServiceNameCode ?? "")
                .Replace("{2}", year ?? "")
                .Replace("{3}", projectcode ?? "");
            requestJson = apiUrl;
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}");

                switch (apiModels.AuthorizationType)
                {
                    case "Bearer":
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiModels.ApiKey);
                        break;

                    case "Basic":
                        // ✅ ใส่ Basic Authentication ถ้ามี Username & Password
                        if (!string.IsNullOrEmpty(apiModels.Password))
                        {
                            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiModels.Username}:{apiModels.Password}"));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
                        }
                        break;
                    case "ApiKey":
                        // ✅ ใส่ API Key ถ้ามี
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Add("X-Api-Key", apiModels.ApiKey);

                        break;
                }


                // ✅ Call API

                using var response = await httpClient.SendAsync(request);
                string responseData = await response.Content.ReadAsStringAsync();
                var resultx = JsonSerializer.Deserialize<APIResponseDataReturnPPayModels>(responseData);
                return new ApiResponseReturnPPayModels
                {
                    StatusCode = (int)response.StatusCode,
                    Message = response.IsSuccessStatusCode ? "Success" : $"API Error: {response.ReasonPhrase}",
                    Data = resultx,
                };
            }
            catch (Exception ex)
            {
                var errorLog = new ErrorLogModels
                {
                    Message = "Function " + apiModels.ServiceNameTh + " " + ex.Message,
                    StackTrace = ex.StackTrace,
                    Source = ex.Source,
                    TargetSite = ex.TargetSite?.ToString(),
                    ErrorDate = DateTime.Now,
                    UserName = apiModels.Username, // ดึงจาก context หรือ session
                    Path = apiModels.Urlproduction,
                    HttpMethod = apiModels.MethodType,
                    RequestData = requestJson, // serialize เป็น JSON
                    InnerException = ex.InnerException?.ToString(),
                    SystemCode = Api_SysCode,
                    CreatedBy = "system"

                };
                await RecErrorLogApiAsync(apiModels, errorLog);
                throw new Exception("Error in GetData: " + ex.Message + " | Inner Exception: " + ex.InnerException?.Message);
            }
        }
        public async Task<ApiRecPRResponseModel> RecDataApiAsync_RecPRs(MapiInformationModels apiModels, string? year, string? projectcode,string? ref_code, RecPRSubDataModel SendData)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            var httpClient = new HttpClient(handler);
            string requestJson = "";
            if (apiModels == null)
                return new ApiRecPRResponseModel { StatusCode = 400, Message = "Invalid API parameters" };

            // ✅ เลือก URL ตาม _FlagDev
            string? apiUrl = _FlagDev == "Y" ? apiModels.Urldevelopment : apiModels.Urlproduction;

            if (string.IsNullOrEmpty(apiUrl))
                return new ApiRecPRResponseModel { StatusCode = 400, Message = "API URL is missing" };

            // ✅ Replace ค่าใน URL (ใช้ ?? เพื่อป้องกัน null)
            apiUrl = apiUrl
                .Replace("{1}", apiModels.ServiceNameCode ?? "")
                .Replace("{2}", year ?? "")
                .Replace("{3}", projectcode ?? "")
                  .Replace("{4}", ref_code ?? "")
                ;
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                switch (apiModels.AuthorizationType)
                {
                    case "Bearer":
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiModels.ApiKey);
                        break;

                    case "Basic":
                        // ✅ ใส่ Basic Authentication ถ้ามี Username & Password
                        if (!string.IsNullOrEmpty(apiModels.Password))
                        {
                            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiModels.Username}:{apiModels.Password}"));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
                        }
                        break;
                    case "ApiKey":
                        // ✅ ใส่ API Key ถ้ามี
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Add("X-Api-Key", apiModels.ApiKey);

                        break;
                }


                // ✅ แปลง SendData เป็น JSON และแนบไปกับ Body ของ Request
                var jsonData = JsonSerializer.Serialize(SendData);
                requestJson = jsonData;
                request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // ✅ Call API และรอผลลัพธ์
                using var response = await httpClient.SendAsync(request);
                string responseData = await response.Content.ReadAsStringAsync();

                // ✅ Deserialize ข้อมูล JSON Response
                //var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                //var resultx = JsonSerializer.Deserialize<Dictionary<string, object>>(responseData, options);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return new ApiRecPRResponseModel
                    {
                        StatusCode = (int)response.StatusCode,
                        Message = response.IsSuccessStatusCode ? "Success" : $"API Error: {response.ReasonPhrase}",
                         Data = responseData
                    };
                }
                else 
                {
                    return new ApiRecPRResponseModel
                    {
                        StatusCode = 500,
                        Message = response.IsSuccessStatusCode ? "Success" : $"API Error: {response.ReasonPhrase}",
                        // Data = resultx
                    };
                }
            
            }
            catch (Exception ex)
            {
                var errorLog = new ErrorLogModels
                {
                    Message = "Function " + apiModels.ServiceNameTh + " " + ex.Message,
                    StackTrace = ex.StackTrace,
                    Source = ex.Source,
                    TargetSite = ex.TargetSite?.ToString(),
                    ErrorDate = DateTime.Now,
                    UserName = apiModels.Username, // ดึงจาก context หรือ session
                    Path = apiModels.Urlproduction,
                    HttpMethod = apiModels.MethodType,
                    RequestData = requestJson, // serialize เป็น JSON
                    InnerException = ex.InnerException?.ToString(),
                    SystemCode = Api_SysCode,
                    CreatedBy = "system"

                };
                await RecErrorLogApiAsync(apiModels, errorLog);
                throw new Exception("Error in GetData: " + ex.Message + " | Inner Exception: " + ex.InnerException?.Message);
            }
        }

        public async Task<ApiRecP301ResponseModel> RecDataApiAsync_RecP301(MapiInformationModels apiModels, RecP301Models SendData)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            var httpClient = new HttpClient(handler);
            string requestJson = "";
            if (apiModels == null)
                return new ApiRecP301ResponseModel { StatusCode = 400, Message = "Invalid API parameters" };

            // ✅ เลือก URL ตาม _FlagDev
            string? apiUrl = _FlagDev == "Y" ? apiModels.Urldevelopment : apiModels.Urlproduction;

            if (string.IsNullOrEmpty(apiUrl))
                return new ApiRecP301ResponseModel { StatusCode = 400, Message = "API URL is missing" };

            // ✅ Replace ค่าใน URL (ใช้ ?? เพื่อป้องกัน null)
            apiUrl = apiUrl
                .Replace("{1}", apiModels.ServiceNameCode ?? "")
           
                ;
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                switch (apiModels.AuthorizationType)
                {
                    case "Bearer":
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiModels.ApiKey);
                        break;

                    case "Basic":
                        // ✅ ใส่ Basic Authentication ถ้ามี Username & Password
                        if (!string.IsNullOrEmpty(apiModels.Password))
                        {
                            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiModels.Username}:{apiModels.Password}"));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
                        }
                        break;
                    case "ApiKey":
                        // ✅ ใส่ API Key ถ้ามี
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Add("X-Api-Key", apiModels.ApiKey);

                        break;
                }


                // ✅ แปลง SendData เป็น JSON และแนบไปกับ Body ของ Request
                var jsonData = JsonSerializer.Serialize(SendData);
                requestJson = jsonData;
                request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // ✅ Call API และรอผลลัพธ์
                using var response = await httpClient.SendAsync(request);
                string responseData = await response.Content.ReadAsStringAsync();

                // ✅ Deserialize ข้อมูล JSON Response
                //var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                //var resultx = JsonSerializer.Deserialize<Dictionary<string, object>>(responseData, options);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return new ApiRecP301ResponseModel
                    {
                        StatusCode = (int)response.StatusCode,
                        Message = response.IsSuccessStatusCode ? "Success" : $"API Error: {response.ReasonPhrase}",
                        Data = responseData
                    };
                }
                else
                {
                    return new ApiRecP301ResponseModel
                    {
                        StatusCode = 500,
                        Message = response.IsSuccessStatusCode ? "Success" : $"API Error: {response.ReasonPhrase}",
                        // Data = resultx
                    };
                }

            }
            catch (Exception ex)
            {
                var errorLog = new ErrorLogModels
                {
                    Message = "Function " + apiModels.ServiceNameTh + " " + ex.Message,
                    StackTrace = ex.StackTrace,
                    Source = ex.Source,
                    TargetSite = ex.TargetSite?.ToString(),
                    ErrorDate = DateTime.Now,
                    UserName = apiModels.Username, // ดึงจาก context หรือ session
                    Path = apiModels.Urlproduction,
                    HttpMethod = apiModels.MethodType,
                    RequestData = requestJson, // serialize เป็น JSON
                    InnerException = ex.InnerException?.ToString(),
                    SystemCode = Api_SysCode,
                    CreatedBy = "system"

                };
                await RecErrorLogApiAsync(apiModels, errorLog);
                throw new Exception("Error in GetData: " + ex.Message + " | Inner Exception: " + ex.InnerException?.Message);
            }
        }

        public async Task RecErrorLogApiAsync(MapiInformationModels apiModels,ErrorLogModels eModels)
        {
    
            // ✅ เลือก URL ตาม _FlagDev
            string?  apiUrl = Api_ErrorLog;

          
           
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);

                switch (apiModels.AuthorizationType)
                {
                    case "Bearer":
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiModels.ApiKey);
                        break;

                    case "Basic":
                        // ✅ ใส่ Basic Authentication ถ้ามี Username & Password
                        if (!string.IsNullOrEmpty(apiModels.Password))
                        {
                            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiModels.Username}:{apiModels.Password}"));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
                        }
                        break;
                    case "ApiKey":
                        // ✅ ใส่ API Key ถ้ามี
                        if (!string.IsNullOrEmpty(apiModels.ApiKey))
                            request.Headers.Add("X-Api-Key", apiModels.ApiKey);

                        break;
                }


                // ✅ แปลง SendData เป็น JSON และแนบไปกับ Body ของ Request
                var jsonData = JsonSerializer.Serialize(eModels);
                request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // ✅ Call API และรอผลลัพธ์
                using var response = await _httpClient.SendAsync(request);
                string responseData = await response.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                
            }
        }

    }
}
