using Kedu.API.Extensions;
using Kedu.API.Middlewares;
using Kedu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kedu Planos de Pagamento API v1"));

app.MapControllers();
app.MapGraphQL(); // endpoint: /graphql
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    DatabaseSeeder.SeedAsync(db).Wait();
}

app.Run();
