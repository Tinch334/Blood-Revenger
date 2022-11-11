using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersonajeVida : MonoBehaviour
{
    public void die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
