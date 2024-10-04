# Azure OpenAI GPT-4o Structured Outputs with .NET 9

The goal of this sample is to demonstrate how to use the new `JsonSchemaExporter` type in .NET 9 to simplify the creation of JSON schemas for Azure OpenAI GPT-4o structured outputs.

## How it works

`JsonSchemaExporter` is simply a built in .NET mechanism to generate valid JSON schema objects from .NET types.

For the .NET OpenAI SDK however, the JSON schema must be in a specific format to be considered valid. On top of the default generated schemas using the exporter, the current known additional rules must be applied:

- The base type must be an `object` only by setting the `TreatNullObliviousAsNonNullable = true` option in the `JsonSchemaExporterOptions`.
- Any properties that are of type `object` must additionally include the following properties:
  - `additionalProperties` set to `false`.
  - `required` which is an array of **all** the keys in the object.

The [`StructuredOutputsExtensions` class](./src/StructuredOutputs/StructuredOutputsExtensions.cs) provides a generic `CreateJsonSchemaFormat` method that can be passed a model object type and will return the `ChatResponseFormat` object that can be used by the OpenAI SDK. This method will apply the additional rules to the generated schema using a Transform function.

> [!NOTE]
> For complex object nesting where there are self-referencing types, this implementation does not support. It would be possible to provide this capability by simplifying the nested classes as `$defs` at the root of the schema, and then referencing them in the properties with `'$ref': '#/$defs/ClassName'`.

In addition to the generic `CreateJsonSchemaFormat` method, a generic `CompleteChat` extension is provided for the `ChatClient` that will automatically deserialize the structured output into the expected model object type.

### Example

```csharp
class Family
{
    public List<Person> Parents { get; set; }
    public List<Person>? Children { get; set; }

    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}

ChatCompletionOptions options = new()
{
    ResponseFormat = StructuredOutputsExtensions.CreateJsonSchemaFormat<Family>("family", jsonSchemaIsStrict: true),
    MaxOutputTokenCount = 4096,
    Temperature = 0.1f,
    TopP = 0.1f
};

List<ChatMessage> messages =
[
    new SystemChatMessage("You are an AI assistant that creates families."),
    new UserChatMessage("Create a family with 2 parents and 2 children.")
];

var family = chatClient.CompleteChat<Family>(messages, options);
```

## Running the sample

- Install the latest [**.NET 9 SDK**](https://dotnet.microsoft.com/en-us/download/dotnet/9.0).
- Install [**PowerShell Core**](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell).
- Install the [**Azure CLI**](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli).
- Install [**Visual Studio Code**](https://code.visualstudio.com/).

Additionally, you will require:

- An Azure subscription. If you don't have an Azure subscription, create an [account](https://azure.microsoft.com/en-us/).

To setup a local development environment, follow these steps:

> [!NOTE]
> For the most optimal sample experience, it is recommended to deploy the necessary infrastructure in a region that supports `GPT-4o` version `2024-08-06`. Find out more about region availability for the [`GPT-4o`](https://learn.microsoft.com/en-us/azure/ai-services/openai/concepts/models#standard-and-global-standard-deployment-model-quota) model.

```pwsh
az login

./Setup-Environment.ps1 -DeploymentName <UniqueDeploymentName> -Location <AzureRegion> -SkipInfrastructure $false
```

The script will deploy the following resources to your Azure subscription:

- [**Azure AI Services**](https://learn.microsoft.com/en-us/azure/ai-services/what-are-ai-services), a managed service for all Azure AI Services, including Azure OpenAI.
  - **Note**: GPT-4o will be deployed as Global Standard with 10K TPM quota allocation. This can be adjusted based on your quota availability in the [main.bicep](./infra/main.bicep) file.

> [!NOTE]
> Resources are secured by default with Microsoft Entra ID using Azure RBAC. Your user client ID will be added with the necessary least-privilege roles to access the resources created.

After the script completes, a `.env` file will be crated in the `src/StructuredOutputs` folder and you can run the sample project by following these steps:

```pwsh
cd src/StructuredOutputs
dotnet run
```

## Resources

- [Azure OpenAI GPT-4o Structured Outputs Supported Schemas and Limitations](https://learn.microsoft.com/en-us/azure/ai-services/openai/how-to/structured-outputs?tabs=python-secure#supported-schemas-and-limitations)
- [How to use chat completions with structured outputs in OpenAI .NET SDK](https://github.com/openai/openai-dotnet?tab=readme-ov-file#how-to-use-chat-completions-with-structured-outputs)
- [.NET 9 JSON Schema Exporter](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9/libraries#jsonschemaexporter)

## License

This project is licensed under the MIT License - see the [LICENSE](./LICENSE) file for details.
