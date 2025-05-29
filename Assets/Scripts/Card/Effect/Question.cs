using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

namespace Cards.CardEffect
{
    public class QuestionEffect : IEffect
    {
        private IEffect trueEffect; // 正确答案的效果
        private IEffect falseEffect; // 错误答案的效果

        private bool answerCorrect = false; // 是否回答正确

        public QuestionEffect(IEffect trueEffect, IEffect falseEffect)
        {
            this.trueEffect = trueEffect;
            this.falseEffect = falseEffect;
        }

        public IEnumerator Work(Character source, List<Character> targets)
        {
            var questionPrefab = CardResources.QuestionPrefab;
            var instance = GameObject.Instantiate(questionPrefab);
            QuestionManager qm = instance.GetComponent<QuestionManager>();
            qm.init(() => answerCorrect = true, () => answerCorrect = false);
            yield return qm.WaitUntilAnswered(); // 等待玩家回答
            yield return new WaitForSeconds(0.2f); // 等待一段时间以便玩家看到问题和答案
            qm.close();
            if (answerCorrect)
            {
                yield return trueEffect.Work(source, targets); // 执行正确答案的效果
            }
            else
            {
                yield return falseEffect.Work(source, targets); // 执行错误答案的效果
            }
        }
    }

    public static class QuestionEffectFactory
    {
        public static IEffect AmpilfyQuestion(float correctRate, float falseRate = 1.0f)
        {
            return new QuestionEffect(
                AmplifyEffectFactory.AmplifyRandomCard(correctRate),
                AmplifyEffectFactory.AmplifyRandomCard(falseRate)
            );
        }
    }
}