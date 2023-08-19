using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSeven_GamePlatform.Shared.Models
{
    public class SetNameRequestModel
    {
        public SetNameRequestModel(string name)
        {
            Name=name;
        }

        public string Name { get; set; }   
    }
}
