namespace EzEvade_Port.Draw
{
    using System;
    using System.Drawing;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Utils;

    class RenderLine : RenderObject
    {
        public Color Color = Color.White;
        public Vector2 End;
        public Vector2 Start;

        public int Width;

        public RenderLine(Vector2 start, Vector2 end, float renderTime, int radius = 65, int width = 3)
        {
            StartTime = Environment.TickCount;
            EndTime = StartTime + renderTime;
            Start = start;
            End = end;

            Width = width;
        }

        public RenderLine(Vector2 start, Vector2 end, float renderTime, Color color, int radius = 65, int width = 3)
        {
            StartTime = Environment.TickCount;
            EndTime = StartTime + renderTime;
            Start = start;
            End = end;

            Color = color;

            Width = width;
        }

        public override void Draw()
        {
            if (!Start.IsOnScreen() && !End.IsOnScreen())
            {
                return;
            }

            Render.WorldToScreen(Start.To3D(), out var realStart);
            Render.WorldToScreen(End.To3D(), out var realEnd);

            Render.Line(realStart, realEnd, Color);
        }
    }
}