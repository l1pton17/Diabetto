namespace Diabetto.Core.ViewModelResults
{
    public class DestructionResult<TEntity>
    {
        public TEntity Entity { get; set; }

        public bool Destroyed { get; set; }
    }
}