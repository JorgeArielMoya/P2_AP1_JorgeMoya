using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2_AP1_JorgeMoya.Models;

public class PedidosDetalle
{
    [Key]
    public int DetalleId { get; set; }
    public int ComponenteId { get; set; }
    public int PedidoId { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }

    [ForeignKey(nameof(PedidoId))]
    public virtual Pedidos Pedido { get; set; }

    [ForeignKey(nameof(ComponenteId))]
    public virtual Componentes Componente { get; set; }

    public PedidosDetalle() { }

    public PedidosDetalle(int componenteId, int cantidad, decimal precio) 
    { 
        ComponenteId = componenteId;
        Cantidad = cantidad;
        Precio = precio;    
    }

}
