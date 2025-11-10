using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class FillStatus : MonoBehaviour
{


    public PlayHealth playerHealth;

    public image fillimage; 

    private slider slider; 
    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<slider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value <= slider.minValue)
        {
            fillimage.enabled = false;
        }

        if (slider.value > slider.minValue && !fillmage.enabled)

    {
            fillimage.enabled = true;
    }

        float fillValue = playerHealth.currentHealth / playerHealth.maxHealth;
        if(fillvalue <= slider.maxValue / 3)
        {
            fillimage.color = color.white;
        }

        else if (fillvalue > slider.maxValue / 3)
    {
        fillimage.color = Color.red;
    }
        
        slider.value = fillValue;


        
    }
}
