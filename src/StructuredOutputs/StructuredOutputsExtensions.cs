using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;
using OpenAI.Chat;

namespace StructuredOutputs;

internal static class StructuredOutputsExtensions
{
    public static Func<JsonSchemaExporterContext, JsonNode, JsonNode> StructuredOutputsTransform = new((context, node) =>
    {
        static void ProcessJsonObject(JsonObject jsonObject)
        {
            if (jsonObject["type"]?.ToString().Contains("object") == true)
            {
                // Ensures that object types include the "additionalProperties" field, set to false.
                if (!jsonObject.ContainsKey("additionalProperties"))
                {
                    jsonObject.Add("additionalProperties", false);
                }

                var required = new JsonArray();
                var properties = jsonObject["properties"] as JsonObject;
                foreach (var property in properties!)
                {
                    required.Add(property.Key);

                    if (property.Value is JsonObject nestedObject)
                    {
                        // Process nested objects to ensure schema validity.
                        ProcessJsonObject(nestedObject);
                    }
                }

                // Ensures that object types include the "required" field containing all of the property keys.
                if (!jsonObject.ContainsKey("required"))
                {
                    jsonObject.Add("required", required);
                }
            }
        }

        if (node is JsonObject rootObject)
        {
            ProcessJsonObject(rootObject);
        }

        return node;
    });

    public static T? CompleteChat<T>(this ChatClient chatClient, List<ChatMessage> messages, ChatCompletionOptions options)
    {
        ChatCompletion completion = chatClient.CompleteChat(messages, options);
        return JsonSerializer.Deserialize<T>(completion.Content[0].Text);
    }

    public static ChatResponseFormat CreateJsonSchemaFormat<T>(string jsonSchemaFormatName, string? jsonSchemaFormatDescription = null, bool? jsonSchemaIsStrict = null)
    {
        return ChatResponseFormat.CreateJsonSchemaFormat(
            jsonSchemaFormatName,
            jsonSchema: BinaryData.FromString(JsonSchemaExporter.GetJsonSchemaAsNode(JsonSerializerOptions.Default, typeof(T), new JsonSchemaExporterOptions() { TreatNullObliviousAsNonNullable = true, TransformSchemaNode = StructuredOutputsTransform }).ToString()),
            jsonSchemaFormatDescription: jsonSchemaFormatDescription,
            jsonSchemaIsStrict: jsonSchemaIsStrict
        );
    }
}
