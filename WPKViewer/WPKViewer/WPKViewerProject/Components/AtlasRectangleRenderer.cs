#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics; 
#endregion

namespace WaveAssetsViewerProject.Components
{
    public class AtlasRectangleRenderer : Drawable
    {
        [RequiredComponent]
        private Transform2D transform2D;

        IEnumerable<Rectangle> rectangles;

        public AtlasRectangleRenderer(IEnumerable<Rectangle> rectangles)
        {
            this.rectangles = rectangles;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose();
        }

        public override void Draw(TimeSpan gameTime)
        {
            foreach (var textureRectangle in rectangles)
            {
                var rectangle = textureRectangle;
                rectangle.X = this.transform2D.X - (this.transform2D.Origin.X * rectangle.Width) *
                    this.transform2D.XScale;
                this.rectangle.Y = this.transform2D.Y - (this.transform2D.Origin.Y * rectangle.Height) *
                    this.transform2D.YScale;
                this.rectangle.Width = rectangle.Width * this.transform2D.XScale;
                this.rectangle.Height = rectangle.Height * this.transform2D.YScale;

                this.RenderManager.LineBatch2D.DrawRectangle(this.rectangle, Color.White);

                this.entity.AddChild(new Entity(spriteRectangle.Key)
                        .AddComponent(new Transform2D()
                        {
                            X = spriteRectangle.Value.X - parentOffset.X,
                            Y = spriteRectangle.Value.Y - parentOffset.Y,
                            Rectangle = new RectangleF(
                                0,
                                0,
                                spriteRectangle.Value.Width,
                                spriteRectangle.Value.Height)
                        })
                        .AddComponent(new ChildScaleBehavior())
                        .AddComponent(new BorderDrawable2D()));
            }
        }

        protected override void DrawBasicUnit(int parameter)
        {
        }

        protected override void DrawDebugLines()
        {
        }
    }
}
