namespace Core
{
    public class Result
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }

        public static Result Error(string message)
        {
            return new Result() { IsSucceed = false, Message = message };
        }

        public static Result Success(string message)
        {
            return new Result() { IsSucceed = true, Message = message };
        }

        public static Result Success()
        {
            return Success(null);
        }

        public override string ToString()
        {
            return  Message;
        }
    }
}