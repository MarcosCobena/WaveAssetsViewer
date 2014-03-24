#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
#endregion

namespace WaveAssetsViewerProject.Components
{
    public class SpritesheetRectanglesRenderer : Drawable2D
    {
        [RequiredComponent]
        private Transform2D transform2D;

        public IEnumerable<Rectangle> Rectangles;

        protected override void Dispose(bool disposing)
        {
        }

        public override void Draw(TimeSpan gameTime)
        {
            if (this.Rectangles != null)
            {
                var entityOffset = new Vector2(this.transform2D.Rectangle.Width, this.transform2D.Rectangle.Height) * this.transform2D.Origin;

                foreach (var spriteRectangle in this.Rectangles)
                {
                    var rectangle = new RectangleF(
                                (spriteRectangle.X - entityOffset.X) * this.transform2D.XScale,
                                (spriteRectangle.Y - entityOffset.Y) * this.transform2D.YScale,
                                spriteRectangle.Width * this.transform2D.XScale,
                                spriteRectangle.Height * this.transform2D.YScale);

                    rectangle.Offset(this.transform2D.X, this.transform2D.Y);

                    this.RenderManager.LineBatch2D.DrawRectangle(rectangle, Color.White);
                }
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
