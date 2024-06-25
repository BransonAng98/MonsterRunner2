using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IDriveUIToken : MonoBehaviour
{
    private RectTransform rectTransform;
    private TextMeshPro text;
    private string letter;

    public float shrinkDuration;
    public Vector2 targetScale = new Vector2(0.1f, 0.1f);
    public bool endEntity;
    public Vector2 startPos;
    public Vector2 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();
        text = this.GetComponent<TextMeshPro>();
        startPos = rectTransform.position;
    }

    public void AssignTargetPos(Vector2 target, string letterData)
    {
        targetPos = target;
        letter = letterData;
    }

    // Update is called once per frame
    void Update()
    {
        if (!endEntity)
        {
            Vector2 startScale = rectTransform.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < shrinkDuration)
            {
                float t = elapsedTime / shrinkDuration;
                rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
                rectTransform.localScale = Vector2.Lerp(startScale, targetScale, t);
                elapsedTime += Time.deltaTime;
            }

            endEntity = true;
            //rectTransform.anchoredPosition = targetPosition;
            //rectTransform.localScale = targetScale;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
