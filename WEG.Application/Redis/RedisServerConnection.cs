using StackExchange.Redis;
using System;

namespace WEG.Application.Redis
{
    class RedisServerConnection
    {
        private readonly ConnectionMultiplexer _redis;

        public RedisServerConnection()
        {
            _redis = ConnectionMultiplexer.Connect("localhost");//localhost (127.0.0.1) na porcie 6379
        }

        public void ConnectAndOperateWithRedis(string key, string value)// połączenie i zapisanie
        {
            IDatabase db = _redis.GetDatabase();

            db.StringSet(key, value);
            string retrievedValue = db.StringGet(key);
            Console.WriteLine($"Wartość pobrana z Redis dla klucza {key}: {retrievedValue}");
        }

        public void CloseConnection()
        {
            _redis.Close();
        }
    }
}