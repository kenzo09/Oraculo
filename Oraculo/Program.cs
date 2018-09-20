using StackExchange.Redis;
using System;

namespace Oraculo
{
    class Program
    {
        private const string canal = "perguntas";
        private const string time = "Kensiderio";

        static void Main(string[] args)
        {
            Console.WriteLine("Oráculo está observando!");

            var client = ConnectionMultiplexer.Connect("191.232.234.20");

            var db = client.GetDatabase();

            var sub = client.GetSubscriber();

            sub.Subscribe(canal, (ch, msg) =>
            {
                var msgString = msg.ToString();
                var hashIndex = msgString.Substring(0, msgString.IndexOf(":"));
                try
                {
                    var param1 = msgString.Substring(msgString.IndexOf(" "), msgString.IndexOf("+") - msgString.IndexOf(" "));
                    var param2 = msgString.Substring(msgString.IndexOf("+") + 1, msgString.IndexOf("?") - msgString.IndexOf("+") - 1);

                    var resposta = Convert.ToInt32(param1) + Convert.ToInt32(param2);

                    db.HashSet(hashIndex, time, resposta);
                    var p1 = db.HashGetAll(hashIndex);
                    Console.WriteLine(p1.GetValue(0));
                }
                catch (Exception)
                {
                    db.HashSet(hashIndex, time, "Exception ;-;");
                }
            });

            string exit;
            do
            {
                exit = Console.ReadLine();
            } while (exit != "Sair!");
        }
    }
}
