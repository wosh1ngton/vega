using Microsoft.EntityFrameworkCore;
using vega.Core;
using vega.Core.Models;

namespace vega.Persistence
{
    public class PhotoRepository : IPhotoRepository
    {
        public VegaDbContext _context { get; set; }

        public PhotoRepository(VegaDbContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<Photo>> GetPhotos(int vehicleId)
        {
            return await _context.Photos
                .Where(p => p.VehicleId == vehicleId).ToListAsync();
                
        }

        public async Task<Photo> GetPhoto(int id)
        {
            return await _context.Photos.SingleOrDefaultAsync(x => x.Id == id);
        }

        public void Remove(Photo photo)
        {
            _context.Remove(photo);
        }
    }
}