namespace FinRost.BL.Extensions
{
    public class CustomException : Exception
    {
        public int StatusCode { get; set; }

        public CustomException()
        {
            StatusCode = 400;
        }

        public CustomException(string message)
            : base(message)
        {

        }
    }
}
