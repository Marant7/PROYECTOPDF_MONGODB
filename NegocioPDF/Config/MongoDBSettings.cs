using System;

namespace NegocioPDF.Config
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string UsuariosCollection { get; set; } = string.Empty;
        public string OperacionesCollection { get; set; } = string.Empty;
        public string SuscripcionesCollection { get; set; } = string.Empty;
    }
}