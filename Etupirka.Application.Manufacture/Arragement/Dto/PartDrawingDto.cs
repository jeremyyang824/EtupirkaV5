using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Etupirka.Application.Manufacture.Arragement.Dto
{
    /// <summary>
    /// 零件图纸信息（每个版本生成一个）
    /// </summary>
    [XmlRoot("PartDrawing")]
    public class PartDrawingDto
    {
        [XmlElement("PartNumber")]
        public string PartNumber { get; set; }

        [XmlElement("PartVersion")]
        public string PartVersion { get; set; }

        [XmlElement("PartName")]
        public string PartName { get; set; }

        [XmlElement("DocItem")]
        public List<PartDocDrawingDto> PartDocs { get; set; }
    }

    /// <summary>
    /// 文档项
    /// </summary>
    public class PartDocDrawingDto
    {
        [XmlElement("DocNumber")]
        public string DocNumber { get; set; }

        [XmlElement("DocName")]
        public string DocName { get; set; }

        [XmlArray("DocVersions")]
        [XmlArrayItem("Item")]
        public List<PartDocVersionDto> Versions { get; set; }
    }

    /// <summary>
    /// 文档版本
    /// </summary>
    public class PartDocVersionDto
    {
        [XmlAttribute("version")]
        public string DocVersion { get; set; }

        [XmlElement("DownloadUrl3D")]
        public string DownloadUrl3D { get; set; }

        [XmlElement("DownloadUrl2D")]
        public string DownloadUrl2D { get; set; }

        [XmlElement("PublishTime")]
        public DateTime PublishTime { get; set; }

        [XmlElement("DocModifier")]
        public string DocModifier { get; set; }

        [XmlElement("Flag")]
        public string Flag { get; set; }
    }
}
