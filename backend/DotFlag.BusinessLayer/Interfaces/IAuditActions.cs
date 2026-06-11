using DotFlag.Domain.Models.Audit;
using DotFlag.Domain.Models.Responses;

namespace DotFlag.BusinessLayer.Interfaces
{
    public interface IAuditActions
    {
        AuditLogPageDto GetAll(AuditLogFilterDto filter);
        string ExportToCsv(AuditLogFilterDto filter);
        ActionResponse DeleteOlderThan(DateTime cutoff, int actorId);
        ActionResponse DeleteById(int id, int actorId);
    }
}
