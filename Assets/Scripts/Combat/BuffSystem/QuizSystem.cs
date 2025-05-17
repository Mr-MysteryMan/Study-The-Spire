using Combat.Buffs;

public class QuizSystem
{
    private IBuff buff;

    public QuizSystem(IBuff buff)
    {
        this.buff = buff;
    }

    public void OnAnswerResult(bool isCorrect)
    {
        if (isCorrect)
        {
            buff.SetMultiplier(2f); // 答题正确，倍率加倍
        }
        else
        {
            buff.SetMultiplier(0.5f); // 答题错误，倍率减半
        }
    }
}
