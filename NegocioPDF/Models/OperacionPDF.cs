using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace NegocioPDF.Models
{
   public class OperacionPDF
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("usuario_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UsuarioId { get; set; }

        [BsonElement("tipo_operacion")]
        public string TipoOperacion { get; set; }

        [BsonElement("fecha_operacion")]
        public DateTime FechaOperacion { get; set; }
    }
}
