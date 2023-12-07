using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Field : MonoBehaviour
{
    public bool HasMine = false;
    public bool IsClick = false;
    public Field[] NeighborFields;
    Text text;
    private SpriteRenderer sr;
    private int count = 0;
    

    public GameObject SuwaruKuma;
    public GameObject OboreruKuma;

    GameManager gm;
    AudioSource audioSource;
    public AudioClip putSound;
    public AudioClip fallSound;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.white;
        text = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
        gm = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        count = 0;
        for(var i = 0; i < NeighborFields.Length; i++ )
        {
            if(NeighborFields[i].HasMine){
                count = count + 1;
            }

        }
        text.text = count.ToString();
    }

    public void Click()
    {
        if(IsClick) return;

        IsClick = true;
        if(HasMine){
            sr.color = new Color(1, 1, 1, 0);
            Instantiate(OboreruKuma, transform.position, Quaternion.identity);
            gm.GameOver();
            audioSource.PlayOneShot(fallSound);
            
        }
        else{
            sr.color = new Color(0, 0.7490f, 0.6471f, 1);
            Instantiate(SuwaruKuma, transform.position, Quaternion.identity);
            audioSource.PlayOneShot(putSound);
            if(gm.Judge()){
                Invoke("Clear", 1.0f);
            }
        }
    }

    void Clear()
    {
        gm.GameClear();
    }
    
    public void BreakAnimation()
    {
        if(IsClick) return;
        StartCoroutine(BreakAnimationCoroutine());
    }
    
    IEnumerator BreakAnimationCoroutine()
    {
        for (float i = 0; i < 1; i += 0.1f)
        {
            sr.color = new Color(1, 1, 1, 1 - i);
            yield return new WaitForSeconds(0.1f);

        }
    }
}