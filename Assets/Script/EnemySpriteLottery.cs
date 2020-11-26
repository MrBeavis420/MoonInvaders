using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteLottery : MonoBehaviour
{
    SpriteRenderer sr;

    [Header("Sprite Options")]
    public Sprite[] avalibleEnemySprites;
    public bool onlyUseRedGreenOrBlue = false;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        sr.sprite = avalibleEnemySprites[Random.Range(0, avalibleEnemySprites.Length)];

        if(onlyUseRedGreenOrBlue) { ChooseColorRGB(); }
        else { ChooseColorAll(); }

        
    }

    void ChooseColorRGB()
    {
        int choice = Random.Range(1, 4);

        if(choice == 1)
        {
            sr.color = new Color (1, 0, 0);
        }

        if (choice == 2)
        {
            sr.color = new Color(0, 1, 0);
        }

        if (choice == 3)
        {
            sr.color = new Color(0, 0, 1);
        }
    }

    void ChooseColorAll()
    {
        float red = Random.Range(0, 255);
        float green = Random.Range(0, 255);
        float blue = Random.Range(0, 255);

        Color spriteColor = new Color (red/255,green/255,blue/255);

        sr.color = (spriteColor);
    }
}
