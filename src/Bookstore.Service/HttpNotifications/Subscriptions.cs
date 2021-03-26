using Rhetos.Dom.DefaultConcepts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhetos.HttpNotifications
{
    public class Subscriptions: ISubscriptions
    {
        private readonly IQueryableRepository<IHttpNotificationsSubscription> _subscriptionRepository;

        public Subscriptions(IQueryableRepository<IHttpNotificationsSubscription> subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public IEnumerable<IHttpNotificationsSubscription> GetSubscriptions(string eventType)
        {
            // TODO: Cache.

            return _subscriptionRepository.Query(s => s.EventType == eventType).ToList();
        }
    }
}