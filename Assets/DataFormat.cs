using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataFormat
{
    [System.Serializable]
    public class CardData
    {
        public int cardID;
        //カードID、名前と同様
        public bool isOriginal;
        //オリジナルカードかどうか
        public string cardName;
        public string cardNameKana;
        //フリガナ
        public int attackPower;

        public List<CardColor> cardColor = new List<CardColor>{CardColor.Red};
        public CardType cardType;
        public int cardLevel;
        public int cardCost;
        public int puressure;
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
}
