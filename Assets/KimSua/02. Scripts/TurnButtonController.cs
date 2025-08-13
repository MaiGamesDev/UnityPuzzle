using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class TurnButtonController : MonoBehaviour
{
    private PuzzleExample puzzleEx;
    public GameObject selectedTile;
    public AudioClip audioPuzzle;

    private Vector3 defaultScale = Vector3.one;

    bool isRotating = false;

    private void Awake()
    {
        puzzleEx = FindFirstObjectByType<PuzzleExample>();
    }

    public void SelectTile(int index)
    {
        if (index < 0 || puzzleEx.tiles == null)
            return;

        // 이전 선택 퍼즐은 원래 크기로
        foreach (GameObject tile in puzzleEx.tiles)
        {
            tile.transform.localScale = defaultScale;
        }

        selectedTile = puzzleEx.tiles[index];

        // 선택 퍼즐 확대
        selectedTile.transform.localScale = defaultScale * 1.1f;

        // 퍼즐 사운드 재생
        SoundManager.Instance.PlaySound(audioPuzzle);
    }

    public void RotateLeft()
    {
        if (selectedTile == null || isRotating)
            return;

        isRotating = true;
        selectedTile.transform
            .DORotate(selectedTile.transform.eulerAngles + new Vector3(0, 0, 90f), 0.1f)// DOTween기능을 이용한 회전 애니메이션 구현
            .SetEase(Ease.OutCubic).OnComplete(() => isRotating = false); // 회전하고 있는 상태일 때는 다시 회전 하지 못하게 막아두었음
                                                                          // (float타입 부동 소수점 연산에 따라 연속으로 회전하게 되면 회전값이 이상해 질수 있음)
    }

    public void RotateRight()
    {
        if (selectedTile == null || isRotating)
            return;

        isRotating = true;
        selectedTile.transform
            .DORotate(selectedTile.transform.eulerAngles + new Vector3(0, 0, -90f), 0.1f) // DOTween기능을 이용한 회전 애니메이션 구현
            .SetEase(Ease.OutCubic).OnComplete(() => isRotating = false);// 회전하고 있는 상태일 때는 다시 회전 하지 못하게 막아두었음
                                                                         // (float타입 부동 소수점 연산에 따라 연속으로 회전하게 되면 회전값이 이상해 질수 있음)
    }
}