//// Guy Ronen © 2008-2011 
using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators
{
    public class BlinkAnimator : SpriteAnimator
    {
        private TimeSpan m_BlinkLength;
        private TimeSpan m_TimeLeftForNextBlink;

        public TimeSpan BlinkLength
        {
            get { return m_BlinkLength; }
            set { m_BlinkLength = value; }
        }

        public BlinkAnimator(string i_Name, float i_BlinksPerSecond, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            m_BlinkLength = TimeSpan.FromSeconds(1 / (i_BlinksPerSecond * 2));
            this.m_TimeLeftForNextBlink = m_BlinkLength;
        }

        public BlinkAnimator(float i_BlinksPerSecond, TimeSpan i_AnimationLength)
            : this("Blink", i_BlinksPerSecond, i_AnimationLength)
        {
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            m_TimeLeftForNextBlink -= i_GameTime.ElapsedGameTime;
            if (m_TimeLeftForNextBlink.TotalSeconds < 0)
            {
                this.BoundSprite.Visible = !this.BoundSprite.Visible;
                m_TimeLeftForNextBlink = m_BlinkLength;
            }
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Visible = m_OriginalSpriteInfo.Visible;
        }
    }
}
