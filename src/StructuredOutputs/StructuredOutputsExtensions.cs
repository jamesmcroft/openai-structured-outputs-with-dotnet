using OpenAI.Chat;

namespace StructuredOutputs;

public static class StructuredOutputsExtensions
{
    public static ChatResponseFormat CreateJsonSchemaFormat<T>(
        string jsonSchemaFormatName,
        string? jsonSchemaFormatDescription = null,
        bool? jsonSchemaIsStrict = null)
    {
        var formatObjectType = typeof(T);
        var type = formatObjectType.IsGenericType && formatObjectType.GetGenericTypeDefinition() == typeof(Nullable<>)
            ? Nullable.GetUnderlyingType(formatObjectType)!
            : formatObjectType;

        var jsonSchema = OpenAIJsonSchema.For(type);

        return ChatResponseFormat.CreateJsonSchemaFormat(
            jsonSchemaFormatName,
            jsonSchema: BinaryData.FromString(jsonSchema),
            jsonSchemaFormatDescription: jsonSchemaFormatDescription,
            jsonSchemaIsStrict: jsonSchemaIsStrict
        );
    }

    public static ParsedChatCompletion<T?> CompleteChat<T>(
        this ChatClient chatClient,
        List<ChatMessage> messages,
        ChatCompletionOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        return chatClient.CompleteChat(messages, options, cancellationToken);
    }

    public static async Task<ParsedChatCompletion<T?>> CompleteChatAsync<T>(
        this ChatClient chatClient,
        List<ChatMessage> messages,
        ChatCompletionOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        return await chatClient.CompleteChatAsync(messages, options, cancellationToken);
    }
}
