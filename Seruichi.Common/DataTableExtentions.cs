using System;
using System.Collections.Generic;
using System.Data;

namespace Seruichi.Common
{
    public static class DataTableExtentions
    {
        public static T ToEntity<T>(this DataRow row) where T : new()
        {
            var obj = new T();
            var properties = typeof(T).GetProperties();
            var columns = row.Table.Columns;

            foreach (var property in properties)
            {
                if (!columns.Contains(property.Name)) continue;

                var value = row[property.Name];
                if (value == DBNull.Value)
                {
                    property.SetValue(obj, null);
                }
                else
                {
                    property.SetValue(obj, row[property.Name]);
                }
            }

            return obj;

        }

        public static IEnumerable<T> AsEnumerableEntity<T>(this DataTable dt) where T : new()
        {
            int i = 0;

            while (i < dt.Rows.Count)
            {
                var r = dt.Rows[i].ToEntity<T>();
                i++;
                yield return r;
            }
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> valuesList)
        {
            DataTable dt = new DataTable();
            var objectReference = valuesList.GetType().GetGenericArguments()[0];
            var properties = objectReference.GetProperties();
            foreach (var prop in properties)
            {
                dt.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in valuesList)
            {
                var dataArray = new List<object>();
                foreach (var prop in properties)
                {
                    dataArray.Add(prop.GetValue(item));
                }

                dt.Rows.Add(dataArray.ToArray());
            }

            return dt;
        }
    }
}
