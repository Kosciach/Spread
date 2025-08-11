using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Spread.Player.Animating
{
    public enum AnimatorLayer
    {
        InAir, Crawl, Slide
    }
    
    public enum AnimatorIkRig
    {
        Ladder, LadderSlide
    }

    [System.Serializable]
    public class AnimatorLayerData
    {
        [SerializeField] private int _index;
        public int Index => _index;
        
        public Tween Tween;
    }
    
    [System.Serializable]
    public class AnimatorIkRigData
    {
        [SerializeField] private Rig _rig;
        public Rig Rig => _rig;
        
        public Tween Tween;
    }
}