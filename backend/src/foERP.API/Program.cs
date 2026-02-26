using foERP.Application;
using foERP.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok(new { status = "ok", name = "foERP API" }));

app.MapGet("/api/modules", () => Results.Ok(new[]
{
    "Finance",
    "SupplyChain",
    "Procurement",
    "Sales",
    "HumanCapital",
    "ProjectOperations"
}));

app.Run();
