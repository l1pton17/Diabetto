using System.Collections.Generic;
using System.Linq;
using Diabetto.Core.Models;
using Diabetto.Core.ViewModels.Dialogs;

namespace Diabetto.Core.ViewModels.Tags.Dialogs
{
    public sealed class SelectTagPickerViewModel : DialogPickerViewModel<Tag>
    {
        /// <inheritdoc />
        public SelectTagPickerViewModel(IEnumerable<Tag> tags)
            : base("Tags")
        {
            Item1Values
                .AddRange(
                    tags
                        .Select(v => DialogPickerOption.Create(v))
                        .Prepend(DialogPickerOption.Empty<Tag>()));
        }

        protected override string FormatItem1(DialogPickerOption<Tag> item)
        {
            if (item.IsEmpty)
            {
                return "Not selected";
            }

            return item.Item.Name;
        }
    }
}