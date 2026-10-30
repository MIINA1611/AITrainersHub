using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITrainersHub.Application.Interfaces
{
    public interface ICacheInvalidatorCommand
    {
        IEnumerable<string> CacheKeys { get; }
    }
}
