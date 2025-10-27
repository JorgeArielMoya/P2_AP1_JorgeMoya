using Microsoft.EntityFrameworkCore;
using P2_AP1_JorgeMoya.DAL;
using P2_AP1_JorgeMoya.Models;
using System.Linq.Expressions;

namespace P2_AP1_JorgeMoya.Services;

public class RegistroService (IDbContextFactory<Contexto> Dbfactory)
{
    public async Task<List<Registros>> Listar (Expression<Func<Registros, bool>> criterio)
    {
        await using var contexto = await Dbfactory.CreateDbContextAsync();
        return await contexto.Registros
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();

    }
}
