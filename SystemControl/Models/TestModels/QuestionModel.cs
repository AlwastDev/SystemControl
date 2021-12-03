using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemControl.Models.TestModels
{
    class QuestionModel
    {
        public long Id { get; set; }
        public List<AnswerModel> AnswerList { get; set; } = new List<AnswerModel>();
        public string Question { get; set; }

        public QuestionModel() { }

        public QuestionModel(string Question, List<AnswerModel> AnswerList)
        {
            this.Question = Question;
            this.AnswerList = AnswerList;
        }
        public bool IsValid()
        {
            foreach(var item in AnswerList)
            {
                if (!item.IsValid())
                    return false;
            }
            return true;
        }
    }
}
