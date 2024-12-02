using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace NegocioPDF.Models
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; }

        [BsonElement("correo")]
        public string Correo { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }
    }
}
