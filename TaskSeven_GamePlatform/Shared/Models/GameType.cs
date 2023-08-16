using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSeven_GamePlatform.Shared.Models
{
    public class GameType
    {
        public GameType(Guid id)
        {
            Id=id;
        }

        public Guid Id { get; set; }
        public int FieldSize { get; set; }
    }
}
