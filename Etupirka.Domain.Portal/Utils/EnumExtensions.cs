using System;
using System.ComponentModel;
using System.Reflection;

namespace Etupirka.Domain.Portal.Utils
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举描述信息
        /// </summary>
        public static string GetDescription(this Enum enumSubitem)
        {
            string strValue = enumSubitem.ToString();
            FieldInfo fieldinfo = enumSubitem.GetType().GetField(strValue);
            object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs.Length == 0)
            {
                return strValue;
            }
            else
            {
                DescriptionAttribute da = (DescriptionAttribute)objs[0];
                return da.Description;
            }
        }
    }
}
