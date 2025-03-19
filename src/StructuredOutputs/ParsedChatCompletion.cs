using System.ClientModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenAI.Chat;

namespace StructuredOutputs;

public class ParsedChatCompletion<T>
{
    internal ParsedChatCompletion(ChatCompletion completion)
    {
        Origin = completion;

        Id = completion.Id;
        Model = completion.Model;
        SystemFingerprint = completion.SystemFingerprint;
        Usage = completion.Usage;
        CreatedAt = completion.CreatedAt;
        FinishReason = completion.FinishReason;
        ContentTokenLogProbabilities = completion.ContentTokenLogProbabilities;
        RefusalTokenLogProbabilities = completion.RefusalTokenLogProbabilities;
        Role = completion.Role;
        Content = completion.Content;
        ToolCalls = completion.ToolCalls;
        Refusal = completion.Refusal;
        Parsed = JsonSerializer.Deserialize<T?>(completion.Content[0].Text);
    }

    [JsonIgnore] public ChatCompletion Origin { get; }

    public string Id { get; }

    public string Model { get; }

    public string SystemFingerprint { get; }

    public ChatTokenUsage Usage { get; }

    public DateTimeOffset CreatedAt { get; }

    public ChatFinishReason FinishReason { get; }

    public IReadOnlyList<ChatTokenLogProbabilityDetails> ContentTokenLogProbabilities { get; }

    public IReadOnlyList<ChatTokenLogProbabilityDetails> RefusalTokenLogProbabilities { get; }

    public ChatMessageRole Role { get; }

    public ChatMessageContent Content { get; }

    public IReadOnlyList<ChatToolCall> ToolCalls { get; }

    public string Refusal { get; }

    public T? Parsed { get; }

    public static implicit operator ParsedChatCompletion<T?>(ChatCompletion result)
    {
        return new ParsedChatCompletion<T?>(result);
    }

    public static implicit operator ParsedChatCompletion<T?>(ClientResult<ChatCompletion> result)
    {
        return new ParsedChatCompletion<T?>(result);
    }
}
