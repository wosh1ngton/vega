using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using vega.Controllers.Resources;
using vega.Core;
using vega.Core.Models;

namespace vega.Controllers
{
    // /api/vehicles/1/photos
        
    [Route("/api/vehicles/{vehicleId}/photos")]
    public class PhotosController : Controller
    {
        private readonly PhotoSettings _photoSettings;       
        private readonly IWebHostEnvironment _host;
        private readonly IVehicleRepository _repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoRepository _photoRepository;
        public PhotosController(IWebHostEnvironment host, 
        IVehicleRepository repository, 
        IUnitOfWork unitOfWork,
        IMapper mapper,        
        IOptionsSnapshot<PhotoSettings> options,
        IPhotoRepository photoRepository)
        {
            this.unitOfWork = unitOfWork;
            this._repository = repository;
            this._host = host;
            _mapper = mapper;
            _photoSettings = options.Value;
            _photoRepository = photoRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Upload(int vehicleId, IFormFile file)
        {
            var vehicle = await _repository.GetVehicle(vehicleId, includeRelated: false);
            if (vehicle == null)
                return NotFound();

            if(file == null) return BadRequest("null file");
            if(file.Length == 0) return BadRequest("empty file");
            if(file.Length > _photoSettings.MaxBytes) return BadRequest("Max file size exceeded");
           // if(_photoSettings.IsSupported(file.FileName)) return BadRequest("Invalid file type");

            var uploadsFolderPath = Path.Combine(this._host.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            //System.Drawing()
            var photo = new Photo { FileName = fileName };
            vehicle.Photos.Add(photo);
            await unitOfWork.CompleteAsync();
            return Ok(_mapper.Map<Photo, PhotoResource>(photo));

        }

        [HttpGet]
        public async Task<IEnumerable<PhotoResource>> GetPhotos(int vehicleId)
        {
            var photos = await _photoRepository.GetPhotos(vehicleId);
            return _mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
        }

        [HttpDelete("{photoId}")]
        public async Task<IActionResult> Delete(int photoId)
        {
            
            var photo = await _photoRepository.GetPhoto(photoId);
            if(photo is null) 
                return NotFound();
            
            _photoRepository.Remove(photo);
            await unitOfWork.CompleteAsync();
            return Ok(photoId);
        }
    }
}