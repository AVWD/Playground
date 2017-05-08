using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOut : MonoBehaviour
{

    Image img;
    Color col;
    public float fadeRate = 0.01f;

    // Use this for initialization
    void Start()
    {
        img = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        col = img.color;
        if(col.a > 0)
        {
            img.color = new Color(col.r, col.g, col.b, col.a - fadeRate);
        }
    }
}
