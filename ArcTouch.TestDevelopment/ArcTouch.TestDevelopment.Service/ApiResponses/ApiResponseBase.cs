using ArcTouch.TestDevelopment.Service.ApiObjects;

namespace ArcTouch.TestDevelopment.Service.ApiResponses
{
    public abstract class APIResponseBase<T> where T : new()
    {
        public Error Error { get; set; }
        public T Response { get; set; }

        public APIResponseBase()
        {
            Response = new T();
        }
    }
}
