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
    public string overrideIdleName = "";

    public AnimationData currentAnimation;
    private int currentFrame = -1;
    private new SpriteRenderer renderer;
    private bool isLooping = false;

    private void Start()
    {
        if (overrideIdleName == "") overrideIdleName = "__idle";
        renderer = GetComponent<SpriteRenderer>();
        AnimationData idle = new AnimationData
        {
            name = "__idle",
            frames = idleAnimationFrames
        };
        animations.Add(idle);
        StartAnimation(overrideIdleName);
    }

    public void StartAnimation(string name, bool inf = true)
    {
        isLooping = inf;
        CancelInvoke(nameof(NextFrame));
        currentFrame = -1;
        currentAnimation = animations.Find((anim) => anim.name == name);
        InvokeRepeating(nameof(NextFrame), 0f, 1f / FPS);
    }

    private void NextFrame()
    {
        currentFrame++;
        if (currentFrame == currentAnimation.frames.Count)
        {
            if (isLooping)
            {
                currentFrame = 0;
            } else
            {
                StartAnimation(overrideIdleName);
                return;
            }
        }
        renderer.sprite = currentAnimation.frames[currentFrame];
    }
}
