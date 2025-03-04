using Common.Api.Logic.Models;
using Common.Notification.Logic.Business.Intefaces;
using Common.Notification.Logic.Business.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Common.Api.Logic.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {

        private readonly INotification _notification;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string userID { get; set; }

        public BaseApiController(INotification notification,
                                 IHttpContextAccessor httpContextAccessor)
        {
            _notification = notification;
            _httpContextAccessor = httpContextAccessor;

            if (_httpContextAccessor.HttpContext.Request.Host.Host.ToString() != "localhost")
            {
                userID = _httpContextAccessor.HttpContext.Request.Headers["usuario"].FirstOrDefault();
            }
        }

        protected bool isValid()
        {
            return !_notification.HasNotifications;
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (isValid())
            {
                return Ok(new CustomResult()
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new CustomResult()
            {
                success = false,
                errors = _notification.List.Select(n => n.ToString())
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotificationModelisValid(modelState);
            return CustomResponse();
        }

        protected void NotificationModelisValid(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                AddNotification(errorMsg);
            }
        }

        protected void AddNotification(string mensagem)
        {
            _notification.Add(new Description(mensagem));
        }
    }

}

