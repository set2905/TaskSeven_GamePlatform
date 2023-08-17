﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSeven_GamePlatform.Shared.Models
{
    public class GameType
    {
        public GameType()
        {
            Name=string.Empty;
        }

        public Guid Id { get; set; }
        public int FieldSize { get; set; }
        public string Name { get; set; }
    }
}
