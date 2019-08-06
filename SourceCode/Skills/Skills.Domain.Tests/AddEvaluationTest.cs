using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Skills.Domain.Tests
{
    [TestClass]
    public class AddEvaluationTest
    {
        [TestMethod]
        public void Should_add_evaluation_to_one_consultant()
        {
            Consultant dany = new Consultant("dany");
            Skill skill = new Skill();
            EvaluationDate evaluationDate = new EvaluationDate();
            LikeLevel likeLevel = new LikeLevel();
            KnowledgeLevel knowledgeLevel = new KnowledgeLevel();
            Evaluation addedEvaluation = dany.AddEvaluation(skill, evaluationDate, likeLevel, knowledgeLevel);

            Evaluation expectedEvaluation = new Evaluation(skill, evaluationDate, likeLevel, knowledgeLevel, dany);
            Check.That(addedEvaluation).IsEqualTo(expectedEvaluation);
        }
    }
}
