using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diabetto.Core.Events.Measures;
using Diabetto.Core.Models;
using ReactiveUI;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace Diabetto.Core.Services.Repositories
{
    public interface IMeasureService
    {
        Task<Measure> GetAsync(int id);

        Task<List<Measure>> GetAsync(DateTime date);

        Task<List<Measure>> GetAsync(DateTime from, DateTime until);

        Task<List<Measure>> GetAllAsync();

        Task AddAsync(Measure value);

        Task EditAsync(Measure value);

        Task DeleteAsync(Measure value);
    }

    internal sealed class MeasureService : SQLiteConnection, IMeasureService
    {
        private static readonly object _lockObject = new object();

        public MeasureService(IDatabaseConnectionStringHolder connectionStringHolder)
            : base(connectionStringHolder.DatabaseFilePath)
        {
            CreateTable<Measure>();
            CreateTable<ProductMeasure>();
        }

        /// <inheritdoc />
        public Task<List<Measure>> GetAllAsync()
        {
            lock (_lockObject)
            {
                var values = Table<Measure>()
                    .Select(v => FixDateTime(v))
                    .ToList();

                return Task.FromResult(values);
            }
        }

        /// <inheritdoc />
        public Task AddAsync(Measure value)
        {
            lock (_lockObject)
            {
                value.Version = 0;

                this.InsertOrReplaceWithChildren(value, recursive: true);
            }

            MessageBus.Current.SendMessage(new MeasureAddedEvent(value));

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task EditAsync(Measure value)
        {
            lock (_lockObject)
            {
                value.Version++;

                this.InsertOrReplaceWithChildren(value, recursive: true);
            }

            MessageBus.Current.SendMessage(new MeasureChangedEvent(value));

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task DeleteAsync(Measure value)
        {
            lock (_lockObject)
            {
                Delete(
                    new Measure
                    {
                        Id = value.Id
                    });
            }

            MessageBus.Current.SendMessage(new MeasureDeletedEvent(value));

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<Measure> GetAsync(int id)
        {
            lock (_lockObject)
            {
                var value = this.GetWithChildren<Measure>(id, recursive: true);

                FixDateTime(value);

                return Task.FromResult(value);
            }
        }

        /// <inheritdoc />
        public Task<List<Measure>> GetAsync(DateTime date)
        {
            lock (_lockObject)
            {
                var beginDate = new DateTime(
                    date.Year,
                    date.Month,
                    date.Day,
                    0,
                    0,
                    0,
                    DateTimeKind.Utc);

                var endDate = beginDate.AddDays(1);

                var values = this
                    .GetAllWithChildren<Measure>(v => v.Date >= beginDate && v.Date <= endDate, recursive: true)
                    .Select(v => FixDateTime(v))
                    .ToList();

                return Task.FromResult(values);
            }
        }

        /// <inheritdoc />
        public Task<List<Measure>> GetAsync(DateTime from, DateTime until)
        {
            lock (_lockObject)
            {
                var values = this
                    .GetAllWithChildren<Measure>(v => v.Date >= from && v.Date <= until, recursive: false)
                    .Select(v => FixDateTime(v))
                    .ToList();

                return Task.FromResult(values);
            }
        }

        private static Measure FixDateTime(Measure measure)
        {
            if (measure == null)
            {
                return null;
            }

            measure.Date = DateTime.SpecifyKind(measure.Date, DateTimeKind.Utc);

            return measure;
        }
    }
}