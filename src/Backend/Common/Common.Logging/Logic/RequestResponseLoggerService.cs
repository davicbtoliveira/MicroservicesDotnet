using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging.Logic.Interface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Northwind.Data.Northwind.Entity;

namespace Common.Logging.Logic
{
    public class RequestResponseLoggerService : IRequestResponseLoggerService
    {
        private readonly ILogger<RequestResponseLoggerService> _logger;
        private IServiceProvider _services;

        public RequestResponseLoggerService(ILogger<RequestResponseLoggerService> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }
        public async Task Log(IRequestResponseLogModelCreator logCreator)
        {

            var requestResponseLogger = new RequestResponseLogger()
            {
                Aplicacao = logCreator.LogModel.Aplicacao,
                Usuario = logCreator.LogModel.Usuario,
                RequestDateTimeUtc = logCreator.LogModel.RequestDateTimeUtc.GetValueOrDefault(),
                ResponseDateTimeUtc = logCreator.LogModel.ResponseDateTimeUtc.GetValueOrDefault(),
                JsonString = JsonConvert.SerializeObject(logCreator.LogModel),
                ExceptionMessage = logCreator.LogModel.ExceptionMessage,
                ExceptionStackTrace = logCreator.LogModel.ExceptionStackTrace
            };

        }

    }
}
