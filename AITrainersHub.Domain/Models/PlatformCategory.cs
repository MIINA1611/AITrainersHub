using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITrainersHub.Domain.Models
{
    public class PlatformCategory
    {
        public Guid PlatformId { get; set; }
        public Platform Platform { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;


        public PlatformCategory()
        {
            
        }
        public PlatformCategory(Guid platformId, Guid categoryId)
        {
            PlatformId = platformId;
            CategoryId = categoryId;
        }
    }
}
