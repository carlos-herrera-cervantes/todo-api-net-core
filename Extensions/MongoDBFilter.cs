using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using MongoDB.Driver;
using TodoApiNet.Models;

namespace TodoApiNet.Extensions
{
    public static class MongoDBFilter<T> where T : class
    {
        #region snippet_GetDocumentWithoutRelationships

        public static async Task<T> GetDocument(IMongoCollection<T> collection, Request queryParameters, List<Relation> relations)
        {
            var ( entities, filters ) = queryParameters;
            var filter = QueryObject.CreateObjectQuery<T>(filters);

            if (entities.IsEmpty())
            {
                return await collection.Find<T>(filter).FirstOrDefaultAsync();
            }
            
            return await GetDocumentWithRelationships(collection, queryParameters, relations);
        }

        #endregion

        #region snippet_GetDocumentWithRelationships

        static async Task<T> GetDocumentWithRelationships(IMongoCollection<T> collection, Request queryParameters, List<Relation> relations)
        {
            var ( _, _, _, entities, filters ) = queryParameters;
            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            var entity = textInfo.ToTitleCase(entities[0]);
            var ( localKey, foreignKey ) = relations.Find(r => r.Entity == entity);
            var query = collection
                .Aggregate()
                .Lookup(entity, localKey, foreignKey, $"{entity}Embedded")
                .Match(filters[0].ToBsonDocument("string"));

            return await query.As<T>().FirstOrDefaultAsync();
        }

        #endregion

        #region snippet_GetDocumentsWithoutRelationships

        public static async Task<IEnumerable<T>> GetDocuments(IMongoCollection<T> collection, Request queryParameters, List<Relation> relations)
        {
            var ( sort, _, _, entities, filters ) = queryParameters;
            var ( _, pageSize, page, _, _ ) = QueryObject.CreateObjectPaginate(queryParameters);
            var filter = QueryObject.CreateObjectQuery<T>(filters);
            var sortQuery = String.IsNullOrEmpty(sort) ? "{}" : QueryObject.CreateObjectQuerySort(sort);
            
            if (entities.IsEmpty())
            {
                return await collection
                    .Find(filter)
                    .Skip(page * pageSize)
                    .Limit(pageSize)
                    .Sort(sortQuery)
                    .ToListAsync();
            }
            
            return await GetDocumentsWithRelationships(collection, queryParameters, relations);
        }

        #endregion

        #region snippet_GetDocumentsWithRelationships

        static async Task<IEnumerable<T>> GetDocumentsWithRelationships(IMongoCollection<T> collection, Request queryParameters, List<Relation> relations)
        {
            var ( sort, _, _, entities, filters ) = queryParameters;
            var ( _, pageSize, page, _, _ ) = QueryObject.CreateObjectPaginate(queryParameters);
            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            var entity = textInfo.ToTitleCase(entities[0]);
            var ( localKey, foreignKey ) = relations.Find(r => r.Entity == entity);
            
            var query = collection
                .Aggregate()
                .Lookup(entity, localKey, foreignKey, $"{entity}Embedded")
                .Skip(page * pageSize);
            
            var isNotEmptyPageSize = pageSize.IsNotEqual(0);
            query = isNotEmptyPageSize ? query.Limit(pageSize) : query;

            var isNotEmptyFilters = !filters.IsEmpty();
            query = isNotEmptyFilters ? query.Match(filters[0].ToBsonDocument("string")) : query;

            var itHasSort = String.IsNullOrEmpty(sort) ? null : QueryObject.CreateObjectQuerySort(sort, "lookup");
            query = itHasSort is null ? query : query.Sort(itHasSort.ToBsonDocument());
            
            return await query.As<T>().ToListAsync();
        }

        #endregion

        #region snippet_CountDocuments

        public static async Task<int> GetNumberOfDocuments(IMongoCollection<T>  collection, Request queryParameters)
        {
            var ( entities, filters ) = queryParameters;
            var filter = QueryObject.CreateObjectQuery<T>(filters);
            var totalDocuments = await collection.CountDocumentsAsync(filter);
            return (int)totalDocuments;
        }

        #endregion
    }
}