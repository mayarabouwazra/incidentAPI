using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IncidentAPI_MayaraBouazra.Models;

namespace IncidentAPI_X.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsDbController : ControllerBase
    {
        private readonly IncidentsDbContext _context;

        private static readonly string[] AllowedSeverities =
            { "LOW", "MEDIUM", "HIGH", "CRITICAL" };

        private static readonly string[] AllowedStatuses =
            { "OPEN", "IN_PROGRESS", "RESOLVED" };

        public IncidentsDbController(IncidentsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Incident>>> GetIncidents()
        {
            return await _context.Incidents.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Incident>> GetIncident(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);

            if (incident == null)
                return NotFound();

            return incident;
        }

        [HttpPost]
        public async Task<ActionResult<Incident>> PostIncident(Incident incident)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!AllowedSeverities.Contains(incident.Severity))
                return BadRequest("Invalid severity");

            incident.Status = "IN_PROGRESS";
            incident.CreatedAt = DateTime.UtcNow;

            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIncident), new { id = incident.Id }, incident);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncident(int id, string status)
        {
            var incident = await _context.Incidents.FindAsync(id);

            if (incident == null)
                return NotFound();

            if (!AllowedStatuses.Contains(status))
                return BadRequest("Invalid status");

            incident.Status = status;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncident(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);

            if (incident == null)
                return NotFound();

            if (incident.Severity == "CRITICAL" && incident.Status == "OPEN")
                return BadRequest("Cannot delete OPEN CRITICAL incident");

            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet("filter-by-status")]
        public IActionResult FilterByStatus(string status)
        {
            var result = _context.Incidents
                .Where(i => i.Status.Contains(status))
                .ToList();

            return Ok(result);
        }

        [HttpGet("filter-by-severity")]
        public IActionResult FilterBySeverity(string severity)
        {
            var result = _context.Incidents
                .Where(i => i.Severity.Contains(severity))
                .ToList();

            return Ok(result);
        }

        [HttpGet("filter-by-status-async")]
        public async Task<IActionResult> FilterByStatusAsync(string status)
        {
            var result = await _context.Incidents
                .Where(i => i.Status.Contains(status))
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("filter-by-severity-async")]
        public async Task<IActionResult> FilterBySeverityAsync(string severity)
        {
            var result = await _context.Incidents
                .Where(i => i.Severity.Contains(severity))
                .ToListAsync();

            return Ok(result);
        }


        [HttpPatch("{id}/status")]
        public async Task<IActionResult> PutIncidentStatus(int id, string status)
        {
            if (!AllowedStatuses.Contains(status.ToUpper()))
            {
                return BadRequest();
            }

            var incident = await _context.Incidents.FindAsync(id);

            if (incident == null)
                return NotFound();

            incident.Status = status;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}