using StackExchange.Redis;
using System;

namespace Oraculo
{
    class Program
    {
        private const string canal = "perguntas";

        static void Main(string[] args)
        {
            Console.WriteLine("Oráculo está observando!");

            var client = ConnectionMultiplexer.Connect("localhost");

            var db = client.GetDatabase();

            var sub = client.GetSubscriber();

            sub.Subscribe(canal, (ch, msg) =>
            {
                db.HashSet("P1", "Los Jeff's", msg);
                Console.WriteLine(msg.ToString());
                var p1 = db.HashGetAll("P1");
                Console.WriteLine(p1.GetValue(0));
            });

            string exit;
            do
            {
                exit = Console.ReadLine();
            } while (exit != "Sair!");
        }
    }
}
