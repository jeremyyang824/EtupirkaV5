namespace Etupirka.Web.Mvc
{
    public enum XmlRequestBehavior
    {
        /// <summary>
        /// HTTP GET requests from the client are allowed.
        /// 允许来自客户端的HTTP GET请求
        /// </summary>      
        AllowGet = 0,
        /// <summary>
        /// HTTP GET requests from the client are not allowed.
        /// 不允许来自客户端的HTTP GET请求
        /// </summary>
        DenyGet = 1,
    }
}
