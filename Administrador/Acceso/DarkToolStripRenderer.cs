using System.Drawing;
using System.Windows.Forms;

namespace Acceso
{
    public class DarkToolStripRenderer : ToolStripProfessionalRenderer
    {
        private static readonly Color BgColor = Color.FromArgb(11, 17, 32);
        private static readonly Color HoverColor = Color.FromArgb(30, 40, 65);
        private static readonly Color BorderColor = Color.FromArgb(201, 168, 76);

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            using (var brush = new SolidBrush(BgColor))
                e.Graphics.FillRectangle(brush, e.AffectedBounds);
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected || e.Item.Pressed)
            {
                var rect = new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 1);
                using (var brush = new SolidBrush(HoverColor))
                    e.Graphics.FillRectangle(brush, rect);
                using (var pen = new Pen(BorderColor))
                    e.Graphics.DrawRectangle(pen, rect);
            }
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            // No border
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            // No separator lines
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = e.Item.ForeColor;
            base.OnRenderItemText(e);
        }
    }
}
