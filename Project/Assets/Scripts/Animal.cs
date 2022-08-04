using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    enum State
    {
        searching,
        getting
    }

    [SerializeField] string diet = "Plant";
    [SerializeField] float aboveWater;
    List<GameObject> foodSeen;
    public float food = 100;
    public Color color;
    public float speed;
    GameObject target;
    SpriteRenderer sr;
    float dir = .1f;
    float primaryY;
    State state;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        var r = Random.Range(1, 11);

        switch (r)
        {
            case 1: color = Color.green; break;
            case 2: color = Color.blue; break;
            case 3: color = Color.red; break;
            case 4: color = Color.black; break;
            case 5: color = Color.grey; break;
            case 6: color = Color.cyan; break;
            case 7: color = Color.clear; break;
            case 8: color = Color.gray; break;
            case 9: color = Color.magenta; break;
            case 10: color = Color.yellow; break;
        }

        sr.color = color;

        speed = Random.Range(.01f, .2f);

        primaryY = transform.position.y;

        foodSeen = new List<GameObject>();


        state = State.searching;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(diet))
        {
            food = 100;
            state = State.searching;
            target = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(diet))
        {
            foodSeen.Add(collision.gameObject);
        }
    }

    void Update()
    {
        DoStates();
        CheckNeeds();
        SimulateNeeds();
        TurnTowardDir();
        CheckForBounds();
    }

    void TurnTowardDir()
    {
        if (dir > 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    private void OnMouseDown()
    {
        Camera.main.GetComponent<Cam>().ClickedOn(gameObject);
    }

    void CheckForBounds()
    {
        if (transform.position.x > 50)
        {
            dir = -speed;
        }
        else if (transform.position.x < -50)
        {
            dir = speed;
        }
    }

    void SimulateNeeds()
    {
        food -= Time.deltaTime * 5;
    }

    void CheckNeeds()
    {
        if (food < 50)
        {
            if (foodSeen.Count == 0)
            {
                state = State.searching;
            }
            else
            {
                state = State.getting;
            }
        }
    }

    void DoStates()
    {
        if (state == State.searching)
        {
            if (transform.position.y != primaryY)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector3(transform.position.x, primaryY, 0), .1f);
            }

            transform.Translate(dir, 0, 0);
        }
        else if (state == State.getting)
        {
            if (foodSeen.Count == 0)
            {
                state = State.searching;
            }
            else if (target)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, .1f);
            }
            else if (foodSeen.Count > 0)
            {
                target = ClosestFood();
            }
        }
    }

    GameObject ClosestFood()
    {
        GameObject closest = null;

        for (int i = 0; i < foodSeen.Count; i++)
        {
            if (!closest)
            {
                closest = foodSeen[i];
            }
            else if (Vector2.Distance(transform.position, foodSeen[i].transform.position) < Vector2.Distance(transform.position, closest.transform.position))
            {
                closest = foodSeen[i];
            }
        }

        return closest;
    }
}
