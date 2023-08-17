using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSeven_GamePlatform.Shared.Models
{
    public class GetByIdRequestModel
    {
        public GetByIdRequestModel(Guid id)
        {
            Id=id;
        }

        public Guid Id { get; set; }
    }
}
