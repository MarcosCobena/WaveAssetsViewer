using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace WPKViewerProject.Components
{
    class BorderDrawable2D : Drawable2D
    {
        private RectangleF rectangle;

        [RequiredComponent]
        public Transform2D transform2D;

        public BorderDrawable2D()
            : base(DefaultLayers.Alpha) // Don't know why but lines are on top of GUI :-(
        {
            this.rectangle = new RectangleF();
        }

        protected override void Dispose(bool disposing)
        {
        }

        protected override void DrawBasicUnit(int parameter)
        {
            var rectangle = this.transform2D.Rectangle;
            this.rectangle.X = this.transform2D.X - (this.transform2D.Origin.X * rectangle.Width) * 
                this.transform2D.XScale;
            this.rectangle.Y = this.transform2D.Y - (this.transform2D.Origin.Y * rectangle.Height) *
                this.transform2D.YScale;
            this.rectangle.Width = rectangle.Width * this.transform2D.XScale;
            this.rectangle.Height = rectangle.Height * this.transform2D.YScale;

            this.RenderManager.LineBatch2D.DrawRectangle(this.rectangle, Color.White);
        }
    }
}
