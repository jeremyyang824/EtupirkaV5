using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Etupirka.Web.Mvc
{
    public class XmlResult : ActionResult
    {
        /// <summary>
        /// 初始化
        /// </summary>         
        public XmlResult() { }
        /// <summary>
        /// Encoding
        /// 编码格式
        /// </summary>
        public Encoding ContentEncoding { get; set; }
        /// <summary>
        /// Gets or sets the type of the content.
        /// 获取或设置返回内容的类型
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// Gets or sets the data
        /// 获取或设置内容
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates whether HTTP GET requests from the client
        /// 获取或设置一个值,指示是否HTTP GET请求从客户端
        /// </summary>
        public XmlRequestBehavior XmlRequestBehavior { get; set; }
        /// <summary>
        /// Enables processing of the result of an action method by a custom type that
        /// 处理结果
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpRequestBase request = context.HttpContext.Request;
            if (XmlRequestBehavior == XmlRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("XmlRequest_GetNotAllowed");
            }
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(this.ContentType) ? this.ContentType : "application/xml";
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (Data != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    XmlSerializer xs = new XmlSerializer(Data.GetType());
                    xs.Serialize(ms, Data); // 把数据序列化到内存流中                     
                    ms.Position = 0;
                    using (StreamReader sr = new StreamReader(ms))
                    {
                        context.HttpContext.Response.Output.Write(sr.ReadToEnd()); // 输出流对象 
                    }
                }
            }
        }
    }
}
