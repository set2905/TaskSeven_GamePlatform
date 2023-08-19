using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSeven_GamePlatform.Shared.Models.Requests
{
    public class SetConnIdRequestModel
    {
        public SetConnIdRequestModel(string connectionId, Guid playerId)
        {
            ConnectionId = connectionId;
            PlayerId = playerId;
        }

        public string ConnectionId { get; set; }
        public Guid PlayerId { get; set; }
    }
}
