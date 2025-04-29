using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AplicacionCamiones.Front;

public class NewRoundPanel : Panel
{
    private int cornerRadius;

    //Constructor
    public NewRoundPanel(int cornerRadius)
    {
        this.cornerRadius = cornerRadius;
        this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
    }

    protected override void OnResize(System.EventArgs e)
    {
        base.OnResize(e);
        SetRoundedRegion(cornerRadius);
    }


    //Functions
    private void SetRoundedRegion(int cornerRadius)
    {
        GraphicsPath path = new GraphicsPath();
        int arcWidth = cornerRadius * 2;
        int arcHeight = cornerRadius * 2;

        path.StartFigure();
        path.AddArc(0, 0, arcWidth, arcHeight, 180, 90); // Superior izquierda
        path.AddArc(this.Width - arcWidth - 1, 0, arcWidth, arcHeight, 270, 90); // Superior derecha
        path.AddArc(this.Width - arcWidth - 1, this.Height - arcHeight - 1, arcWidth, arcHeight, 0, 90); // Inferior derecha
        path.AddArc(0, this.Height - arcHeight - 1, arcWidth, arcHeight, 90, 90); // Inferior izquierda
        path.CloseFigure();

        this.Region = new Region(path);
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using (Brush brush = new SolidBrush(this.BackColor))
        {
            e.Graphics.FillRectangle(brush, this.ClientRectangle);
        }
    }
}
