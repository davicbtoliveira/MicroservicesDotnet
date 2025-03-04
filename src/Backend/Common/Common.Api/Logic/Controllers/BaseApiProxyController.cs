using Common.Api.Logic.Models;
using Common.Notification.Logic.Business.Intefaces;
using Common.Notification.Logic.Business.Notification;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Common.Api.Logic.Controllers
{
    [ApiController]
    public abstract class BaseApiProxyController : ControllerBase
    {
        private readonly INotification _notification;

        public BaseApiProxyController(INotification notification)
        {
            _notification = notification;
        }

        protected bool isValid()
        {
            return !_notification.HasNotifications;
        }

        protected void NotificationError(string mensagem)
        {
            _notification.Add(new Description(mensagem));
        }

        protected ActionResult CustomProxyResponse<T>(HttpResponseMessage response)
        {
            return ResponseProxy<T>(response);
        }

        private ActionResult ResponseProxy<T>(HttpResponseMessage response)
        {
            CustomProxyResult<T> proxyResult;

            if (response.StatusCode == 0)
            {
                proxyResult = ErrorResult<T>(new List<string>() { "Erro de comunicação com o servidor." });
                return BadRequest(proxyResult);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var conteudo = response.Content.ReadAsStringAsync().Result;
                object? resposta = string.IsNullOrEmpty(conteudo) ? ErrorResult<T>(new List<string>() { "Sem Permissão" }) : conteudo;
                return Unauthorized(resposta);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                proxyResult = ErrorResult<T>(new List<string>() { "Recurso não encontrado." });
                return NotFound(proxyResult);
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                try
                {
                    var conteudo = response.Content.ReadAsStringAsync().Result;

                    var proxyResultString = !string.IsNullOrEmpty(conteudo) ?
                            JsonConvert.DeserializeObject<CustomProxyResult<IEnumerable<string>>>(conteudo) : new CustomProxyResult<IEnumerable<string>>();

                    return StatusCode((int)HttpStatusCode.InternalServerError, proxyResultString);
                }
                catch (Exception ex)
                {
                    proxyResult = ErrorResult<T>(new List<string>() { "Erro Interno. Entre em contato com o suporte técnico." });
                    return StatusCode((int)HttpStatusCode.InternalServerError, proxyResult);
                }
            }
            else
            {
                var conteudo = response.Content.ReadAsStringAsync().Result;
                proxyResult = !string.IsNullOrEmpty(conteudo) ?
                        JsonConvert.DeserializeObject<CustomProxyResult<T>>(conteudo) : new CustomProxyResult<T>();

                if (proxyResult.success)
                {
                    return Ok(proxyResult);
                }
                else
                {
                    return BadRequest(proxyResult);
                }
            }
        }

        private CustomProxyResult<T> ErrorResult<T>(List<string> errors)
        {
            return new CustomProxyResult<T> { errors = errors };
        }
    }
}
