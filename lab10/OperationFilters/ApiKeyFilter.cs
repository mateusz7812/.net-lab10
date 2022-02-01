using System;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using lab10.Attributes;

public class ApiKeyFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();
        foreach(var atr in context.MethodInfo.GetCustomAttributes(typeof(ApiKeyAttribute), false)){
            if (atr is ApiKeyAttribute attribute)
            {
                if (operation.Security == null)
                    operation.Security = new List<OpenApiSecurityRequirement>();
                
                operation.Security.Add(
                    new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "ApiKey",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },                            
                        new string[]{ }
                    }
                });
            }
        }
    }
}