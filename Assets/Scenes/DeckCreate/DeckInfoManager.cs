using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataFormat;

//ゲーム内表示中の全デッキの管理するやつ、現在選択中のデッキと他デッキのインスタンスを保持
public class DeckInfoManager : MonoBehaviour
{
    public GameObject deckInst;
    public DeckData ActiveDeck;
    public DeckCardInfoManager deckCardInfoManager;

    public List<GameObject> deckList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < DeckJsonManager.Instance.DeckDatas.Count; i++)
        {
            deckList.Add(Instantiate(deckInst,transform));
            deckList[i].GetComponent<DeckAnim>().deckData = DeckJsonManager.Instance.DeckDatas[i];
            //Debug.Log(deckList[i].GetComponent<DeckAnim>().deckData.deckFrontCard.Count);
            deckList[i].GetComponent<DeckAnim>().SetUp();
            deckList[i].GetComponent<DeckAnim>().Myparant = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectDeck()
    {
        Debug.Log("SelectDeck" + ActiveDeck.deckName);
        deckCardInfoManager.RefreshDeckPreview();
    }
    public void CreateNewDeck()
    {
        DeckJsonManager.Instance.CreateNewDeck();
        deckList.Add(Instantiate(deckInst,transform));
        deckList[DeckJsonManager.Instance.DeckDatas.Count - 1].GetComponent<DeckAnim>().deckData = DeckJsonManager.Instance.DeckDatas[DeckJsonManager.Instance.DeckDatas.Count - 1];
        deckList[DeckJsonManager.Instance.DeckDatas.Count - 1].GetComponent<DeckAnim>().SetUp();
        deckList[DeckJsonManager.Instance.DeckDatas.Count - 1].GetComponent<DeckAnim>().Myparant = this;
    }

    public void ReloadPreview()
    {
        for(int i = 0; i < deckList.Count; i++)
        {
            deckList[i].GetComponent<DeckAnim>().SetUp();
        }
    }
}
