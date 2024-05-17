using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;

public class TextureManager : MonoBehaviour
{
    public static TextureManager Instance { get; private set; }
        private List<Sprite> spriteList = new List<Sprite>();
        private List<Sprite> tempSpriteList = new List<Sprite>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    static int FindIndexOfSequence(List<List<int>> nestedList, List<int> targetSequence)
    {
        for (int i = 0; i < nestedList.Count; i++)
        {
            if (nestedList[i].SequenceEqual(targetSequence))
            {
                return i;
            }
        }
        return -1;
    }
}
