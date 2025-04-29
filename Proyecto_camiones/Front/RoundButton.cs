using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AplicacionCamiones.Front;
public class RoundButton : Button
{
    //Constructor
    public RoundButton()
    {
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);

        GraphicsPath path = new GraphicsPath();

        path.AddArc(0, 0, 10, 10, 180, 90); // Esquina superior izquierda
        path.AddArc(this.Width - 10, 0, 10, 20, 270, 90); // Esquina superior derecha
        path.AddArc(this.Width - 10, this.Height - 10, 10, 10, 0, 90); // Esquina inferior derecha
        path.AddArc(0, this.Height - 10, 20, 10, 90, 90); // Esquina inferior izquierda

        path.CloseAllFigures();

        this.Region = new Region(path);
    }
}
