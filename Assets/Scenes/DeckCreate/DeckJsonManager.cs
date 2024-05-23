using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using DataFormat;
using System.Linq;
using System.Text.RegularExpressions;
//コルーチン、シングルトン
//デッキのjsonファイルの管理、メモリでの保存をするやつ
public class DeckJsonManager : MonoBehaviour
{
    //deckName以外はjsonファイルを信用する,deckNameはファイル名から取得する
    string DeckDataPass;
    string CardDataPass;

    public List<string> DeckJsonFiles = new List<string>();
    public List<DeckData> DeckDatas = new List<DeckData>();
    public static DeckJsonManager Instance { get; private set; }

    string[] tempNames = new string[] { "我が最強のデッキ!!!", "メチャクチャ強いデッキ", "マイフェイバリットデッキ"};
    static char[] invalidChars;

    private void Awake()
    {
        DeckDataPass = Application.dataPath + "/" + "DeckData";
        CardDataPass = Application.dataPath + "/" + "CardData";
        invalidChars = Path.GetInvalidFileNameChars();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DeckJsonFiles = new List<string>(GetAllJsonFile(DeckDataPass));
            StartCoroutine(LoadAllCardJsonAsync(DeckJsonFiles, DeckDatas));
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public string IsValidFileName(string fileName)
    {
        string a = "";

        // ファイル名に使用できない文字が含まれているかチェック
        foreach (char c in fileName)
        {
            if (invalidChars.Contains(c))
            {
                a += "," + c;
            }
        }
        return a;
    }
    public IEnumerator LoadAllCardJsonAsync(List<string> DeckJsonFiles, List<DeckData> DeckDatas)
    {
        foreach (string deckName in DeckJsonFiles)
        {
            bool Found = false;
            for(int i = 0; i < DeckDatas.Count; i++)
            {
                if (DeckDatas[i].deckName == deckName)
                {
                    StartCoroutine(LoadDeckJsonAsync(deckName, DeckDatas[i]));
                    Found = true;
                    break;
                }
            }
            if (!Found)
            {
                DeckData deck = new DeckData();
                DeckDatas.Add(deck);
                StartCoroutine(LoadDeckJsonAsync(deckName, deck));
            }
        }
        yield return null;
    }

    public string CreateNewDeck()
    {
        string tempName = tempNames[Random.Range(0, tempNames.Length)];
        string deckName = CheckOverLapDeckName(tempName);
        DeckData deck = new DeckData();
        deck.deckName = deckName;
        deck.deckFrontCard = new List<CardPath>();
        for(int i = 0; i < 3; i++)
        {
            deck.deckFrontCard.Add(new CardPath());
            deck.deckFrontCard[i].cardPath = new List<int>();
            deck.deckFrontCard[i].cardPath.Add(10);
            deck.deckFrontCard[i].cardPath.Add(0);
        }
        string deckData = JsonUtility.ToJson(deck);
        File.WriteAllText(DeckDataPass + "/" + deckName + ".json", deckData);
        ReloadDeckJsonFiles();
        return deckName;
    }

    public string RenameDeck(string deckName, string newDeckName)
    {
        newDeckName = CheckOverLapDeckName(newDeckName);
        string deckPath = DeckDataPass + "/" + deckName + ".json";
        string newDeckPath = DeckDataPass + "/" + newDeckName + ".json";
        if (File.Exists(deckPath))
        {
            File.Move(deckPath, newDeckPath);
            //ReloadDeckJsonFiles();
            return newDeckName;
        }
        return null;
    }

    public void SaveDeck(DeckData deck)
    {
        string deckData = JsonUtility.ToJson(deck);
        File.WriteAllText(DeckDataPass + "/" + deck.deckName + ".json", deckData);
    }

    public void DeleteDeck(string deckName)
    {
        string deckPath = DeckDataPass + "/" + deckName + ".json";
        if (File.Exists(deckPath))
        {
            File.Delete(deckPath);
            ReloadDeckJsonFiles();
        }
    }
    public string CheckOverLapDeckName(string deckName)
    {
        ReloadDeckJsonFiles();
        if (DeckJsonFiles.Contains(deckName))
        {
            Match match = Regex.Match(deckName, @"(\d+)$");
            if(match.Success)
            {
                int num = int.Parse(match.Value);
                string mainPart = deckName.Substring(0, deckName.Length - num.ToString().Length);
                for(;true;)
                {
                    num++;
                    if (!DeckJsonFiles.Contains(mainPart + num.ToString()))return mainPart + num.ToString();
                }
            }
            else
            {
                //Debug.Log("NotOverlap" + deckName);
                return CheckOverLapDeckName(deckName + "1");
            }
        }
        return deckName;
    }

    public DeckData GetDeckFromName(string deckName)
    {
        for(int i = 0; i < DeckDatas.Count; i++)
        {
            if (DeckDatas[i].deckName == deckName)return DeckDatas[i];
        }
        DeckData deck = new DeckData();
        StartCoroutine(LoadDeckJsonAsync(deckName, deck));
        DeckDatas.Add(deck);
        return deck;
    }

    public void ReloadDeckJsonFiles()
    {
        DeckJsonFiles = new List<string>(GetAllJsonFile(DeckDataPass));
        foreach (string DeckName in DeckJsonFiles)
        {
            bool Found = false;
            for(int i = 0; i < DeckDatas.Count; i++)
            {
                if (DeckDatas[i].deckName == DeckName)
                {
                    Found = true;
                    break;
                }
            }
            if (!Found)
            {
                DeckData deck = new DeckData();
                StartCoroutine(LoadDeckJsonAsync(DeckName, deck));
                DeckDatas.Add(deck);
            }
        }
    }

    public static List<string> GetAllJsonFile(string folderPath)
    {
        string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");
        List<string> fileNamesWithoutExtension = jsonFiles.Select(Path.GetFileNameWithoutExtension).ToList();
        return fileNamesWithoutExtension;
    }
    public IEnumerator LoadDeckJsonAsync(string deckName, DeckData deck)
    {
        if (!File.Exists(DeckDataPass + "/" + deckName + ".json"))yield break;
        string json = File.ReadAllText(DeckDataPass + "/" + deckName + ".json");
        DeckData loadData = JsonUtility.FromJson<DeckData>(json);
        deck.deckName = deckName;
        deck.deckFrontCard = loadData.deckFrontCard;
        deck.deckCardPath = loadData.deckCardPath;
        yield return null;
    }
}
