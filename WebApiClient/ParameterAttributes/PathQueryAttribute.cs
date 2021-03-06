﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApiClient.Attributes
{
    /// <summary>
    /// 表示将参数值作为url路径参数或query参数的特性
    /// 支持单一值类型如string、int、guid、枚举等，以及他们的可空类型或集合
    /// 支持POCO类型、IDictionaryOf(string,string)类型、IDictionaryOf(string,object)类型
    /// 没有任何特性修饰的普通参数，将默认为PathQuery修饰
    /// 依赖于HttpApiConfig.KeyValueFormatter
    /// 不可继承
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class PathQueryAttribute : Attribute, IApiParameterAttribute
    {
        /// <summary>
        /// 表示Url路径参数或query参数的特性
        /// </summary>
        public PathQueryAttribute()
        {
        }

        /// <summary>
        /// http请求之前
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="parameter">特性关联的参数</param>
        /// <returns></returns>
        public async Task BeforeRequestAsync(ApiActionContext context, ApiParameterDescriptor parameter)
        {
            var uri = context.RequestMessage.RequestUri;
            var url = uri.ToString();
            var relativeUrl = url.Substring(url.IndexOf(uri.AbsolutePath)).TrimEnd('&', '?');
            var keyValues = context.HttpApiConfig.KeyValueFormatter.Serialize(parameter);
            var pathQuery = this.GetPathQuery(relativeUrl, keyValues);

            context.RequestMessage.RequestUri = new Uri(uri, pathQuery);
            await ApiTask.CompletedTask;
        }


        /// <summary>
        /// 获取新的Path与Query
        /// </summary>
        /// <param name="pathQuery">原始path与query</param>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        private string GetPathQuery(string pathQuery, IEnumerable<KeyValuePair<string, string>> keyValues)
        {
            foreach (var keyValue in keyValues)
            {
                pathQuery = this.GetPathQuery(pathQuery, keyValue);
            }
            return pathQuery;
        }

        /// <summary>
        /// 获取新的Path与Query
        /// </summary>
        /// <param name="pathQuery">原始path与query</param>
        /// <param name="keyValue">键值对</param>
        /// <returns></returns>
        private string GetPathQuery(string pathQuery, KeyValuePair<string, string> keyValue)
        {
            var key = keyValue.Key;
            var value = keyValue.Value == null ? string.Empty : keyValue.Value;
            var regex = new Regex("{" + key + "}", RegexOptions.IgnoreCase);

            if (regex.IsMatch(pathQuery) == true)
            {
                return regex.Replace(pathQuery, value);
            }

            var query = string.Format("{0}={1}", key, value);
            var concat = pathQuery.Contains('?') ? "&" : "?";
            return pathQuery + concat + query;
        }
    }
}
