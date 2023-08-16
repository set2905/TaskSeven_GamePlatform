using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSeven_GamePlatform.Shared.Models
{
    public class GameSearchRequestModel
    {
        public GameSearchRequestModel(string playerName, Guid gameTypeId)
        {
            PlayerName=playerName;
            GameTypeId=gameTypeId;
        }

        public string PlayerName { get; set; }
        public Guid GameTypeId { get; set; }  
    }
}
