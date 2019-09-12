namespace Diabetto.Core.Services
{
    public interface IBreadUnitsCalculator
    {
        float Calculate(int amount, float carbohydrates);
    }

    internal sealed class BreadUnitsCalculator : IBreadUnitsCalculator
    {
        /// <inheritdoc />
        public float Calculate(int amount, float carbohydrates)
        {
            var carbs = carbohydrates * amount;

            return carbs / 12.0f;//TODO: move to settings
        }
    }
}