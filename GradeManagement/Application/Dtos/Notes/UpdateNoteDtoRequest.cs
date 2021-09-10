using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Notes
{
    public class UpdateNoteDtoRequest
    {
        public string Title { get; set; }

        public string Payload { get; set; }
    }
}
