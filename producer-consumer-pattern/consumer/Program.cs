using Confluent.Kafka;
using consumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace consumer
{
    static class Program
    {
        static ConsumerConfig _config = new ConsumerConfig();
        static string topicName = "";
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            var service = new consumerService(_config, topicName);
            await service.Receive();
        }


        /// <summary>
        /// Método estático usado para bootstrap de propriedades, injeção de dependência e outras configurações ambientais.
        /// Normalmente, isso seria configurado em um aplicativo web ou API ASP.NET Core mais tradicional. Este método está sendo usado
        /// principalmente para extrair itens de configuração que preencherão a configuração do consumidor Kafka.
        /// Observe que AllowAutoCreateTopics está definido como true — isso permitirá que você use qualquer nome de tópico que desejar.
        /// </summary>
        /// <param name="args">Argumentos de tempo de execução adicionais</param>
        /// <returns>Uma implementação concreta da interface IHostBuilder, permitindo que as configurações do host sejam instanciadas.</returns>
        static IHostBuilder CreateHostBuilder(string[] args) =>
                    Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, configuration) =>
                    {
                        configuration.Sources.Clear();
                        IHostEnvironment env = hostingContext.HostingEnvironment;
                        configuration
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
                        IConfigurationRoot config = configuration.Build();
                        _config = new ConsumerConfig() { BootstrapServers = config.GetValue<string>("KafkaServer"), GroupId = config.GetValue<string>("DefaultGroupId"), AllowAutoCreateTopics = true,  };
                        topicName = config.GetValue<string>("Topic");
                    });
    }
    
}
