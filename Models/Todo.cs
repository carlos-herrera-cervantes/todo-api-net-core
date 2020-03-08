using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoApiNet.Models
{
    public class Todo
    {
        #region snippet_Properties

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public bool Done { get; set; } = false;

        #endregion

        #region snippet_ForeignKeys

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        #endregion
    }
}
