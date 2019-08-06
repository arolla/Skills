using System;

namespace Skills.Domain.Tests
{
    public class Consultant
    {
        private string name;

        public Consultant(string name)
        {
            this.name = name;
        }

        public Evaluation AddEvaluation(Skill skill, EvaluationDate evaluationDate, LikeLevel likeLevel, KnowledgeLevel knowledgeLevel)
        {
            return new Evaluation(skill, evaluationDate, likeLevel, knowledgeLevel, this);
        }
    }
}