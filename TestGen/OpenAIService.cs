using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
public class ChatService
{
    private readonly IConfiguration _configuration;

    public ChatService(IConfiguration configuration)
    {
        
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration), "Configuration cannot be null.");
    }

    public async Task<string> GeneratePrompt(string course, string content, string competency, string excerpt)
    {
        var prompt = $@"
Course: {course}
Content: {content}
Learning Competency: {competency}
Excerpt from Learning Material: {excerpt}

Suggest a real-life scenario related to the above and create SOLO test questions for this scenario.";

        // Retrieve the API key from appsettings.json
        string apiKey = _configuration["OpenAI:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("OpenAI API key is not configured.");
        }

        ChatClient client = new(model: "gpt-4o", apiKey: apiKey);
        ChatCompletion completion = await client.CompleteChatAsync(prompt);

        return System.Net.WebUtility.HtmlEncode(completion.Content[0].Text.Replace("\n", "<br>"));
    }
}
