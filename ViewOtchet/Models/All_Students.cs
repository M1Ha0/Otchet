using System;
using System.Collections.Generic;
using System.Text;

namespace OtchetClient.Models
{
    public class All_Students
    {
        public int IdStudents { get; set; }
        public string Name { get; set; } = null!;
        public string SurName { get; set; } = null!;
        public int IdGroup { get; set; }
    }
}
