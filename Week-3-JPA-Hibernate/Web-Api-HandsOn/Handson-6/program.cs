using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace ConsoleKafkaChat
{
    class Program
    {
        private const string BootstrapServers = "localhost:9092";
        private const string TopicName = "local-chat-stream";

        static async Task Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("====================================================");
            Console.WriteLine("    KAFKA COMMAND PROMPT MULTI-WAY CHAT COMPONENT   ");
            Console.WriteLine("====================================================");
            Console.Write("Enter your chat alias username: ");
            string username = Console.ReadLine() ?? "AnonymousUser";

            // Establish background thread tracking incoming message loops
            CancellationTokenSource cts = new CancellationTokenSource();
            Task.Run(() => StartConsumerPipeline(cts.Token));

            // Run main thread pipeline tracking message generation input writes
            await StartProducerPipeline(username);
            
            cts.Cancel();
        }

        private static async Task StartProducerPipeline(string userAlias)
        {
            var config = new ProducerConfig { BootstrapServers = BootstrapServers };

            using var producer = new ProducerBuilder<Null, string>(config).Build();
            Console.WriteLine($"\nSystem Status: Connection Ready. Begin typing messages below (Type 'exit' to terminate):\n");

            while (true)
            {
                string messageText = Console.ReadLine() ?? "";
                if (messageText.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

                if (!string.IsNullOrWhiteSpace(messageText))
                {
                    string structuredPayload = $"[{DateTime.Now:HH:mm:ss}] {userAlias}: {messageText}";
                    
                    try
                    {
                        // Push message payload stream async targets to Kafka topic partitions
                        await producer.ProduceAsync(TopicName, new Message<Null, string> { Value = structuredPayload });
                    }
                    catch (ProduceException<Null, string> ex)
                    {
                        Console.WriteLine($"[System Error Logging Intercept]: Broker write failure -> {ex.Error.Reason}");
                    }
                }
            }
        }

        private static void StartConsumerPipeline(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = BootstrapServers,
                GroupId = $"console-chat-group-{Guid.NewGuid()}", // Unique Group ID targets dynamic scaling duplication bypass
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(TopicName);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken);
                        // Write out received message streams down to the console command prompt view screen layout
                        Console.WriteLine($"\n{consumeResult.Message.Value}");
                    }
                    catch (ConsumeException ex)
                    {
                        // Safe capture logging tracking background errors without breaking terminal console interactions
                    }
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }
    }
}