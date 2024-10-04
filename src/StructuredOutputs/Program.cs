using System.Text.Json;
using StructuredOutputs.Models;
using OpenAI.Chat;
using Azure.AI.OpenAI;
using Azure.Identity;
using DotNetEnv;
using StructuredOutputs;

// Load the environment variables from the .env file
Env.Load("./.env");

// Set up the OpenAI client
AzureOpenAIClient openAIClient = new(new Uri(Environment.GetEnvironmentVariable("OPENAI_ENDPOINT") ?? string.Empty), new DefaultAzureCredential(), new AzureOpenAIClientOptions(AzureOpenAIClientOptions.ServiceVersion.V2024_08_01_Preview));
var chatClient = openAIClient.GetChatClient(Environment.GetEnvironmentVariable("GPT4O_MODEL_DEPLOYMENT_NAME"));

// Construct the configuration for the chat including the structured outputs response JSON schema.
ChatCompletionOptions options = new()
{
    ResponseFormat = StructuredOutputsExtensions.CreateJsonSchemaFormat<Invoice>("invoice", jsonSchemaIsStrict: true),
    MaxOutputTokenCount = 4096,
    Temperature = 0.1f,
    TopP = 0.1f
};

// Send a request to extract data using Structured Outputs
var markdown = File.ReadAllText("Assets/Invoice-Markdown.md");

List<ChatMessage> messages =
[
    new SystemChatMessage("You are an AI assistant that extracts data from documents."),
    new UserChatMessage("Extract the data from this invoice. If a value is not present, provide null. Dates should be in the format YYYY-MM-DD."),
    new UserChatMessage(markdown)
];

var invoice = chatClient.CompleteChat<Invoice>(messages, options);

Console.WriteLine(JsonSerializer.Serialize(invoice, new JsonSerializerOptions { WriteIndented = true }));
