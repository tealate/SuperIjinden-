using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class DeckJsonManager : MonoBehaviour
{
    string DeckDataPass;
    string CardDataPass;
    string CardJsonPass;
    // Start is called before the first frame update

    private void Awake()
    {
        DeckDataPass = Application.dataPath + "/" + "DeckData";
        CardDataPass = Application.dataPath + "/" + "CardData";
        CardJsonPass = Application.dataPath + "/" + "CardData" + "CardData.json";
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateNewDeck()
    {

    }
}
