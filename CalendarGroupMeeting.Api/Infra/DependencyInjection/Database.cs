using CalendarGroupMeeting.Api.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace CalendarGroupMeeting.Api.Infra.DependencyInjection;

public static class Database
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
            optionsPostgres =>
            {
                optionsPostgres.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            }));
    }
}
