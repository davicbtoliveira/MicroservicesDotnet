using Common.Notification.Logic.Business.Intefaces;
using Common.Notification.Logic.Business.Notification;
using FluentValidation;
using FluentValidation.Results;

namespace Common.Notification.Logic.Services
{
    public abstract class BaseNotification
    {
        private readonly INotification _notification;

        protected BaseNotification(INotification notification)
        {
            _notification = notification;
        }

        protected void NotificationErrors(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                NotificationErrors(error.ErrorMessage);
            }
        }

        protected void NotificationErrors(string mensagem)
        {
            _notification.Add(new Description(mensagem));
        }

        protected bool ExecutValidation<V, E>(V validacao, E entidade) where V : AbstractValidator<E> where E : class
        {
            var validator = validacao.Validate(entidade);

            if (validator.IsValid) return true;

            NotificationErrors(validator);

            return false;
        }

    }

}
