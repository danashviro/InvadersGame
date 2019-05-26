using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.ObjectModel
{
    public class LineOfSprites : CompositeDrawableComponent<Sprite>
    {
        private int m_NumOfSprites;
        private Sprite[] m_SpritesArray;

        public LineOfSprites(Game i_Game, string i_AssetName, int i_InitialNumOfSprites, Color i_Color, Vector2 i_Scale) : base(i_Game)
        {
            m_NumOfSprites = i_InitialNumOfSprites;
            m_SpritesArray = new Sprite[m_NumOfSprites];
            for (int i = 0; i < m_NumOfSprites; i++)
            {
                m_SpritesArray[i] = new Sprite(i_AssetName, i_Game, i_Color);
                m_SpritesArray[i].Scales = i_Scale;
                this.Add(m_SpritesArray[i]);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            for (int i = 0; i < m_NumOfSprites; i++)
            {
                m_SpritesArray[i].UseNonPremultipliedSpriteBatch();
            }
        }

        public void InitPositions(Vector2 i_Position)
        {          
            for (int i = 0; i < m_NumOfSprites; i++)
            {
                m_SpritesArray[i].Position = new Vector2(i_Position.X + (i * m_SpritesArray[0].Width), i_Position.Y);
            }
        }

        public void ChangeNumOfSprites(int i_NewNum)
        {
            if (i_NewNum >= 0)
            {
                while (m_NumOfSprites > i_NewNum)
                {
                    m_NumOfSprites--;
                    this.Remove(m_SpritesArray[m_NumOfSprites]);
                }
            }        
        }
    }
}
