using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataFormat;
using System.Linq;
using TMPro;

public class DeckAnim : MonoBehaviour
{
    bool PointerStay = false;
    public GameObject deckCardInst;
    public GameObject[] deckFrontCard;
    public SpriteRenderer[] deckFrontCardSprite;
    public DeckData deckData;

    public TMP_InputField NameText;
    // Start is called before the first frame update
    void Awake()
    {
        deckFrontCard = new GameObject[3];
        deckFrontCardSprite = new SpriteRenderer[3];
        Vector3 scale = deckCardInst.transform.localScale;
        for(int i = 0; i < 3; i++)
        {
            deckFrontCard[i] = Instantiate(deckCardInst, transform.TransformPoint(new Vector3((i-1)*4, -1, i*0.1f)), Quaternion.identity, transform);
            deckFrontCard[i].transform.localScale = scale * 1.5f;
            deckFrontCardSprite[i] = deckFrontCard[i].GetComponent<SpriteRenderer>();
        }
    }

    void Start()
    {
    }

    public void SetUp()
    {
        NameText.text = deckData.deckName;
        if(deckData.deckFrontCard.Count() > 0)
        {
            List<CardColor> colors = new List<CardColor>();
            for(int i = 0; i < 3; i++)
            {
                if(i < deckData.deckFrontCard.Count)colors.Add(CardJsonManager.Instance.CardDatas[deckData.deckFrontCard[i].cardPath[0]][deckData.deckFrontCard[i].cardPath[1]].cardColor[0]);
                else colors.Add(CardColor.Red);
            }
            UpdateCardTexture(deckData.deckFrontCard, colors);
        }
        else Debug.Log("deckFrontCard is empty");
        //Debug.Log(deckData.deckName);
    }

    public void UpdateCardTexture(List<CardPath> texturePaths, List<CardColor> colorCodes)
    {
        for(int i = 0; i < 3; i++)
        {
            TextureManager.Instance.AttachSpriteOfIndex(deckFrontCardSprite[i],texturePaths[i].cardPath,colorCodes[i]);
        }
    }

    public void RenameDeck()
    {
        Debug.Log("RenameDeck" + NameText.text);
        if(DeckJsonManager.Instance.IsValidFileName(NameText.text).Count() > 0)
        {
            //後で警告出す処理実装しろ
            return;
        }
        if(deckData.deckName != NameText.text)deckData.deckName = DeckJsonManager.Instance.RenameDeck(deckData.deckName, NameText.text);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DebugMan()
    {
    }
    public void PointerEnter()
    {
        PointerStay = true;
    }

    public void PointerExit()
    {
        PointerStay = false;
    }
}
