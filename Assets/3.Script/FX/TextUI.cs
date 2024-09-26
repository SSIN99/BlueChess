using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TextUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private Text amout;
    [SerializeField] private GameObject fx;
    [SerializeField] private RectTransform rect;
    private Vector3 anchor;
    private Vector2 offset = new Vector2(15f, 20f);
    private Color[] colors =
    {
        new Color(1, 0.84f, 0.15f),
        new Color(0.66f,0.89f,0.11f)
    };

    private void Awake()
    {
        anchor = rect.anchoredPosition;
    }

    public void InitText(float d, Vector2 pos, bool fx, TextType type)
    {
        amout.text = d.ToString();
        rect.anchoredPosition = anchor;
        rect.position = pos;

        if (fx)
        {
            this.fx.SetActive(true);
        }
        else
        {
            this.fx.SetActive(false);
        }
        amout.color = colors[(int)type];

        switch (type)
        {
            case TextType.Attack:
                break;
            case TextType.Heal:
                break;
        }

        rect.DOAnchorPos(rect.anchoredPosition + offset, 0.4f)
            .OnComplete(() =>
            {
                rect.DOAnchorPos(rect.anchoredPosition + Vector2.right * 5f, 0.4f);
                canvas.DOFade(0f, 0.4f)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
            });
    }
}
