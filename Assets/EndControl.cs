using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndControl : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.isEndWatched = true;
        SceneManager.LoadScene("01");
        GameUIManager.Instance.endCg.gameObject.SetActive(false);
    }
}
