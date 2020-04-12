using System;
using MongoDB.Driver;

namespace TodoApiNet.Extensions
{
    public static class QueryObject<T> where T : class
    {
        #region snippet_CreateFilterForQuery

        public static FilterDefinition<T> CreateObjectQuery(string filter)
        {
            var builder = Builders<T>.Filter;

            if (String.IsNullOrEmpty(filter)) { return builder.Exists("_id", true); }

            var query = filter.Split('-');
            
            return builder.Eq(query[0], query[1]);
        }

        #endregion

        #region snippet_CreateObjectForSort

        public static string CreateObjectQuerySort(string field)
        {
            var isAscending = field.Contains('-');
            var queryField = isAscending ? field.Split('-')[1] : field;
            var objectQuerySort = String.Format("{{ {0}: {1} }}", queryField, isAscending ? -1 : 1);
            
            return objectQuerySort;
        }

        #endregion
    }
}