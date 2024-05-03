using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEG.Application.Claims;
using WEG.Infrastructure.Services;


namespace WEG.Application.Services
{
    public class AiCommunicationService : IAiCommunicationService
    {
        private readonly OpenAIClient _client;

        public AiCommunicationService(IConfiguration config)
        {
            _client = new OpenAIClient(config["gpt_api_key"]);
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

            return response.Value.Choices[0].Message.Content;
        }
    }
}
