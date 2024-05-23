using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataFormat
{
    [System.Serializable]
    public class CardData
    {
        public int cardID { get; set; } = 0;
        //カードID、名前と同様
        public bool isOffical { get; set; } = false;

        public Rarity cardRarity { get; set; } = Rarity.N;
        //オリジナルカードかどうか
        public string cardName { get; set; }
        public string cardNameKana { get; set; }
        //フリガナ
        public int attackPower { get; set; }
        public List<string> cardTokusei { get; set; }
        public List<CardColor> cardColor { get; set; } = new List<CardColor>{CardColor.Red};
        public CardType cardType { get; set; }

        public string cardText { get; set; } = "なにもないよ";

        public List<string> cardIgyoo { get; set; }
        public int cardLevel { get; set; }
        public int cardCost { get; set; }
        public string illustraterName { get; set; }

        public CardData Clone()
        {
            return new CardData
            {
                cardID = cardID,
                isOffical = isOffical,
                cardName = cardName,
                cardNameKana = cardNameKana,
                attackPower = attackPower,
                cardTokusei = new List<string>(cardTokusei),
                cardColor = new List<CardColor>(cardColor),
                cardType = cardType,
                cardText = cardText,
                cardIgyoo = new List<string>(cardIgyoo),
                cardLevel = cardLevel,
                cardCost = cardCost,
                illustraterName = illustraterName
            };
        }
    }
    [System.Serializable]
    public class DeckData
    {
        public string deckName;
        public List<CardPath> deckFrontCard = new List<CardPath>();
        public List<CardPath> deckCardPath = new List<CardPath>();
        //カードの情報をlist<int>で格納して、それをさらにlistで格納してる
        //カード情報はlistのゼロ番目にカード枚数、それ以降にカードのパスを格納
        //例えばlistのa番目に上杉謙信を三枚入れる場合,
        //list[a] = [3, 10, 1]となる
    }

    [System.Serializable]
    public class CardPath
    {
        public int cardNum;
        public List<int> cardPath;
    }

    public class CardActivePreview
    {
        public bool cardType;
        public bool cardLevel;
        public bool cardTokusei;
        public bool cardIgyoo;
        public bool cardCost;
    }

    [System.Serializable]
    public enum CardColor
    {
        Red,
        Blue,
        Green,
        Yellow,
        Purple,
        White,
        Black,
        Colorless,
        Rainbow,
        Gold,
        Silver,
        Bronze
    }
    [System.Serializable]
    public enum CardType
    {
        Ijin,
        Mahou,
        Haikei,
        Maryoku
    }
    [System.Serializable]
    public enum TagAbility
    {
        None,
        SokuOu,
        Sippitu,
        Koukai
    }
    //カードのレアリティ
    public enum Rarity
    {
        C,
        N,
        R,
        SR,
        SSR,
        UR
    }
}
