using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Data;
using System.Reflection.Emit;
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
            return await Task.Run(() =>
            {
                var raw = _rawPrompts.GenerateWordsPrompt;
                var result = string.Format(raw, level, role);
                var exceptedWordsFormat = _rawPrompts.ExceptedWordsFormat;
                result += exceptedWordsFormat;

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
        public async Task<string> GetStartBossPromptAsync(string wordToTranslate)
        {
            return await Task.Run<string>(() =>
            {
                var raw = _rawPrompts.GetTranslationIntoPolishPrompt;
                var result = raw.Replace("{0}", wordToTranslate);
                return result;
            }); 
        }
    }
}
