using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using AVWD.Engine.Services;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2f;
    Vector3 translation = Vector3.zero;
    float _targetOrtho = 5f;
    public float orthoZoomSpeed = 5f;
    public float MinZoom = 1.0f;
    public float MaxZoom = 20.0f;
    public Vector3 target;
    [Range(0.01f, 10f)]
    public float turretTurnSpeed = 5;
    public bool InstantTurn = false;
    Quaternion tgtRot;

    [Header("Projectiles")]
    public Pool<GameObject> ObjectPool;
    public GameObject PrefabInstance;
    [Range(1, 32)]
    public int poolCount = 32;
    [Range(1, 32)]
    public int shotInterval = 5;

    int frames = 0;
    Rigidbody2D rb;
    GameObject bullet;
    Vector3 na = Vector3.zero;
    float sprint = 1;
    public float stamina = 100f;
    public float sprintCost = 1f;
    public float sprintRegen = 5f;
    public Slider staminaSlider;


    // Use this for initialization
    void Start()
    {
        ObjectPool = new Pool<GameObject>((o) => {
            return (GameObject)Instantiate(PrefabInstance);
        }, poolCount);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mousePresent)
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Turning to mouse
            if (InstantTurn)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.forward, target - transform.position);
            }
            else
            {
                tgtRot = Quaternion.LookRotation(Vector3.forward, target - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, tgtRot, turretTurnSpeed * Time.deltaTime);
            }

            // Shooting
            if (Input.GetMouseButton(0))
            {
                if (++frames >= shotInterval)
                {
                    frames = 0;

                    bullet = ObjectPool.Fetch();
                    if (bullet != null)
                    {
                        bullet.SetActive(true);
                        rb = bullet.GetComponent<Rigidbody2D>();
                        rb.position = transform.position;
                        float rot = 0;
                        transform.rotation.ToAngleAxis(out rot, out na);
                        rb.rotation = rot;
                        rb.AddForce(transform.up * 600);
                        StartCoroutine(Fire(bullet));
                    }
                }
            }
        }

        // Sprint / Stamina
        if(stamina > sprintCost)
        {
            sprint = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
            if (sprint == 2) stamina -= sprintCost;
        } else
        {
            sprint = 1;
        }
        stamina = Mathf.Min(100, (stamina + sprintRegen * Time.deltaTime)); // regen
        staminaSlider.value = (stamina / 100f); // gui

        // Movement
        translation.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime * sprint;
        translation.y = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime * sprint;
        transform.Translate(translation);
    }

    public void Release(GameObject obj)
    {
        obj.SetActive(false);
        ObjectPool.Release(obj);
    }

    IEnumerator Fire(GameObject bullet)
    {
        yield return new WaitForSeconds(10);
        Release(bullet);
    }


    public GameObject OuchText;
    Image img;
    Text txt;
    List<string> txtList = new List<string>() { "Ouch!", "Oh god!", "Make it stop", "Stop it", "Merely a flesh wound", "I'm dying!" };

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!OuchText) return;
        if (!img) img = OuchText.GetComponent<Image>();
        if (!txt) txt = OuchText.GetComponent<Text>();

        if (coll.gameObject.name == "bullet")
        {
            txt.text = txtList[(int)Random.Range(0, txtList.Count)];
            img.color = new Color(1, 0, 0, 1.0f); 
        }
    }

}
