using Microsoft.AspNetCore.Mvc;
using ApiClinica.Models;

namespace ApiClinica.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicoController : ControllerBase
{
    private static List<Medico> medicos = new();
    private static int _nextId = 1;

    // GET: api/medico
    [HttpGet]
    public IActionResult GetMedicos()
    {
        return Ok(medicos);
    }

    // GET: api/medico/{id}
    [HttpGet("{id}")]
    public IActionResult GetMedicoById(int id)
    {
        var medico = medicos.FirstOrDefault(m => m.Id == id);

        if (medico == null)
            return NotFound();

        return Ok(medico);
    }

    // POST: api/medico
    [HttpPost]
    public IActionResult CreateMedico([FromBody] Medico medico)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (medicos.Any(m => m.CRM == medico.CRM))
            return Conflict(new { mensagem = "Já existe um médico cadastrado com esse CRM." });

        if (medicos.Any(m => m.Email == medico.Email))
            return Conflict(new { mensagem = "Já existe um médico cadastrado com esse Email." });

        medico.Id = _nextId++;
        medicos.Add(medico);

        return CreatedAtAction(nameof(GetMedicoById), new { id = medico.Id }, medico);
    }

    // DELETE: api/medico/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteMedico(int id)
    {
        var medico = medicos.FirstOrDefault(m => m.Id == id);

        if (medico == null)
            return NotFound();

        medicos.Remove(medico);
        return NoContent();
    }

    // PUT: api/medico/{id}
    [HttpPut("{id}")]
    public IActionResult PutMedico(int id, [FromBody] Medico medicoAtualizado)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var medico = medicos.FirstOrDefault(m => m.Id == id);

        if (medico == null)
            return NotFound();

        medico.Nome = medicoAtualizado.Nome;
        medico.Email = medicoAtualizado.Email;
        medico.Telefone = medicoAtualizado.Telefone;
        medico.CRM = medicoAtualizado.CRM;

        return Ok(medico);
    }
}