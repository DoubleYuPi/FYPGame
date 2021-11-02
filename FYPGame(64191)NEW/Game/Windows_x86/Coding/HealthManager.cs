using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfFullHeart;
    public Sprite emptyHearts;
    public FloatValue heartContainer;
    public FloatValue playerCurrentHeart;
    // Start is called before the first frame update
    void Start()
    {
        InitHearts();
    }

    // Update is called once per frame
    public void InitHearts()
    {
        for (int i = 0; i < heartContainer.initialValue; i++)
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = fullHeart;
        }
    }

    public void UpdateHearts()
    {
        float tempHeart = playerCurrentHeart.runTimeVal / 2;
        for(int i = 0; i < heartContainer.initialValue; i++)
        {
            if(i<= tempHeart-1)
            {
                hearts[i].sprite = fullHeart;
            }else if (i >= tempHeart)
            {
                hearts[i].sprite = emptyHearts;
            }
            else
            {
                hearts[i].sprite = halfFullHeart;
            }
        }
    }
}
