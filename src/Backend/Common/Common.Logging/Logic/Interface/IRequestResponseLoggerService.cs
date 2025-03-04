using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging.Logic.Interface
{
    public interface IRequestResponseLoggerService
    {
        Task Log(IRequestResponseLogModelCreator logCreator);

    }
}
