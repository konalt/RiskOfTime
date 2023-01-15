using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationComponent : MonoBehaviour
{
    [System.Serializable]
    public struct AnimationData { public string name; public List<Sprite> frames; }

    public int FPS = 24;
    public List<Sprite> idleAnimationFrames;
    public List<AnimationData> animations;

    private AnimationData currentAnimation;
    private int currentFrame = 0;
    private new SpriteRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        AnimationData idle = new AnimationData
        {
            name = "__idle",
            frames = idleAnimationFrames
        };
        animations.Add(idle);
        StartAnimation("__idle");
    }

    public void StartAnimation(string name)
    {
        CancelInvoke(nameof(NextFrame));
        currentFrame = 0;
        currentAnimation = animations.Find((anim) => anim.name == name);
        InvokeRepeating(nameof(NextFrame), 0f, 1f / FPS);
    }

    private void NextFrame()
    {
        currentFrame++;
        if (currentFrame == currentAnimation.frames.Count)
        {
            currentFrame = 0;
        }
        renderer.sprite = currentAnimation.frames[currentFrame];
    }
}
