using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class Session
    {
        public DateTime? Expires { get; set; }
        public string Token { get; set; } = null!;
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
    }
}
