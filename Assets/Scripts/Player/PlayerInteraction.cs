using UnityEngine;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour, IMaskHolder
{
    public MaskType CurrentMaskType;
    [SerializeField] private SpriteRenderer _renderer;
    
    [SerializeField] private Animator _animator;
    [SerializeField] private RuntimeAnimatorController blueController;
    [SerializeField] private RuntimeAnimatorController greenController;
    [SerializeField] private RuntimeAnimatorController redController;
    [SerializeField] private RuntimeAnimatorController yellowController;
    public List<PersonView> People = new List<PersonView>();

    [SerializeField] private Animator _maskUI;

    public MaskType MaskType => CurrentMaskType;

    public void CalculateEncounter(PersonView person)
    {
        if (person.IsMaskless || IsConversion(CurrentMaskType, person.CurrentMaskType))
        {
            AddFollower(person);
            person.SpriteRenderer.color = MaskColor.GetMaskColor(CurrentMaskType);
        }
        else
        {
            DestroyFollower();
            Destroy(person.gameObject);
        }
    }

    private void AddFollower(PersonView person)
    {
        People.Add(person);
    }

    private void DestroyFollower()
    {
        if (People.Count == 0)
        {
            LoseGame();
            return;
        }
        PersonView follower = People[People.Count - 1];
        People.RemoveAt(People.Count - 1);
        Destroy(follower.gameObject);
    }

    private void LoseGame()
    {
        FindAnyObjectByType<GameOverMenu>( FindObjectsInactive.Include).gameObject.SetActive(true);
        Destroy(gameObject);
        Debug.Log("Game Over");
    }

    private bool IsConversion(MaskType playerMask, MaskType enemyMask)
    {
        //return ((int)playerMask + 1) % 4 == (int)enemyMask;
        return playerMask == enemyMask;
    }

    public void ChangeMask()
    {
        CurrentMaskType = (MaskType)(((int)CurrentMaskType + 1) % 4);
        //_renderer.color = MaskColor.GetMaskColor(CurrentMaskType);
        UpdatePlayerAnimator();
        foreach (var person in People)
        {
            //person.CurrentMaskType = CurrentMaskType;
            person.GetComponent<SpriteRenderer>().color = MaskColor.GetMaskColor(CurrentMaskType);
        }
    }

    private void UpdatePlayerAnimator()
    {
        switch (CurrentMaskType)
        {
            case MaskType.Blue:
                _animator.runtimeAnimatorController = blueController;
                _maskUI.Play("blue");
                break;
            case MaskType.Green:
                _animator.runtimeAnimatorController = greenController;
                _maskUI.Play("green");
                break;
            case MaskType.Red:
                _animator.runtimeAnimatorController = redController;
                _maskUI.Play("red");
                break;
            case MaskType.Yellow:
                _animator.runtimeAnimatorController = yellowController;
                _maskUI.Play("yellow");
                break;
        }
    }
    
}


public enum MaskType
{
    Red,
    Green,
    Blue,
    Yellow,
}