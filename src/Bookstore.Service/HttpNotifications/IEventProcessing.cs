namespace Rhetos.Events
{
    /// <summary>
    /// This interface represents an event channel, and is implemented by the central event processing component.
    /// The processing component is extended by code generators, to contain various internal event handlers.
    /// This is similar to the publish-subscribe pattern, but with subscribers specified at build-time.
    /// The event handlers are executed synchronously (they might generate asynchronous tasks or background jobs).
    /// </summary>
    public interface IEventProcessing
    {
        /// <summary>
        /// Emitting an event allows various event handlers to process them (for example, HTTP notifications to external system).
        /// </summary>
        /// <remarks>
        /// There is no need to use event processing and the <see cref="EmitEvent"/> method for a specific event that is intended for a single specific event handler.
        /// The emitted events are expected to be handled by generic event handlers (for example, a handler that manages run-time event subscriptions),
        /// or to allow implementation of an event handler that has no information on which component emits the event.
        /// </remarks>
        void EmitEvent(RhetosEventType eventType, object eventData);
    }
}