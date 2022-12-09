using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Persistence;

namespace vega.Controllers
{
    [Route("/api/[controller]")]
    public class FeaturesController : Controller
    {
        private readonly VegaDbContext _context;
        private readonly IMapper _mapper;
        public FeaturesController(VegaDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }        


        [HttpGet("/api/features")]
        [Authorize]
        public async Task<IEnumerable<KeyValuePairResource>> GetFeatures()
        {

            //  var makes = await _context.Makes.Include(m => m.Models).ToListAsync();
            // return _mapper.Map<List<Make>,List<MakeResource>>(makes);
           
            var features = await _context.Features.ToListAsync();
            return _mapper.Map<List<Feature>,List<KeyValuePairResource>>(features);
        }

    }
}
