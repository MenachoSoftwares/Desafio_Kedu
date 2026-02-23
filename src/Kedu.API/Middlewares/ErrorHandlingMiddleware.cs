using System.Net;
using System.Text.Json;
using Kedu.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Kedu.API.Middlewares;
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar requisição {Method} {Path}",
                context.Request.Method, context.Request.Path);

            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message, tipo) = exception switch
        {
            NotFoundException => (
                HttpStatusCode.NotFound,
                exception.Message,
                "NotFound"),

            DomainException => (
                HttpStatusCode.BadRequest,
                exception.Message,
                "DomainError"),

            DbUpdateException dbEx => HandleDbUpdateException(dbEx),

            ArgumentException => (
                HttpStatusCode.BadRequest,
                exception.Message,
                "ValidationError"),

            InvalidOperationException => (
                HttpStatusCode.UnprocessableEntity,
                exception.Message,
                "InvalidOperation"),

            _ => (
                HttpStatusCode.InternalServerError,
                "Ocorreu um erro interno inesperado.",
                "InternalError")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new Dictionary<string, object?>
        {
            ["erro"] = message,
            ["tipo"] = tipo
        };
        if (_env.IsDevelopment())
        {
            response["detalhes"] = exception.InnerException?.Message ?? exception.Message;
            response["stackTrace"] = exception.StackTrace;
        }

        var options = new JsonSerializerOptions { WriteIndented = true };
        var result = JsonSerializer.Serialize(response, options);
        return context.Response.WriteAsync(result);
    }

    private static (HttpStatusCode, string, string) HandleDbUpdateException(DbUpdateException dbEx)
    {
        var inner = dbEx.InnerException;
        var innerMessage = inner?.Message ?? dbEx.Message;
        if (innerMessage.Contains("does not exist", StringComparison.OrdinalIgnoreCase)
            || innerMessage.Contains("não existe", StringComparison.OrdinalIgnoreCase))
        {
            return (
                HttpStatusCode.InternalServerError,
                $"Erro de banco de dados: uma tabela necessária não foi encontrada. Verifique se as migrations foram aplicadas. Detalhe: {innerMessage}",
                "DatabaseMigrationError");
        }
        if (innerMessage.Contains("duplicate key", StringComparison.OrdinalIgnoreCase)
            || innerMessage.Contains("unique constraint", StringComparison.OrdinalIgnoreCase))
        {
            return (
                HttpStatusCode.Conflict,
                $"Registro duplicado: já existe um registro com os mesmos dados únicos. Detalhe: {innerMessage}",
                "DuplicateError");
        }
        if (innerMessage.Contains("foreign key", StringComparison.OrdinalIgnoreCase)
            || innerMessage.Contains("violates foreign key", StringComparison.OrdinalIgnoreCase))
        {
            return (
                HttpStatusCode.UnprocessableEntity,
                $"Erro de referência: o registro referenciado não existe ou não pode ser removido. Detalhe: {innerMessage}",
                "ForeignKeyError");
        }
        if (innerMessage.Contains("not-null constraint", StringComparison.OrdinalIgnoreCase)
            || innerMessage.Contains("null value", StringComparison.OrdinalIgnoreCase))
        {
            return (
                HttpStatusCode.BadRequest,
                $"Campo obrigatório não preenchido. Detalhe: {innerMessage}",
                "RequiredFieldError");
        }
        return (
            HttpStatusCode.InternalServerError,
            $"Erro ao salvar dados no banco. Detalhe: {innerMessage}",
            "DatabaseError");
    }
}
