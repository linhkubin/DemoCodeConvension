using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    //public bool IsAvoidBackKey = false;
    public bool IsDestroyOnClose = false;
    public bool IsHandlingRabbitEars = false;
    public bool IsWidescreenProcessing = false;

    protected RectTransform m_RectTransform;
    private Animator m_Animator;
    private float m_OffsetY = 0;

    private void Start()
    {
        OnInit();
    }

    //Init default Canvas
    //khoi tao gia tri canvas
    protected void OnInit()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_Animator = GetComponent<Animator>();

        // xu ly tai tho
        float ratio = (float)Screen.height / (float)Screen.width;
        if (IsHandlingRabbitEars)
        {
            if (ratio > 2.1f)
            {
                Vector2 leftBottom = m_RectTransform.offsetMin;
                Vector2 rightTop = m_RectTransform.offsetMax;
                rightTop.y = -100f;
                m_RectTransform.offsetMax = rightTop;
                leftBottom.y = 0f;
                m_RectTransform.offsetMin = leftBottom;
                m_OffsetY = 100f;
            }
        }

        //=> CODE BY: MINH AN
        //TODO: Xu ly man hinh with/height qua rong - xu ly voi man hinh co ti le with/height < 2.1f
        if (IsWidescreenProcessing)
        {
            ratio = (float)Screen.width / (float)Screen.height;
            if (ratio < 2.1f)
            {
                //size tieu chuan
                float ratioDefault = 850 / 1920f;
                float ratioThis = ratio;

                float value = 1 - (ratioThis - ratioDefault);

                float with = m_RectTransform.rect.width * value;

                m_RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, with);
            }

        }

        //Set parent cho popup child
        //cai nay tien cho viec truy suat truc tiep tu thang con ve thang cha quan ly
        for (int i = 0; i < popups.Length; i++)
        {
            popups[i].ParentsPopup = this;
        }
    }

    //Setup canvas to avoid flash UI
    //set up mac dinh cho UI de tranh truong hop bi nhay' hinh
    public virtual void Setup()
    {
        UIManager.Ins.AddBackUI(this);
        UIManager.Ins.PushBackAction(this, BackKey);
    }


    //back key in android device
    //back key danh cho android
    public virtual void BackKey()
    {

    }

    //Open canvas
    //mo canvas
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    //close canvas directly
    //dong truc tiep, ngay lap tuc
    public virtual void CloseDirectly()
    {
        UIManager.Ins.RemoveBackUI(this);
        gameObject.SetActive(false);
        if (IsDestroyOnClose)
        {
            Destroy(gameObject);
        }
        
    }

    //close canvas with delay time, used to anim UI action
    //dong canvas sau mot khoang thoi gian delay
    public virtual void Close(float delayTime)
    {
        Debug.Log("Close");
        Invoke(nameof(CloseDirectly), delayTime);
    }


    #region Popup
    [Header("Popup Child")]
    [SerializeField] UICanvas[] popups;
    public UICanvas ParentsPopup { get; set; }

    public T GetPopup<T>() where T: UICanvas
    {
        T ui = null;
        for (int i = 0; i < popups.Length; i++)
        {
            if (popups[i] is T)
            {
                ui = popups[i] as T;
                break;
            }
        }

        return ui;
    }

    public T OpenPopup<T>() where T: UICanvas
    {
        T ui = GetPopup<T>();
        ui.Setup();
        ui.Open();
        return ui;
    }

    public bool IsOpenedPopup<T>() where T : UICanvas
    {
        return GetPopup<T>().gameObject.activeSelf;
    }


    public void ClosePopup<T>(float delayTime) where T: UICanvas
    {
        GetPopup<T>().Close(delayTime);
    }
    
    public void ClosePopupDirect<T>() where T: UICanvas
    {
        GetPopup<T>().CloseDirectly();
    }

    public void CloseAllPopup()
    {
        for (int i = 0; i < popups.Length; i++)
        {
            popups[i].CloseDirectly();
        }
    }

    #endregion
}
