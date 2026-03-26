using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityPractice.Shared.Models
{
    public class VacationDTO
    {
        public int Id { get; set; }
        public string Place { get; set; } = string.Empty;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
