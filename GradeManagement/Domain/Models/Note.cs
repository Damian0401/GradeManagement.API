﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Note
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Payload { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
