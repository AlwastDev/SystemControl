using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SystemControl.Models.TestModels
{
    class AnswerModel
    {
        public long Id { get; set; }
        public string Answer { get; set; }
        public bool IsCorrectly { get; set; }
        [XmlIgnore]
        public bool? IsSelect { get; set; } = null;
        public bool IsValid()
        {
            return (IsCorrectly && IsSelect == true) || (!IsCorrectly && (IsSelect == false || IsSelect == null));
        }
    }
}
