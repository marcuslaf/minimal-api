using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using minimal_api.Dominio.Interfaces;
using minimal_api.Infraestrutura.Db;
using Test.Mocks;

namespace Test.Helpers;

public class Setup
{
    public const string PORT = "5001";
    public static TestContext testContext = default!;
    public static WebApplicationFactory<Program> http = default!;
    public static HttpClient client = default!;

    public static void ClassInit(TestContext testContext)
    {
        Setup.testContext = testContext;
        Setup.http = new WebApplicationFactory<Program>();

        Setup.http = Setup.http.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("https_port", Setup.PORT).UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.AddScoped<IAdministradorServico, AdministradorServicoMock>();
                services.AddScoped<IVeiculoServico, VeiculoServicoMock>();

                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContexto));
                if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);

                var optionsDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<DbContexto>));
                if (optionsDescriptor != null) services.Remove(optionsDescriptor);

                services.AddDbContext<DbContexto>(options =>
                    options.UseInMemoryDatabase("MinimalApiTest_" + Guid.NewGuid().ToString()));

                services.RemoveAll<Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck>();
            });
        });

        Setup.client = Setup.http.CreateClient();
    }

    public static void ClassCleanup()
    {
        Setup.http.Dispose();
    }
}
