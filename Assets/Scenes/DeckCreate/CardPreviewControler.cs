using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DataFormat;
public class CardPreviewControler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject CardType;
    public GameObject CardLevel;
    public GameObject Tokusei;
    public GameObject Igyoo;
    public GameObject CardCost;
    public TextMeshProUGUI CardTypeText;
    public TextMeshProUGUI CardLevelText;
    public TextMeshProUGUI TokuseiText;
    public TextMeshProUGUI IgyooText;
    public TextMeshProUGUI CardCostText;
    public CardData cardData;
    public CardActivePreview cardActivePreview;
    public SpriteRenderer cardSprite;
    public CardPath cardPath;
    void Awake()
    {
        //CardTypeText = CardType.GetComponent<TextMeshProUGUI>();
        //CardLevelText = CardLevel.GetComponent<TextMeshProUGUI>();
        //TokuseiText = Tokusei.GetComponent<TextMeshProUGUI>();
        //IgyooText = Igyoo.GetComponent<TextMeshProUGUI>();
        //CardCostText = CardCost.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCardPreview()
    {
        //カードタイプの一文字目だけ取得
        TextureManager.Instance.AttachSpriteOfIndex(cardSprite,cardPath.cardPath,cardData.cardColor[0]);
        CardTypeText.text = CardJsonManager.CardTypeString[cardData.cardType.GetHashCode()].Substring(0,1);
        CardLevelText.text = cardData.cardLevel.ToString();
        if(cardData.cardTokusei.Count != 0)TokuseiText.text = string.Join(",",cardData.cardTokusei);
        else TokuseiText.text = "なし";
        if(cardData.cardIgyoo.Count != 0)IgyooText.text = string.Join(",",cardData.cardIgyoo);
        else IgyooText.text = "なし";
        CardCostText.text = cardData.cardCost.ToString();
    }
    public void SetCardActivePreview()
    {
        CardType.SetActive(cardActivePreview.cardType);
        CardLevel.SetActive(cardActivePreview.cardLevel);
        Tokusei.SetActive(cardActivePreview.cardTokusei);
        Igyoo.SetActive(cardActivePreview.cardIgyoo);
        CardCost.SetActive(cardActivePreview.cardCost);
    }
}
