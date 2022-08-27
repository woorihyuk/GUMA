using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GameOver
{
    public class GameOverManager : MonoBehaviour
    {
        public void OnClickRetry()
        {
            if (!GameManager.Instance.LoadGame())
            {
                SceneManager.LoadScene("01");
            }
        }
    }
}