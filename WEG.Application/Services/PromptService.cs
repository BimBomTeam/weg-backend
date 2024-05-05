using System.Text.Json;
using WEG.Application.Claims;

namespace WEG.Application.Services
{
    internal class PromptService
    {
        private readonly PromptsJsonContainer _rawPrompts;
        public PromptService()
        {
            string jsonFilePath = "Static/prompts.json";
            string jsonString = File.ReadAllText(jsonFilePath);
            _rawPrompts = JsonSerializer.Deserialize<PromptsJsonContainer>(jsonString);
        }
        public async Task<string> GetGenerateWordsPromptAsync(string level, string role)
        {
            return await Task.Run<string>(() =>
            {
                var raw = _rawPrompts.GenerateWordsPrompt;
                var result = string.Format(raw, level, role);

                return result;
            });
        }
        public async Task<string> GetStartChatPromptAsync(string level, string role, string wordsString)
        {
            return await Task.Run<string>(() =>
            {
                var raw = _rawPrompts.StartChatPrompt;
                var result = string.Format(raw, level, role, wordsString);

                return result;
            });
        }
    }
}
