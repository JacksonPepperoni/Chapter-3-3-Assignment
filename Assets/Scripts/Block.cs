using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    private int _number = 0;


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

    private void Awake()
    {

    }




    void Initialize()
    {




    }


    void Setting() // 기본칸으로 되돌리고 내가 폭탄이 아닐때 자기주위에 폭탄몇개인지 체크 
    {


    }


    void Update()
    {

    }
}
