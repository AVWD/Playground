using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AVWD.Engine.Services;

public class ObjectPooler : MonoBehaviour {

    [Header("Settings")]
    public static ObjectPooler Instance;
    public Pool<GameObject> ObjectPool;
    public GameObject PrefabInstance;
    [Range(1, 100000)]
    public int poolCount = 512;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        ObjectPool = new Pool<GameObject>((o) => {
            return (GameObject)Instantiate(PrefabInstance);
        }, poolCount);

        basex = transform.position.x;
        position = transform.position;
    }

    int frames = 0;
    Rigidbody2D rb;

    [Range(0.05f, 5)]
    public float speedMult = .05f;
    [Range(0.05f, 50)]
    public float rangeMult = 1.0f;
    [Range(0.05f, 5)]
    public float shootInterval = 1.0f;
    float basex = 0.0f;
    Vector3 position;
    float interval = 0;

    // Update is called once per frame
    void Update () {

        interval = Mathf.Sin(Time.time * (speedMult / rangeMult)) * rangeMult;
        
        if (++frames >= shootInterval)
        {
            frames = 0;
            GameObject bullet = ObjectPool.Fetch();
            if(bullet != null)
            {
                position.x = basex + interval;
                bullet.transform.position = position;
                //bullet.transform.rotation.SetAxisAngle(this.transform.position, 45.0f);

                rb = bullet.GetComponent<Rigidbody2D>();
                rb.velocity = Vector3.zero;
                rb.angularVelocity = 0f;
                rb.rotation = 45f * interval;
                rb.AddForce(bullet.transform.forward * 1f);
                bullet.SetActive(true);

                StartCoroutine(BlockWait(10, () =>
                {
                    this.Release(bullet);
                }));
            }
        }
	}

    public void Release(GameObject obj)
    {
        obj.SetActive(false);
        ObjectPool.Release(obj);
    }
    public GameObject Fetch()
    {
        GameObject obj = ObjectPool.Fetch();
        return obj;
    }

    IEnumerator BlockWait(float delayInSeconds, Action action)
    {
        yield return new WaitForSeconds(delayInSeconds);
        if (action != null) action.Invoke();

        print("wait completed");
    }
}
