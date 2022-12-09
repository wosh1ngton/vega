
using vega.Controllers.Resources;
using vega.Core.Models;

namespace vega.Core
{
    public interface IPhotoRepository
    {
        
        Task<IEnumerable<Photo>> GetPhotos(int id);
        Task<Photo> GetPhoto(int id);
        void Remove(Photo photo);

    }
}