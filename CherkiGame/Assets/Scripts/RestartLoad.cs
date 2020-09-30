using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Loading());
    }

    IEnumerator Loading() //Go back to cherki scene after 1 second
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Cherki");
    }
}
