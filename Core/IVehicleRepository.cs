
using vega.Controllers.Resources;
using vega.Core.Models;

namespace vega.Core
{
    public interface IVehicleRepository
    {
        void Remove(Vehicle vehicle);
        void Add(Vehicle vehicle);
        Task<Vehicle> GetVehicle(int id, bool includeRelated = true);
        Task<QueryResult<Vehicle>> GetVehicles(VehicleQuery filter);

    }
}