using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SmallAPI.Services.Swagger
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileUploadMethods = context.MethodInfo.GetCustomAttributes(true)
                .OfType<FileUploadAttribute>()
                .Any();

            if (fileUploadMethods)
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = new Dictionary<string, OpenApiSchema>
                                {
                                    ["title"] = new OpenApiSchema { Type = "string" },
                                    ["content"] = new OpenApiSchema { Type = "string" },
                                    ["imageFile"] = new OpenApiSchema 
                                    { 
                                        Type = "string", 
                                        Format = "binary",
                                        Description = "Text için resim dosyası"
                                    },
                                    ["audioFile"] = new OpenApiSchema 
                                    { 
                                        Type = "string", 
                                        Format = "binary",
                                        Description = "Text için ses dosyası (mp3, wav, m4a, aac, max 50MB)"
                                    },
                                    ["difficultyLevel"] = new OpenApiSchema { Type = "integer" },
                                    ["questions"] = new OpenApiSchema
                                    {
                                        Type = "array",
                                        Items = new OpenApiSchema
                                        {
                                            Type = "object",
                                            Properties = new Dictionary<string, OpenApiSchema>
                                            {
                                                ["questionText"] = new OpenApiSchema { Type = "string" },
                                                ["imageFile"] = new OpenApiSchema 
                                                { 
                                                    Type = "string", 
                                                    Format = "binary",
                                                    Description = "Question için resim dosyası"
                                                },
                                                ["type"] = new OpenApiSchema { Type = "integer" },
                                                ["order"] = new OpenApiSchema { Type = "integer" },
                                                ["answerOptions"] = new OpenApiSchema
                                                {
                                                    Type = "array",
                                                    Items = new OpenApiSchema
                                                    {
                                                        Type = "object",
                                                        Properties = new Dictionary<string, OpenApiSchema>
                                                        {
                                                            ["optionText"] = new OpenApiSchema { Type = "string" },
                                                            ["imageFile"] = new OpenApiSchema
                                                            {
                                                                Type = "string",
                                                                Format = "binary",
                                                                Description = "AnswerOption için resim dosyası (jpg, jpeg, png, gif, max 10MB)"
                                                            },
                                                            ["isCorrect"] = new OpenApiSchema { Type = "boolean" },
                                                            ["feedback"] = new OpenApiSchema { Type = "string" }
                                                        }
                                                    }
                                                },
                                                ["matchingPairs"] = new OpenApiSchema
                                                {
                                                    Type = "array",
                                                    Items = new OpenApiSchema
                                                    {
                                                        Type = "object",
                                                        Properties = new Dictionary<string, OpenApiSchema>
                                                        {
                                                            ["leftItem"] = new OpenApiSchema { Type = "string" },
                                                            ["rightItem"] = new OpenApiSchema { Type = "string" }
                                                        }
                                                    }
                                                },
                                            }
                                        }
                                    },
                                    ["paragraphs"] = new OpenApiSchema
                                    {
                                        Type = "array",
                                        Items = new OpenApiSchema
                                        {
                                            Type = "object",
                                            Properties = new Dictionary<string, OpenApiSchema>
                                            {
                                                ["content"] = new OpenApiSchema { Type = "string" },
                                                ["order"] = new OpenApiSchema { Type = "integer" },
                                                ["endTime"] = new OpenApiSchema { Type = "number", Description = "Saniye cinsinden bitiş zamanı" }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class FileUploadAttribute : Attribute
    {
    }
}
