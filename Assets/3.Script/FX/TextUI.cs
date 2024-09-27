using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TextUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private Text amout;
    [SerializeField] private Image sfx;
    [SerializeField] private Sprite[] sfxImage;
    [SerializeField] private RectTransform rect;
    private Vector3 anchor;
    private Vector2 offset = new Vector2(15f, 20f);
    private Color[] colors =
    {
        new Color(1, 0.84f, 0.15f),
        new Color(1, 0.84f, 0.15f),
        new Color(0.66f,0.89f,0.11f),
        Color.gray
    };

    private void Awake()
    {
        anchor = rect.anchoredPosition;
    }

    public void InitText(string value, Vector2 pos, TextType type)
    {
        amout.text = value;
        rect.anchoredPosition = anchor;
        rect.position = pos;

        switch (type)
        {
            case TextType.Crit:
                sfx.sprite = sfxImage[1];
                break;
            case TextType.Avoid:
                sfx.sprite = sfxImage[2];
                break;
            case TextType.Attack:
            case TextType.Heal:
                sfx.sprite = sfxImage[0];
                break;
        }
        amout.color = colors[(int)type];
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
