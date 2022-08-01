﻿using Characters.Grabber;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PointUpdate : MonoBehaviour
    {
        public GameObject player;
        GrabberAgent playerAgent;

        TextMeshProUGUI amountText;
        public Vector3 pos;
        float panelWidth;
        float panelHeight;

        /*private void Start()
    {
        playerAgent = player.GetComponent<GrabberAgent>();
        GetComponentInChildren<RawImage>().color = playerAgent.color;
        amountText = GetComponentInChildren<TextMeshProUGUI>();

        panelWidth = Screen.width;
        panelHeight = Screen.height;
    }

    private void Update()
    {
        if (player != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(player.transform.GetChild(0).transform.position);

            transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -(Screen.width + 20), Screen.width - 20), Mathf.Clamp(transform.localPosition.y, -(Screen.height + 20), Screen.height - 20), transform.localPosition.z);

            amountText.text = playerAgent.amount.ToString();
        }
        else
        {
            Destroy(gameObject);
        }
    }*/


    }
}