using System.Threading.Tasks;

namespace BaseDeProjetos.Helpers
{
    public class CacheHelper
    {
        public static async Task CleanupEmpresasCache(DbCache cache)
        {
            await cache.InvalidateCacheAsync("AllEmpresas");
            await cache.InvalidateCacheAsync("EmpresasFunil");
            await cache.InvalidateCacheAsync("EmpresasFunilUnique");
            await cache.InvalidateCacheAsync("EmpresasDTO");
        }
        public static async Task CleanupProspeccoesCache(DbCache cache)
        {
            await cache.InvalidateCacheAsync("AllProspeccoes");
            await cache.InvalidateCacheKeysAsync("Prospeccoes:");
        }

        public static async Task CleanupUsuariosCache(DbCache cache) {
            await cache.InvalidateCacheAsync("AllUsuarios");
            await cache.InvalidateCacheKeysAsync("Usuarios:");
        }

        public static async Task CleanupCargosCache(DbCache cache) {
            await cache.InvalidateCacheAsync("AllCargos");
            await cache.InvalidateCacheKeysAsync("Cargos:");
        }

        public static async Task CleanupProjetosCache(DbCache cache) {
            await cache.InvalidateCacheAsync("AllProjetos");
            await cache.InvalidateCacheKeysAsync("Projetos:");
            await cache.InvalidateCacheKeysAsync("ProjetosExecucaoHome:");
        }
        
    }
}