using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Persistence;

namespace vega.Controllers
{
    [Route("/api/[controller]")]
    public class MakesController : Controller
    {
        private readonly VegaDbContext _context;
        private readonly IMapper _mapper;
        public MakesController(VegaDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

       
        // [HttpGet]
        // public async Task<IEnumerable<MakeResource>> GetMakes()
        // {
           
        //     var makes = await _context.Makes.Include(m => m.Models).ToListAsync();
        //     var makesVM = new List<MakeResource>();
            
        //     foreach (var item in makes)
        //     {
        //         MakeResource m = new MakeResource();
        //         m.Id = item.Id;
        //         m.Name = item.Name;
        //         foreach (var innerItem in item.Models)
        //         {
        //             ModelResource mr = new ModelResource();
        //             mr.Id = innerItem.Id;
        //             mr.Name = innerItem.Name;   
        //             m.Models.Add(mr);
        //         }
        //         makesVM.Add(m);

        //     }
                                
        //     return makesVM;
            
        // }


        [HttpGet("/api/makes")]
        public async Task<IEnumerable<MakeResource>> GetMakes()
        {
            var makes = await _context.Makes.Include(m => m.Models).ToListAsync();
            return _mapper.Map<List<Make>,List<MakeResource>>(makes);
        }

    }
}
