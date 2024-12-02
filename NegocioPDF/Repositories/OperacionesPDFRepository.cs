using MongoDB.Driver;
using Microsoft.Extensions.Options;
using NegocioPDF.Config;
using NegocioPDF.Models;

namespace NegocioPDF.Repositories
{
    public class OperacionesPDFRepository
    {
       private readonly IMongoCollection<OperacionPDF> _operacionesCollection;
        private readonly IMongoCollection<DetalleSuscripcion> _suscripcionesCollection;

        public OperacionesPDFRepository(IOptions<MongoDBSettings> mongoSettings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
            _operacionesCollection = database.GetCollection<OperacionPDF>(mongoSettings.Value.OperacionesCollection);
            _suscripcionesCollection = database.GetCollection<DetalleSuscripcion>(mongoSettings.Value.SuscripcionesCollection);
        }

        public async Task<bool> RegistrarOperacionPDF(string usuarioId, string tipoOperacion)
        {
            var suscripcion = await _suscripcionesCollection
                .Find(s => s.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (suscripcion.tipo_suscripcion == "basico" && suscripcion.operaciones_realizadas >= 5)
                return false;

            var operacion = new OperacionPDF
            {
                UsuarioId = usuarioId,
                TipoOperacion = tipoOperacion,
                FechaOperacion = DateTime.UtcNow
            };

            await _operacionesCollection.InsertOneAsync(operacion);

            if (suscripcion.tipo_suscripcion == "basico")
            {
                var update = Builders<DetalleSuscripcion>.Update
                    .Inc(s => s.operaciones_realizadas, 1);
                await _suscripcionesCollection.UpdateOneAsync(
                    s => s.UsuarioId == usuarioId,
                    update
                );
            }

            return true;
        }

        public async Task<IEnumerable<OperacionPDF>> ObtenerOperacionesPorUsuario(string usuarioId)
        {
            return await _operacionesCollection
                .Find(op => op.UsuarioId == usuarioId)
                .SortByDescending(op => op.FechaOperacion)
                .ToListAsync();
        }

        public async Task<bool> ValidarOperacion(string usuarioId)
        {
            var suscripcion = await _suscripcionesCollection
                .Find(s => s.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            return !(suscripcion.tipo_suscripcion == "basico" && suscripcion.operaciones_realizadas >= 5);
        }

    }
}
