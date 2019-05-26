////*** Guy Ronen © 2008-2011 ***//
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ObjectModel.Animators;

namespace Infrastructure.ObjectModel
{
    public class Sprite : LoadableDrawableComponent
    {
        private Texture2D m_Texture;
        protected int m_Width;
        protected int m_Height;
        protected Vector2 m_Position = Vector2.Zero;
        protected Color m_TintColor = Color.White;
        protected Vector2 m_Velocity = Vector2.Zero;
        protected bool m_UseSharedBatch = true;
        protected SpriteBatch m_SpriteBatch;
        protected bool m_RestrictedToWindow = false;
        protected BlendState m_BlendState = BlendState.AlphaBlend;
        protected Color[] m_Pixels;
        protected CompositeAnimator m_Animations;
        protected float m_WidthBeforeScale;
        protected float m_HeightBeforeScale;
        protected Vector2 m_PositionOrigin;
        protected Vector2 m_RotationOrigin = Vector2.Zero;
        protected float m_Rotation = 0;
        protected Rectangle m_SourceRectangle;
        protected Vector2 m_Scales = Vector2.One;
        protected float m_LayerDepth;

        public bool IsActive { get; protected set; }

        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        public override void Initialize()
        {
            base.Initialize();
            IsActive = true;
            m_Animations = new CompositeAnimator(this);
        }

        protected void InitPixels()
        {
            m_Pixels = new Color[Texture.Width * Texture.Height];
            Texture.GetData(m_Pixels);
        }

        public Color[] Pixels
        {
            get
            {
                if(m_Pixels == null)
                {
                    InitPixels();
                }

                return m_Pixels;
            }

            set
            {
                m_Pixels = value;
            }
        }

        public CompositeAnimator Animations
        {
            get { return m_Animations; }
            set { m_Animations = value; }
        }

        public BlendState BlendState
        {
            get { return m_BlendState; }
            set
            {
                m_BlendState = value;
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)TopLeftPosition.X,
                    (int)TopLeftPosition.Y,
                    (int)this.SourceRectangle.Width,
                    (int)this.SourceRectangle.Height);
            }
        }

        public Rectangle BoundsBeforeScale
        {
            get
            {
                return new Rectangle(
                    (int)TopLeftPosition.X,
                    (int)TopLeftPosition.Y,
                    (int)this.WidthBeforeScale,
                    (int)this.HeightBeforeScale);
            }
        }

        public float Width
        {
            get { return m_WidthBeforeScale * m_Scales.X; }
            set { m_WidthBeforeScale = value / m_Scales.X; }
        }

        public float Height
        {
            get { return m_HeightBeforeScale * m_Scales.Y; }
            set { m_HeightBeforeScale = value / m_Scales.Y; }
        }

        public float WidthBeforeScale
        {
            get { return m_WidthBeforeScale; }
            set { m_WidthBeforeScale = value; }
        }

        public float HeightBeforeScale
        {
            get { return m_HeightBeforeScale; }
            set { m_HeightBeforeScale = value; }
        }

        public virtual Vector2 Position
        {
            get { return m_Position; }

            set
            {
                if (m_RestrictedToWindow)
                {
                    value.X = MathHelper.Clamp(value.X, 0, this.GraphicsDevice.Viewport.Width - Width);
                    value.Y = MathHelper.Clamp(value.Y, 0, this.GraphicsDevice.Viewport.Height - Height);
                }

                if (m_Position != value)
                {  
                    m_Position = value;
                    OnPositionChanged();
                }
            }
        }

        public Vector2 PositionOrigin
        {
            get { return m_PositionOrigin; }
            set { m_PositionOrigin = value; }
        }

        public Vector2 RotationOrigin
        {
            get { return m_RotationOrigin; }

            set { m_RotationOrigin = value; }
        }

        private Vector2 PositionForDraw
        {
            get { return this.Position - this.PositionOrigin + this.RotationOrigin; }
        }

        public Vector2 TopLeftPosition
        {
            get { return this.Position - this.PositionOrigin; }
            set { this.Position = value + this.PositionOrigin; }
        }

        public Rectangle SourceRectangle
        {
            get { return m_SourceRectangle; }
            set { m_SourceRectangle = value; }
        }

        public Vector2 TextureCenter
        {
            get
            {
                return new Vector2((float)(m_Texture.Width / 2), (float)(m_Texture.Height / 2));
            }
        }

        public Vector2 SourceRectangleCenter
        {
            get { return new Vector2((float)(m_SourceRectangle.Width / 2), (float)(m_SourceRectangle.Height / 2)); }
        }

        public float Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; }
        }

        public Vector2 Scales
        {
            get { return m_Scales; }
            set
            {
                if (m_Scales != value)
                {
                    m_Scales = value;
                    OnPositionChanged();
                }
            }
        }

        public Color TintColor
        {
            get { return m_TintColor; }
            set { m_TintColor = value; }
        }

        public float Opacity
        {
            get { return (float)m_TintColor.A / byte.MaxValue; }
            set { m_TintColor.A = (byte)(value * byte.MaxValue); }
        }

        public Vector2 Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; }
        }

        public Sprite(string i_AssetName, Game i_Game, int i_UpdateOrder, int i_DrawOrder)
            : base(i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        {
        }

        public Sprite(string i_AssetName, Game i_Game, int i_CallsOrder)
            : base(i_AssetName, i_Game, i_CallsOrder)
        {
        }

        public Sprite(string i_AssetName, Game i_Game)
            : base(i_AssetName, i_Game, int.MaxValue)
        {
        }

        public Sprite(string i_AssetName, Game i_Game, Vector2 i_Position)
           : base(i_AssetName, i_Game, int.MaxValue)
        {
            m_Position = i_Position;
        }

        public Sprite(string i_AssetName, Game i_Game, Color i_Color)
   : base(i_AssetName, i_Game, int.MaxValue)
        {
            m_TintColor = i_Color;
        }

        protected override void InitBounds()
        {
            m_WidthBeforeScale = m_Texture.Width;
            m_HeightBeforeScale = m_Texture.Height;
            InitSourceRectangle();
            InitOrigins();
        }

        protected virtual void InitOrigins()
        {
        }

        protected virtual void InitSourceRectangle()
        {
            m_SourceRectangle = new Rectangle(0, 0, (int)m_WidthBeforeScale, (int)m_HeightBeforeScale);
        }

        public SpriteBatch SpriteBatch
        {
            get
            {
                return m_SpriteBatch;
            }

            set
            {
                m_SpriteBatch = value;
                m_UseSharedBatch = true;
            }
        }

        protected override void LoadContent()
        {
            m_Texture = Game.Content.Load<Texture2D>(m_AssetName);

            base.LoadContent();
        }   

        private void loadSpriteBatch()
        {
            if (m_SpriteBatch == null)
            {
                m_SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

                if (m_SpriteBatch == null)
                {
                    m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
                    m_UseSharedBatch = false;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);

            this.Animations.Update(gameTime);
        }

        public void UseNonPremultipliedSpriteBatch()
        {
            m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            m_BlendState = BlendState.NonPremultiplied;
            m_UseSharedBatch = false;
        }

        public override void Draw(GameTime gameTime)
        {
            if(m_SpriteBatch == null)
            {
                loadSpriteBatch();
            }

            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.Begin(SpriteSortMode.Deferred, m_BlendState);
            }

            m_SpriteBatch.Draw(m_Texture, this.PositionForDraw, this.SourceRectangle, this.TintColor, this.Rotation, this.RotationOrigin, this.Scales, SpriteEffects.None, this.LayerDepth);
            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.End();
            }

            base.Draw(gameTime);
        }

        public float LayerDepth
        {
            get { return m_LayerDepth; }
            set { m_LayerDepth = value; }
        }

        public virtual bool CheckCollision(ICollidable i_Source)
        {
            bool collided = false;
            ICollidable2D source = i_Source as ICollidable2D;
            if (source != null)
            {
                collided = (source.Bounds.Intersects(this.Bounds) || source.Bounds.Contains(this.Bounds)) && checkPixleCollision(source);
            }

            return collided;
        }

        private bool checkPixleCollision(ICollidable2D i_Source)
        {
            bool collided = false;
            Rectangle rectangle = findIntersectingRectangle(i_Source.Bounds);
            for (int i = rectangle.Left; (i < rectangle.Right) && (!collided); i++)
            {
                for (int j = rectangle.Top; (j < rectangle.Bottom) && (!collided); j++) 
                {
                    Color sourceColor = i_Source.Pixels[(i - i_Source.Bounds.Left) + ((j - i_Source.Bounds.Top) * i_Source.Bounds.Width)];
                    Color targetColor = Pixels[(i - Bounds.Left) + ((j - Bounds.Top) * Bounds.Width)];
                 
                    if (sourceColor.A > 0 && targetColor.A > 0)
                    {
                        collided = true;
                    }
                }
            }

            return collided;
        }

        private Rectangle findIntersectingRectangle(Rectangle i_SourceBounds)
        {
            int left = Math.Max(Bounds.Left, i_SourceBounds.Left);
            int top = Math.Max(Bounds.Top, i_SourceBounds.Top);
            int width = Math.Min(Bounds.Right, i_SourceBounds.Right) - left;
            int height = Math.Min(Bounds.Bottom, i_SourceBounds.Bottom) - top;
            return new Rectangle(left, top, width, height);
        }

        protected virtual bool deleteSharedPixels(Rectangle i_SourceBounds, Color[] i_SourcePixels)
        {
            Rectangle rectangle = findIntersectingRectangle(i_SourceBounds);
            bool changed = false;
            for (int i = rectangle.Left; i < rectangle.Right; i++)
            {
                for (int j = rectangle.Top; j < rectangle.Bottom; j++)
                {
                    Color sourceColor = i_SourcePixels[(i - i_SourceBounds.Left) + ((j - i_SourceBounds.Top) * i_SourceBounds.Width)];
                    Color targetColor = Pixels[(i - Bounds.Left) + ((j - Bounds.Top) * Bounds.Width)];
                    if (sourceColor.A > 0 && targetColor.A > 0)
                    {
                        Pixels[(i - Bounds.Left) + ((j - Bounds.Top) * Bounds.Width)] = Color.Transparent;
                        changed = true;
                    }
                }
            }

            if (changed)
            {
                Texture.SetData(Pixels);
            }

            return changed;
        }

        public virtual void Collided(ICollidable i_Collidable)
        {
            // defualt behavior
        }

        public Sprite ShallowClone()
        {
            return this.MemberwiseClone() as Sprite;
        }
    }
}