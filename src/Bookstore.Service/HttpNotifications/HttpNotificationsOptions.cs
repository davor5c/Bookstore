namespace Rhetos.HttpNotifications
{
    [Options("HttpNotifications")]
    public class HttpNotificationsOptions
    {
        /// <summary>
        /// Suppress all notification.
        /// Note that this option can also be configured per scope (per web request) in <see cref="HttpNotificationsDispatcher"/> class.
        /// </summary>
        public bool SuppressAll { get; set; }

        /// <summary>
        /// Suppress notification for specific event types.
        /// Note that this option can also be configured per scope (per web request) in <see cref="HttpNotificationsDispatcher"/> class.
        /// </summary>
        public string[] SuppressEventTypes { get; set; }
    }
}