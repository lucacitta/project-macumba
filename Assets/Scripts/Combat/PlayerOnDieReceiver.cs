using UnityEngine;


class PlayerOnDieReceiver : MonoBehaviour, IDie
{
    public void OnDied()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}