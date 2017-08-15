namespace ArcTouch.TestDevelopment.Service.ApiObjects
{
    public class Error
    {
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
    }
}
