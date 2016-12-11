using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class Tooltip : MonoBehaviour
{
    public static Tooltip Instance;
    
    //Text of the tooltip
    Text mText;

    //If the tooltip is active and should render
    bool mActive;
    
    [SerializeField]
    float mWidth;
    [SerializeField]
    float mHeight;

    float mCanvasWidth;
    [SerializeField]
    float mCanvasHeight;

    [SerializeField]
    float mYShift;
    [SerializeField]
    float mXShift;

    RectTransform mCornerImage;
    RectTransform mRectTransform;
    RenderMode mGUIMode;
    CanvasScaler mScaler;

    void Awake()
    {
        Instance = this;
        mRectTransform = transform.GetComponent<RectTransform>();
    }

    void Start()
    {
        mText = GetComponentInChildren<Text>();
        mCornerImage = transform.Find("Corner").GetComponent<RectTransform>();
        mGUIMode = transform.parent.GetComponent<Canvas>().renderMode;
        mScaler = transform.parent.GetComponent<CanvasScaler>();
        HideTooltip();
    }

    public void SetTooltip(string aText, int aMaxWidth = 0)
    {
        //ScreenSpaceOverlay Tooltip
        if (mGUIMode == RenderMode.ScreenSpaceOverlay && aText != "" && aText != null)
        {
            mText.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
            mText.text = aText;

            if (aMaxWidth != 0)
            {
                mText.horizontalOverflow = HorizontalWrapMode.Wrap;
                mText.GetComponent<RectTransform>().sizeDelta = new Vector2(aMaxWidth, 100);
                mRectTransform.sizeDelta = new Vector2(aMaxWidth + 60f, mText.preferredHeight + 20f);

            }
            else
            {
                mText.horizontalOverflow = HorizontalWrapMode.Overflow;
                mRectTransform.sizeDelta = new Vector2(mText.preferredWidth + 60f, mText.preferredHeight + 20f);
            }
            mWidth = mRectTransform.sizeDelta[0];
            mHeight = mRectTransform.sizeDelta[1];
            
            this.gameObject.SetActive(true);
            mActive = true;
        }
    }

    public void HideTooltip()
    {
        //ScreenSpaceOverlay Tooltip
        if (mGUIMode == RenderMode.ScreenSpaceOverlay)
        {
            mText.text = "";
            gameObject.SetActive(false);
            mActive = false;
        }
    }

    void FixedUpdate()
    {
        mCanvasWidth = transform.parent.GetComponent<RectTransform>().rect.width;
        mCanvasHeight = transform.parent.GetComponent<RectTransform>().rect.height;

        mWidth = mRectTransform.sizeDelta[0];
        mHeight = mRectTransform.sizeDelta[1];
        if (mActive)
        {
            //ScreenSpaceOverlay Tooltip
            if (mGUIMode == RenderMode.ScreenSpaceOverlay)
            {

                //Figure out which quad we are in
                //Determine X Shift
                if (Input.mousePosition.x > Screen.width / 2f)
                {
                    mXShift = mWidth / 2 + 1;
                    //Determine Y Shift
                    if (Input.mousePosition.y > Screen.height / 2f)
                    {
                        //mBGTR;
                        mCornerImage.anchorMin = new Vector2(1,1);
                        mCornerImage.anchorMax = new Vector2(1,1);
                        mCornerImage.localRotation = Quaternion.Euler(0, 0, 270);
                        mCornerImage.anchoredPosition = Vector2.zero;
                        mYShift = mHeight / 2 + 1;
                    }
                    else
                    {
                        //mBGBR;
                        mCornerImage.anchorMin = new Vector2(1, 0);
                        mCornerImage.anchorMax = new Vector2(1, 0);
                        mCornerImage.localRotation = Quaternion.Euler(0, 0, 180);
                        mCornerImage.anchoredPosition = Vector2.zero;
                        mYShift = -mHeight / 2 - 1;
                    }
                }
                else
                {
                    mXShift = -mWidth / 2 - 1;
                    //Determine Y Shift
                    if (Input.mousePosition.y > Screen.height / 2f)
                    {
                        //mBGTL;
                        mCornerImage.anchorMin = new Vector2(0, 1);
                        mCornerImage.anchorMax = new Vector2(0, 1);
                        mCornerImage.localRotation = Quaternion.Euler(0, 0, 0);
                        mCornerImage.anchoredPosition = Vector2.zero;
                        mYShift = mHeight / 2 + 1;
                    }
                    else
                    {
                        //mBGBL;
                        mCornerImage.anchorMin = new Vector2(0, 0);
                        mCornerImage.anchorMax = new Vector2(0, 0);
                        mCornerImage.localRotation = Quaternion.Euler(0, 0, 90);
                        mCornerImage.anchoredPosition = Vector2.zero; 
                        mYShift = -mHeight / 2 - 1;
                    }
                }

                //TODO: So when scaling by a reference resolution, this gets messi. I need to scale with it somehow.

                if (mScaler != null)
                {
                    //Get the different in our base res and the scaled res
                    Vector2 screenSizeDifference = new Vector2(mScaler.referenceResolution.x - Screen.width, mScaler.referenceResolution.y - Screen.height);
                    //newPos = new Vector3(newPos.x - screenSizeDifference.x, newPos.y - screenSizeDifference.y, 0);
                    //Get the ratio?
                    float ratio = Screen.width / mScaler.referenceResolution.x;
                    mXShift *= ratio;
                    mYShift *= ratio;
                }

                Vector3 newPos = Input.mousePosition - new Vector3(mXShift, mYShift, 0f);                
                transform.position = newPos;
            }
        }
    }



}
