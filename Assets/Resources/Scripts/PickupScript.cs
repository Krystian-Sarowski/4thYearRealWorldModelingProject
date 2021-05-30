using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    public GameObject go;

    private void Start()
    {
        StartCoroutine("DestroyObject");
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(6f);
        Destroy(gameObject);
    }
}
