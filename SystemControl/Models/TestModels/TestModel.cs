using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemControl.Models.TestModels
{
    class TestModel
    {
        public List<QuestionModel> QuestionList { get; set; } = new List<QuestionModel>();
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public long AuthorId { get; set; }
        public DateTime CreateDate { get; set; }

        public TestModel()
        {
        }

        public TestModel(string Name, string Description, List<QuestionModel> QuestionList, int AuthorId, DateTime CreateDate)
        {
            this.Name = Name;
            this.QuestionList = QuestionList;
            this.Description = Description;
            this.AuthorId = AuthorId;
            this.CreateDate = CreateDate;
        }

        public double PercentageCorrectAnswers()
        {
            double validCount = 0;
            foreach(var item in QuestionList)
            {
                if (item.IsValid())
                    validCount++;
            }
            return Math.Round(((validCount * 100) / QuestionList.Count()), 2);
        }

    }
}
