using Common.Notification.Logic.Business.Notification;

namespace Common.Notification.Logic.Business.Intefaces
{
    public interface INotification
    {
        IList<object> List { get; }
        bool HasNotifications { get; }
        bool Includes(Description error);
        void Add(Description error);
    }
}
