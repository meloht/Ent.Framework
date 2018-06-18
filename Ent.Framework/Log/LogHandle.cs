using System;
using System.Collections.Generic;
using System.Text;

namespace Ent.Framework.Log
{
    public static class LogHandle
    {
        static LogHandle()
        {
            Log = Ioc.ServiceLocator.GetInstance<ILogWrapper>();
        }

        private static readonly ILogWrapper Log;

        /// <summary>
        /// 结束日志记录线程
        /// </summary>
        public static void ThreadStop()
        {
            Log.ThreadStop();
        }

        /// <summary>
        /// 输出提示信息
        /// </summary>
        /// <param name="message">提示信息</param>
        public static void Info(object message)
        {
            Info(message, null);
        }

        /// <summary>
        /// 输出提示信息
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <param name="source">触发提示信息的异常</param>
        public static void Info(object message, Exception source)
        {
            Log.Info(message, source);
        }

        /// <summary>
        /// 输出调试信息
        /// </summary>
        /// <param name="message">调试信息</param>
        public static void Debug(object message)
        {
            Debug(message, null);
        }

        /// <summary>
        /// 输出调试信息
        /// </summary>
        /// <param name="message">调试信息</param>
        /// <param name="source">触发调试信息的异常</param>
        public static void Debug(object message, Exception source)
        {
            Log.Debug(message, source);
        }

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="message">错误信息</param>
        public static void Error(object message)
        {
            Error(message, null);
        }

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="source">触发错误信息的异常</param>
        public static void Error(object message, Exception source)
        {
            Log.Error(message, source);
        }

        /// <summary>
        /// 输出警告信息
        /// </summary>
        /// <param name="message">警告信息</param>
        public static void Warn(object message)
        {
            Warn(message, null);
        }

        /// <summary>
        /// 输出警告信息
        /// </summary>
        /// <param name="message">警告信息</param>
        /// <param name="source">触发警告信息的异常</param>
        public static void Warn(object message, Exception source)
        {
            Log.Warn(message, source);
        }

        /// <summary>
        /// 记录操作日志
        /// </summary>
        public static void SaveOperLog(LogOperInfo info)
        {
            Log.SaveOperLog(info);
        }


    }
}
