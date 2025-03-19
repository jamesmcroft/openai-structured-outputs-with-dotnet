using System.Text;
using System.Text.Json;
using Azure.AI.OpenAI;
using Azure.Identity;
using DotNetEnv;
using OpenAI.Chat;
using StructuredOutputs;
using StructuredOutputs.Models;

// Load the environment variables from the .env file
Env.Load("./.env");

// Set up the OpenAI client
AzureOpenAIClient openAIClient = new(new Uri(Environment.GetEnvironmentVariable("OPENAI_ENDPOINT") ?? string.Empty), new DefaultAzureCredential());
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

var systemPrompt = "You are an AI assistant that extracts data from documents.";
var userContent = new List<ChatMessageContentPart>();
var userTextPromptBuilder = new StringBuilder();
userTextPromptBuilder.AppendLine("Extract the data from this invoice.");
userTextPromptBuilder.AppendLine("- If a value is not present, provide null.");
userTextPromptBuilder.AppendLine("- Dates should be in the format YYYY-MM-DD.");

var userTextPrompt = userTextPromptBuilder.ToString();

userContent.Add(ChatMessageContentPart.CreateTextPart(userTextPrompt));
userContent.Add(ChatMessageContentPart.CreateTextPart(markdown));

ParsedChatCompletion<Invoice?> completion = chatClient.CompleteChat(
    [
        new SystemChatMessage(systemPrompt),
        new UserChatMessage(userContent)
    ],
    options);

Console.WriteLine(JsonSerializer.Serialize(completion.Parsed, new JsonSerializerOptions { WriteIndented = true }));
