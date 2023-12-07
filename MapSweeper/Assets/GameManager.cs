using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Field[] Fields;
    private Field[] Mines;
    public GameObject GameOverDisplay;
    public GameObject GameClearDisplay;
    public int totalMines = 5;

    public AudioClip clearSound;
    void Awake(){
        Fields = FindObjectsOfType<Field>();
    }

    void Start() {
        int[] Mines = new int[Fields.Length];
        
        for (int i = 0; i < Fields.Length; i++)
        {
            Mines[i] = i;
        }
        
        for (int i = 0; i < Fields.Length; i++)
        {
            int swapIndex = Random.Range(i, Fields.Length);
            int temp = Mines[i];
            Mines[i] = Mines[swapIndex];
            Mines[swapIndex] = temp;
        }
        
        for (int i = 0; i < totalMines; i++)
        {
            Fields[Mines[i]].HasMine = true;
        }
    }

    void Update() {
        Judge();
    }

    public void GameOver() {
        StartCoroutine(GameOverAnimation());
    }
    
    IEnumerator GameOverAnimation() {
        for (int i = 0; i < Fields.Length; i++)
        {
            if (Fields[i].HasMine)
            {
                Fields[i].BreakAnimation();
                yield return new WaitForSeconds(0.1f);
            }
        }
        GameOverDisplay.SetActive(true);
    }

    public void GameClear() {
        StartCoroutine(GameClearAnimation());
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        // 音量を上げる
        audioSource.volume = 0.8f;
        audioSource.PlayOneShot(clearSound);
        GameClearDisplay.SetActive(true);
    }

    IEnumerator GameClearAnimation() {
        GameObject[] kumas = GameObject.FindGameObjectsWithTag("Kuma");
        foreach (GameObject kuma in kumas)
        {
            kuma.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("しろくま(手振り)");
            yield return new WaitForSeconds(0.1f);
        }
    }
    public bool Judge()
    {
        bool win = true;
        for (var i = 0; i < Fields.Length; i++)
        {
            if (!Fields[i].HasMine)
            {
                if (!Fields[i].IsClick)
                {
                    win = false;
                }
            }
        }


        return win;
    }


}