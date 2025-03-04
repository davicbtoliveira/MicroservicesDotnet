using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging.Logic.Interface;
using Newtonsoft.Json;

namespace Common.Logging.Logic
{
    public class RequestResponseLogModelCreator : IRequestResponseLogModelCreator
    {
        public RequestResponseLogModel LogModel { get; private set; }

        public RequestResponseLogModelCreator()
        {
            LogModel = new RequestResponseLogModel();
        }

        public string LogString()
        {
            var jsonString = JsonConvert.SerializeObject(LogModel);
            return jsonString;
        }
    }
}
