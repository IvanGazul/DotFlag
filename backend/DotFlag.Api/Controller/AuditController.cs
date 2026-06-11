using System.Text;
using DotFlag.Api.Extensions;
using DotFlag.BusinessLayer;
using DotFlag.BusinessLayer.Interfaces;
using DotFlag.Domain.Models.Audit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotFlag.Api.Controller
{
    [Route("api/admin/logs")]
    [ApiController]
    [Authorize(Roles = "Admin,Owner")]
    public class AuditController : ControllerBase
    {
        private readonly IAuditActions _auditActions;

        public AuditController()
        {
            var bl = new BusinessLogic();
            _auditActions = bl.GetAuditActions();
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] AuditLogFilterDto filter)
        {
            var result = _auditActions.GetAll(filter);
            return Ok(result);
        }

        [HttpGet("export")]
        public IActionResult Export([FromQuery] AuditLogFilterDto filter)
        {
            var csv = _auditActions.ExportToCsv(filter);
            var bytes = Encoding.UTF8.GetBytes(csv);
            var fileName = $"audit-logs-{DateTime.UtcNow:yyyyMMdd-HHmmss}.csv";
            return File(bytes, "text/csv", fileName);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner")]
        public IActionResult Delete(int id)
        {
            var result = _auditActions.DeleteById(id, User.GetId());
            if (!result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        [HttpDelete("older-than")]
        [Authorize(Roles = "Owner")]
        public IActionResult DeleteOlderThan([FromQuery] DateTime cutoff)
        {
            var result = _auditActions.DeleteOlderThan(cutoff, User.GetId());
            return Ok(result);
        }
    }
}
