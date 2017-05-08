using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AVWD.Engine.Services;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class TowerScript : MonoBehaviour {
    public Transform Target;
    public Vector3 target;
    public Vector3 dir;

    [Header("Projectiles")]
    public Pool<GameObject> ObjectPool;
    public GameObject PrefabInstance;
    [Range(1, 32)]
    public int poolCount = 32;
    [Range(1, 32)]
    public int shotInterval = 5;

    // Internals
    int frames = 0;
    Rigidbody2D rb;
    GameObject bullet;
    Vector3 myPos;
    float angle;
    Vector3 offset;
    Quaternion tgtRot;
    [Range(0.01f, 10f)]
    public float turretTurnSpeed = 5;
    public bool InstantTurn = false;
    public float shotSpeed = 1.5f;

    Rigidbody2D myRigidBody;

    // Use this for initialization
    void Start () {
        ObjectPool = new Pool<GameObject>((o) => {
            return (GameObject)Instantiate(PrefabInstance);
        }, poolCount);

        myPos = Camera.main.WorldToScreenPoint(transform.position);

        DOTween.Init();
        myRigidBody = GetComponent<Rigidbody2D>();

        InvokeRepeating("SearchTarget", 1f, 1.0f);
    }
    void SearchTarget()
    {
        switch (searchType)
        {
            case SearchType.Closest:
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        // TODO: Use coroutine to pick targets every few frames
        //target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target = Target.transform.position;
        
        if(InstantTurn)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, target - transform.position);
        } else
        {
            tgtRot = Quaternion.LookRotation(Vector3.forward, target - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, tgtRot, turretTurnSpeed * Time.deltaTime);
        }

        /*
        if (Input.GetKey(KeyCode.D))
        {
            offset = transform.rotation.eulerAngles;
            offset.z -= 500 * Time.deltaTime;
            transform.rotation = Quaternion.Euler(offset);
        } else if (Input.GetKey(KeyCode.A)) {
            offset = transform.rotation.eulerAngles;
            offset.z += 500 * Time.deltaTime;
            transform.rotation = Quaternion.Euler(offset);
        }

        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * 6 * Time.deltaTime);
        } else if (Input.GetKey(KeyCode.S)) {
            transform.Translate(Vector3.up * 6 * Time.deltaTime * -1);
        }
        */

        if (++frames >= shotInterval)
        {
            frames = 0;

            bullet = ObjectPool.Fetch();
            if (bullet != null)
            {
                bullet.SetActive(true);
                //bullet.transform.parent = this.transform;
                rb = bullet.GetComponent<Rigidbody2D>();
                rb.position = myRigidBody.position;
                rb.rotation = myRigidBody.rotation;
                rb.AddForce(transform.up * 600);
                StartCoroutine(Fire(bullet));
            }
        }

    }

    public SearchType searchType = SearchType.Closest;
    public enum SearchType
    {
        Closest, HighestHP, LowestHP, Strongest, Weakest
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

    IEnumerator Fire(GameObject bullet, Vector3 target)
    {
        bullet.SetActive(true);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
        Tween shotTween = bullet.transform.DOMove(target, shotSpeed);
        yield return shotTween.WaitForCompletion();
        bullet.SetActive(false);
        ObjectPool.Release(bullet);
    }

    /*
    IEnumerator SearchForTarget(float delayInSeconds)
    {
        // TODO: Only attempt to change targets every few frames to lighten load
        // Instead of following mouse, pick a transform within a range and criteria
        while(true)
        {
            switch(searchType)
            {
                case SearchType.Closest:
                    break;
            }
            yield return new WaitForSeconds(delayInSeconds);
        }        
    }
    */

    IEnumerator BlockWait(float delayInSeconds, Action action)
    {
        yield return new WaitForSeconds(delayInSeconds);
        if (action != null) action.Invoke();
    }
}
