using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;
using WEG.Application.Claims;
using WEG.Infrastructure.Dto;
using WEG.Infrastructure.Dto.Boss;
using WEG.Infrastructure.Dto.WordsGenerate;
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
        public async Task<IEnumerable<DialogDto>> StartDialog(string role, string level, string wordsArray)
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

            var list = new List<DialogDto>()
            {
                new DialogDto() { Message = prompt, Role = DialogRoles.User },
                new DialogDto() { Message = response.Value.Choices[0].Message.Content, Role = DialogRoles.Assistant }
            };

            return list;
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
                if (message.Role == DialogRoles.Assistant)
                    chatHistory.Add(new ChatRequestAssistantMessage(message.Message));
                else if (message.Role == DialogRoles.User)
                    chatHistory.Add(new ChatRequestUserMessage(message.Message));
                else
                    chatHistory.Add(new ChatRequestUserMessage(message.Message));
            }

            chatCompletionsOptions.Messages.Add(new ChatRequestUserMessage(messageStr));
            var response = await _client.GetChatCompletionsAsync(chatCompletionsOptions);

            var conversation = new List<DialogDto>(messages);
            conversation.Add(new DialogDto()
            {
                Message = messageStr,
                Role = DialogRoles.User
            });
            conversation.Add(new DialogDto()
            {
                Message = response.Value.Choices[0].Message.Content,
                Role = DialogRoles.Assistant
            });

            return conversation;
        }

        public async Task<GenerateWordsResponseDto> GenerateWordsAsync(string level, string role)
        {
            string prompt = await _promptService.GetGenerateWordsPromptAsync(level, role);

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                DeploymentName = "gpt-3.5-turbo",
                Temperature = (float)1,
                MaxTokens = 800,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
                ResponseFormat = ChatCompletionsResponseFormat.JsonObject,
            };

            chatCompletionsOptions.Messages.Add(new ChatRequestUserMessage(prompt));
            var response = await _client.GetChatCompletionsAsync(chatCompletionsOptions);
            var responseStr = response.Value.Choices[0].Message.Content;
            try
            {
                var wordsArray = JsonSerializer.Deserialize<GenerateWordsResponseDto>(responseStr);
                if (wordsArray == null)
                    throw new Exception("Bad JSON words format from OpenAi");
                return wordsArray;
            }
            catch (Exception)
            {
                throw new Exception("Bad JSON words format from OpenAi");
                throw;
            }

        }

        public async Task<BossQuizUnitDto> GenerateBossQuiz(string wordtoTranslate)
        {
            var prompt = await _promptService.GetStartBossPromptAsync(wordtoTranslate);

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                DeploymentName = "gpt-3.5-turbo",
                Temperature = (float)1,
                MaxTokens = 800,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
                ResponseFormat = ChatCompletionsResponseFormat.JsonObject,
            };

            chatCompletionsOptions.Messages.Add(new ChatRequestUserMessage(prompt));
            var response = await _client.GetChatCompletionsAsync(chatCompletionsOptions);
            var responseStr = response.Value.Choices[0].Message.Content;

            try
            {
                var answers = JsonSerializer.Deserialize<BossQuizUnitDto>(responseStr);
                if (answers == null)
                    throw new Exception("Bad JSON words format from OpenAi");
                return answers;
            }
            catch (Exception)
            {
                throw new Exception("Bad JSON words format from OpenAi");
                throw;
            }

        }
    }
}
