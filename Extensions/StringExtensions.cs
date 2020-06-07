using System;
using System.Text.RegularExpressions;
using MongoDB.Bson;

namespace TodoApiNet.Extensions
{
    public static class StringExtensions
    {
        public static BsonDocument ToBsonDocument(this String value, string type = "int")
        {
            var field = value.Split("=")[0];
            dynamic order = type switch
            {
                "int" => int.Parse(value.Split("=")[1]),
                "string" => value.Split("=")[1],
                _ => ""
            };

            if (order is String)
            {
                var regex = new Regex(@"^[0-9a-fA-F]{24}$").IsMatch(order);
                return new BsonDocument{ { field, regex ? ObjectId.Parse(order) : order } };
            }
            
            return new BsonDocument{ { field, order } };
        }
    }
}