using FunctionTestLegacyServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapPost("api/getLegacyData", [Authorize] (LegacySystemRequest request) =>
{
    if (!string.IsNullOrWhiteSpace(request.CorrelationId) && !string.IsNullOrWhiteSpace(request.Id))
    {
        var response = new LegacySystemResponse()
        {
            ErrorCode = 0,
            Message = "Ok",
            Timestamp = DateTime.Now
        };

        return Results.Ok(response);
    }
    else
    {
        // To test the error code mapping through Automapper
        var errResponse = new LegacySystemResponse()
        {
            ErrorCode = 1,
            Message = "Invalid data",
            Timestamp = DateTime.Now
        };
        return Results.BadRequest(errResponse);
    }

}).WithName("GetLegacyData"); ;

app.Run();


internal class LegacySystemResponse
{
    public int ErrorCode { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
}

internal class LegacySystemRequest
{
    public string CorrelationId { get; set; }
    public string Id { get; set; }
}