using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject ModelObject;
    public GameObject ViewObjet;
    public GameObject DefaultDisplayPoint;

    private Database db;
    private CharacterDisplayController _defaultDispController;

    // Start is called before the first frame update
    void Start()
    {
        db = ModelObject.GetComponent<Database>();
        _defaultDispController = DefaultDisplayPoint.GetComponent<CharacterDisplayController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenRandomCharacter()
    {
        var chosen = db.OpenPackCharacter();
        _defaultDispController.DisplayCharacter(chosen);
    }
}
