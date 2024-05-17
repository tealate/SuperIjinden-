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

    public static string[] GetAllJsonFile(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            throw new DirectoryNotFoundException($"The folder path '{folderPath}' does not exist.");
        }

        // �t�H���_���̂��ׂĂ�JSON�t�@�C�����擾
        string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");

        // JSON�t�@�C���̖��O���X�g��Ԃ�
        return jsonFiles;
    }
    public static void LoadDeckJson(string DeckPath,DataFormat.DeckData Deck)
    {
        
    }
}
