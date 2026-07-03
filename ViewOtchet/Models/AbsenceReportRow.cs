using System;
using System.Collections.Generic;
using System.Text;

namespace OtchetClient.Models
{
    public class AbsenceReportRow
    {
        public string StudentName { get; set; } = null!;
        public string StudentSurname { get; set; } = null!;
        public string GroupName { get; set; } = null!;
        public DateTime Date { get; set; }
        public int NumPara { get; set; }
        public string Status { get; set; } = null!;
    }
}
