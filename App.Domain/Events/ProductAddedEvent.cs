namespace App.Domain.Events
{
    //request veya response' un body' sinde herhangi bir değişiklik yapılmasını istemiyorsak record
    public record ProductAddedEvent(int Id, string Name, decimal Price, int stock) : IEventOrMessage;
}
