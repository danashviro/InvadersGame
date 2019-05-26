using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators
{
    public class FadeAnimator : SpriteAnimator
    {
        private float m_FadeVelocity;
        private float m_CurrentOpacity;

        public FadeAnimator(string i_Name, TimeSpan i_AnimationLength) : base(i_Name, i_AnimationLength)
        {
        }
      
        protected override void DoFrame(GameTime i_GameTime)
        {
            m_CurrentOpacity -= (float)(m_FadeVelocity * i_GameTime.ElapsedGameTime.TotalSeconds);
            BoundSprite.Opacity = m_CurrentOpacity;
        }

        protected override void RevertToOriginal()
        {
            BoundSprite.Opacity = m_OriginalSpriteInfo.Opacity;
            m_CurrentOpacity = BoundSprite.Opacity;
        }

        public override void Initialize()
        {
            base.Initialize();
            m_CurrentOpacity = BoundSprite.Opacity;
            m_FadeVelocity = m_CurrentOpacity / (float)AnimationLength.TotalSeconds;
        }
    }
}
