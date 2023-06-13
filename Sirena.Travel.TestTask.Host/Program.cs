using Sirena.Travel.TestTask.Impl;
using Sirena.Travel.TestTask.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMappingProfiles()
    .AddSearchService()
    .AddHttpClients(builder.Configuration)
    .AddProviders()
    .AddCache(builder.Configuration);

builder.Services.AddMvc();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseRouting();

app.MapControllers();

app.Run();
