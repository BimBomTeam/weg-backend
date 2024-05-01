using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using WEG.Domain;
using WEG.Infrastructure.Dto;
using WEG.Infrastructure.Services;


namespace WEG.Application.Services
{
    public class AiCommunicationService : IAiCommunicationService
    {
        private readonly OpenAIClient _client;
        private readonly PromptService _promptService;

        public AiCommunicationService(IConfiguration config)
        {
            _client = new OpenAIClient(config["gpt_api_key"]);
            _promptService = new PromptService();
        }
        public async Task<string> GetMessageFromAi(string message)
        {
            var chatCompletionsOptions = new ChatCompletionsOptions
            {
                DeploymentName = "gpt-3.5-turbo",
                Temperature = (float)1,
                MaxTokens = 800,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            };

            chatCompletionsOptions.Messages.Add(new ChatRequestUserMessage("napisz cokolwiek"));
            var response = await _client.GetChatCompletionsAsync(chatCompletionsOptions);

            //response.Value.Choices.Select(x => )
            return response.Value.Choices[0].Message.Content;
        }
        public async Task<DialogDto> StartDialog(string role, string level, string wordsArray)
        {
            string prompt = await _promptService.GetStartChatPromptAsync(level, role, wordsArray);

            var chatCompletionsOptions = new ChatCompletionsOptions
            {
                DeploymentName = "gpt-3.5-turbo",
                Temperature = (float)1,
                MaxTokens = 800,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            };

            chatCompletionsOptions.Messages.Add(new ChatRequestUserMessage(prompt));
            var response = await _client.GetChatCompletionsAsync(chatCompletionsOptions);

            return new DialogDto() { Message = response.Value.Choices[0].Message.Content, Role = UserRoles.User.ToString() };
        }
        public async Task<IEnumerable<DialogDto>> ContinueDialog(IEnumerable<DialogDto> messages, string messageStr)
        {
            var chatCompletionsOptions = new ChatCompletionsOptions
            {
                DeploymentName = "gpt-3.5-turbo",
                Temperature = (float)1,
                MaxTokens = 800,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            };

            List<ChatRequestMessage> chatHistory = new List<ChatRequestMessage>();
            foreach (var message in messages)
            {
                if (message.Role == ChatRole.Assistant)
                    chatHistory.Add(new ChatRequestAssistantMessage(message.Message));
                else if (message.Role == ChatRole.User)
                    chatHistory.Add(new ChatRequestUserMessage(message.Message));
                else
                    chatHistory.Add(new ChatRequestUserMessage(message.Message));
            }

            chatCompletionsOptions.Messages.Add(new ChatRequestUserMessage(messageStr));
            var response = await _client.GetChatCompletionsAsync(chatCompletionsOptions);

            var result = response.Value.Choices.Select(mess =>
                new DialogDto()
                {
                    Role = mess.Message.Role.ToString(),
                    Message = mess.Message.Content
                });

            return result;
        }
    }
}
