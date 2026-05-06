using System.ComponentModel.DataAnnotations;

namespace ApiClinica.Models;

public class Consulta
{
    public int id { get; set; }
    public int PacienteId { get; set; }
    public int MedicoId { get; set; }
    public DateTime DataHora { get; set;} 
}