using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IQuestionClassificationService
    {
        QuestionType ClassifyQuestion(string question);
    }

    public enum QuestionType
    {
        Relevant,
        Irrelevant
    }
} 