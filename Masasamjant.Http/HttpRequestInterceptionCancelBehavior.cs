namespace Masasamjant.Http
{
    /// <summary>
    /// Defines behavior of HTTP request interception cancellation.
    /// </summary>
    public enum HttpRequestInterceptionCancelBehavior : int
    {
        /// <summary>
        /// Returns from method after canceling request because of interception.
        /// </summary>
        Return = 0,

        /// <summary>
        /// Throws <see cref="HttpRequestInterceptionException"/> after canceling request because of interception.
        /// </summary>
        Throw = 1
    }
}
