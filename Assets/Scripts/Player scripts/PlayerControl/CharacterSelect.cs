using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public ParticleSystem Highlighter;
    public Color hoveredColor = Color.white;
    public Color selectedColor = Color.green;

    private ParticleSystem _hoveredHighlighter;
    private ParticleSystem _slectedHighlighter;
    private PlayerData _playerData;
    private ControlledСharacter _hoveredCharacter;

    void Start()
    {
        this._playerData = GetComponent<PlayerData>();   
        this._hoveredHighlighter = Instantiate(this.Highlighter);
        this._hoveredHighlighter.startColor = this.hoveredColor;
        this._slectedHighlighter = Instantiate(this.Highlighter);
        this._slectedHighlighter.startColor = this.selectedColor;

    }

    void Update()
    {
        this.HighlighteSelected();
        this.HighlighteHovered();
    }

    private void FixedUpdate() 
    {
        GameObject target = Mouse.GetTargetObject(this._playerData.selectedCamera);
        if (target != null) {
            ControlledСharacter character;
            target.TryGetComponent<ControlledСharacter>(out character);
            this._hoveredCharacter = character;
            
            if (character != null && Input.GetMouseButtonUp(0))
            {
                _playerData.selectedCharacter = character;
            }
        }
    }

    private void HighlighteHovered()
    {
        if (this._hoveredCharacter != null && this._hoveredCharacter != _playerData.selectedCharacter) {
            this._hoveredHighlighter.transform.position = this._hoveredCharacter.transform.position;
        } else {
            this._hoveredHighlighter.transform.position = new Vector3(0, -100, 0);
        }
    }

    private void HighlighteSelected()
    {
        if (_playerData.selectedCharacter != null) {
            this._slectedHighlighter.transform.position = _playerData.selectedCharacter.transform.position;
        } else {
            this._slectedHighlighter.transform.position = new Vector3(0, -100, 0);
        }
    }
}
