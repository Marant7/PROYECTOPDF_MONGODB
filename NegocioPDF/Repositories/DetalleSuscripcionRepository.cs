using MongoDB.Driver;
using Microsoft.Extensions.Options;
using NegocioPDF.Config;
using NegocioPDF.Models;
using System;
using System.Threading.Tasks;

namespace NegocioPDF.Repositories
{
    public class DetalleSuscripcionRepository
    {
        private readonly IMongoCollection<DetalleSuscripcion> _suscripcionesCollection;
        private readonly IMongoCollection<Usuario> _usuariosCollection;

        public DetalleSuscripcionRepository(IOptions<MongoDBSettings> mongoSettings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
            _suscripcionesCollection = database.GetCollection<DetalleSuscripcion>(mongoSettings.Value.SuscripcionesCollection);
            _usuariosCollection = database.GetCollection<Usuario>(mongoSettings.Value.UsuariosCollection);
        }

        public async Task<DetalleSuscripcion> ObtenerPorUsuarioId(string usuarioId)
        {
            var suscripcion = await _suscripcionesCollection
                .Find(s => s.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (suscripcion != null)
            {
                suscripcion.Usuario = await _usuariosCollection
                    .Find(u => u.Id == usuarioId)
                    .FirstOrDefaultAsync();
            }

            return suscripcion;
        }

        public async Task ActualizarSuscripcion(DetalleSuscripcion suscripcion)
{
    try
    {
        var filter = Builders<DetalleSuscripcion>.Filter.Eq(s => s.UsuarioId, suscripcion.UsuarioId);
        var update = Builders<DetalleSuscripcion>.Update
            .Set(s => s.tipo_suscripcion, suscripcion.tipo_suscripcion)
            .Set(s => s.fecha_inicio, suscripcion.fecha_inicio)
            .Set(s => s.fecha_final, suscripcion.fecha_final)
            .Set(s => s.precio, suscripcion.precio)
            .Set(s => s.operaciones_realizadas, suscripcion.operaciones_realizadas);

        var options = new UpdateOptions { IsUpsert = true };
        
        await _suscripcionesCollection.UpdateOneAsync(filter, update, options);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error en ActualizarSuscripcion: {ex.Message}");
        throw;
    }
}
    }
}