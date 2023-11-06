using System.Threading.Tasks;

namespace BaseDeProjetos.Helpers
{
    public class CacheHelper
    {
        public static async Task CleanupEmpresasCache(DbCache cache)
        {
            await cache.InvalidateCache("AllEmpresas");
            await cache.InvalidateCache("EmpresasFunil");
            await cache.InvalidateCache("EmpresasFunilUnique");
            await cache.InvalidateCache("EmpresasDTO");
        }
        public static async Task CleanupProspeccoesCache(DbCache cache)
        {
            await cache.InvalidateCache("AllProspeccoes");
            await cache.InvalidateCache("ProspeccoesEmpresas");
        }

        public static async Task CleanupUsuariosCache(DbCache cache) {
            await cache.InvalidateCache("UsuariosFunil");
        }
    }
}