using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using IncidentAPI_MayaraBouazra.Models;
using IncidentAPI_X.Controllers;

namespace AppTest
{
    public class IncidentsTest
    {
        private IncidentsDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<IncidentsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new IncidentsDbContext(options);
        }
        [Fact]
        public async Task GetIncidents_WhenDataExists_ReturnsAllIncidents()
        {
            var context = GetDbContext();

            context.Incidents.AddRange(
                new Incident { Title = "Incident1", Status = "OPEN", Severity = "HIGH" },
                new Incident { Title = "Incident2", Status = "CLOSED", Severity = "LOW" }
            );

            context.SaveChanges();

            var controller = new IncidentsDbController(context);

            var result = await controller.GetIncidents();

            var incidents = Assert.IsType<List<Incident>>(result.Value);

            Assert.Equal(2, incidents.Count);
        }
        [Fact]
        public async Task GetIncident_ExistingId_ReturnsIncident()
        {
            var context = GetDbContext();

            var incident = new Incident
            {
                Id = 1,
                Title = "Test",
                Status = "OPEN"
            };

            context.Incidents.Add(incident);
            context.SaveChanges();

            var controller = new IncidentsDbController(context);

            var result = await controller.GetIncident(1);

            Assert.NotNull(result.Value);
            Assert.Equal("Test", result.Value.Title);
        }

    }
}
