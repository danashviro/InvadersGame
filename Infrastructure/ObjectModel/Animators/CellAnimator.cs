////Guy Ronen © 2008-2011
using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators
{
    public class CellAnimator : SpriteAnimator
    {
        public TimeSpan CellTime { get; set; }

        private readonly int r_NumOfCells = 1;
        private readonly int[] r_CellsIndexes;
        private TimeSpan m_TimeLeftForCell;
        private bool m_Loop = true;
        private int m_CurrIdx = 0;
  
        public CellAnimator(TimeSpan i_CellTime, TimeSpan i_AnimationLength, int[] i_CellsIndexes)
            : base("CelAnimation", i_AnimationLength)
        {
            CellTime = i_CellTime;
            m_TimeLeftForCell = i_CellTime;
            r_CellsIndexes = i_CellsIndexes;
            r_NumOfCells = i_CellsIndexes.Length;
            m_Loop = i_AnimationLength == TimeSpan.Zero;
        }

        private void goToNextFrame()
        {
            m_CurrIdx++;
            if (m_CurrIdx >= r_NumOfCells)
            {
                if (m_Loop)
                {
                    m_CurrIdx = 0;
                }
                else
                {
                    m_CurrIdx = r_NumOfCells - 1; 
                    this.IsFinished = true;
                }
            }
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.SourceRectangle = m_OriginalSpriteInfo.SourceRectangle;
            m_TimeLeftForCell = CellTime;
            m_CurrIdx = 0;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            if (CellTime != TimeSpan.Zero)
            {
                m_TimeLeftForCell -= i_GameTime.ElapsedGameTime;
                if (m_TimeLeftForCell.TotalSeconds <= 0)
                {
                    goToNextFrame();
                    m_TimeLeftForCell = CellTime;
                }
            }

            this.BoundSprite.SourceRectangle = new Rectangle(
                r_CellsIndexes[m_CurrIdx] * this.BoundSprite.SourceRectangle.Width,
                this.BoundSprite.SourceRectangle.Top,
                this.BoundSprite.SourceRectangle.Width,
                this.BoundSprite.SourceRectangle.Height);
        }
    }
}
