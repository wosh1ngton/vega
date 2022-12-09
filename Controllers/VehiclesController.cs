using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Core;
using vega.Persistence;
using Microsoft.AspNetCore.Authorization;

namespace vega.Controllers
{
    // [ApiController]
    [Route("/api/[controller]")]
    public class VehiclesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVehicleRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public VehiclesController(IUnitOfWork unitOfWork, IMapper mapper, IVehicleRepository repository)
        {
            this._unitOfWork = unitOfWork;
            this._repository = repository;
            this._mapper = mapper;            
        }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateVehicle([FromBody] SaveVehicleResource vehicleResource)
    {
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        //recebe uma viewModel e transforma em enttity pra salvar no banco
        var vehicle = _mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource);
        vehicle.LastUpdate = DateTime.Now;

        //Salva a entidade no banco
        _repository.Add(vehicle);
        await _unitOfWork.CompleteAsync();

        vehicle = await _repository.GetVehicle(vehicle.Id);

        //Retorna o resultado como viewModel
        var result = _mapper.Map<Vehicle, VehicleResource>(vehicle);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateVehicle(int id, [FromBody] SaveVehicleResource vehicleResource)
    {

        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        //recebe uma viewModel e transforma em enttity pra salvar no banco
        var vehicle = await _repository.GetVehicle(id);

        if (vehicle == null)
            return NotFound();

        _mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicle);
        vehicle.LastUpdate = DateTime.Now;
        await _unitOfWork.CompleteAsync();
        
        vehicle = await _repository.GetVehicle(vehicle.Id);
        //Retorna o resultado como viewModel
        var result = _mapper.Map<Vehicle, VehicleResource>(vehicle);
        return Ok(result);
    }

    // [HttpGet]
    // public IActionResult Get([FromQuery] Feature feature) 
    // {
    //     return Ok(feature);
    // }

    // [HttpPost]
    // public IActionResult Post([FromBody] Feature feature) 
    // {
    //     return Ok(feature);
    // }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteVehicle(int id)
    {
        var vehicle = await _repository.GetVehicle(id, includeRelated: false);

        if (vehicle == null)
            return NotFound();

        _repository.Remove(vehicle);
        await _unitOfWork.CompleteAsync();
        return Ok(id);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVehicle(int id)
    {
        var vehicle = await _repository.GetVehicle(id);

        if (vehicle == null)
            return NotFound();

        var vehicleResource = _mapper.Map<Vehicle, VehicleResource>(vehicle);
        return Ok(vehicleResource);
    }

    [HttpGet]
    public async Task<QueryResultResource<VehicleResource>> GetVehicles(VehicleQueryResource filterResource) 
    {
        var filter = _mapper.Map<VehicleQueryResource,VehicleQuery>(filterResource);
        var queryResult = await _repository.GetVehicles(filter);
        return _mapper.Map<QueryResult<Vehicle>, QueryResultResource<VehicleResource>>(queryResult);         
         
    }

    }
}