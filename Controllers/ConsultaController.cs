using Microsoft.AspNetCore.Mvc;
using ApiClinica.Models;

namespace ApiClinica.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsultaController : ControllerBase
{
    private static List<Consulta> consultas = new();
    private static int _nextId = 1;

    // GET: api/consulta
    [HttpGet]
    public IActionResult GetConsulta()
    {
        return Ok(consultas);
    }

    // GET: api/consulta/{id}
    [HttpGet("{id}")]
    public IActionResult GetConsultaById(int id)
    {
        var consulta = consultas.FirstOrDefault(c => c.Id == id);

        if (consulta == null)
            return NotFound();

        return Ok(consulta);
    }

    // POST: api/consulta
    [HttpPost]
    public IActionResult CreateConsulta([FromBody] Consulta consulta)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (consulta.DataHora < DateTime.Now)
            return BadRequest(new { mensagem = "Agendamento de consulta não pode ser em data passada." });

        var dataConsultaMedico = consultas.FirstOrDefault(c => c.MedicoId == consulta.MedicoId
            && c.DataHora == consulta.DataHora);

        var dataConsultaPaciente = consultas.FirstOrDefault(c => c.PacienteId == consulta.PacienteId
            && c.DataHora == consulta.DataHora);

        if (dataConsultaMedico != null)
            return BadRequest(new { mensagem = "Esse médico já possui consulta nesse horário." });

        if (dataConsultaPaciente != null)
            return BadRequest(new { mensagem = "Esse paciente já possui consulta nesse horário." });

        consulta.Id = _nextId++;
        consultas.Add(consulta);

        return CreatedAtAction(nameof(GetConsultaById), new { id = consulta.Id }, consulta);
    }

    // DELETE: api/consulta/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteConsulta(int id)
    {
        var consulta = consultas.FirstOrDefault(c => c.Id == id);

        if (consulta == null)
            return NotFound();

        consultas.Remove(consulta);
        return NoContent();
    }

    // PUT: api/consulta/{id}
    [HttpPut("{id}")]
    public IActionResult PutConsulta(int id, [FromBody] Consulta consultaAtualizada)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var consulta = consultas.FirstOrDefault(c => c.Id == id);

        if (consulta == null)
            return NotFound();

        if (consultaAtualizada.DataHora < DateTime.Now)
            return BadRequest(new { mensagem = "Agendamento de consulta não pode ser em data passada." });

        var dataConsultaMedico = consultas.FirstOrDefault(c => c.MedicoId == consultaAtualizada.MedicoId
            && c.DataHora == consultaAtualizada.DataHora
            && c.Id != id);

        var dataConsultaPaciente = consultas.FirstOrDefault(c => c.PacienteId == consultaAtualizada.PacienteId
            && c.DataHora == consultaAtualizada.DataHora
            && c.Id != id);

        if (dataConsultaMedico != null)
            return BadRequest(new { mensagem = "Esse médico já possui consulta nesse horário." });

        if (dataConsultaPaciente != null)
            return BadRequest(new { mensagem = "Esse paciente já possui consulta nesse horário." });

        consulta.PacienteId = consultaAtualizada.PacienteId;
        consulta.MedicoId = consultaAtualizada.MedicoId;
        consulta.DataHora = consultaAtualizada.DataHora;

        return Ok(consulta);
    }
}