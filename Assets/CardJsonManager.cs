using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DataFormat;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;


public class CardJsonManager : MonoBehaviour
{
    public static CardJsonManager Instance { get; private set; }
    JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };
    public List<List<CardData>> CardDatas = new List<List<CardData>>();
    public List<CardData> CardDataListPre;

    //List<List<int>> CardJsonFiles = new List<List<int>>();

    public CardData defaultCardData;

    public string Demo;
    public int FolderCount;

    string[] ErrorMessages = new string[]
    {
        "jsonの記述ミスったな？マヌケめ!!",
        "コンパイラからお叱りの言葉だ!!くらえ!!",
        "jsonなに書いてんのかわかんねぇよ!!",
        "なんで私が怒ってるかわかる?もう一回説明するね?"
    };

    public static string[] CardTypeString = new string[]
    {
        "イジン",
        "マホウ",
        "ハイケイ",
        "マリョク"
    };
    // Start is called before the first frame update
    private void Awake()
    {
        defaultCardData = new CardData{
            cardID = 0,
            isOffical = false,
            cardName = "NotFound",
            cardNameKana = "",
            cardText = "なにもないよ",
            attackPower = 0,
            cardTokusei = new List<string>{"まぬけ"},
            cardColor = new List<CardColor>{CardColor.Red},
            cardType = CardType.Maryoku,
            cardIgyoo = new List<string>{""},
            cardLevel = 0,
            cardCost = 0,
            illustraterName = ""
        };

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            int CardDataCount = GetLargestNumberedFolder(Application.dataPath + "/" + "CardData");
            for(int i = 0;i <= CardDataCount; i++)
            {
                CardDatas.Add(new List<CardData>());
                //CardJsonFiles.Add(new List<int>());
                //if(CardJsonFiles[i].Count == 0)continue;
                int CardDataCount2 = GetLargestNumberedFile(Application.dataPath + "/" + "CardData" + "/" + i, ".json");
                Debug.Log(CardDataCount2);
                for(int j = 0; j <= CardDataCount2; j++)
                {
                    CardDatas[i].Add(new CardData());
                    //CardJsonFiles[i].Add(GetLargestNumberedFile(Application.dataPath + "/" + "CardData" + "/" + j, ".json"));
                }
            }
            CardDataListPre = CardDatas[11];
            StartCoroutine(LoadCardJson());
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
        //Demo = CardDatas[10][0].cardText;
        FolderCount = CardDatas.Count;
    }

    public static int GetLargestNumberedFile(string folderPath, string extension)
    {
        // フォルダ内の指定された拡張子のファイルを取得
        string[] files = Directory.GetFiles(folderPath, $"*{extension}");

        // ファイル名が拡張子部分を除いて数字のみであるものをフィルタリングし、整数に変換
        var numericFiles = files
            .Select(Path.GetFileNameWithoutExtension) // 拡張子を除いたファイル名を取得
            .Where(name => int.TryParse(name, out _)) // ファイル名が数字のみで構成されているかを確認
            .Select(int.Parse); // ファイル名を整数に変換
            if (!numericFiles.Any())return -1;
        // 最大の数字を返す
        return numericFiles.Max();
    }

    public IEnumerator LoadCardJson()
    {
        string CardDataPass = Application.dataPath + "/" + "CardData";
        for(int i = 0; i < CardDatas.Count; i++)
        {
            if(!Directory.Exists(CardDataPass + "/" + i))continue;
            for(int j = 0; j < CardDatas[i].Count; j++)
            {
                string CardJsonPass = CardDataPass + "/" + i + "/" + j + ".json";
                if(!File.Exists(CardJsonPass))continue;
                string jsonData = File.ReadAllText(CardJsonPass);
                try
                {
                    CardDatas[i][j] = JsonSerializer.Deserialize<CardData>(jsonData, options);
                }
                catch (JsonException e)
                {
                    Debug.Log("シリアライズに失敗しました: " + e.Message);
                    CardData defaultCard = defaultCardData.Clone();
                    CardDatas[i][j] = defaultCard;
                    CardDatas[i][j].cardColor = new List<CardColor>{(CardColor)Random.Range(0, 2)};
                    CardDatas[i][j].cardText = ErrorMessages[Random.Range(0, ErrorMessages.Length)] + "\n" + "エラー個所は" + e.LineNumber + "行目かその上下の行、多分\n\n" + e.Message;
                    CardDatas[i][j].cardIgyoo = new List<string>{"例外"};
                }
            }
        }
        yield return null;
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
            if (!numericFolders.Any())return -1;

        // 最大の数字を返す
        return numericFolders.Max();
    }
}
