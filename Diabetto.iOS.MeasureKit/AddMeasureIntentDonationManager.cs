using System;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Diabetto.Core.Events.Measures;
using Diabetto.Core.Models;
using Diabetto.Core.Services;
using Foundation;
using Intents;
using MvvmCross.Logging;
using ReactiveUI;

namespace Diabetto.iOS.MeasureKit
{
    public interface IAddMeasureIntentDonationManager { }

    public sealed class AddMeasureIntentDonationManager : IAddMeasureIntentDonationManager
    {
        private readonly IMvxLog _logger;
        private readonly IBreadUnitsCalculator _breadUnitsCalculator;

        public AddMeasureIntentDonationManager(
            IBreadUnitsCalculator breadUnitsCalculator,
            IMvxLogProvider logProvider)
        {
            _breadUnitsCalculator = breadUnitsCalculator ?? throw new ArgumentNullException(nameof(breadUnitsCalculator));
            _logger = logProvider.GetLogFor<AddMeasureIntentDonationManager>();

            Console.WriteLine($"Subscribe to {nameof(MeasureAddedEvent)}");

            MessageBus.Current
                .Listen<MeasureAddedEvent>()
                .Where(v => v.Value != null)
                .Select(v => CreateInteraction(v.Value))
                .Subscribe(v => DonateInteraction(v));
        }

        private void DonateInteraction(INInteraction interaction)
        {
            interaction.DonateInteraction(
                error =>
                {
                    if (error != null)
                    {
                        _logger.Error($"Interaction donation failed: {error}");
                    }
                    else
                    {
                        _logger.Info("Successfully donated interaction.");
                    }
                });
        }

        private INInteraction CreateInteraction(Measure measure)
        {
            var breadUnits = measure.Products
                .Select(v => _breadUnitsCalculator.Calculate(v.Amount, v.ProductMeasureUnit.Carbohydrates))
                .DefaultIfEmpty(0)
                .Sum();

            var addMeasureIntent = new AddMeasureIntent
            {
                Level = new NSNumber(measure.Level / 10.0 ?? 0.0),
                Shortinsulin = new NSNumber(measure.ShortInsulin),
                Breadunits = new NSNumber(breadUnits)
            };

            var phraseBuilder = new StringBuilder("Add measure");

            if (measure.Level.HasValue)
            {
                phraseBuilder.Append($" {measure.Level.Value / 10.0} blood sugar");
            }

            if (measure.ShortInsulin > 0)
            {
                phraseBuilder.Append($" {measure.ShortInsulin} insulin");
            }
            
            if (breadUnits > 0)
            {
                phraseBuilder.Append($" {breadUnits} bread units");
            }

            var phrase = phraseBuilder.ToString();

            addMeasureIntent.SuggestedInvocationPhrase = phrase;

            var interaction = new INInteraction(addMeasureIntent, null)
            {
                Identifier = measure.Id.ToString()
            };

            return interaction;
        }
    }
}