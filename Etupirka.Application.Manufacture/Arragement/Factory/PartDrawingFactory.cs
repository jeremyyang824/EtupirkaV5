using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Etupirka.Application.Manufacture.Arragement.Dto;
using Etupirka.Domain.External.Entities.Winchill;

namespace Etupirka.Application.Manufacture.Arragement.Factory
{
    public static class PartDrawingFactory
    {
        /// <summary>
        /// 创建PartDrawingDto
        /// </summary>
        public static List<PartDrawingDto> Create(IEnumerable<PartItemDoc> sourceData)
        {
            /*
            return sourceData?
                .GroupBy(item => new { item.PartNumber, item.PartVersion, item.PartName })
                .Select(grp => new PartDrawingDto
                {
                    PartNumber = grp.Key.PartNumber,
                    PartVersion = grp.Key.PartVersion,
                    PartName = grp.Key.PartName,
                    //文档版本
                    Versions = buildVersionList(grp)
                })
                .OrderBy(d => d.PartNumber)
                .ThenBy(d => d.PartVersion)
                .ToList();
            */

            return sourceData?
                .GroupBy(item => new { item.PartNumber, item.PartVersion, item.PartName })
                .Select(grp => new PartDrawingDto
                {
                    PartNumber = grp.Key.PartNumber,
                    PartVersion = grp.Key.PartVersion,
                    PartName = grp.Key.PartName,
                    //文档项
                    PartDocs = buildDocList(grp)
                })
                .OrderBy(d => d.PartNumber)
                .ThenBy(d => d.PartVersion)
                .ToList();
        }

        private static List<PartDocDrawingDto> buildDocList(IEnumerable<PartItemDoc> partDocs)
        {
            return partDocs
                .GroupBy(item => new { item.DocNumber, item.DocName })
                .Select(grp => new PartDocDrawingDto
                {
                    DocNumber = grp.Key.DocNumber,
                    DocName = grp.Key.DocName,
                    //文档版本
                    Versions = grp.Select(item => new PartDocVersionDto
                    {
                        DocVersion = item.DocVersion,
                        DownloadUrl3D = item.DownloadUrl3D,
                        DownloadUrl2D = item.DownloadUrl2D,
                        PublishTime = item.PublishTime,
                        DocModifier = item.DocModifier,
                        Flag = item.Flag
                    })
                    .OrderBy(item => item.DocVersion)
                    .ToList()
                })
                .OrderBy(d => d.DocNumber)
                .ToList();
        }

        /*
        private static List<PartDocVersionDto> buildVersionList(IEnumerable<PartItemDoc> partVersionDocs)
        {
            
            return partVersionDocs
                .GroupBy(item => item.DocVersion)
                .Select(grp => new PartDocVersionDto
                {
                    DocVersion = grp.Key,
                    //文档项
                    Docs = grp.Select(item => new PartDocDrawingDto
                    {
                        DocNumber = item.DocNumber,
                        DocName = item.DocName,
                        DownloadUrl3D = item.DownloadUrl3D,
                        DownloadUrl2D = item.DownloadUrl2D,
                        PublishTime = item.PublishTime,
                        DocModifier = item.DocModifier,
                        Flag = item.Flag
                    })
                    .OrderBy(item => item.DocName)
                    .ToList()
                })
                .OrderBy(d => d.DocVersion)
                .ToList();
            
        }
        */

        /// <summary>
        /// 从XML反序列化
        /// </summary>
        public static PartDrawingDto DeserializeFromXML(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                throw new ArgumentNullException(nameof(xml));

            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer xmldes = new XmlSerializer(typeof(PartDrawingDto));
                return (PartDrawingDto)xmldes.Deserialize(sr);
            }
        }

        /// <summary>
        /// 序列化到XML
        /// </summary>
        public static string SerializerToXML(PartDrawingDto data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xmlser = new XmlSerializer(typeof(PartDrawingDto));
                xmlser.Serialize(ms, data);

                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
