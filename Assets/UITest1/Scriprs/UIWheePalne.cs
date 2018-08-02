using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIWheePalne : MonoBehaviour, IPointerDownHandler, IPointerExitHandler
{
    [HideInInspector]
    public List<string> allShowData = new List<string>();
    public List<Text> showPalne = new List<Text>();
    [HideInInspector]
    public int CenIndex = 0;
    bool isDown = false;
    Vector2 OldPoint;
    Vector2 Nowpoint;
    float AllMove = 0;//需要移动的量
    float MoveAcc = 0;//移动的累计量
    float MovePar = 10;//移动系数
    int onePiceMove = 60;//一片的高度
    bool isEnable = true;
    public event System.Action<string> OnChoiceClick;
    public void AddListenr(System.Action<string> _action)
    {
        OnChoiceClick = _action;
    }
    public void ChoiseClick()
    {
        if (OnChoiceClick != null)
        {
            string str;
            try
            {
                str = allShowData[CenIndex];
            }
            catch (System.Exception)
            {
                str = "";
            }
            OnChoiceClick(str);
        }
        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (isDown && Input.touchCount > 0)
        {
            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    OldPoint = Input.GetTouch(0).position;
                    AllMove = 0;
                    MoveAcc = 0;
                    break;
                case TouchPhase.Moved:
                    if (isDown)
                    {
                        Nowpoint = Input.GetTouch(0).position;
                        float rang = Nowpoint.y - OldPoint.y;
                        AllMove += rang;
                        OldPoint = Nowpoint;
                    }
                    break;
                case TouchPhase.Ended:
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator MoveUpDate()
    {
        while (isEnable)
        {
            if (AllMove != 0)
            {
                MoveAcc += (AllMove / MovePar);
                AllMove -= (AllMove / MovePar);
                if (Mathf.Abs(MoveAcc) > onePiceMove)
                {
                    if (MoveAcc > 0)
                    {
                        MoveAcc -= onePiceMove;
                    }
                    else
                    {
                        MoveAcc += onePiceMove;
                    }
                    if (AllMove > 0)
                    {
                        if (CenIndex < (allShowData.Count - 1))
                        {
                            CenIndex++;
                        }
                        ShowPlaneUpDate();

                    }
                    else if (AllMove < 0)
                    {
                        if (CenIndex > 0)
                        {
                            CenIndex--;
                        }
                        ShowPlaneUpDate();
                    }
                }
                if (Mathf.Abs(AllMove) < (onePiceMove / MovePar))
                {
                    AllMove = 0;
                    MoveAcc = 0;
                }
            }
            yield return null;
        }
    }

    private void ShowPlaneUpDate()
    {
        if (allShowData.Count < 1)
        {
            showPalne[0].text = "";
            showPalne[1].text = "";
            showPalne[2].text = "";
            showPalne[3].text = "";
            showPalne[4].text = "";
            return;
        }
        if (CenIndex > 1 && CenIndex < (allShowData.Count - 2))
        {
            showPalne[0].text = allShowData[CenIndex - 2];
            showPalne[1].text = allShowData[CenIndex - 1];
            showPalne[2].text = allShowData[CenIndex];
            showPalne[3].text = allShowData[CenIndex + 1];
            showPalne[4].text = allShowData[CenIndex + 2];

        }
        else if (CenIndex == 1)
        {
            showPalne[0].text = "";
            showPalne[1].text = allShowData[CenIndex - 1];
            showPalne[2].text = allShowData[CenIndex];
            try
            {
                showPalne[3].text = allShowData[CenIndex + 1];
            }
            catch (System.Exception)
            {
                showPalne[3].text = "";
            }
            try
            {
                showPalne[4].text = allShowData[CenIndex + 2];
            }
            catch (System.Exception)
            {
                showPalne[4].text = "";
            }
        }
        else if (CenIndex == 0)
        {
            showPalne[0].text = "";
            showPalne[1].text = "";
            showPalne[2].text = allShowData[CenIndex];
            try
            {
                showPalne[3].text = allShowData[CenIndex + 1];
            }
            catch (System.Exception)
            {
                showPalne[3].text = "";
            }
            try
            {
                showPalne[4].text = allShowData[CenIndex + 2];
            }
            catch (System.Exception)
            {
                showPalne[4].text = "";
            }
        }
        else if (CenIndex == (allShowData.Count - 1))
        {
            showPalne[0].text = allShowData[CenIndex - 2];
            showPalne[1].text = allShowData[CenIndex - 1];
            showPalne[2].text = allShowData[CenIndex];
            showPalne[3].text = "";
            showPalne[4].text = "";
        }
        else if (CenIndex == (allShowData.Count - 2))
        {
            showPalne[0].text = allShowData[CenIndex - 2];
            showPalne[1].text = allShowData[CenIndex - 1];
            showPalne[2].text = allShowData[CenIndex];
            showPalne[3].text = allShowData[CenIndex + 1];
            showPalne[4].text = "";
        }
    }

    private void OnEnable()
    {
        isDown = false;
        isEnable = true;
        ShowPlaneUpDate();
        StopAllCoroutines();
        StartCoroutine(MoveUpDate());
    }
    private void OnDisable()
    {
        isEnable = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isDown = false;
    }
}
