using System.Collections;
using Microsoft.VisualBasic;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp;

namespace Giacom.CallDetails.WebApi.Client;

public static class ClientSettings
{
    public static CSharpClientGeneratorSettings Create(string @namespace, string className)
    {
        return new CSharpClientGeneratorSettings
        {
            CSharpGeneratorSettings =
            {
                GenerateOptionalPropertiesAsNullable = false,
                GenerateNullableReferenceTypes = false,
                TemplateDirectory = "./Templates",
                HandleReferences = false,
                GenerateImmutableArrayProperties = false,
                GenerateImmutableDictionaryProperties = false,
                JsonSerializerSettingsTransformationMethod = null,
                InlineNamedArrays = false,
                InlineNamedDictionaries = false,
                InlineNamedTuples = true,
                InlineNamedAny = false,
                Namespace = @namespace,
                RequiredPropertiesMustBeDefined = true,
                DateType = nameof(DateOnly),
                JsonConverters = null,
                AnyType = "object",
                TimeType = nameof(TimeSpan),
                ArrayType = nameof(IEnumerable),
                ArrayInstanceType = nameof(Collection),
                DictionaryType = nameof(IDictionary),
                DictionaryInstanceType = "Dictionary",
                ArrayBaseType = nameof(Collection),
                DictionaryBaseType = "Dictionary",
                ClassStyle = CSharpClassStyle.Poco,
                JsonLibrary = CSharpJsonLibrary.NewtonsoftJson,
                GenerateDefaultValues = true,
                GenerateDataAnnotations = true,
                GenerateJsonMethods = false,
                EnforceFlagEnums = false,
                TypeAccessModifier = "public",
            },
            GenerateClientClasses = true,
            GenerateClientInterfaces = true,
            InjectHttpClient = true,
            DisposeHttpClient = true,
            GenerateExceptionClasses = true,
            ExceptionClass = "ApiException",
            WrapDtoExceptions = true,
            UseHttpClientCreationMethod = true,
            HttpClientType = nameof(HttpClient),
            UseHttpRequestMessageCreationMethod = false,
            UseBaseUrl = false,
            GenerateBaseUrlProperty = false,
            GenerateSyncMethods = false,
            GeneratePrepareRequestAndProcessResponseAsAsyncMethods = false,
            ExposeJsonSerializerSettings = false,
            ClientClassAccessModifier = "public",
            ParameterDateTimeFormat = "s",
            ParameterDateFormat = "yyyy-MM-dd",
            GenerateUpdateJsonSerializerSettingsMethod = true,
            UseRequestAndResponseSerializationSettings = false,
            SerializeTypeInformation = false,
            QueryNullValue = "",
            ClassName = className,
            GenerateOptionalParameters = true,
            ParameterArrayType = nameof(IEnumerable),
            ParameterDictionaryType = nameof(IDictionary),
            ResponseArrayType = nameof(IEnumerable),
            ResponseDictionaryType = nameof(IDictionary),
            WrapResponses = false,
            GenerateResponseClasses = true,
            ResponseClass = "SwaggerResponse",
            GenerateDtoTypes = true,
        };
    }
}