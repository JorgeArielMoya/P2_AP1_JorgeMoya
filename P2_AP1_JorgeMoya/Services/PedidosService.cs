using Microsoft.EntityFrameworkCore;
using P2_AP1_JorgeMoya.DAL;
using P2_AP1_JorgeMoya.Models;
using System.Linq.Expressions;

namespace P2_AP1_JorgeMoya.Services;

public class PedidosService (IDbContextFactory<Contexto> Dbfactory)
{
    public async Task<List<Pedidos>> Listar (Expression<Func<Pedidos, bool>> criterio)
    {
        await using var contexto = await Dbfactory.CreateDbContextAsync();
        return await contexto.Pedidos
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();

    }
}
