using System.Threading.Tasks;

namespace BaseDeProjetos.Helpers
{
    public class CacheHelper
    {
        public static void CleanupEmpresasCache(DbCache cache)
        {
            if (cache != null)
            {
                cache.InvalidateCache("AllEmpresas");
                cache.InvalidateCache("Empresas:Funil");
                cache.InvalidateCache("Empresas:FunilUnique");
                cache.InvalidateCache("Empresas:DTO");
            }
        }

        public static async Task CleanupProspeccoesCache(DbCache cache)
        {
            if (cache != null)
            {
                cache.InvalidateCache("AllProspeccoes");
                await cache.InvalidateCacheKeysAsync("Prospeccao:");
                await cache.InvalidateCacheKeysAsync("Prospeccoes:");
                await cache.InvalidateCacheKeysAsync("Participacoes:");
                await cache.InvalidateCacheKeysAsync("Followup:");
            }
        }

        public static async Task CleanupIndicadoresFinanceirosCache(DbCache cache)
        {
            if (cache != null)
            {
                cache.InvalidateCache("AllIndicadoresFinanceiros");
                await cache.InvalidateCacheKeysAsync("IndicadoresFinanceiros:");
            }
        }

        public static async Task CleanupUsuariosCache(DbCache cache)
        {
            if (cache != null)
            {
                cache.InvalidateCache("AllUsuarios");
                await cache.InvalidateCacheKeysAsync("Usuarios:");
            }
        }

        public static async Task CleanupCargosCache(DbCache cache)
        {
            if (cache != null)
            {
                cache.InvalidateCache("AllCargos");
                await cache.InvalidateCacheKeysAsync("Cargos:");
            }
        }

        public static async Task CleanupProjetosCache(DbCache cache)
        {
            if (cache != null)
            {
                cache.InvalidateCache("AllProjetos");
                await cache.InvalidateCacheKeysAsync("Projeto:");
                await cache.InvalidateCacheKeysAsync("Projetos:");
                await cache.InvalidateCacheKeysAsync("ProjetosExecucaoHome:");
            }
        }
    }
}