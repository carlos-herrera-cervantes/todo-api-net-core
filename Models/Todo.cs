using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace TodoApiNet.Models
{
    public class Todo
    {
        #region snippet_Properties

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "TitleRequired")]
        [StringLength(50)]
        public string Title { get; set; }

        [Required(ErrorMessage = "DescriptionRequired")]
        public string Description { get; set; }

        public bool Done { get; set; } = false;

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        #endregion

        #region snippet_ForeignKeys

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonIgnoreIfNull]
        [JsonProperty("usersEmbedded", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<User> UsersEmbedded { get; set; } = null;

        #endregion

        #region snippet_Relations

        [BsonIgnore]
        [JsonIgnore]
        public List<Relation> Relations { get; set; } = new List<Relation>
        {
            new Relation
            {
                Entity = "Users",
                LocalKey = "UserId",
                ForeignKey = "_id"
            }
        };

        #endregion
    }
}
