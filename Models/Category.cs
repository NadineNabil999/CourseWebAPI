public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }

    //3shan el category el wa7da gwaha aktr mn product fa kda I collection
    public ICollection<Product>? Products { get; set; }
}
