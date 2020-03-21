using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
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

        #endregion

        #region snippet_ForeignKeys

        public List<string> Todos { get; set; } = new List<string>();

        #endregion
    }
}
