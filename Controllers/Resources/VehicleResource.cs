using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vega.Core.Models;

namespace vega.Controllers.Resources
{
    public class VehicleResource
    {       
        public KeyValuePairResource Model { get; set; }     
        public KeyValuePairResource Make { get; set; }  
        public int Id { get; set; }
        public bool IsRegistered { get; set; }      
        public ContactResource Contact { get; set; }         
        public DateTime LastUpdate { get; set; }
        public ICollection<KeyValuePairResource> Features { get; set; }
        public VehicleResource() 
        {
            Features = new Collection<KeyValuePairResource>();
        } 
       


    }
}