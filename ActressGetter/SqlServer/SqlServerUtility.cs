using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace ActressGetter.SqlServer
{
    public static class SqlServerUtilityExtention
    {
        public static void Insert<T>(this SqlConnection sqlConnection, T data)
            => sqlConnection.Execute(data.GetInsertSql(), data.GetDynamicParameters());

        public static void Insert<T>(this SqlConnection sqlConnection, IEnumerable<T> data)
            => sqlConnection.Execute(data.First().GetInsertSql(), data.Select(x => x.GetDynamicParameters()));

        public static void Update<T>(this SqlConnection sqlConnection, T data, string key)
            => sqlConnection.Execute(data.GetUpdateSql(key), data.GetDynamicParameters());

        public static void Update<T>(this SqlConnection sqlConnection, IEnumerable<T> data, string key)
            => sqlConnection.Execute(data.First().GetUpdateSql(key), data.Select(x => x.GetDynamicParameters()));

        public static void Delete<T>(this SqlConnection sqlConnection, T data, string key)
            => sqlConnection.Execute(data.GetDeleteSql(key), data.GetDynamicParameters());

        public static void Delete<T>(this SqlConnection sqlConnection, IEnumerable<T> data, string key)
            => sqlConnection.Execute(data.First().GetDeleteSql(key), data.Select(x => x.GetDynamicParameters()));

        private static string GetInsertSql<T>(this T data)
        {
            var properties = data.GetPropertyInfoSkipId();
            var names = string.Join(",", properties.Select(x => x.Name));
            var atNames = string.Join(",", properties.Select(x => "@" + x.Name));
            return $@"insert into {data?.GetType()?.Name} ({names}) values ({atNames});";
        }

        public static string GetUpdateSql<T>(this T data, string key)
        {
            var properties = data.GetPropertyInfoSkipId();
            var names = string.Join(",", properties.Select(x => $"{x.Name} = @{x.Name}"));
            return $@"update {data?.GetType()?.Name} set {names} where {key} = @{key};";
        }

        private static string GetDeleteSql<T>(this T data, string key)
            => $@"delete from {data?.GetType()?.Name} where {key} = @{key};";

        private static DynamicParameters GetDynamicParameters<T>(this T data)
            => new DynamicParameters(data.GetPropertyInfoSkipId().ToDictionary(p => $"@{p.Name}", p => p.GetValue(data)));

        //最初のIDは飛ばす
        private static IEnumerable<PropertyInfo> GetPropertyInfoSkipId<T>(this T data) => data?.GetType()?.GetProperties().Skip(1)
            ?? new List<PropertyInfo>();
    }
}
