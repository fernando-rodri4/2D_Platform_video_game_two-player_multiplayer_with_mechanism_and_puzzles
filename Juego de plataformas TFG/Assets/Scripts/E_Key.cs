using System.Collections;
using UnityEngine;

public class E_Key : MonoBehaviour
{
    public GameObject keyAnimation;

    int counter = 4;

    IEnumerator OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.GetComponent<PlayerMovement>().isLocalPlayer && keyAnimation.activeSelf == false) {
            keyAnimation.SetActive(true);

            yield return new WaitForSeconds(5f);

            keyAnimation.SetActive(false);

            counter--;
        }
    }

    void Update()
    {
        if (counter == 0)
        {
            Destroy(this);
        }
    }
}
