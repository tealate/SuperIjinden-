using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataFormat
{
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

        public CardColor cardColor;
        public CardType cardType;
        public int cardLevel;
        public int cardCost;
        public int puressure;
    }
    public class DeckData
    {
        public List<string> deckFrontCard;
        public List<List<int>> deckCardPath;
        //カードの情報をlist<int>で格納して、それをさらにlistで格納してる
        //カード情報はlistのゼロ番目にカード枚数、それ以降にカードのパスを格納
        //例えばlistのa番目に上杉謙信を三枚入れる場合,
        //list[a] = [3, 10, 1]となる
    }
    public enum CardColor
    {
        Red,
        Blue,
        Green,
        Yellow
    }
    public enum CardType
    {
        Ijin,
        Mahou,
        Haikei,
        Maryoku
    }
    public enum Tokusei
    {
        None,
        Kenjyutu,
        Ongaku,
        Bijyutu
    }

    public enum TagAbility
    {
        None,
        SokuOu,
        Sippitu,
        Koukai
    }
}
