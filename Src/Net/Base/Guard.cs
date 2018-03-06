
namespace Net.Base
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class Guard
    {
        public static void ThrowIfArgumentIsNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void ThrowIfArgumentIsNull(object argumentValue, string argumentName, string message)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName, message);
            }
        }

        public static void ThrowIfArgumentIsNullOrEmpty(string argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
            if (argumentValue.Length == 0)
            {
                throw new ArgumentException("不能为空", argumentName);
            }
        }

        public static void ThrowIfArgumentLessThanZero(int argumentValue, string argumentName)
        {
            if (argumentValue < 0)
            {
                throw new ArgumentException("不能小于0", argumentName);
            }
        }

        public static void ThrowIsArrayEmpty(Array ary, string argumentName)
        {
            if (ary == null)
            {
                throw new ArgumentNullException(argumentName);
            }
            if (ary.Length == 0)
            {
                throw new ArgumentException("数组必须有值", argumentName);
            }
        }

        public static object TryDo(this Delegate method, int tryCount, int interval, params object[] args)
        {
            return method.TryDo(null, null, tryCount, interval, args);
        }

        public static object TryDo(this Delegate method, Type exceptionType, int tryCount, int interval, params object[] args)
        {
            return method.TryDo(exceptionType, null, tryCount, interval, args);
        }

        public static object TryDo(this Delegate method, Action<int> calcback, int tryCount, int interval, params object[] args)
        {
            return method.TryDo(null, calcback, tryCount, interval, args);
        }

        public static object TryDo(this Delegate method, Type exceptionType, Action<int> calcback, int tryCount, int interval, params object[] args)
        {
            for (int i = 0; i < tryCount; i++)
            {
                try
                {
                    return method.DynamicInvoke(args);
                }
                catch (Exception exception)
                {
                    if (((exceptionType != null) && !(exception.GetType() == exceptionType)) && ((exception.InnerException == null) || !(exception.InnerException.GetType() == exceptionType)))
                    {
                        throw new Exception(exception.InnerException.Message);
                    }
                    if (calcback != null)
                    {
                        calcback(i);
                    }
                    if (i == (tryCount - 1))
                    {
                        throw new Exception(exception.InnerException.Message);
                    }
                    Thread.Sleep(interval);
                }
            }
            return null;
        }
    }
}
