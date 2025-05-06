using System.Collections.Generic;

namespace Cards {
    public class LocalCards {
        public static List<ICardData> cards = new List<ICardData>(); // 本地卡片数据

        public static List<ICardData> GetCards() {
            return cards; // 获取本地卡片数据
        }
    }
}