using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMove : MonoBehaviour
{
    private PlayerData _playerData;

    private LineRenderer _pathLine;
    void Start()
    {
        this._playerData = GetComponent<PlayerData>();
        this._pathLine = new GameObject("PathLine").AddComponent<LineRenderer>();
        
    }

    void Update()
    {
        MovableCharacter character;
        _playerData.selectedCharacter.TryGetComponent<MovableCharacter>(out character);
        if (character) {
            Vector3? mouseTargetPosition = Mouse.GetTargetPoint(this._playerData.selectedCamera);
            if (mouseTargetPosition != null) {
                if (Input.GetMouseButton(0)) {
                    character.Move((Vector3)mouseTargetPosition);
                }

                DrawPath(character, (Vector3)mouseTargetPosition);
            }
        }

    }

    private void DrawPath(MovableCharacter character, Vector3 mouseTargetPosition)
    {
        List<Vector3> path = character.getPathTo(mouseTargetPosition);
        if (path != null && path.Count > 0) {
            path.Insert(0, character.transform.position);
            this._pathLine.positionCount = path.Count;
            this._pathLine.SetPositions(path.ToArray());
            this._pathLine.enabled = true;
        } else {
            this._pathLine.enabled = false;
        }
    }
}
