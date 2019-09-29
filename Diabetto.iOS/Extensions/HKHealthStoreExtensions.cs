using System.Threading.Tasks;
using Foundation;
using HealthKit;

namespace Diabetto.iOS.Extensions
{
    public static class HKHealthStoreExtensions
    {
        public static Task<(bool Result, NSError Error)> DeleteObjectsAsync(
            this HKHealthStore healthStore,
            HKObjectType objectType,
            NSPredicate predicate)
        {
            var tcs = new TaskCompletionSource<(bool, NSError)>();

            healthStore.DeleteObjects(
                objectType,
                predicate,
                (isCompleted, _, error) => tcs.SetResult((isCompleted, error)));

            return tcs.Task;
        }
    }
}