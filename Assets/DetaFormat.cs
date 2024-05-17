using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataFormat
{
    public class CardData
    {
        public string cardName;
        public string imagePath;
        public int attackPower;
    }
    public class DeckData
    {
        public string[] deckFrontCard;
        public string[] deckMainCard;
        public string[] deckSideCard;
    }
}
