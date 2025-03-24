using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthManager : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]
    private SpriteRenderer _healthBar;
    [SerializeField] 
    private Color _fullHealthColor;
    [SerializeField]
    private Color _noHealthColor;


    [Header("Blood")]
    [SerializeField]
    private SpriteRenderer _bloodBar;

    [Header("BodyParts")]
    [SerializeField]
    private List<SpriteRenderer> _bodyParts = new List<SpriteRenderer>();

    public void UpdateHealth(Component sender, object obj)
    {
        HealthEventArgs args = obj as HealthEventArgs;
        if (args == null) return;

        if(sender.gameObject.GetComponent<PlayerController>() == null)
        {
            if (sender.gameObject != gameObject) return;
        }
        else
        {
            if (gameObject.GetComponent<AIController>() != null) return;
        }

        Vector2 barSize = new Vector2(args.CurrentHealth / args.MaxHealth, 1);
        _healthBar.size = barSize;
        _healthBar.gameObject.transform.localPosition = new Vector3(0 - ((1 - barSize.x) / 2), 0, 0);


        if (sender.gameObject.GetComponent<PlayerController>() == null) return;

        foreach (SpriteRenderer part in _bodyParts)
        {
            foreach(var partHealth in args.BodyPartsHealth)
            {
                if(part.name == partHealth.Key.ToString())
                {
                    Color newColor = Color.Lerp(_noHealthColor, _fullHealthColor, args.BodyPartsHealth[partHealth.Key] / args.MaxBodyPartsHealth[partHealth.Key]);
                    part.color = newColor;
                }
            }
        }
    }

    public void UpdateBlood(Component sender, object obj)
    {
        HealthEventArgs args = obj as HealthEventArgs;
        if (args == null) return;

        if (sender.gameObject.GetComponent<PlayerController>() == null)
        {
            if (sender.gameObject != gameObject) return;
        }
        else
        {
            if (gameObject.GetComponent<AIController>() != null) return;
        }
        Vector2 barSize = new Vector2(args.CurrentBlood / args.MaxBlood, 1);
        _bloodBar.size = barSize;
        _bloodBar.gameObject.transform.localPosition = new Vector3(0 - ((1 - barSize.x) / 2), 0, 0);
    }
}
