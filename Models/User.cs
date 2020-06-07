using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TodoApiNet.Models
{
    public class User
    {
        #region snippet_Properties

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "FirstNameRequired")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastNameRequired")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "EmailRequired")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        #endregion

        #region snippet_ForeignKeys

        public List<string> Todos { get; set; } = new List<string>();

        [BsonIgnoreIfNull]
        [JsonProperty("todosEmbedded", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<Todo> TodosEmbedded { get; set; } = null;

        #endregion

        #region snippet_Relations

        [BsonIgnore]
        [JsonIgnore]
        public List<Relation> Relations { get; set; } = new List<Relation>
        {
            new Relation 
            {
                Entity = "Todos",
                LocalKey = "_id",
                ForeignKey = "UserId"
            }
        };

        #endregion
    }
}
