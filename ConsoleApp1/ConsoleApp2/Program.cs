using System.Text;
using RabbitMQ.Client;
class Program
{
    static void Main(string[] args)
    {
        var count = 0;

        do
        {
            int timeToSleep = new Random().Next(100, 3000);
            Thread.Sleep(timeToSleep);

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "dev-queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                string message = $"Message from publisher, {count}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: "dev-queue",
                    basicProperties: null,
                    body: body);

                Console.WriteLine($"Message sent, [{count++}]");
            }
        } while (true);
    }
}