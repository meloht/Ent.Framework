using System;
using System.Collections.Generic;
using System.Text;

namespace Ent.Framework.Log
{
    public interface ILogWrapper
    {
        #region 公共方法

        /// <summary>
        /// 结束日志记录线程
        /// </summary>
        void ThreadStop();

        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="message"></param>
        void Trace(object message);

        void Trace(object message, Exception source);
        /// <summary>
        /// 输出提示信息
        /// </summary>
        /// <param name="message">提示信息</param>
        void Info(object message);

        /// <summary>
        /// 输出提示信息
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <param name="source">触发提示信息的异常</param>
        void Info(object message, Exception source);

        /// <summary>
        /// 输出调试信息
        /// </summary>
        /// <param name="message">调试信息</param>
        void Debug(object message);

        /// <summary>
        /// 输出调试信息
        /// </summary>
        /// <param name="message">调试信息</param>
        /// <param name="source">触发调试信息的异常</param>
        void Debug(object message, Exception source);

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="message">错误信息</param>
        void Error(object message);

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="source">触发错误信息的异常</param>
        void Error(object message, Exception source);

        /// <summary>
        /// 输出警告信息
        /// </summary>
        /// <param name="message">警告信息</param>
        void Warn(object message);

        /// <summary>
        /// 输出警告信息
        /// </summary>
        /// <param name="message">警告信息</param>
        /// <param name="source">触发警告信息的异常</param>
        void Warn(object message, Exception source);

        /// <summary>
        /// 输出严重错误信息
        /// </summary>
        /// <param name="message">严重错误信息</param>
        void Fatal(object message);

        /// <summary>
        /// 输出严重错误信息
        /// </summary>
        /// <param name="message">严重错误信息</param>
        /// <param name="source">触发严重错误的异常</param>
        void Fatal(object message, Exception source);

        /// <summary>
        /// 记录操作日志
        /// </summary>
        void SaveOperLog(LogOperInfo info);

        #endregion
    }
}
