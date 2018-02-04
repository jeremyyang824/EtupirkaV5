using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Etupirka.Web.Mvc
{
    public static class ControllerExtension
    {
        public static XmlResult Xml(this Controller request, object obj)
        {
            return Xml(obj, null, null, XmlRequestBehavior.DenyGet);
        }

        public static XmlResult Xml(this Controller request, object obj, XmlRequestBehavior behavior)
        {
            return Xml(obj, null, null, behavior);
        }

        public static XmlResult Xml(this Controller request, object obj, Encoding contentEncoding,
            XmlRequestBehavior behavior)
        {
            return Xml(obj, null, contentEncoding, behavior);
        }

        public static XmlResult Xml(this Controller request, object obj, string contentType, Encoding contentEncoding,
            XmlRequestBehavior behavior)
        {
            return Xml(obj, contentType, contentEncoding, behavior);
        }

        internal static XmlResult Xml(object data, string contentType, Encoding contentEncoding,
            XmlRequestBehavior behavior)
        {
            return new XmlResult()
            {
                ContentEncoding = contentEncoding,
                ContentType = contentType,
                Data = data,
                XmlRequestBehavior = behavior
            };
        }
    }
}