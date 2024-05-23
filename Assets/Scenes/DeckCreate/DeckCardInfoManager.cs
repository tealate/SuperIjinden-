using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataFormat;
using System.Linq;
using TMPro;


public class DeckCardInfoManager : MonoBehaviour
{
    public DeckInfoManager deckInfoManager;
    public DeckData activeDeck;
    public TMP_InputField NameText;
    public CardActivePreview MycardActivePreview;
    public List<GameObject> cardPreviewList = new List<GameObject>();
    public List<CardPreviewControler> cardPreviewControlerList = new List<CardPreviewControler>();

    public GameObject cardPreviewPrefab;
    public Transform Content;
    // Start is called before the first frame update
    void Start()
    {
        activeDeck = deckInfoManager.ActiveDeck;
        MycardActivePreview = new CardActivePreview();
        MycardActivePreview.cardType = true;
        MycardActivePreview.cardLevel = true;
        MycardActivePreview.cardTokusei = true;
        MycardActivePreview.cardIgyoo = true;
        MycardActivePreview.cardCost = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshDeckPreview()
    {
        activeDeck = deckInfoManager.ActiveDeck;
        NameText.text = activeDeck.deckName;
        ReloadDeck();
    }
    public void RenameDeck()
    {
        activeDeck = deckInfoManager.ActiveDeck;
        //Debug.Log("RenameDeck" + NameText.text);
        if(DeckJsonManager.Instance.IsValidFileName(NameText.text).Count() > 0)
        {
            NameText.text = activeDeck.deckName;
            return;
        }
        if(activeDeck.deckName != NameText.text)activeDeck.deckName = DeckJsonManager.Instance.RenameDeck(activeDeck.deckName, NameText.text);
        deckInfoManager.ReloadPreview();
    }
    public void RefreshCardPreview()
    {
        activeDeck = deckInfoManager.ActiveDeck;
        foreach(CardPreviewControler cardPreviewControler in cardPreviewControlerList)cardPreviewControler.SetCardPreview();
    }
    public void RefreshCardActivePreview()
    {
        activeDeck = deckInfoManager.ActiveDeck;
        foreach(CardPreviewControler cardPreviewControler in cardPreviewControlerList)cardPreviewControler.SetCardActivePreview();
    }
    public void ReloadDeck()
    {
        activeDeck = deckInfoManager.ActiveDeck;
        int deckcount = 0;
        for(int i = 0; i < activeDeck.deckCardPath.Count; i++)
        {
            for(int j = 0; j < activeDeck.deckCardPath[i].cardNum; j++)
            {
                if(deckcount < cardPreviewList.Count)
                {
                    cardPreviewControlerList[deckcount].cardData = CardJsonManager.Instance.CardDatas[activeDeck.deckCardPath[i].cardPath[0]][activeDeck.deckCardPath[i].cardPath[1]];
                    cardPreviewControlerList[deckcount].cardPath = activeDeck.deckCardPath[i];
                    cardPreviewControlerList[deckcount].SetCardPreview();
                    cardPreviewControlerList[deckcount].SetCardActivePreview();
                }
                else
                {
                    cardPreviewList.Add(Instantiate(cardPreviewPrefab,Content));
                    cardPreviewControlerList.Add(cardPreviewList[cardPreviewList.Count-1].GetComponent<CardPreviewControler>());
                    cardPreviewControlerList[cardPreviewList.Count-1].cardData = CardJsonManager.Instance.CardDatas[activeDeck.deckCardPath[i].cardPath[0]][activeDeck.deckCardPath[i].cardPath[1]];
                    cardPreviewControlerList[cardPreviewList.Count-1].cardPath = activeDeck.deckCardPath[i];
                    cardPreviewControlerList[cardPreviewList.Count-1].SetCardPreview();
                    cardPreviewControlerList[cardPreviewList.Count-1].cardActivePreview = MycardActivePreview;
                    cardPreviewControlerList[cardPreviewList.Count-1].SetCardActivePreview();
                }
                deckcount++;
            }
        }
        while(deckcount < cardPreviewList.Count)
        {
            Destroy(cardPreviewList[deckcount]);
            cardPreviewList.RemoveAt(deckcount);
            cardPreviewControlerList.RemoveAt(deckcount);
        }
    }
}
