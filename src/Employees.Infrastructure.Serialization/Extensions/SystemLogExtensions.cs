using Employees.Domain.Model;
using Employees.Shared.Responses;

namespace Employees.Infrastructure.Extensions;

public static class SystemLogExtensions
{
    public static IdResponse FormIdResponse(this SystemLog self) =>
        new IdResponse
        {
            Id = self.GetId(),
        };
}
