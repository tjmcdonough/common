using Airslip.Common.Types.Failures;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Airslip.Common.Utilities.Extensions
{
    public static class HttpExtensions
    {
        public static async Task<HttpActionResult> GetApiRequest<TResponse>(this HttpClient httpClient,  string url, 
            Dictionary<string, string> headers, CancellationToken cancellationToken)
            where TResponse : class, IResponse
        {
            HttpRequestMessage httpRequestMessage = new()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };
            
            foreach (KeyValuePair<string, string> keyValuePair in headers)
            {
                httpRequestMessage.Headers.Add(keyValuePair.Key, keyValuePair.Value);                
            }

            return await _sendRequest<TResponse>(httpClient, httpRequestMessage, cancellationToken);
        }
        
        public static async Task<HttpActionResult> GetApiRequest<TResponse>(this HttpClient httpClient,  string url, 
            string apiKey, CancellationToken cancellationToken)
            where TResponse : class, IResponse
        {
            return await GetApiRequest<TResponse>(httpClient, url, new Dictionary<string, string>
            {
                {"x-api-key", apiKey}
            }, cancellationToken);
        }
        
        public static async Task<HttpActionResult> PatchApiRequest<TResponse>(this HttpClient httpClient,  string url, 
            Dictionary<string, string> headers, string requestContent, CancellationToken cancellationToken)
            where TResponse : class, IResponse
        {
            HttpRequestMessage httpRequestMessage = new()
            {
                Method = HttpMethod.Patch,
                RequestUri = new Uri(url),
                Content = new StringContent(requestContent, 
                    Encoding.UTF8, Json.MediaType)
            };
            
            foreach (KeyValuePair<string, string> keyValuePair in headers)
            {
                httpRequestMessage.Headers.Add(keyValuePair.Key, keyValuePair.Value);                
            }

            return await _sendRequest<TResponse>(httpClient, httpRequestMessage, cancellationToken);
        }

        public static async Task<HttpActionResult> ApiRequestWithBody<TResponse, TRequestType>(this HttpClient httpClient,  string url, 
            Dictionary<string, string> headers, TRequestType requestContent, HttpMethod httpMethod, CancellationToken cancellationToken)
            where TResponse : class, IResponse
        {
            HttpRequestMessage httpRequestMessage = new()
            {
                Method = httpMethod,
                RequestUri = new Uri(url),
                Content = new StringContent(Json.Serialize(requestContent!), 
                    Encoding.UTF8, Json.MediaType)
            };
            
            foreach (KeyValuePair<string, string> keyValuePair in headers)
            {
                httpRequestMessage.Headers.Add(keyValuePair.Key, keyValuePair.Value);                
            }

            return await _sendRequest<TResponse>(httpClient, httpRequestMessage, cancellationToken);
        }
        
        public static async Task<HttpActionResult> ApiRequestWithBody<TResponse, TRequestType>(this HttpClient httpClient,  
            string url, string apiKey, TRequestType requestContent, HttpMethod httpMethod, CancellationToken cancellationToken)
            where TResponse : class, IResponse
        {
            return await ApiRequestWithBody<TResponse, TRequestType>(httpClient, url, new Dictionary<string, string>
            {
                {"x-api-key", apiKey}
            }, requestContent, httpMethod, cancellationToken);
        }

        private static async Task<HttpActionResult> _sendRequest<TResponse>(HttpClient httpClient, 
            HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken) 
            where TResponse : class, IResponse
        {
            try
            {
                HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

                string content = await response.Content.ReadAsStringAsync(cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return new HttpActionResult(response.IsSuccessStatusCode, response.StatusCode, content,
                        ValidContent(content) ? Json.Deserialize<TResponse>(content) : null);
                }

                return new HttpActionResult(response.IsSuccessStatusCode, response.StatusCode, content,
                    string.IsNullOrWhiteSpace(content) ? null : _parseError(content));
            }
            catch (Exception ee)
            {
                return new (false, 
                    HttpStatusCode.BadRequest, string.Empty,
                    new ErrorResponse("UNHANDLED", ee.Message));
            }
        }

        private static IResponse _parseError(string content)
        {
            if (content.Contains("errors")) return Json.Deserialize<ErrorResponses>(content);
            return Json.Deserialize<ErrorResponse>(content);
        }
        
        public static async Task<IResponse> CommonResponseHandler<TExpectedType>(
            this HttpResponseMessage httpResponseMessage)
            where TExpectedType : class, IResponse
        {
            string content = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.IsSuccessStatusCode)
                return Json.Deserialize<TExpectedType>(content);

            // Backwards compatability - We need a common error response, probably best to default to an array of errors
            ErrorResponse errorObject = Json.Deserialize<ErrorResponse>(content);

            return string.IsNullOrEmpty(errorObject.ErrorCode)
                ? Json.Deserialize<ErrorResponses>(content)
                : new ErrorResponses(new List<ErrorResponse> {errorObject});
        }

        private static bool ValidContent(string content)
        { 
            return content != string.Empty;
        }
    }
}