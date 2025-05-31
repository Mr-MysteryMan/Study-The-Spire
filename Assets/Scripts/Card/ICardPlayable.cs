using Combat;

/// <summary>
/// 卡牌可用性的扩展接口，在CardData中实现该接口将会取代CardManager的IsPlayable方法。
/// 此时，CardManager将仅判断是否在手牌，将不会判断费用和目标等因素，需要自行实现逻辑。
/// </summary>
public interface ICardPlayable
{
    public Result IsPlayable(CombatSystem combatSystem);
}

public static class CardPlayableExtensions
{
    public static Result IsCostEnough(this CardData card, CombatSystem combatSystem)
    {
        if (card.Cost == 0 || combatSystem.CardManager.EnergyPoint >= card.Cost)
        {
            return Result.Ok();
        }
        return Result.Fail($"能量点不足: {combatSystem.CardManager.EnergyPoint}/{card.Cost}");
    }
}