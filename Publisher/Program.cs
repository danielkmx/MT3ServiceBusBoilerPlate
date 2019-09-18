using System;
using System.Threading.Tasks;
using Contratos;
using GreenPipes;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using MassTransit.Transports;
using MassTransit.Util;
using Microsoft.Azure.ServiceBus.Primitives;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingAzureServiceBus(h =>
            {
                var host = h.Host("azure service bus endpoint"
                    ,
                    z =>
                    {
                        z.TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", "key");
                    });

               
            });
            
            bus.Start();
            TaskUtil.Await(() => bus.Publish<DoSomething>(new DoSomething {Value = "algo aconteceu"}));
            TaskUtil.Await(() => bus.Publish<DoSomething>(new DoSomething {Value = "algo aconteceu de novo"}));
            TaskUtil.Await(() => Task.Delay(2000));
                
            bus.Stop();
        }
    }
}