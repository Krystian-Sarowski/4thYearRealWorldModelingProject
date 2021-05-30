using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombKick : MonoBehaviour
{
    protected Rigidbody2D r;
    public float thrust = 10.0f;
    public float speed;
    public float cur_drag;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //BombUp();
    }

    void FixedUpdate()
    {
        speed = r.velocity.magnitude;

    }


    void BombUp()
    {
        r.AddForce(Vector2.up * thrust, ForceMode2D.Impulse);

        StartCoroutine(AddDrag());
    }

    IEnumerator AddDrag()
    {
        float maxDrag = 1000;
        float mul_drag = 500;
        float current_drag = 1;

        while (current_drag < maxDrag)
        {
            current_drag += Time.deltaTime * mul_drag;
            r.drag = current_drag;
            yield return null;
        }

        r.velocity = new Vector2(0,0);
        r.angularDrag = 0;
        r.drag = 0;
        print("Done");
    }
}
