using Airslip.Common.Testing;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.OperationNameGenerators;

OpenApiDocument document = await OpenApiDocument.FromUrlAsync("https://airslip-dev-merchant-integrations-internal-app.azurewebsites.net/swagger.json");
string fileName = "InternalApiV1Client";
            
CSharpClientGeneratorSettings clientSettings = new()
{
    ClassName = fileName,
    CSharpGeneratorSettings =
    {
        Namespace = "Airslip.Common.MerchantTransactions.Generated"
    },
    GenerateClientInterfaces = true,
    OperationNameGenerator = new SingleClientFromOperationIdOperationNameGenerator(),
    ClientBaseClass = "MerchantIntegrationApi",
    ClientBaseInterface = "IMerchantIntegrationApi",
    UseHttpRequestMessageCreationMethod = true,
    AdditionalNamespaceUsages = new []{ "Airslip.Common.MerchantTransactions.Interfaces", "Airslip.Common.MerchantTransactions.Implementations" },
    GenerateDtoTypes = true,
    GenerateExceptionClasses = true
};

CSharpClientGenerator clientGenerator = new(document, clientSettings);
string? code = clientGenerator.GenerateFile();
            
string commonLibrary = "Airslip.Common.MerchantTransactions";
string workingDirectory = Path.Combine(
    OptionsMock.GetBasePath(commonLibrary)!,
    "Generated");
Directory.CreateDirectory(workingDirectory);

string path = Path.Combine(workingDirectory, $"{fileName}.cs");

if (!File.Exists(path))
{
    await File.Create(path).DisposeAsync();
}

await using StreamWriter tw = new(path);
await tw.WriteLineAsync(code);

document = await OpenApiDocument.FromUrlAsync("https://airslip-dev-merchant-integrations-api-app.azurewebsites.net/swagger.json");
fileName = "ExternalApiV1Client";
clientSettings.ClassName = fileName;
clientSettings.GenerateDtoTypes = false;
clientSettings.GenerateExceptionClasses = false;

CSharpClientGenerator clientGenerator2 = new(document, clientSettings);
code = clientGenerator2.GenerateFile();
path = Path.Combine(workingDirectory, $"{fileName}.cs");

if (!File.Exists(path))
{
    await File.Create(path).DisposeAsync();
}

await using StreamWriter tw2 = new(path);
await tw2.WriteLineAsync(code);











