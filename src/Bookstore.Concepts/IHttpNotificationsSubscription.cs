using Rhetos.Dom.DefaultConcepts;

namespace Rhetos.HttpNotifications
{
    public interface IHttpNotificationsSubscription : IEntity
    {
        string EventType { get; set; }
        string CallbackUrl { get; set; }
        string Description { get; set; }
    }
}
