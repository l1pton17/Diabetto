namespace Diabetto.Core.ViewModelResults
{
    public class EditResult<TEntity>
    {
        public TEntity Entity { get; set; }

        public bool Save { get; set; }
    }

    public static class EditResult
    {
        public static EditResult<T> Close<T>()
        {
            return new EditResult<T>
            {
                Save = false,
                Entity = default
            };
        }

        public static EditResult<T> Create<T>(T entity, bool save = true)
        {
            return new EditResult<T>
            {
                Save = save,
                Entity = entity
            };
        }
    }
}