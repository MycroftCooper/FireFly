using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameKit;
public class GameCam : MonoBehaviour
{
    [Header("General Settings")]
    public Vector2 cameraSize = new Vector2(2.9f, 2.9f);
    public Vector2 pivot = new Vector2(-0.5f, -0.5f);
    public LayerMask detectLayer;
    public Transform rotatable;
    public Transform unRotatable;
    public SpriteRenderer maskSr;
    public SpriteRenderer cameraSr;
    private Texture2D camTexture;
    private Camera sampleCam;

    [Header("Debug")]
    [SerializeField] private bool isActive = true;
    [SerializeField] private bool isOcuppied = false;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool canShot = true;
    [SerializeField] private bool canMove = false;
    [SerializeField] private bool canRoate = false;
    private Vector3 targetPos;


    [Header("Move Constrait")]
    [SerializeField] private bool canUp = true;
    [SerializeField] private bool canDown = true;
    [SerializeField] private bool canLeft = true;
    [SerializeField] private bool canRight = true;

    [Header("Move Settings")]
    public float stepLen = 1;
    public float interval = 0.2f;
    [Range(0f, 1f)] public float smoothDistance = 0.3f;

    void Start()
    {
        // cameraSr = this.GetComponent<SpriteRenderer>();
        // maskSr = this.GetComponentsInChildren<SpriteRenderer>()[1];
        targetPos = this.transform.position;
        sampleCam = this.GetComponentInChildren<Camera>();
        EventCenter.instance.AddEventListener("Activate Rotate",()=>{
            canRoate = true;
        });
        // Usage: EventCenter.instance.EventTrigger("Activate Rotate");
        EventCenter.instance.AddEventListener("Activate Move",()=>{
            canMove = true;
        });
        // Usage: EventCenter.instance.EventTrigger("Activate Move");
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!canShot)
            {
                CameraUse();
                TurnOffCam();
            }
            else if (!isOcuppied && canShot)
            {
                if (isActive)
                    TurnOffCam();
                else
                    TurnOnCam();
            }
        }

        if (!isActive)
            return;

        CameraRotate();
        CameraMove();
        if (Input.GetKeyDown(KeyCode.X))
            CameraUse();
        UpdateMoveConstrait();
    }

    private void CameraRotate()
    {
        if(!canRoate)
            return;
        if (!canShot && !isOcuppied)
        {
            if (camTexture != null)
            {
                Sprite tempSprite = Sprite.Create(camTexture, new Rect(0, 0, camTexture.width, camTexture.height), -pivot, 86);
                maskSr.sprite = tempSprite;
                maskSr.enabled = true;
            }


            if (Input.GetKeyDown(KeyCode.Q))
            {
                isOcuppied = true;
                rotatable.transform.DORotate(rotatable.transform.eulerAngles + new Vector3(0, 0, 90), interval).OnComplete(() =>
                {
                    isOcuppied = false;
                });
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                isOcuppied = true;
                rotatable.transform.DORotate(rotatable.transform.eulerAngles + new Vector3(0, 0, -90), interval).OnComplete(() =>
                {
                    isOcuppied = false;
                });
            }
        }
    }
    private void CameraMove()
    {
        if(!canMove)
            return;
        if (isMoving)
        {
            if ((targetPos - transform.position).magnitude < smoothDistance)
                isMoving = false;
            else
                return;
        }
        float X = 0, Y = 0;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            X = -1;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            X = 1;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            Y = 1;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            Y = -1;
        if (X == 0 && Y == 0)
            return;
        Vector3 tempPos = targetPos;
        if (tempPos != transform.position)
            transform.position = tempPos;

        Vector3 moveDir = new Vector3(X * stepLen, Y * stepLen, 0);
        targetPos = tempPos + moveDir;
        isMoving = true;
        if (!canRight && X > 0)
            targetPos = transform.position;
        else if (!canLeft && X < 0)
            targetPos = transform.position;
        else if (!canUp && Y > 0)
            targetPos = transform.position;
        else if (!canDown && Y < 0)
            targetPos = transform.position;

        if ((this.transform.position - targetPos).magnitude > 0.01f)
            this.transform.DOMove(targetPos, interval);
    }

    private void CameraUse()
    {

        if (!canShot)
        {
            canShot = true;
            maskSr.enabled = false;
            canUp = true;
            canDown = true;
            canLeft = true;
            canRight = true;
            if (MomentManager.instance.moments.Count > 0)
            {
                foreach (var item in MomentManager.instance.moments)
                {
                    item.entity.gameObject.transform.parent = item.parent;
                    item.entity.UncatchCallBack();
                }
            }
            rotatable.transform.rotation = Quaternion.identity;
            MomentManager.instance.Clear();
            return;
        }
        RecordMoment();
    }

    private void RecordMoment()
    {
        if (isOcuppied || isMoving)
            return;
        cameraSr.DOColor(Color.black, 0.1f).OnComplete(() =>
        {
            cameraSr.DOColor(Color.white, 0.1f);
            Collider2D[] colls = Physics2D.OverlapBoxAll(this.transform.position, cameraSize, 0, detectLayer);
            Debug.Log(colls.Length);
            if (colls.Length > 0)
            {
                canShot = false;
                for (int i = 0; i < colls.Length; i++)
                {
                    ICapturable capturable = colls[i].gameObject.GetComponent<ICapturable>();
                    if (capturable.IsCatchable())
                    {
                        MomentManager.instance.AddMoment(capturable.GetMoment(this.transform));
                        if (MomentManager.instance.moments[i].entity.IsRotatable())
                            colls[i].gameObject.transform.parent = rotatable.transform;
                        else
                            colls[i].gameObject.transform.parent = unRotatable.transform;
                        MomentManager.instance.moments[i].entity.CatchCallBack();
                    }
                }
                camTexture = new Texture2D((int)sampleCam.pixelRect.width, (int)sampleCam.pixelRect.width, TextureFormat.RGB24, false);
                canShot = false;
                StartCoroutine(EndofFrame(camTexture));
            }
        });
    }

    private void UpdateMoveConstrait()
    {
        if (!canShot && MomentManager.instance.moments != null)
        {
            if (MomentManager.instance.moments.Count == 0)
                return;
            canUp = true;
            canDown = true;
            canLeft = true;
            canRight = true;
            for (int i = 0; i < MomentManager.instance.moments.Count; i++)
            {
                canUp = !MomentManager.instance.moments[i].entity.GetConstraitData("OnUp") && canUp;
                canDown = !MomentManager.instance.moments[i].entity.GetConstraitData("OnDown") && canDown;
                canLeft = !MomentManager.instance.moments[i].entity.GetConstraitData("OnLeft") && canLeft;
                canRight = !MomentManager.instance.moments[i].entity.GetConstraitData("OnRight") && canRight;
            }

        }
    }


    public void TweenColor(Color color)
    {
        cameraSr.DOColor(color, 0.05f).OnComplete(() =>
        {
            cameraSr.DOColor(Color.white, 0.5f);
        });
    }

    public void TurnOnCam()
    {
        isActive = true;
        isOcuppied = true;
        // Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        // Vector3 gridMouseWorldPos = new Vector3(Mathf.Ceil(mouseWorldPos.x) - 0.5f, Mathf.Ceil(mouseWorldPos.y) - 0.5f, 0);
        // this.transform.position = gridMouseWorldPos;
        this.transform.DOScale(Vector3.one, 0.15f).OnComplete(() =>
        {
            isOcuppied = false;
        });
    }

    public void TurnOffCam()
    {
        isActive = false;
        isOcuppied = true;
        this.transform.DOScale(Vector3.zero, 0.15f).OnComplete(() =>
        {
            isOcuppied = false;
        });
    }

    IEnumerator EndofFrame(Texture2D camTexture)
    {
        yield return new WaitForEndOfFrame();
        Vector3 cornorPos = new Vector3(sampleCam.transform.position.x, sampleCam.transform.position.y, 0);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(cornorPos);
        Rect rect = new Rect(screenPos.x - sampleCam.pixelRect.width / 2, screenPos.y - sampleCam.pixelRect.height / 2, sampleCam.pixelRect.width, sampleCam.pixelRect.height);

        // 相机超出屏幕无法采样
        if (rect.x + rect.width < 0 || rect.x + rect.width > Screen.width || rect.y + rect.height < 0 || rect.y + rect.height > Screen.height)
        {
            TweenColor(Color.red);
        }
        else
        {
            camTexture.ReadPixels(rect, 0, 0);
            camTexture.Apply();
        }
        sampleCam.Render();
    }


    //----------------------------------* Debug  *-----------------------------------/
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position, cameraSize);
    }

}
