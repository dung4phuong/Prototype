
using NUnit.Framework;
using System;
using System.Runtime.Serialization;

namespace  Theorem
{
    [Serializable]
    public class WaitTimeoutException : Exception
    {
        public WaitTimeoutException(string message) : base(message)
        {
            Assert.Fail($"Failed with Waiter Exception: { message }");
        }
        public WaitTimeoutException()
            : base()
        {
            Assert.Fail("Failed with Waiter Exception");

        }
        public WaitTimeoutException(string format, params object[] args)
            : base(string.Format(format, args))
        {
            Assert.Fail("Failed with Waiter Exception: " + format);
        }
        public WaitTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
            Assert.Fail("Failed with Waiter Exception: " + message);
        }
        public WaitTimeoutException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
            Assert.Fail("Failed with Waiter Exception: " + format);
        }
        protected WaitTimeoutException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Assert.Fail("Failed with Waiter Exception: " + info);
        }
    }
}
