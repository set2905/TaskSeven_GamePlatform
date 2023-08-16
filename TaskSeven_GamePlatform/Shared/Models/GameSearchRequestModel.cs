using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSeven_GamePlatform.Shared.Models
{
    public class GameSearchRequestModel
    {
        public GameSearchRequestModel(Guid playerId, Guid gameTypeId)
        {
            PlayerId=playerId;
            GameTypeId=gameTypeId;
        }

        public Guid PlayerId { get; set; }
        public Guid GameTypeId { get; set; }  
    }
}
