using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataFormat;

public class DemoScript : MonoBehaviour
{
    public TextureManager textureManager;
    List<int> demo = new List<int>{10,1};
    // Start is called before the first frame update
    void Start()
    {
        textureManager = GetComponent<TextureManager>();
        textureManager.AttachSpriteOfIndex(GetComponent<SpriteRenderer>(),demo,DataFormat.CardColor.Red);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
