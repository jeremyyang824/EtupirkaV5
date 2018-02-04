using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Etupirka.Domain.Portal.Utils
{
    /// <summary>
    /// DataTable扩展方法
    /// </summary>
    public static class DataTableExtension
    {
        /// <summary>
        /// 将泛型对象List（中的指定属性集合）转换成DataTable
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="entitys">对象列表</param>
        /// <param name="propertyDic">过滤指定属性集合</param>
        /// <returns>DataTable对象</returns>
        public static DataTable ToDataTable<T>(this IList<T> entitys, Dictionary<string, string> propertyDic)
        {
            if (propertyDic == null || propertyDic.Count < 1)
                return ToDataTable<T>(entitys, (Func<PropertyInfo[], ColumnDefine[]>)null);

            int idx = 0;
            Dictionary<string, int> idxDic = propertyDic.Keys.ToDictionary(d => d, d => ++idx);

            return ToDataTable<T>(entitys, (piArray =>
            {
                return (from pi in piArray
                        where propertyDic.ContainsKey(pi.Name)
                        select new ColumnDefine
                        {
                            Property = pi,
                            ColumnName = propertyDic[pi.Name],
                            ColumnIdx = idxDic[pi.Name]
                        });
            }));
        }

        /// <summary>
        /// 将泛型对象List（中的指定属性集合）转换成DataTable
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="entitys">对象列表</param>
        /// <param name="propertyDic">过滤指定属性集合</param>
        /// <returns>DataTable对象</returns>
        public static DataTable ToDataTable<T>(this IList<T> entitys, Dictionary<string, PropertyConventer> propertyDic)
        {
            if (propertyDic == null || propertyDic.Count < 1)
                return ToDataTable<T>(entitys, (Func<PropertyInfo[], ColumnDefine[]>)null);

            int idx = 0;
            Dictionary<string, int> idxDic = propertyDic.Keys.ToDictionary(d => d, d => ++idx);

            return ToDataTable<T>(entitys, (piArray =>
            {
                return (from pi in piArray
                        where propertyDic.ContainsKey(pi.Name)
                        select new ColumnDefine
                        {
                            Property = pi,
                            ColumnName = propertyDic[pi.Name].ColumnName,
                            ColumnIdx = idxDic[pi.Name],
                            ValueConventer = propertyDic[pi.Name].ValueConventer
                        });
            }));
        }

        /// <summary>
        /// 将泛型对象List（中的指定属性集合）转换成DataTable
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="entitys">对象列表</param>
        /// <param name="propertyFilter">过滤指定属性集合</param>
        /// <returns>DataTable对象</returns>
        public static DataTable ToDataTable<T>(this IList<T> entitys, Func<PropertyInfo[], IEnumerable<ColumnDefine>> propertyFilter)
        {
            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                throw new Exception("需转换的集合为空");
            }

            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            ColumnDefine[] columnDefineArr;   //[属性成员,列名]
            if (propertyFilter != null)
                columnDefineArr = propertyFilter(entityProperties)
                    .OrderBy(cd => cd.ColumnIdx)
                    .ToArray();
            else
                columnDefineArr = entityProperties
                    .Select(pi => new ColumnDefine { Property = pi, ColumnName = pi.Name, ValueConventer = null })
                    .OrderBy(cd => cd.ColumnIdx)
                    .ToArray();    //所有属性

            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable();
            for (int i = 0; i < columnDefineArr.Length; i++)
            {
                dt.Columns.Add(columnDefineArr[i].ColumnName);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[columnDefineArr.Length];
                for (int i = 0; i < columnDefineArr.Length; i++)
                {
                    object columnValue = columnDefineArr[i].Property.GetValue(entity, null);    //原始值

                    Func<object, object> tempValueConventer = columnDefineArr[i].ValueConventer;
                    if (tempValueConventer != null)
                        columnValue = tempValueConventer(columnValue);  //值转换

                    entityValues[i] = columnValue;
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }

        #region 内部类

        public class ColumnDefine
        {
            private PropertyInfo property;
            private int columnIdx = 99;
            private string columnName;
            private Func<object, object> valueConventer;

            /// <summary>
            /// 对象属性
            /// </summary>
            public PropertyInfo Property
            {
                get { return this.property; }
                set { this.property = value; }
            }
            /// <summary>
            /// 列顺序
            /// </summary>
            public int ColumnIdx
            {
                get { return this.columnIdx; }
                set { this.columnIdx = value; }
            }
            /// <summary>
            /// 转换后的列名
            /// </summary>
            public string ColumnName
            {
                get { return this.columnName; }
                set { this.columnName = value; }
            }
            /// <summary>
            /// 值转换器
            /// </summary>
            public Func<object, object> ValueConventer
            {
                get { return this.valueConventer; }
                set { this.valueConventer = value; }
            }

            #region 重写比较(比较Property)
            public override int GetHashCode()
            {
                return this.property.Name.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }
                if (obj.GetType() == this.GetType())
                {
                    return obj.GetHashCode() == this.GetHashCode();
                }
                return false;
            }
            public static bool operator ==(ColumnDefine first, ColumnDefine second)
            {
                if (Object.ReferenceEquals(first, second))
                {
                    return true;
                }
                if ((object)first == null || (object)second == null)
                {
                    return false;
                }
                return first.GetHashCode() == second.GetHashCode();
            }
            public static bool operator !=(ColumnDefine first, ColumnDefine second)
            {
                return !(first == second);
            }
            #endregion
        }

        public class PropertyConventer
        {
            /// <summary>
            /// 转换后的列名
            /// </summary>
            public string ColumnName { get; set; }

            /// <summary>
            /// 值转换器
            /// </summary>
            public Func<object, object> ValueConventer { get; set; }

            public PropertyConventer(string columnName, Func<object, object> valueConventer)
            {
                this.ColumnName = columnName;
                this.ValueConventer = valueConventer;
            }
        }

        #endregion
    }

}
