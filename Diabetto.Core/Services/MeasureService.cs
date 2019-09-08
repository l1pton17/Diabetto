using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diabetto.Core.Models;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace Diabetto.Core.Services
{
    public interface IMeasureService
    {
        Task<List<Measure>> GetAsync(DateTime date);

        Task<List<Measure>> GetAsync(DateTime from, DateTime until);

        Task AddAsync(Measure value);

        Task EditAsync(Measure value);

        Task DeleteAsync(int id);
    }

    internal sealed class MeasureService : SQLiteConnection, IMeasureService
    {
        private static readonly object _lockObject = new object();

        public MeasureService(IDatabaseConnectionStringHolder connectionStringHolder)
            : base(connectionStringHolder.DatabaseFilePath)
        {
            CreateTable<Measure>();
        }

        /// <inheritdoc />
        public Task AddAsync(Measure value)
        {
            lock (_lockObject)
            {
                this.InsertOrReplaceWithChildren(value, recursive: true);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task EditAsync(Measure value)
        {
            lock (_lockObject)
            {
                this.InsertOrReplaceWithChildren(value, recursive: true);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task DeleteAsync(int id)
        {
            lock (_lockObject)
            {
                Delete(
                    new Measure
                    {
                        Id = id
                    });
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<List<Measure>> GetAsync(DateTime date)
        {
            return Task.Run(
                () =>
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

                        return this
                            .GetAllWithChildren<Measure>(v => v.Date >= beginDate && v.Date <= endDate, recursive: true)
                            .Select(v => FixDateTime(v))
                            .ToList();
                    }
                });
        }

        /// <inheritdoc />
        public Task<List<Measure>> GetAsync(DateTime from, DateTime until)
        {
            return Task.Run(
                () =>
                {
                    lock (_lockObject)
                    {
                        return this
                            .GetAllWithChildren<Measure>(v => v.Date >= from && v.Date <= until, recursive: false)
                            .Select(v => FixDateTime(v))
                            .ToList();
                    }
                });
        }

        private Measure FixDateTime(Measure measure)
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