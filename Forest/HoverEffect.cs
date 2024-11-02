using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HoverEffect : MonoBehaviour
{
    public float hoverSpeed = 2.0f; // מהירות התנועה
    public float hoverAmount = 0.5f; // כמה רחוק הוא יזוז למעלה ולמטה

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // שמירת מיקום התחלתי
    }

    void Update()
    {
        // חישוב המיקום החדש באמצעות Sin כדי ליצור אפקט של תנועה מחזורית למעלה ולמטה
        float newY = startPos.y + Mathf.Sin(Time.time * hoverSpeed) * hoverAmount;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
