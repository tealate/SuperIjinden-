using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class DeckJsonManager : MonoBehaviour
{
    string DeckDataPass;
    string CardDataPass;

    public List<string> DeckJsonFiles = new List<string>();
    public DeckJsonManager Instance { get; private set; }
    // Start is called before the first frame update

    private void Awake()
    {
        DeckDataPass = Application.dataPath + "/" + "DeckData";
        CardDataPass = Application.dataPath + "/" + "CardData";
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DeckJsonFiles = new List<string>(GetAllJsonFile(DeckDataPass));
        }
        else
        {
            Destroy(gameObject);
        }
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

    public void RenameDeck()
    {

    }

    public void DeleteDeck()
    {

    }

    public void LoadDeck()
    {

    }

    public static string[] GetAllJsonFile(string folderPath)
    {

        // ?t?H???_?????????JSON?t?@?C???????
        string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");

        // JSON?t?@?C??????O???X?g????
        return jsonFiles;
    }
    public static bool LoadDeckJson(string deckPath, DataFormat.DeckData deck)
    {
        if (!File.Exists(deckPath))return false;
        string json = File.ReadAllText(deckPath);
        JsonUtility.FromJsonOverwrite(json, deck);
        return true;
    }
}
