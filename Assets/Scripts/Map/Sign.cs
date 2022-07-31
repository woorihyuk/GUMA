using UnityEngine;

public class Sign : MonoBehaviour
{
    public int type;
    public GameObject talk;

    private int textType;
    private int maxText;
    private string[] talkText;

    private GameUIManager _gameUIManager;

    private void Start()
    {
        _gameUIManager = FindObjectOfType<GameUIManager>();

        switch (type)
        {
            case 1:
                talkText = new[] { "뭐야 너!", "다 까먹어버린거야?", "A 키를 누르면 왼쪽으로, ", "D키를 누르면 오른쪽으로 이동할 수 있어." };
                maxText = 3;
                break;
            case 2:
                talkText = new[] { "Space Bar를 누르면 점프를 해", "한번 더 누르면 더블 점프!" };
                maxText = 1;
                break;
            case 3:
                talkText = new[] { "Shift키를 누르면 대쉬를 하고,", "가만히 있는 상태에서 Shift를 누르면 백스탭을 해" };
                maxText = 1;
                break;
            case 4:
                talkText = new[] { "좌클릭을 하면 창 공격을 할 수 있어,", "한 번 공격해봐" };
                maxText = 1;
                break;
            case 5:
                talkText = new[] { "우클릭을 누르면 총알이 앞으로 나가", "푸슝-!" };
                maxText = 1;
                break;
            case 6:
                talkText = new[]
                    { "창 공격과 총공격을 섞어서 콤보를 만들 수 있는 건 안 까먹었지?", "그럴거라고 믿어 친구~", "우리를 배척하는 인간놈들을 도와주러 가보자고…!" };
                maxText = 2;
                break;
        }
    }

    public void TalkStart()
    {
        // Debug.Log(type);
        talk.SetActive(true);
        textType = 0;
        NextText();
    }

    public void NextText()
    {
        if (textType < talkText.Length)
        {
            if (textType % 2 == 1)
            {
                _gameUIManager.text2.text = talkText[textType++];
            }
            else
            {
                _gameUIManager.text1.text = talkText[textType++];
                _gameUIManager.text2.text = "";
            }
        }
        else
        {
            TextEnd();
        }
    }

    public void TextEnd()
    {
        talk.SetActive(false);
    }
}