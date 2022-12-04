using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NinerFiVisualize.API.Data;
using NinerFiVisualize.API.Data.Models;

namespace NinerFiVisualize.API.Data.Services
{
    public class ChartsService
    {
        private NINERFIContext _context;


        public ChartsService(NINERFIContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<VwErrorTracking> GetErrorTrackingView()
        {
            IOrderedQueryable<VwErrorTracking> query;

            query = from view in _context.VwErrorTrackings
                    orderby view.Year, view.Month, view.Day
                    select view;

            return query.ToList();
        }

        public List<VwLogCount> GetLogCountView()
        {
            IOrderedQueryable<VwLogCount> query;

            query = from view in _context.VwLogCounts
                    orderby view.Year, view.Month, view.Day, view.Hour
                    select view;

            return query.ToList();
        }
    }
}
