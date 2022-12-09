using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Core;
using vega.Core.Models;
using vega.Extensions;

namespace vega.Persistence
{
    public class VehicleRepository : IVehicleRepository
    {
        public VegaDbContext _context { get; set; }
        public VehicleRepository(VegaDbContext context)
        {
            this._context = context;

        }
        public async Task<Vehicle> GetVehicle(int id, bool includeRelated = true)
        {
            if(!includeRelated)
                return await _context.Vehicles.FindAsync(id);

            return await _context.Vehicles
                     .Include(v => v.Features)
                         .ThenInclude(vf => vf.Feature)
                     .Include(v => v.Model)
                         .ThenInclude(m => m.Make)
                     .SingleOrDefaultAsync(v => v.Id == id);
        }

        public void Add(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
        }

        public void Remove(Vehicle vehicle) 
        {
            _context.Remove(vehicle);
        }

        public async Task<QueryResult<Vehicle>> GetVehicles(VehicleQuery queryObject)
        {

            var result = new QueryResult<Vehicle>();

            var query = _context.Vehicles
                .Include(x => x.Model)
                    .ThenInclude(x => x.Make)
                .Include(x => x.Features)
                    .ThenInclude(x => x.Feature)
                .AsQueryable();
            
            var columnsMap = new Dictionary<string, Expression<Func<Vehicle, object>>>()
            {
                ["make"] = v => v.Model.Make.Name,
                ["model"] = v => v.Model.Name,
                ["contactName"] = v => v.ContactName                
            };


            if (queryObject.MakeId.HasValue)
                query = query.Where(v => v.Model.MakeId == queryObject.MakeId.Value);

            if (queryObject.ModelId.HasValue)
                query = query.Where(v => v.ModelId == queryObject.ModelId.Value);

            // if(queryObject.SortBy == "make")
            //     query = (queryObject.IsSortAscending) ? query.OrderBy(v => v.Model.Make.Name) : query.OrderByDescending(v => v.Model.Make.Name);

            //query = ApplyOrdering(queryObject, query, columnsMap);
            query = query.ApplyOrdering(queryObject, columnsMap);
            result.TotalItems = await query.CountAsync();
            query = query.ApplyPaging(queryObject);
            result.Items = await query.ToListAsync();
            return result;

        }

        // private static IQueryable<Vehicle> ApplyOrdering(VehicleQuery queryObject, IQueryable<Vehicle> query, Dictionary<string, Expression<Func<Vehicle, object>>> columnsMap)
        // {
        //     if (queryObject.IsSortAscending)
        //         return query.OrderBy(columnsMap[queryObject.SortBy]);
        //     else
        //         return query.OrderByDescending(columnsMap[queryObject.SortBy]);            
        // }
    }
}