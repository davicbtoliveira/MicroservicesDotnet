using Common.Api.Logic.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace Common.Api.Logic.Extension
{
    public class RestSharpClientExtension
    {
        public static async Task<RestResponse> GetResponseSerializedJsonData<T>(string urlParameters, string token) where T : new()
        {
            var client = new RestClient(urlParameters);

            RestRequest request = new RestRequest(string.Empty, Method.Get);
            request.AddHeader("cache-control", "no-cache");

            if (token != null)
                request.AddHeader("authorization", token);

            try
            {
                return await client.ExecuteAsync(request);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<RestResponse> DeleteResponseSerializedJsonData<T>(string urlParameters, string token) where T : new()
        {
            var client = new RestClient(urlParameters);

            RestRequest request = new RestRequest(string.Empty, Method.Delete);
            request.AddHeader("cache-control", "no-cache");

            if (token != null)
                request.AddHeader("authorization", token);

            try
            {
                return await client.ExecuteAsync(request);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task<RestResponse> PostResponseSerializedJsonData<T>(string urlParameters, object data, string token) where T : new()
        {

            var client = new RestClient(urlParameters);

            RestRequest request = new RestRequest(string.Empty, Method.Post);
            request.AddHeader("cache-control", "no-cache");

            if (token != null)
                request.AddHeader("authorization", token);

            var body = JsonConvert.SerializeObject(data);

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            try
            {
                return await client.ExecuteAsync(request);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task<RestResponse> PutResponseSerializedJsonData<T>(string urlParameters, object data, string token) where T : new()
        {
            var client = new RestClient(urlParameters);

            RestRequest request = new RestRequest(string.Empty, Method.Put);
            request.AddHeader("cache-control", "no-cache");

            if (token != null)
                request.AddHeader("authorization", token);

            var body = JsonConvert.SerializeObject(data);

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            try
            {
                return await client.ExecuteAsync(request);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
