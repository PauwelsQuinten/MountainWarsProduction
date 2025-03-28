using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

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
    [SerializeField]
    private SpriteRenderer _patchUpBar;
 
    [Header("BodyParts")]
    [SerializeField]
    private List<SpriteRenderer> _bodyParts = new List<SpriteRenderer>();

    [Header("Stamina")]
    [SerializeField]
    private SpriteRenderer _staminaBar;

    private Coroutine _patchUp;
    private bool _completedPatchUp;

    
    [Header("Equipment")]
    [SerializeField]
    private SpriteRenderer _shield;
    [SerializeField]
    private SpriteRenderer _weapon;
   
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

        UpdateBodyPartColor(sender, args);
    }

    private void UpdateBodyPartColor(Component sender, HealthEventArgs args)
    {
        BodyParts? partToRemove = null;
        if (sender.gameObject.GetComponent<PlayerController>() == null) return;

        foreach (SpriteRenderer part in _bodyParts)
        {
            foreach (var damagedPart in args.DamagedBodyParts)
            {
                if (part.name == damagedPart.ToString())
                {
                    float progress = args.BodyPartsHealth[damagedPart] / args.MaxBodyPartsHealth[damagedPart];
                    Color newColor = Color.Lerp(_noHealthColor, _fullHealthColor, progress);

                    if (args.BodyPartsHealth[damagedPart] >= args.MaxBodyPartsHealth[damagedPart])
                    {
                        partToRemove = damagedPart;
                    }

                    part.color = newColor;
                }
            }
        }

        if(partToRemove != null) 
            args.DamagedBodyParts.Remove(partToRemove.GetValueOrDefault());
    }

    public void UpdateBlood(Component sender, object obj)
    {
        BloodEventArgs args = obj as BloodEventArgs;
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

    public void UpdatePatchUp(Component sender, object obj)
    {
        if (_completedPatchUp)
        {
            _completedPatchUp = false;
            return;
        }
        if(_patchUp == null) 
            _patchUp = StartCoroutine(PathUpBar());

        bool? canReset = obj as bool?;
        if((bool)canReset)
        {
            StopCoroutine(_patchUp);

            _patchUpBar.transform.parent.gameObject.SetActive(false);
            Vector2 size = new Vector2(0, 1);
            _patchUpBar.size = size;
            _patchUpBar.gameObject.transform.localPosition = new Vector3(0 - ((1 - size.x) / 2), 0, 0);

            _patchUp = null;
        }
    } 

    public void UpdateStamina(Component sender, object obj)
    {
        StaminaEventArgs args = obj as StaminaEventArgs;
        if (args == null) return;

        if (sender.gameObject.GetComponent<PlayerController>() == null)
        {
            if (sender.gameObject != gameObject) return;
        }
        else
        {
            if (gameObject.GetComponent<AIController>() != null) return;
        }

        Vector2 barSize = new Vector2(args.CurrentStamina / args.MaxStamina, 1);
        _staminaBar.size = barSize;
        _staminaBar.gameObject.transform.localPosition = new Vector3(0 - ((1 - barSize.x) / 2), 0, 0);
    }

    public void UpdateEquipment(Component sender, object obj)
    {
        EquipmentEventArgs args = obj as EquipmentEventArgs;
        if (args == null) return;

        if (sender.gameObject.GetComponent<PlayerController>() == null)
        {
            if (sender.gameObject != gameObject) return;
        }
        else
        {
            if (gameObject.GetComponent<AIController>() != null) return;
        }


        float progress = args.ShieldDurability;
        Color newColor = Color.Lerp(_noHealthColor, _fullHealthColor, progress);
        if (progress <= 0f)
        {
            _shield.enabled = false;
        }
        else
            _shield.color = newColor;
        
        progress = args.WeaponDurability;
        newColor = Color.Lerp(_noHealthColor, _fullHealthColor, progress);
        if (progress <= 0f)
        {
            _weapon.enabled = false;
        }
        else
            _weapon.color = newColor;


    }


    private IEnumerator PathUpBar()
    {
        float time = 0;
        Vector2 size = Vector2.zero;
        _patchUpBar.transform.parent.gameObject.SetActive(true);
        while(_patchUpBar.size.x < 1)
        {
            time += Time.deltaTime;
            size = new Vector2(time, 1);
            _patchUpBar.size = size;
            _patchUpBar.gameObject.transform.localPosition = new Vector3(0 - ((1 - size.x) / 2), 0, 0);
            yield return null;
        }

        _patchUpBar.transform.parent.gameObject.SetActive(false);
        size = new Vector2(0, 1);
        _patchUpBar.size = size;
        _patchUpBar.gameObject.transform.localPosition = new Vector3(0 - ((1 - size.x) / 2), 0, 0);
        _completedPatchUp = true;
        _patchUp = null;
    }
}
