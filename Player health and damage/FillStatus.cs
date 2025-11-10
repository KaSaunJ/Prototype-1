using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class FillStatus : MonoBehaviour
{
    public PlayHealth playerHealth;

    public Image fillImage; 
    private Slider slider; 

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        if (slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }
        else if (slider.value > slider.minValue && !fillImage.enabled)
        {
            fillImage.enabled = true;
        }

        float fillValue = playerHealth.currentHealth / playerHealth.maxHealth;

        if (fillValue <= slider.maxValue / 3)
        {
            fillImage.color = Color.white;
        }
        else
        {
            fillImage.color = Color.red;
        }

        slider.value = fillValue;
    }
}
