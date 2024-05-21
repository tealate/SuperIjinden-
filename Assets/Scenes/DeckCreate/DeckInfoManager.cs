using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataFormat;

public class DeckInfoManager : MonoBehaviour
{
    public GameObject deckInst;

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
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateNewDeck()
    {
        DeckJsonManager.Instance.CreateNewDeck();
        deckList.Add(Instantiate(deckInst,transform));
        deckList[DeckJsonManager.Instance.DeckDatas.Count - 1].GetComponent<DeckAnim>().deckData = DeckJsonManager.Instance.DeckDatas[DeckJsonManager.Instance.DeckDatas.Count - 1];
        deckList[DeckJsonManager.Instance.DeckDatas.Count - 1].GetComponent<DeckAnim>().SetUp();
    }
}
