using Microsoft.AspNetCore.Mvc;
using ApiClinica.Models;

namespace ApiClinica.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacienteController : ControllerBase
{
    private static List<Paciente> pacientes = new();
    private static int _nextId = 1;

    // GET: api/paciente
    [HttpGet]
    public IActionResult GetPacientes()
    {
        return Ok(pacientes);
    }

    // GET: api/paciente/id
    [HttpGet("{id}")]
    public IActionResult GetPacienteById(int id)
    {   
        var paciente = pacientes.FirstOrDefault(p => p.Id == id);

        if (paciente == null)
            return NotFound();

        return Ok(paciente);
    }

    // POST: api/paciente
    [HttpPost]
    public IActionResult CreatePacient([FromBody] Paciente paciente)
    {
        if (paciente.DataNasc > DateOnly.FromDateTime(DateTime.Today))
        {
            return BadRequest(new { mensagem = "Data de nascimento não pode ser futura." });
        }

        paciente.Id = _nextId++;

        pacientes.Add(paciente);
        return CreatedAtAction(nameof(GetPacienteById), new { id = paciente.Id }, paciente);
    }

    // DELETE: api/paciente/id
    [HttpDelete("{id}")]
    public IActionResult DeletePaciente(int id)
    {
        var paciente = pacientes.FirstOrDefault(p => p.Id == id);

        if (paciente == null)
            return NotFound();

        pacientes.Remove(paciente);
        return NoContent();
    }

    // PUT: api/paciente/id
    [HttpPut("{id}")]
    public IActionResult PutPaciente(int id,[FromBody] Paciente pacienteAtualizado)
    {
        var paciente = pacientes.FirstOrDefault(p => p.Id == id);
        
        if (paciente == null)
            return NotFound();

        paciente.Nome = pacienteAtualizado.Nome;
        paciente.DataNasc = pacienteAtualizado.DataNasc;
        paciente.Cpf = pacienteAtualizado.Cpf;
        paciente.Telefone = pacienteAtualizado.Telefone;
        paciente.Email = pacienteAtualizado.Email;

        return Ok(paciente);
    }
}