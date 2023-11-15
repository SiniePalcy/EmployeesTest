using Employees.Domain.Model;
using Employees.Shared.Responses;

namespace Employees.Infrastructure.Extensions;

public static class SystemLogExtensions
{
    public static IdResponse FormIdResponse(this SystemLog self)
    {
        var id = int.Parse(self.ChangeSet["Id"]);
        return new IdResponse
        {
            Id = id,
        };
    }
}
