using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using DataFormat;
using Unity.VisualScripting;

public class TextureManager : MonoBehaviour
{
    public struct AsyncSpriteLoad
    {
        public Sprite sprite;
        public List<int> texturePath;
        public int spriteRefCount;
        public List<SpriteRenderer> spriteRenderers;
    }
    public static TextureManager Instance { get; private set; }
    public List<Sprite> tempSpriteList = new List<Sprite>();
    public List<AsyncSpriteLoad> asyncSpriteLoadList = new List<AsyncSpriteLoad>();

    public List<int> NotRefIndex = new List<int>();
    public int maxLoadCountMax = 100;

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
        for (int i = 0; i < maxLoadCountMax; i++)
        {
            NotRefIndex.Add(i);
            asyncSpriteLoadList.Add(new AsyncSpriteLoad());
        }
    }

    void LateUpdate()
    {
        if (FindIndexTask.Count > 0)
        {
            StartCoroutine(FindSprite(new List<SpriteLoadTask>(FindIndexTask)));
            FindIndexTask.Clear();
        }
    }

    public struct SpriteLoadTask
    {
        public List<int> texturePath;
        public SpriteRenderer spriteRenderer;
        public int spriteIndex;

        public int colorCode;
    }
    private List<SpriteLoadTask> FindIndexTask = new List<SpriteLoadTask>();

    public void AttachSpriteOfIndex(SpriteRenderer spriteRenderer, List<int> texturePath, CardColor cardColor)
    {
        spriteRenderer.sprite = tempSpriteList[cardColor.GetHashCode()];
        FindIndexTask.Add(new SpriteLoadTask { texturePath = texturePath, spriteRenderer = spriteRenderer, colorCode = cardColor.GetHashCode() });
    }

    public IEnumerator FindSprite(List<SpriteLoadTask> NowTask)
    {
        List<int> loadTaskIndex = new List<int>();
        for (int i = 0; i < NowTask.Count; i++)
        {
            int index = FindIndexOfSequence(asyncSpriteLoadList, NowTask[i].texturePath);
            if (index == -1)
            {
                AsyncSpriteLoad newLoad = new AsyncSpriteLoad();
                newLoad.texturePath = NowTask[i].texturePath;
                newLoad.spriteRefCount = 1;
                newLoad.spriteRenderers = new List<SpriteRenderer>();
                newLoad.spriteRenderers.Add(NowTask[i].spriteRenderer);
                if (NotRefIndex.Count != 0)
                {
                    asyncSpriteLoadList[NotRefIndex[0]] = newLoad;
                    loadTaskIndex.Add(NotRefIndex[0]);
                    NotRefIndex.RemoveAt(0);
                }
                else
                {
                    FindIndexOfNotRef(asyncSpriteLoadList);
                    if (NotRefIndex.Count != 0)
                    {
                        asyncSpriteLoadList[NotRefIndex[0]] = newLoad;
                        loadTaskIndex.Add(NotRefIndex[0]);
                        NotRefIndex.RemoveAt(0);
                    }
                    else
                    {
                        asyncSpriteLoadList.Add(newLoad);
                        loadTaskIndex.Add(asyncSpriteLoadList.Count - 1);
                        NotRefIndex.Add(asyncSpriteLoadList.Count - 1);
                    }
                }
                NowTask[i].spriteRenderer.sprite = tempSpriteList[NowTask[i].colorCode];
                Debug.Log("Sprite Not Found");
            }
            else
            {
                AsyncSpriteLoad load = asyncSpriteLoadList[index];
                load.spriteRefCount += 1;
                load.spriteRenderers.Add(NowTask[i].spriteRenderer);
                asyncSpriteLoadList[index] = load;
                NowTask[i].spriteRenderer.sprite = asyncSpriteLoadList[index].sprite;
                NowTask.RemoveAt(i);
                Debug.Log("Sprite Found");
            }
        }
        Debug.Log(loadTaskIndex.Count);
        if (loadTaskIndex.Count != 0)
        {
            StartCoroutine(LoadAllSpritesAsync(new List<int>(loadTaskIndex), 512));
        }
        yield return null;
    }

    public IEnumerator LoadAllSpritesAsync(List<int> loadTaskList, int maxSize)
    {
        string path = Application.dataPath + "/CardData";
        string path2;
        Debug.Log("LoadAllSpritesAsync");
        for (int i = 0; i < loadTaskList.Count; i++)
        {
            path2 = path;
            for (int j = 0; j < asyncSpriteLoadList[loadTaskList[i]].texturePath.Count; j++)
            {
                path2 += "/" + asyncSpriteLoadList[loadTaskList[i]].texturePath[j].ToString();
            }
            path2 += ".png";
            yield return StartCoroutine(LoadSpriteAsync(path2, maxSize, loadTaskList[i]));
        }
    }

    public IEnumerator LoadSpriteAsync(string path, int maxSize, int index)
    {
        if (File.Exists(path))
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(fileData))
            {
                AsyncSpriteLoad load = asyncSpriteLoadList[index];
                Texture2D resizedTexture = ResizeTexture(texture, maxSize);
                load.sprite = Sprite.Create(resizedTexture, new Rect(0, 0, resizedTexture.width, resizedTexture.height), new Vector2(0.5f, 0.5f));
                asyncSpriteLoadList[index] = load;
                for (int i = 0; i < asyncSpriteLoadList[index].spriteRenderers.Count; i++)
                {
                    asyncSpriteLoadList[index].spriteRenderers[i].sprite = asyncSpriteLoadList[index].sprite;
                }
            }
            else
            {
                Debug.LogError("Failed to create texture from file data at path: " + path);
            }
        }
        else
        {
            Debug.LogError("File does not exist at path: " + path);
        }
        yield return null;
    }

    private Texture2D ResizeTexture(Texture2D source, int maxSize)
    {
        int newWidth;
        int newHeight;

        if (source.width > source.height)
        {
            newWidth = maxSize;
            newHeight = Mathf.RoundToInt(source.height * (maxSize / (float)source.width));
        }
        else
        {
            newHeight = maxSize;
            newWidth = Mathf.RoundToInt(source.width * (maxSize / (float)source.height));
        }

        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        rt.filterMode = FilterMode.Bilinear;

        RenderTexture.active = rt;
        Graphics.Blit(source, rt);
        Texture2D newTexture = new Texture2D(newWidth, newHeight);
        newTexture.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        newTexture.Apply();

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);
        return newTexture;
    }

    static int FindIndexOfSequence(List<AsyncSpriteLoad> nestedList, List<int> targetSequence)
    {
        if (nestedList.Count == 0) return -1;
        for (int i = 0; i < nestedList.Count; i++)
        {
            if (nestedList[i].texturePath == null) continue;
            if (nestedList[i].texturePath.Count != targetSequence.Count) continue;
            if (nestedList[i].texturePath.SequenceEqual(targetSequence))
            {
                Debug.Log("Found");
                return i;
            }
        }
        return -1;
    }

    void FindIndexOfNotRef(List<AsyncSpriteLoad> nestedList)
    {
        AsyncSpriteLoad load = new AsyncSpriteLoad();
        for (int i = 0; i < nestedList.Count; i++)
        {
            load = nestedList[i];
            for (int j = 0; j < nestedList[i].spriteRenderers.Count; j++)
            {
                if (nestedList[i].spriteRenderers[j] == null)
                {
                    load.spriteRefCount -= 1;
                    load.spriteRenderers.RemoveAt(j);
                }
            }
            nestedList[i] = load;
            if (load.spriteRefCount == 0)
            {
                NotRefIndex.Add(i);
            }
        }
    }
}
