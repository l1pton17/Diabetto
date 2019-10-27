using System;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Diabetto.Core.Events.Measures;
using Diabetto.Core.Extensions;
using Diabetto.Core.Models;
using Diabetto.Core.Services;
using Foundation;
using Intents;
using MvvmCross.Logging;
using ReactiveUI;

namespace Diabetto.iOS.Intents.Shared
{
    public interface IAddMeasureIntentDonationManager
    {
    }

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
                .Select(v => CreateInteractionOrDefault(v.Value))
                .Where(v => v != null)
                .Subscribe(DonateInteraction);
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

        private INInteraction CreateInteractionOrDefault(Measure measure)
        {
            var breadUnits = measure.Products
                .Select(v => _breadUnitsCalculator.Calculate(v.Amount, v.ProductMeasureUnit.Carbohydrates))
                .DefaultIfEmpty(0)
                .Sum();

            var isEmpty = !measure.Level.HasValue
                && breadUnits < Double.Epsilon
                && measure.ShortInsulin == 0;

            if (isEmpty)
            {
                return null;
            }

            var intent = measure.Level.HasValue
                ? CreateAddMeasureIntent(measure, breadUnits)
                : CreateAddShortInsulinIntent(measure, breadUnits);

            var interaction = new INInteraction(intent, null)
            {
                Identifier = measure.Id.ToString()
            };

            return interaction;
        }

        private INIntent CreateAddShortInsulinIntent(Measure measure, double breadUnits)
        {
            if (measure.Level.HasValue)
            {
                throw new InvalidOperationException("Measure must hasn't a level");
            }

            var intent = new AddShortInsulinIntent
            {
                ShortInsulin = new NSNumber(measure.ShortInsulin),
                BreadUnits = new NSNumber(breadUnits)
            };

            var phraseBuilder = new StringBuilder("Add ");

            if (measure.ShortInsulin > 0)
            {
                phraseBuilder.Append($" {measure.ShortInsulin} short insulin");

                if (breadUnits > 0)
                {
                    phraseBuilder.Append($" {breadUnits} bread units");
                }
            }
            else
            {
                phraseBuilder.Append($" {breadUnits} bread units");
            }

            var phrase = phraseBuilder.ToString();

            intent.SuggestedInvocationPhrase = phrase;

            return intent;
        }

        private INIntent CreateAddMeasureIntent(Measure measure, double breadUnits)
        {
            if (!measure.Level.HasValue)
            {
                throw new InvalidOperationException("Measure must has a level");
            }

            var intent = new AddMeasureIntent
            {
                Level = new NSNumber(measure.Level / 10.0 ?? 0.0),
                Shortinsulin = new NSNumber(measure.ShortInsulin),
                Breadunits = new NSNumber(breadUnits)
            };

            var phraseBuilder = new StringBuilder("Add blood sugar");

            phraseBuilder.Append($" {measure.GetLevel()}");

            if (measure.ShortInsulin > 0)
            {
                phraseBuilder.Append($" {measure.ShortInsulin} insulin");
            }

            if (breadUnits > 0)
            {
                phraseBuilder.Append($" {breadUnits} bread units");
            }

            var phrase = phraseBuilder.ToString();

            intent.SuggestedInvocationPhrase = phrase;

            return intent;
        }
    }
}