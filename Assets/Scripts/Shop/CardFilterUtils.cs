using System;
using System.Collections.Generic;
using System.Linq;

public static class CardFilterUtils
{
    public static List<ICardData> FilterCards(string keyword, IEnumerable<ICardData> cards)
    {
        keyword = keyword.ToLower().Trim();
        var result = cards;

        if (string.IsNullOrEmpty(keyword))
            return result.ToList();

        string[] tokens = keyword.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var token in tokens)
        {
            if (token.StartsWith("cost"))
            {
                result = ApplyCostFilter(token, result, card => card.Cost);
            }
            else if (token.StartsWith("type:"))
            {
                string typeFilter = token.Substring(5);
                result = result.Where(card => card.CardCategory.ToString().ToLower() == typeFilter);
            }
            else if (token.Contains("|"))
            {
                var orKeywords = token.Split('|');
                result = result.Where(card =>
                    orKeywords.Any(k =>
                        card.CardName.ToLower().Contains(k) ||
                        card.Desc.ToLower().Contains(k))
                );
            }
            else
            {
                result = result.Where(card =>
                    card.CardName.ToLower().Contains(token) ||
                    card.CardCategory.ToString().ToLower().Contains(token)
                );
            }
        }

        return result.ToList();
    }

    public static List<ItemData> FilterItems(string keyword, IEnumerable<ItemData> items)
    {
        keyword = keyword.ToLower().Trim();
        var result = items;

        if (string.IsNullOrEmpty(keyword))
            return result.ToList();

        string[] tokens = keyword.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var token in tokens)
        {
            if (token.StartsWith("cost"))
            {
                result = ApplyCostFilter(token, result, item => item.cardData.Cost);
            }
            else if (token.StartsWith("gold"))
            {
                result = ApplyCostFilter(token, result, item => item.gold);
            }
            else if (token.StartsWith("type:"))
            {
                string typeFilter = token.Substring(5);
                result = result.Where(item =>
                    item.cardData.CardCategory.ToString().ToLower() == typeFilter);
            }
            else if (token.Contains("|"))
            {
                var orKeywords = token.Split('|');
                result = result.Where(item =>
                    orKeywords.Any(k =>
                        item.cardData.CardName.ToLower().Contains(k) ||
                        item.cardData.Desc.ToLower().Contains(k))
                );
            }
            else
            {
                result = result.Where(item =>
                    item.cardData.CardName.ToLower().Contains(token) ||
                    item.cardData.CardCategory.ToString().ToLower().Contains(token)
                );
            }
        }

        return result.ToList();
    }

    private static IEnumerable<T> ApplyCostFilter<T>(string token, IEnumerable<T> source, Func<T, int> getValue)
    {
        if (token.Contains("<=") && int.TryParse(token.Substring(token.IndexOf("<=") + 2), out int maxCostInc))
            return source.Where(c => getValue(c) <= maxCostInc);
        else if (token.Contains("<") && int.TryParse(token.Substring(token.IndexOf("<") + 1), out int maxCost))
            return source.Where(c => getValue(c) < maxCost);
        else if (token.Contains(">=") && int.TryParse(token.Substring(token.IndexOf(">=") + 2), out int minCostInc))
            return source.Where(c => getValue(c) >= minCostInc);
        else if (token.Contains(">") && int.TryParse(token.Substring(token.IndexOf(">") + 1), out int minCost))
            return source.Where(c => getValue(c) > minCost);
        else if (token.Contains("=") && int.TryParse(token.Substring(token.IndexOf("=") + 1), out int eqCost))
            return source.Where(c => getValue(c) == eqCost);
        return source;
    }
}