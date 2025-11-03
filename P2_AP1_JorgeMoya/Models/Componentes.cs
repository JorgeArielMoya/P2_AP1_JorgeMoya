using System.ComponentModel.DataAnnotations;

namespace P2_AP1_JorgeMoya.Models;

public class Componentes
{
    [Key]
    public int ComponenteId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Existencia { get; set; }
}
