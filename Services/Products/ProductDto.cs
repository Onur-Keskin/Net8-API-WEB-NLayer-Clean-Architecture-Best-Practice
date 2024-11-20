namespace App.Services.Products
{
    public record ProductDto(int Id, string Name, decimal Price, int Stock,int CategoryId); //referanslarına göre değil propertylerine göre karşılaştırma yapar
    //record lar derlendiğinde class' a dönüşüyor

    //public int Id { get; init; }
    //public string Name { get; init; } = default!;
    //public decimal Price { get; init; }
    //public int Stock { get; init; }
}
