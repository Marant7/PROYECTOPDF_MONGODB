using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace NegocioPDF.Models
{
    public class DetalleSuscripcion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("usuario_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UsuarioId { get; set; }

        [BsonElement("tipo_suscripcion")]
        public string tipo_suscripcion { get; set; }

        [BsonElement("fecha_inicio")]
        public DateTime? fecha_inicio { get; set; }

        [BsonElement("fecha_final")]
        public DateTime? fecha_final { get; set; }

        [BsonElement("precio")]
        public decimal? precio { get; set; }

        [BsonElement("operaciones_realizadas")]
        public int operaciones_realizadas { get; set; }

        [BsonIgnore]
        public Usuario Usuario { get; set; }
    }
}