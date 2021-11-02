using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellManager : MonoBehaviour
{
    public Slider magicSlider;
    public SpellContainer playerMagic;
    // Start is called before the first frame update
    void Start()
    {
        magicSlider.maxValue = playerMagic.maxMana;
        magicSlider.value = playerMagic.maxMana;
        playerMagic.currentMana = playerMagic.maxMana;
    }

    public void AddMagic()
    {
        magicSlider.value += 1;
        playerMagic.currentMana += 1;
        if (magicSlider.value > magicSlider.maxValue)
        {
            magicSlider.value = magicSlider.maxValue;
            playerMagic.currentMana = playerMagic.maxMana;
        }
    }

    public void DecreaseMagic()
    {
        magicSlider.value -= 1;
        playerMagic.currentMana -= 1;
        if (magicSlider.value < 0)
        {
            magicSlider.value = 0;
            playerMagic.currentMana = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
