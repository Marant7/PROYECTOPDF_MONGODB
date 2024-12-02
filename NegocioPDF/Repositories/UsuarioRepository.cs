using MongoDB.Driver;
using Microsoft.Extensions.Options;
using NegocioPDF.Config;
using NegocioPDF.Models;

namespace NegocioPDF.Repositories
{
    // Repositorio de usuario que maneja la interacción con la base de datos
    public class UsuarioRepository
    {
        private readonly IMongoCollection<Usuario> _usuariosCollection;
        private readonly IMongoCollection<DetalleSuscripcion> _suscripcionesCollection;

        public UsuarioRepository(IOptions<MongoDBSettings> mongoSettings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
            _usuariosCollection = database.GetCollection<Usuario>(mongoSettings.Value.UsuariosCollection);
            _suscripcionesCollection = database.GetCollection<DetalleSuscripcion>(mongoSettings.Value.SuscripcionesCollection);
        }

        public async Task<Usuario> Login(string correo, string password)
        {
            return await _usuariosCollection
                .Find(u => u.Correo == correo && u.Password == password)
                .FirstOrDefaultAsync();
        }

        public async Task RegistrarUsuario(Usuario usuario)
        {
            var existeUsuario = await _usuariosCollection
                .Find(u => u.Correo == usuario.Correo)
                .FirstOrDefaultAsync();

            if (existeUsuario != null)
                throw new Exception("El correo ya está registrado");

            await _usuariosCollection.InsertOneAsync(usuario);

            var suscripcion = new DetalleSuscripcion
            {
                UsuarioId = usuario.Id,
                tipo_suscripcion = "basico",
                fecha_inicio = DateTime.UtcNow,
                fecha_final = DateTime.UtcNow.AddYears(1),
                precio = 0.00m,
                operaciones_realizadas = 0
            };

            await _suscripcionesCollection.InsertOneAsync(suscripcion);
        }
    }
}