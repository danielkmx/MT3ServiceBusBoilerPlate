using System;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using Contratos;
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
                var host = h.Host(
                    ""
                    ,
                    z =>
                    {
                        z.TokenProvider =
                            TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey",
                    });


            });

            bus.Start();
            TaskUtil.Await(() => bus.Publish(new BlockedLoginEmail()
                {Destinatario = "danielbezerrakmx@gmail.com", Assunto = "Testando", Nome = "Daniel", Link = "wwww.facebook.com"}));
            

        TaskUtil.Await(() => Task.Delay(2000));
                
            bus.Stop();
        }
    }
}