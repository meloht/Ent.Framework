using System;
using System.Collections.Generic;
using System.Text;

namespace Ent.Framework.Cache
{
    public interface IEntityCache
    {
        bool Set(string key, string entity, TimeSpan exp);
        bool Set(string key, string entity);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存对象键</param>
        /// <param name="entity">缓存对象值</param>
        /// <param name="exp">过期时间</param>
        bool Set<T>(string key, T entity, TimeSpan exp) where T : class;

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存对象键</param>
        /// <param name="entity">缓存对象值</param>
        bool Set<T>(string key, T entity) where T : class;

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key) where T : class;

        string Get(string key);

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        /// <param name="key"></param>
        bool Remove(string key);


    }
}
