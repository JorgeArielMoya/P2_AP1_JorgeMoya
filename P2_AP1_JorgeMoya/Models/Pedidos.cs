using System.ComponentModel.DataAnnotations;

namespace P2_AP1_JorgeMoya.Models;

public class Pedidos
{
    [Key]
    public int PedidoId { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Campo requerido")]
    public string NombreCliente { get; set; } = null!;

    public double Total { get; set; }

    public ICollection<PedidosDetalle> PedidosDetalles { get; set; } = new List<PedidosDetalle>();  
}
