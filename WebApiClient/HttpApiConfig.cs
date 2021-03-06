﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApiClient
{
    /// <summary>
    /// 表示Http接口的配置项
    /// 更多的配置项，可以继承此类
    /// </summary>
    public class HttpApiConfig : IDisposable
    {
        /// <summary>
        /// 获取默认xml格式化工具唯一实例
        /// </summary>
        public static readonly IStringFormatter DefaultXmlFormatter = new DefaultXmlFormatter();

        /// <summary>
        /// 获取默认json格式化工具唯一实例
        /// </summary>
        public static readonly IStringFormatter DefaultJsonFormatter = new DefaultJsonFormatter();

        /// <summary>
        /// 获取默认KeyValue格式化工具唯一实例
        /// </summary>
        public static readonly IKeyValueFormatter DefaultKeyValueFormatter = new DefaultKeyValueFormatter();


        /// <summary>
        /// 与HttpClientHandler实例关联的HttpClient
        /// </summary>
        private HttpClient httpClient;

        /// <summary>
        /// Http处理程序
        /// </summary>
        private HttpClientHandler httpClientHandler;

        /// <summary>
        /// 获取或设置Http服务完整主机域名
        /// 例如http://www.webapiclient.com
        /// 设置了HttpHost值，HttpHostAttribute将失效  
        /// </summary>
        public Uri HttpHost { get; set; }

        /// <summary>
        /// 获取或设置Xml格式化工具
        /// </summary>
        public IStringFormatter XmlFormatter { get; set; }

        /// <summary>
        /// 获取或设置Json格式化工具
        /// </summary>
        public IStringFormatter JsonFormatter { get; set; }

        /// <summary>
        /// 获取或设置KeyValue格式化工具
        /// </summary>
        public IKeyValueFormatter KeyValueFormatter { get; set; }


        /// <summary>
        /// 获取或设置Http处理程序
        /// </summary>
        public HttpClientHandler HttpClientHandler
        {
            get
            {
                if (this.httpClientHandler == null)
                {
                    this.httpClientHandler = new DefaultHttpClientHandler();
                }
                return this.httpClientHandler;
            }
            set
            {
                this.httpClientHandler = value;
            }
        }

        /// <summary>
        /// 获取或设置与HttpClientHandler实例关联的HttpClient
        /// </summary>
        public HttpClient HttpClient
        {
            get
            {
                if (this.httpClient == null)
                {
                    this.httpClient = new HttpClient(this.HttpClientHandler);
                }
                return this.httpClient;
            }
            set
            {
                this.httpClient = value;
            }
        }

        /// <summary>
        /// Http接口的配置项   
        /// </summary>
        public HttpApiConfig()
        {
            this.XmlFormatter = HttpApiConfig.DefaultXmlFormatter;
            this.JsonFormatter = HttpApiConfig.DefaultJsonFormatter;
            this.KeyValueFormatter = HttpApiConfig.DefaultKeyValueFormatter;
        }

        #region IDisposable
        /// <summary>
        /// 获取对象是否已释放
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// 关闭和释放所有相关资源
        /// </summary>
        public void Dispose()
        {
            if (this.IsDisposed == false)
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
            this.IsDisposed = true;
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~HttpApiConfig()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否也释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.HttpClient != null)
            {
                this.HttpClient.Dispose();
            }

            if (disposing == true)
            {
                this.XmlFormatter = null;
                this.JsonFormatter = null;
                this.KeyValueFormatter = null;
                this.HttpClient = null;
                this.HttpClientHandler = null;
                this.HttpHost = null;
            }
        }
        #endregion
    }
}
