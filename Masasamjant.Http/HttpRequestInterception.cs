namespace Masasamjant.Http
{
    /// <summary>
    /// Defines what happens after HTTP request interception.
    /// </summary>
    public enum HttpRequestInterception : int
    {
        /// <summary>
        /// Continue request after interception.
        /// </summary>
        Continue = 0,

        /// <summary>
        /// Cancel request after interception.
        /// </summary>
        Cancel = 1
    }
}
