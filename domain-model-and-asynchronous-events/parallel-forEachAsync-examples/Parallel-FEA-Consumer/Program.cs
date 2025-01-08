using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

var topics = config["Topics"].Split("|").ToList();
var consumerList = new List<IConsumer<string, string>>();

topics.ForEach(topic =>
{
    var consumer = new ConsumerBuilder<string, string>(new ConsumerConfig()
    {
        GroupId = config["ConsumerConfig:GroupId"],
        BootstrapServers = config["ConsumerConfig:BootstrapServers"],
        AllowAutoCreateTopics = true,
    }).Build();
    consumer.Subscribe(topic);
    consumerList.Add(consumer);
});

ParallelOptions options = new ParallelOptions()
{
    MaxDegreeOfParallelism = 3
};

await Parallel.ForEachAsync(consumerList, options, async (s, t) =>
{
    if (t.IsCancellationRequested)
    {
        s.Close();
    }
    else
    {
        await Task.Factory.StartNew(() =>
        {
            Console.WriteLine($"Consuming topic: {s.Subscription.First()}...");
            try
            {
                var result = s.Consume(t);
                if (result != null)
                {
                    Console.WriteLine($"Key: {result.Message.Key}, Value: {result.Message.Value}");
                }
            }
            catch (ConsumeException ex)
            {
                Console.WriteLine($"Error consuming message: {ex.Error.Reason}");
            }
        });
    }
});
