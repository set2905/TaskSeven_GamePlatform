using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSeven_GamePlatform.Shared.Models
{
    public class GameStartRequestModel
    {
        public GameStartRequestModel(Guid playerId, Guid opponentId, string gameTypeName)
        {
            PlayerId=playerId;
            OpponentId=opponentId;
            GameTypeName=gameTypeName;
        }

        public Guid PlayerId { get; set; }
        public Guid OpponentId { get; set; }
        public string GameTypeName { get; set; }
    }
}
