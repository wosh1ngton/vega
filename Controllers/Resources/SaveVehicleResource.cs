using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vega.Core.Models;

namespace vega.Controllers.Resources
{
    public class SaveVehicleResource
    {       
        public int ModelId { get; set; }
        public int Id { get; set; }
        public bool IsRegistered { get; set; }     
        [Required]    
        public ContactResource Contact { get; set; }
        public ICollection<int> Features { get; set; }
        public SaveVehicleResource()
        {
            Features = new Collection<int>();
        }

    }
}