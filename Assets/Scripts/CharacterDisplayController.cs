using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterDisplayController : MonoBehaviour
{
    public GameObject nameTextObject;
    public GameObject portraitObject;
    public GameObject healthStatTextObject;
    public GameObject attackStatTextObject;

    private string _charNameText;
    private Texture _charImage;
    private int _health;
    private int _attack;

    private Renderer _charImgRenderer;
    private TextMeshPro _nameTextMesh;
    private TextMeshPro _healthTextMesh;
    private TextMeshPro _attackTextMesh;

    // Start is called before the first frame update
    void Start()
    {
        _charImgRenderer = portraitObject.GetComponent<Renderer>();
        _nameTextMesh = nameTextObject.GetComponent<TextMeshPro>();

        _healthTextMesh = healthStatTextObject.GetComponent<TextMeshPro>();
        _attackTextMesh = attackStatTextObject.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayCharacter(ScriptableCharacterBase cb)
    {
        CharName = cb.CharName;
        CharacterImage = cb.CardTexture;

        Attack = cb.Attack;
        Health = cb.Health;
    }

    public string CharName {
        get => _charNameText;
        set
        {
            _charNameText = value;
            _nameTextMesh.text = _charNameText;
        } 
    }

    public Texture CharacterImage
    {
        get => _charImage;
        set
        {
            _charImage = value;
            _charImgRenderer.material.mainTexture = _charImage;
        }
    }

    public int Health
    {
        get => _health;
        set
        {
            _health = value;
            _healthTextMesh.text = _health.ToString();
        }
    }

    public int Attack
    {
        get => _attack;
        set
        {
            _attack = value;
            _attackTextMesh.text = _attack.ToString();
        }
    }
}
