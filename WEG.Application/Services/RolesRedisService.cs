using System;
using System.Collections.Generic;
using StackExchange.Redis;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

namespace WEG.Application.Services
{
    public class RolesRedisService
    {
        private readonly ConnectionMultiplexer _redis;

        public RolesRedisService()
        {
            _redis = ConnectionMultiplexer.Connect("localhost");
        }

        public void CloseConnection()
        {
            _redis.Close();
        }

        public void WriteAll(Dictionary<string, string> data)
        {
            IDatabase db = _redis.GetDatabase();

            foreach (var kvp in data)
            {
                db.StringSet(kvp.Key, kvp.Value);
            }

            Console.WriteLine("Wszystkie dane zostały zapisane do Redis.");
            CloseConnection();
        }
        /*public void WriteAll(string key, List<object> data)
        {
            IDatabase db = _redis.GetDatabase();

            foreach (var kvp in data)
            {
                db.StringSet(kvp.Key, kvp.Value);
            }

            Console.WriteLine("Wszystkie dane zostały zapisane do Redis.");
            CloseConnection();
        }*/
        public void ReadAll(Dictionary<string, string> data)
        {
            var allData = new Dictionary<string, string>();

            IDatabase db = _redis.GetDatabase();

            foreach (var kvp in data)
            {
                string key = kvp.Key;
                RedisValue retrievedValue = db.StringGet(key);

                if (retrievedValue.HasValue)
                {
                    allData.Add(key, retrievedValue);
                }
                else
                {
                    Console.WriteLine($"Nie znaleziono wartości dla klucza: {key}");
                }
            }

            Console.WriteLine("Wszystkie dane zostały odczytane z bazy.");
            CloseConnection();
        }

    }
}
