using System.Net;
using Common.Logging.Logic.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Common.Logging.Logic
{
    public class RequestResponseLoggerMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly RequestResponseLoggerOption _options;
        private readonly IRequestResponseLoggerService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRequestResponseLoggerService requestResponseLogger;
        public RequestResponseLoggerMiddleware(RequestDelegate next,
                                               IOptions<RequestResponseLoggerOption> options,
                                               IRequestResponseLoggerService logger,
                                               IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _options = options.Value;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task InvokeAsync(HttpContext httpContext, IRequestResponseLogModelCreator logCreator)
        {
            var userID = _httpContextAccessor.HttpContext?.Request.Headers["Usuario"].FirstOrDefault();
            var perfilUsuario = _httpContextAccessor.HttpContext?.Request.Headers["Perfil"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(userID))
            {
                var userIDSplit = userID.Split(":");
                userID = userIDSplit[1].TrimStart();
            }
            if (!string.IsNullOrWhiteSpace(perfilUsuario))
            {
                var perfilUsuarioSplit = perfilUsuario.Split(":");
                perfilUsuario = perfilUsuarioSplit[1].TrimStart().ToLower();
            }
            RequestResponseLogModel log = logCreator.LogModel;
            if (_options == null || !_options.IsEnabled)
            {
                await _next(httpContext);
                return;
            }

            log.RequestDateTimeUtc = DateTime.UtcNow;
            HttpRequest request = httpContext.Request;

            log.LogId = Guid.NewGuid().ToString();
            log.TraceId = httpContext.TraceIdentifier;
            var ip = request.HttpContext.Connection.RemoteIpAddress;
            log.ClientIp = ip == null ? null : ip.ToString(); //request.HttpContext.Connection.RemoteIpAddress;
            log.Aplicacao = _options.Name;
            log.Usuario = string.IsNullOrWhiteSpace(userID) ? _options.Name : userID;
            log.PerfilUsuario = string.IsNullOrWhiteSpace(perfilUsuario) ? _options.Name : perfilUsuario;
            log.RequestMethod = request.Method;
            log.RequestPath = request.Path;
            log.RequestQuery = request.QueryString.ToString();
            log.RequestQueries = FormatQueries(request.QueryString.ToString());
            log.RequestHeaders = FormatHeaders(request.Headers);
            log.RequestBody = await ReadBodyFromRequest(request);
            log.RequestScheme = request.Scheme;
            log.RequestHost = request.Host.ToString();
            log.RequestContentType = request.ContentType;

            HttpResponse response = httpContext.Response;
            var originalResponseBody = response.Body;
            using var newResponseBody = new MemoryStream();
            response.Body = newResponseBody;

            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                LogError(log, exception);
            }

            newResponseBody.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(response.Body).ReadToEndAsync();

            newResponseBody.Seek(0, SeekOrigin.Begin);
            await newResponseBody.CopyToAsync(originalResponseBody);


            log.ResponseContentType = response.ContentType;
            log.ResponseStatus = response.StatusCode.ToString();
            log.ResponseHeaders = FormatHeaders(response.Headers);
            log.ResponseBody = responseBodyText;
            log.ResponseDateTimeUtc = DateTime.UtcNow;

            var jsonString = logCreator.LogString();

            _logger.Log(logCreator);

            if (!string.IsNullOrWhiteSpace(log.ExceptionMessage))
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

        }

        private List<KeyValuePair<string, string>> FormatQueries(string queryString)
        {
            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();
            string key, value;
            foreach (var query in queryString.TrimStart('?').Split("&"))
            {
                var items = query.Split("=");
                key = items.Count() >= 1 ? items[0] : string.Empty;
                value = items.Count() >= 2 ? items[1] : string.Empty;
                if (!String.IsNullOrEmpty(key))
                {
                    pairs.Add(new KeyValuePair<string, string>(key, value));
                }
            }
            return pairs;
        }

        private Dictionary<string, string> FormatHeaders(IHeaderDictionary headers)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            foreach (var header in headers)
            {
                pairs.Add(header.Key, header.Value);
            }
            return pairs;
        }

        private async Task<string> ReadBodyFromRequest(HttpRequest request)
        {
            request.EnableBuffering();
            using var streamReader = new StreamReader(request.Body, leaveOpen: true);
            var requestBody = await streamReader.ReadToEndAsync();
            request.Body.Position = 0;
            return requestBody;
        }


        private void LogError(RequestResponseLogModel log, Exception exception)
        {
            log.ExceptionMessage = exception.Message;
            log.ExceptionStackTrace = exception.StackTrace;
        }
    }
}
