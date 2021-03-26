using System.Collections.Generic;

namespace Rhetos.HttpNotifications
{
    public interface ISubscriptions
    {
        IEnumerable<IHttpNotificationsSubscription> GetSubscriptions(string eventType);
    }
}