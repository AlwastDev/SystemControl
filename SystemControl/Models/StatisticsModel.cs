using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemControl.Models
{
    class StatisticsModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long TestId { get; set; }
        public double Percent { get; set; }
        public DateTime DatePassage { get; set; }
    }
}
