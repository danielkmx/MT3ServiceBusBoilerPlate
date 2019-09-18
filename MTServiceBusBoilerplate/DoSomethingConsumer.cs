using System;
using System.Threading.Tasks;
using Contratos;
using MassTransit;

namespace MTServiceBusBoilerplate
{
    public class DoSomethingConsumer :
        IConsumer<DoSomething>
    {


        public async Task Consume(ConsumeContext<DoSomething> context)
        {

            Console.WriteLine(context.Message.Value);
            await context.RespondAsync<DoSomething>(new
            {
                
                Value = $"Received: {context.Message.Value}"
            });
        }
    }
}