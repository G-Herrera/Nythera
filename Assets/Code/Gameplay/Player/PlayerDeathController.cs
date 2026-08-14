using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathController : MonoBehaviour
{
    private bool isDead = false;

    public void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log("Player muerto");

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}