using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer spriteRenderer;

    private Dictionary<States, string> states = null;

    private bool locked = false;
    public States currentState = States.None;

    public enum States
    {
        None, Idle, Run, CrouchRun, RunUp,
        Death, Crouch, LookUp,
        Respawn, WallSlide, WallJump, Jump, 
        SporeUp, SporeDown, SporeSide, 
        RiseUp, RiseDown, RiseSide,
        FallUp, FallDown, FallSide,
        NeutralUp, NeutralDown, NeutralSide,
        HamRiseUp, HamRiseDown, HamRiseSide,
        HamFallUp, HamFallDown, HamFallSide,
        HamStandUp, HamStandSide,
        HamRunUp, HamRunSide,
        HamWallSide
    }

    void Start()
    {
        states = new Dictionary<States, string>();
        states.Add(States.None, "");
        states.Add(States.Idle, "idle");
        states.Add(States.Run, "run");
        states.Add(States.Death, "death");
        states.Add(States.CrouchRun, "crouch-run");
        states.Add(States.RunUp, "look-up-run");
        states.Add(States.Crouch, "crouch");
        states.Add(States.LookUp, "look-up");
        states.Add(States.Respawn, "respawn");
        states.Add(States.WallSlide, "wall-slide");
        states.Add(States.WallJump, "wall-jump");
        states.Add(States.Jump, "jump");
        states.Add(States.SporeUp, "spore-up");
        states.Add(States.SporeDown, "spore-down");
        states.Add(States.SporeSide, "spore-side");
        states.Add(States.RiseUp, "rising-up");
        states.Add(States.RiseDown, "rising-down");
        states.Add(States.RiseSide, "rising-side");
        states.Add(States.FallUp, "falling-up");
        states.Add(States.FallDown, "falling-down");
        states.Add(States.FallSide, "falling-side");
        states.Add(States.NeutralUp, "neutral-up");
        states.Add(States.NeutralDown, "neutral-down");
        states.Add(States.NeutralSide, "neutral-side");
        states.Add(States.HamRiseUp, "hammer-rising-up");
        states.Add(States.HamRiseDown, "hammer-rising-down");
        states.Add(States.HamRiseSide, "hammer-rising-side");
        states.Add(States.HamFallUp, "hammer-falling-up");
        states.Add(States.HamFallDown, "hammer-falling-down");
        states.Add(States.HamFallSide, "hammer-falling-side");
        states.Add(States.HamStandUp, "hammer-standing-up");
        states.Add(States.HamStandSide, "hammer-standing-side");
        states.Add(States.HamRunUp, "hammer-running-up");
        states.Add(States.HamRunSide, "hammer-running-side");
        states.Add(States.HamWallSide, "hammer-sliding");
    }

    public void SetState(States state, bool interupt = false)
    {
        if((!locked || interupt) && state != currentState)
        {
            if(interupt)
                locked = false;
            anim.Play(states[state]);
            currentState = state;
        }
    }

    public void SetLock(bool locked)
    {
        this.locked = locked;
    }

    public bool Locked()
    {
        return locked;
    }

    public void flip(bool x)
    {
        if(!locked)
            spriteRenderer.flipX = x;
    }
}
