using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Skills.Domain.Tests
{
    [TestClass]
    public class EvaluationTest
    {
        [TestMethod]
        public void Should_return_true_when_all_evaluation_fields_are_equal()
        {
            Consultant dany = new Consultant("dany");
            Skill skill = new Skill();
            EvaluationDate evaluationDate = new EvaluationDate();
            LikeLevel likeLevel = new LikeLevel();
            KnowledgeLevel knowledgeLevel = new KnowledgeLevel();

            Evaluation expectedEvaluation = new Evaluation(skill, evaluationDate, likeLevel, knowledgeLevel, dany);
            Evaluation evaluation = new Evaluation(skill, evaluationDate, likeLevel, knowledgeLevel, dany);
            Check.That(evaluation).IsEqualTo(expectedEvaluation);
        }

        [TestMethod]
        public void Should_return_false_when_evaluation_skills_are_different()
        {
            Consultant dany = new Consultant("dany");
            Skill skill = new Skill();
            EvaluationDate evaluationDate = new EvaluationDate();
            LikeLevel likeLevel = new LikeLevel();
            KnowledgeLevel knowledgeLevel = new KnowledgeLevel();

            Evaluation expectedEvaluation = new Evaluation(skill, evaluationDate, likeLevel, knowledgeLevel, dany);
            Skill skill2 = new Skill();
            Evaluation evaluation = new Evaluation(skill2, evaluationDate, likeLevel, knowledgeLevel, dany);
            Check.That(evaluation).IsNotEqualTo(expectedEvaluation);
        }

        [TestMethod]
        public void Should_return_false_when_evaluation_dates_are_different()
        {
            Consultant dany = new Consultant("dany");
            Skill skill = new Skill();
            EvaluationDate evaluationDate = new EvaluationDate();
            LikeLevel likeLevel = new LikeLevel();
            KnowledgeLevel knowledgeLevel = new KnowledgeLevel();

            Evaluation expectedEvaluation = new Evaluation(skill, evaluationDate, likeLevel, knowledgeLevel, dany);
            EvaluationDate evaluationDate2 = new EvaluationDate();
            Evaluation evaluation = new Evaluation(skill, evaluationDate2, likeLevel, knowledgeLevel, dany);
            Check.That(evaluation).IsNotEqualTo(expectedEvaluation);
        }

        [TestMethod]
        public void Should_return_false_when_evaluation_like_levels_are_different()
        {
            Consultant dany = new Consultant("dany");
            Skill skill = new Skill();
            EvaluationDate evaluationDate = new EvaluationDate();
            LikeLevel likeLevel = new LikeLevel();
            KnowledgeLevel knowledgeLevel = new KnowledgeLevel();

            Evaluation expectedEvaluation = new Evaluation(skill, evaluationDate, likeLevel, knowledgeLevel, dany);
            LikeLevel likeLevel2 = new LikeLevel();
            Evaluation evaluation = new Evaluation(skill, evaluationDate, likeLevel2, knowledgeLevel, dany);
            Check.That(evaluation).IsNotEqualTo(expectedEvaluation);
        }

        [TestMethod]
        public void Should_return_false_when_evaluation_knowledge_levels_are_different()
        {
            Consultant dany = new Consultant("dany");
            Skill skill = new Skill();
            EvaluationDate evaluationDate = new EvaluationDate();
            LikeLevel likeLevel = new LikeLevel();
            KnowledgeLevel knowledgeLevel = new KnowledgeLevel();

            Evaluation expectedEvaluation = new Evaluation(skill, evaluationDate, likeLevel, knowledgeLevel, dany);
            KnowledgeLevel knowledgeLevel2 = new KnowledgeLevel();
            Evaluation evaluation = new Evaluation(skill, evaluationDate, likeLevel, knowledgeLevel2, dany);
            Check.That(evaluation).IsNotEqualTo(expectedEvaluation);
        }
    }
}
