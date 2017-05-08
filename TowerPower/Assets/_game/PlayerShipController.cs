using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour {

    public float speed;
    Vector2 pos;
    Vector2 input = Vector2.zero;
    Vector2 center = Vector2.zero;

	// Use this for initialization
	void Start () {
        center = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //1pos.x = Camera.main.ViewportToWorldPoint
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        pos = transform.position;

        if (input == Vector2.zero)
        {
            input = center - pos;
        }

        transform.Translate(input.normalized * speed * Time.deltaTime);
    }
}
