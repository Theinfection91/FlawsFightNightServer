using FlawsFightNightServer.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlawsFightNightServer.Managers
{
    public class BaseDataDriven
    {
        public string Name { get; set; }
        protected DataManager _dataManager;
        public BaseDataDriven(string name, DataManager dataManager)
        {
            Name = name;
            _dataManager = dataManager;
        }
    }
}
