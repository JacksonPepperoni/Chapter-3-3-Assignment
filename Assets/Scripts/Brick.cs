using UnityEngine;
using TMPro;
using System.Diagnostics;

public class Brick : MonoBehaviour
{
    Define.BlockState blockState = Define.BlockState.None;

    private int _id = 0; // 몇번째 칸인지
    private TMP_Text _ambientBombsCountText;


    /// <summary>
    /// Look up the Mouse and Keyboard classes to see more items available. However, your goal should be to move away from spot-checking immediate values from specific hardware like Mouse or Keyboard, and to move toward the event driven device-agnostic system using Input System Action Maps.

    /// As for Controllers, that's what the new Input System is to help you cover. You will want to go through an Input System tutorial to see more about how to set up a joystick. 


    ///   Mouse.current.leftButton.isPressed // equivalent to Input.GetMouseButton(0)
    ///   Mouse.current.leftButton.wasPressedThisFrame // equivalent to Input.GetMouseButtonDown(0)
    ///   Mouse.current.position.x // equivalent to Input.mousePosition


    /// 
    /// 
    /// </summary>


    [SerializeField] private GameObject _contentsObj;
    [SerializeField] private GameObject _capObj;

    bool isBomb = false;

    void Initialize() // 겜시작할때 한번만 실행되면 되는것들. 숫자인지 폭탄인지같은거
    {
        // 기본칸으로 되돌리고 내가 폭탄이 아닐때 
        if (isBomb)
        {


        }
        else
        {

            if (_id != 0)
            {

                int num = Main.Game.NumberOfAmbientBombs(_id);

                _ambientBombsCountText.text = $"{num}";
                _ambientBombsCountText.color = Main.Game.numberColors[num];
            }
        }

        _capObj.SetActive(true);
    }
    public void ZeroIdCheck()
    {
        if (_id == 0)
        { 
        
        }
    }

    public void OnRightClick()
    {
        switch (blockState)
        {
            case Define.BlockState.None:
                break;
            case Define.BlockState.Flag:
                break;
            case Define.BlockState.QuestionMark:
                break;
            default:
                break;
        }
    }

}

/*
 
 눌렸는데 나 0 번이면
딱달라붙은 0번버튼들 전부 눌림 처리한다. 전염시키면됨
 
 
 
 */