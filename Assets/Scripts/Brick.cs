using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Brick : UI_Base
{
    Define.BlockState blockState = Define.BlockState.None;

    private int _id = 0; // 몇번째 칸인지
    private TMP_Text _ambientBombsCountText;


    [SerializeField] private GameObject _contentsObj;
    [SerializeField] private GameObject _capObj;

    bool isBomb = false;


    private void Start()
    {
        Initialize();
    }

    void Initialize() // 겜시작할때 한번만 실행되면 되는것들. 숫자인지 폭탄인지같은거
    {

        BindEvent(MouseDown, UIEvent.PointerDown);
        BindEvent(MouseDrag, UIEvent.Drag);
        BindEvent(MouseUp, UIEvent.PointerUp);

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


    private void MouseDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log(gameObject.name + "왼쪽 DOWN");
        }
        else
        {
            Debug.Log(gameObject.name + "오른쪽 DOWN");
        }
    }

    void MouseDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log(gameObject.name + "왼쪽 드래그중");
        }
        else
        {
            Debug.Log(gameObject.name + "오른쪽 드래그중");
        }
    }
    void MouseUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log(gameObject.name + "왼쪽 UP");
        }
        else
        {
            Debug.Log(gameObject.name + "오른쪽 UP");
        }
    }

}
/*
 
 눌렸는데 나 0 번이면
딱달라붙은 0번버튼들 전부 눌림 처리한다. 전염시키면됨
 
 
 
 */