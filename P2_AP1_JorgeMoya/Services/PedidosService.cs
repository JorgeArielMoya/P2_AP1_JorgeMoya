using Microsoft.EntityFrameworkCore;
using P2_AP1_JorgeMoya.DAL;
using P2_AP1_JorgeMoya.Models;
using System.Linq.Expressions;

namespace P2_AP1_JorgeMoya.Services;

public class PedidosService (IDbContextFactory<Contexto> Dbfactory)
{
    public async Task<bool> Guardar (Pedidos pedido)
    {
        if (!await Existe (pedido.PedidoId))
        {
            return await Insertar(Pedido);
        }
        else
        {
            return await Modificar (pedido);    
        }
    }
    public async Task<List<Pedidos>> Listar (Expression<Func<Pedidos, bool>> criterio)
    {
        await using var contexto = await Dbfactory.CreateDbContextAsync();
        return await contexto.Pedidos
            .Include(d => d.PedidosDetalles)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Componentes>> ListarComponentes(Expression<Func<Componentes, bool>> criterio)
    {
        await using var contexto = await Dbfactory.CreateDbContextAsync();
        return await contexto.Componentes
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();

    }
}
