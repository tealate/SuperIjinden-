using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DataFormat;
using System.Linq;

public class CardJsonManager : MonoBehaviour
{
    public static CardJsonManager Instance { get; private set; }
    public List<List<CardData>> CardDatas = new List<List<CardData>>();

    List<List<int>> CardJsonFiles = new List<List<int>>();
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            for(int i = 0; i <= GetLargestNumberedFolder(Application.dataPath + "/" + "CardData"); i++)
            {
                CardDatas.Add(new List<CardData>());
                CardJsonFiles.Add(GetNumericJsonFiles(Application.dataPath + "/" + "CardData" + "/" + i));
                if(CardJsonFiles[i].Count == 0)continue;
                for(int j = 0; j <= CardJsonFiles[i].Max(); j++)
                {
                    CardDatas[i].Add(new CardData());
                }
            }
            StartCoroutine(LoadCardJson());
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        //Debug.Log(CardDatas[10][1].cardName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static int GetLargestNumberedFolder(string folderPath)
    {

        // フォルダ内のサブフォルダを取得
        string[] subdirectories = Directory.GetDirectories(folderPath);

        // サブフォルダ名が数字のみであるものをフィルタリングし、整数に変換
        var numericFolders = subdirectories
            .Select(Path.GetFileName)
            .Where(name => int.TryParse(name, out _))
            .Select(int.Parse);

        // 最大の数字を返す
        return numericFolders.Max();
    }

    public IEnumerator LoadCardJson()
    {
        string CardDataPass = Application.dataPath + "/" + "CardData";
        for(int i = 0; i < CardDatas.Count; i++)
        {
            if(!Directory.Exists(CardDataPass + "/" + i))continue;
            for(int j = 0; j < CardJsonFiles[i].Count; j++)
            {
                string CardJsonPass = CardDataPass + "/" + i + "/" + CardJsonFiles[i][j] + ".json";
                if(!File.Exists(CardJsonPass))continue;
                JsonUtility.FromJsonOverwrite(File.ReadAllText(CardJsonPass), CardDatas[i][CardJsonFiles[i][j]]);
            }
        }
        yield return null;
    }
    public static List<int> GetNumericJsonFiles(string folderPath)
    {
        // フォルダが存在するかを確認
        if (!Directory.Exists(folderPath))
        {
            return new List<int>();
        }

        // フォルダ内の.jsonファイルを取得
        string[] files = Directory.GetFiles(folderPath, "*.json");

        // ファイル名が数字のみで構成されているものをフィルタリングし、数字をリストに追加
        var numericJsonFiles = files
            .Select(file => Path.GetFileNameWithoutExtension(file))
            .Where(fileName => int.TryParse(fileName, out _))
            .Select(fileName => int.Parse(fileName))
            .ToList();

        return numericJsonFiles;
    }
}
