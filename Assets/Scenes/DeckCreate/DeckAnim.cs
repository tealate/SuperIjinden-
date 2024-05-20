using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataFormat;

public class DeckAnim : MonoBehaviour
{
    bool PointerStay = false;
    public GameObject deckCardInst;
    public GameObject[] deckFrontCard;
    public SpriteRenderer[] deckFrontCardSprite;
    // Start is called before the first frame update
    void Awake()
    {
        deckFrontCard = new GameObject[3];
        deckFrontCardSprite = new SpriteRenderer[3];
        Vector3 scale = deckCardInst.transform.localScale;
        for(int i = 0; i < 3; i++)
        {
            deckFrontCard[i] = Instantiate(deckCardInst, transform.TransformPoint(new Vector3((i-1)*50, -1, 0)), Quaternion.identity, transform);
            deckFrontCard[i].transform.localScale = scale * 1.5f;
            deckFrontCardSprite[i] = deckFrontCard[i].GetComponent<SpriteRenderer>();
        }
    }

    void Start()
    {
        List<List<int>> Demo = new List<List<int>>();
        List<CardColor> DemoColor = new List<CardColor>{CardColor.Red,CardColor.Blue,CardColor.Green};
        Demo.Add(new List<int>{10,0});
        Demo.Add(new List<int>{10,1});
        Demo.Add(new List<int>{10,2});
        UpdateCardTexture(Demo,DemoColor);
    }

    public void UpdateCardTexture(List<List<int>> texturePaths, List<CardColor> colorCodes)
    {
        for(int i = 0; i < 3; i++)
        {
            TextureManager.Instance.AttachSpriteOfIndex(deckFrontCardSprite[i],texturePaths[i],colorCodes[i]);
        }
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
