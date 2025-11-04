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
            return await Insertar(pedido);
        }
        else
        {
            return await Modificar (pedido);    
        }
    }

    private async Task AfectarExistencia (ICollection<PedidosDetalle> detalle, TipoOperacion operacion)
    {
        await using var contexto = await Dbfactory.CreateDbContextAsync();

        foreach(var item in detalle)
        {
            var componente = await contexto.Componentes.SingleAsync(c => c.ComponenteId == item.ComponenteId);  

            if (operacion == TipoOperacion.Suma)
            {
                componente.Existencia += item.Cantidad;
            }

            else
            {
                componente.Existencia -= item.Cantidad;
            }

            await contexto.SaveChangesAsync();
        }
    }

    private async Task<bool> Insertar (Pedidos pedido)
    {
        await using var contexto = await Dbfactory.CreateDbContextAsync();
        contexto.Pedidos.Add(pedido);
        await AfectarExistencia(pedido.PedidosDetalles, TipoOperacion.Resta);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar (Pedidos pedido)
    {
        await using var contexto = await Dbfactory.CreateDbContextAsync();

        var pedidoActual = await contexto.Pedidos
            .Include(d => d.PedidosDetalles)
            .FirstOrDefaultAsync(p => p.PedidoId == pedido.PedidoId);

        if (pedidoActual == null) return false;

        await AfectarExistencia(pedidoActual.PedidosDetalles, TipoOperacion.Suma);

        contexto.PedidosDetalles.RemoveRange(pedidoActual.PedidosDetalles);

        pedidoActual.NombreCliente = pedido.NombreCliente;
        pedidoActual.Fecha = pedido.Fecha;

        foreach(var detalle in pedido.PedidosDetalles)
        {
            pedidoActual.PedidosDetalles.Add(new PedidosDetalle(detalle.ComponenteId, detalle.Cantidad, detalle.Precio));
        }

        await AfectarExistencia(pedido.PedidosDetalles, TipoOperacion.Resta);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Eliminar (int? pedidoId)
    {
        await using var contexto = await Dbfactory.CreateDbContextAsync();

        var pedidoActual = await contexto.Pedidos
            .Include(d => d.PedidosDetalles)
            .FirstOrDefaultAsync(p => p.PedidoId == pedidoId);

        if (pedidoActual == null) return false;

        await AfectarExistencia(pedidoActual.PedidosDetalles, TipoOperacion.Suma);

        contexto.PedidosDetalles.RemoveRange(pedidoActual.PedidosDetalles);
        contexto.Pedidos.Remove(pedidoActual);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Existe (int pedidoId )
    {
        await using var contexto = await Dbfactory.CreateDbContextAsync();
        return await contexto.Pedidos.AnyAsync(p => p.PedidoId == pedidoId);    
    }

    public async Task<Pedidos?> Buscar (int? pedidoId)
    {
        await using var contexto = await Dbfactory.CreateDbContextAsync();
        return await contexto.Pedidos
            .Include(d => d.PedidosDetalles)
            .FirstOrDefaultAsync(p => p.PedidoId == pedidoId);
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


    private enum TipoOperacion
    {
        Suma = 1,
        Resta = 2
    }
}
