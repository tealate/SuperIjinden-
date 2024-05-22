using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataFormat;
using System.Linq;
using TMPro;

//現在操作中のデッキの中身管理するやつ
public class DeckCardInfoManager : MonoBehaviour
{
    public DeckInfoManager deckInfoManager;
    public DeckData activeDeck;
    public TMP_InputField NameText;
    // Start is called before the first frame update
    void Start()
    {
        activeDeck = deckInfoManager.ActiveDeck;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshDeckPreview()
    {
        activeDeck = deckInfoManager.ActiveDeck;
        Debug.Log("Refresh: " + activeDeck.deckName);
        NameText.text = activeDeck.deckName;
    }
    public void RenameDeck()
    {
        activeDeck = deckInfoManager.ActiveDeck;
        //Debug.Log("RenameDeck" + NameText.text);
        if(DeckJsonManager.Instance.IsValidFileName(NameText.text).Count() > 0)
        {
            //後で警告出す処理実装しろ
            NameText.text = activeDeck.deckName;
            return;
        }
        if(activeDeck.deckName != NameText.text)activeDeck.deckName = DeckJsonManager.Instance.RenameDeck(activeDeck.deckName, NameText.text);
    }
}
