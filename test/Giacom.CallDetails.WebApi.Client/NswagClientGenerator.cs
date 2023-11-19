using NSwag;
using NSwag.CodeGeneration.CSharp;

namespace Giacom.CallDetails.WebApi.Client;

public static class NswagClientGenerator
{
    public static async Task GenerateAsync(string workingDirectoryPath, string swaggerJsonUrl, string clientName, string @namespace)
    {
        var clientSettings = ClientSettings.Create(@namespace, clientName);

        await GenerateAsync(workingDirectoryPath, swaggerJsonUrl, clientSettings);
    }

    public static async Task GenerateAsync(string workingDirectoryPath, string swaggerJsonUrl, CSharpClientGeneratorSettings settings)
    {
        var document = await OpenApiDocument.FromUrlAsync(swaggerJsonUrl);

        var clientFileName = $"{settings.ClassName}.cs";

        var clientGenerator = new CSharpClientGenerator(document, settings);
        var code = clientGenerator.GenerateFile();
        
        var clientFilePath = Path.Combine(workingDirectoryPath, clientFileName); 
        
        await File.WriteAllTextAsync(clientFilePath, code);
    }
}