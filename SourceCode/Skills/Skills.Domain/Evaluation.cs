using Skills.Domain.Tests;

namespace Skills.Domain
{
    public class Evaluation
    {
        private Skill skill;
        private EvaluationDate evaluationDate;
        private LikeLevel likeLevel;
        private KnowledgeLevel knowledgeLevel;
        private Consultant consultant;

        public Evaluation(Skill skill, EvaluationDate evaluationDate, LikeLevel likeLevel, KnowledgeLevel knowledgeLevel, Consultant consultant)
        {
            this.skill = skill;
            this.evaluationDate = evaluationDate;
            this.likeLevel = likeLevel;
            this.knowledgeLevel = knowledgeLevel;
            this.consultant = consultant;
        }

        public override bool Equals(object obj)
        {
            Evaluation evaluation = obj as Evaluation;
            return base.Equals(obj) || (evaluation != null && Equals(evaluation));
        }

        public bool Equals(Evaluation other)
        {
            bool areEqual = this.skill == other.skill &&
                this.consultant == other.consultant &&
                this.evaluationDate == other.evaluationDate &&
                this.knowledgeLevel == other.knowledgeLevel &&
                this.likeLevel == other.likeLevel;
            return areEqual;
        }
    }
}