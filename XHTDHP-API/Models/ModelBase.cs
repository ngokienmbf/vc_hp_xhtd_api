namespace XHTDHP_API.Models
{
    public class ModelBase
    {
        /// <summary>
        /// Error code
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// Error Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// User ID (if already login always push userId on each request)
        /// </summary>
        public string UserId { get; set; }
    }

    public class ModelBaseStatus
    {
        public string ErrorCode { get; set; } = "0";

        public string Message { get; set; } = "Successed";
    }

}
