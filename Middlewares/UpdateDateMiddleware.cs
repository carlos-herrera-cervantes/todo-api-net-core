using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TodoApiNet.Middlewares;

namespace TodoApiNet.Middlewares
{
    public class UpdateDateMiddleware
    {
        private readonly RequestDelegate _next;

        public UpdateDateMiddleware(RequestDelegate next) => _next = next;

        #region snippet_ExecuteNextTask

        public async Task InvokeAsync(HttpContext context)
        {
            var isValidVerb = context.Request.Method == "PATCH" || context.Request.Method == "PUT";

            if (isValidVerb)
            {
                var body = await ParseRequest(context);
                await _next.Invoke(body);
                return;
            }

            await _next.Invoke(context);
        }

        #endregion

        #region snippet_ParseTheRequest

        private async Task<dynamic> ParseRequest(HttpContext context)
        {
            using var reader = new StreamReader(context.Request.Body);
            var convertBody = JsonConvert.DeserializeObject<List<dynamic>>(await reader.ReadToEndAsync());
            
            convertBody.Add(new { Op = "replace", Path = "UpdatedAt", Value = DateTime.UtcNow });

            var bytesToWrite = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(convertBody));
            var injectedRequestStream = new MemoryStream();

            await injectedRequestStream.WriteAsync(bytesToWrite, 0, bytesToWrite.Length);

            injectedRequestStream.Seek(0, SeekOrigin.Begin);
            context.Request.Body = injectedRequestStream;

            return context;
        }

        #endregion
    }
}

public static class UpdateDateMiddlewareExtensions
{
    public static IApplicationBuilder UseUpdateDate(this IApplicationBuilder builder) => builder.UseMiddleware<UpdateDateMiddleware>();
}