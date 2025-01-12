namespace Mechanics.Product
{
    public interface IGameProductData : IProductData
    {
        public GameGenre Genre { get; }
    }
}