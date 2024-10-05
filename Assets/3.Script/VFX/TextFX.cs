using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public enum TextType
{
    Attack,
    Crit,
    Heal,
    Avoid,
    Skill
}
public class TextFX : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private Text value;
    [SerializeField] private Image vfx;
    [SerializeField] private Sprite[] vfxSprite;
    [SerializeField] private RectTransform rect;
    private Vector3 anchor;
    private Vector2 offset = new Vector2(10f, 15f);
    private Color[] colors =
    {
        new Color(1, 0.84f, 0.15f),
        new Color(1, 0.84f, 0.15f),
        new Color(0.66f,0.89f,0.11f),
        Color.gray,
        new Color(0, 0.8f, 1f)
    };

    private void Start()
    {
        anchor = rect.anchoredPosition;
    }

    public void InitText(string value, Vector2 target, TextType type)
    {
        switch (type)
        {
            case TextType.Crit:
                vfx.sprite = vfxSprite[1];
                break;
            case TextType.Avoid:
                vfx.sprite = vfxSprite[2];
                break;
            case TextType.Attack:
            case TextType.Heal:
            case TextType.Skill:
                vfx.sprite = vfxSprite[0];
                break;
        }
        this.value.color = colors[(int)type];
        this.value.text = value;

        rect.anchoredPosition = anchor;
        rect.position = target;
        rect.DOAnchorPos(rect.anchoredPosition + offset, 0.4f)
            .OnComplete(() =>
            {
                rect.DOAnchorPos(rect.anchoredPosition + Vector2.right * 5f, 0.4f);
                canvas.DOFade(0f, 0.4f)
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                });
            });
    }
}
