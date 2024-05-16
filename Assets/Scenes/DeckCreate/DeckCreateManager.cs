using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckCreateManager : MonoBehaviour
{
    bool OnDeckSet = false;
    bool NowScroll = false;
    public List<GameObject> SwitchObjects;
    public float ScrollPoint = 7.5f;
    public float ScrollSpeed = 2;
    public float DecDistance = 0.5f;
    public float DecSpeed;
    public float demo;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (NowScroll) Scroll(Time.deltaTime);
        demo = transform.position.x;
    }

    public void BackMenu()
    {
        if (!NowScroll) SceneManager.LoadScene("MainMenu");
    }
    public void DeckSet()
    {
        if (!NowScroll)
        {
            ScrollReady();
        }
    }

    public float t = 0;
    public float MoveDelta = 0;
    public float GetMove(float Delta,float Speed)
    {
        t += Delta;
        float Move = 1 - Mathf.Exp(-t * DecSpeed);
        if (Move >= 1)
        {
            Move = 1;
            t = 0;
        }
        return Move * Speed;
    }

    public void ScrollReady()
    {
        NowScroll = true;
        t = 0;
    }

    public void Scroll(float DeltaTime)
    {
        transform.position = new Vector3(-transform.position.x, 0, 0);
        if (!OnDeckSet)
        {
            if (transform.position.x < ScrollPoint - DecDistance) transform.position += new Vector3(DeltaTime * ScrollSpeed, 0, 0);
            else
            {
                transform.position = new Vector3(GetMove(DeltaTime, ScrollSpeed) + ScrollPoint - DecDistance, 0, 0);
                if(ScrollPoint < transform.position.x)
                {
                    transform.position = new Vector3(ScrollPoint, 0, 0);
                    NowScroll = false;
                    t = 0;
                    OnDeckSet = true;
                    SwitchActive();
                }
            }
        }
        else
        {
            if (transform.position.x > DecDistance) transform.position -= new Vector3(DeltaTime * ScrollSpeed, 0, 0);
            else
            {
                transform.position = new Vector3(DecDistance - GetMove(DeltaTime, ScrollSpeed), 0, 0);
                if (0 > transform.position.x)
                {
                    transform.position = new Vector3(0, 0, 0);
                    NowScroll = false;
                    t = 0;
                    OnDeckSet = false;
                    SwitchActive();
                }
            }
        }
        transform.position = new Vector3(-transform.position.x, 0, 0);
    }
    public void SwitchActive()
    {
        foreach (GameObject a in SwitchObjects) a.SetActive(!a.activeSelf);
    }
}


