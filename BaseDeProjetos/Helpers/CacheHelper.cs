using System.Threading.Tasks;

namespace BaseDeProjetos.Helpers
{
    public class CacheHelper
    {
        public static void CleanupEmpresasCache(DbCache cache)
        {
            cache.InvalidateCache("AllEmpresas");
            cache.InvalidateCache("EmpresasFunil");
            cache.InvalidateCache("EmpresasFunilUnique");
            cache.InvalidateCache("EmpresasDTO");
        }
        public static async Task CleanupProspeccoesCache(DbCache cache)
        {
            cache.InvalidateCache("AllProspeccoes");
            await cache.InvalidateCacheKeysAsync("Prospeccoes:");
        }

        public static async Task CleanupUsuariosCache(DbCache cache) {
            cache.InvalidateCache("AllUsuarios");
            await cache.InvalidateCacheKeysAsync("Usuarios:");
        }

        public static async Task CleanupCargosCache(DbCache cache) {
            cache.InvalidateCache("AllCargos");
            await cache.InvalidateCacheKeysAsync("Cargos:");
        }

        public static async Task CleanupProjetosCache(DbCache cache) {
            cache.InvalidateCache("AllProjetos");
            await cache.InvalidateCacheKeysAsync("Projetos:");
            await cache.InvalidateCacheKeysAsync("ProjetosExecucaoHome:");
        }
        
    }
}