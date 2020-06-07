using System;
using MongoDB.Driver;
using TodoApiNet.Models;

namespace TodoApiNet.Extensions
{
    public static class QueryObject
    {
        #region snippet_CreateFilterForQuery

        public static FilterDefinition<T> CreateObjectQuery<T>(string[] filter) where T : class
        {
            var builder = Builders<T>.Filter;
            
            if (filter.IsEmpty()) { return builder.Exists("_id", true); }

            var query = filter[0].Split('=');
            
            return builder.Eq(query[0], query[1]);
        }

        #endregion

        #region snippet_CreateObjectForSort

        public static string CreateObjectQuerySort(string field, string typeQuery = "find")
        {
            var isAscending = field.Contains('-');
            var queryField = isAscending ? field.Split('-')[1] : field;
            var order = isAscending ? -1 : 1;

            if (typeQuery.ToLowerInvariant().Equals("find"))
            {
                return String.Format("{{ {0}: {1} }}", queryField, order);
            }
            
            return $"{queryField}={order}";
        }

        #endregion

        #region snippet_CreateObjectPaginate

        public static Request CreateObjectPaginate(Request queryParameters)
        {
            var ( pageSize, page, paginate ) = queryParameters;
            if (paginate)
            {
                queryParameters.Page = page == 1 ? 0 : page == 0 ? 0 : page - 1;
                return queryParameters;
            }

            queryParameters.PageSize = 0;
            return queryParameters;
        }

        #endregion
    }
}