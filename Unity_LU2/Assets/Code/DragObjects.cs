using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class DragObjects : MonoBehaviour
{
    public static bool isDraggingDisabled = false;
    private string environment2D_Id;
    private bool isDragging = false;

    private void Start()
    {
        environment2D_Id = PlayerPrefs.GetString("environmentID");
    }

    private void Update()
    {
        if (isDragging && !isDraggingDisabled)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            mousePosition.z = 0;
            transform.position = mousePosition;
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
        SetSortingLayerToMinusOne();

        SendObjectCreatedRequest();
    }

    private void SendObjectCreatedRequest()
    {
        string prefabName = gameObject.name.Replace("(Clone)", "").Trim();

        ObjectData data = new ObjectData
        {
            prefabId = prefabName,
            positionX = transform.position.x,
            positionY = transform.position.y,
            scaleX = 0,
            scaleY = 0,
            rotationZ = 0,
            sortingLayer = 0,
            environment2D_Id = environment2D_Id
        };

        string jsonData = JsonUtility.ToJson(data);
        SterreWebAPI.Instance.Post("/Userinfo/createObject", jsonData, ObjectCreated);
    }

    private void ObjectCreated(APIResponse response)
    {
        if (response.Success)
        {
            Debug.Log("Dropped, API request");
            Destroy(this);
        }
    }

    private void SetSortingLayerToMinusOne()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sortingOrder = -1;
        }
    }
}
