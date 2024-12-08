using App.Domain.Events;

namespace App.Application.ServiceBus
{
    public interface IServiceBus
    {
        Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IEventOrMessage; //Event' ler olmuş bitmiş olayları mesajlar olacka olayları temsil eder.

        Task SendAsync<T>(T message, string queueName,CancellationToken cancellationToken = default) where T : IEventOrMessage; //Send direkt kuyruğa, exchange ise routing yapıp ilgili kuyruğa gönderir

    }
}
