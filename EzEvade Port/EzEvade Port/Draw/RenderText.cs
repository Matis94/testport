namespace EzEvade_Port.Draw
{
    using System;
    using System.Drawing;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Utils;

    class RenderText : RenderObject
    {
        public Color Color = Color.White;
        public Vector2 RenderPosition;
        public string Text;

        public RenderText(string text, Vector2 renderPosition, float renderTime)
        {
            StartTime = Environment.TickCount;
            EndTime = StartTime + renderTime;
            RenderPosition = renderPosition;

            Text = text;
        }

        public RenderText(string text, Vector2 renderPosition, float renderTime, Color color)
        {
            StartTime = Environment.TickCount;
            EndTime = StartTime + renderTime;
            RenderPosition = renderPosition;

            Color = color;

            Text = text;
        }

        public override void Draw()
        {
            if (RenderPosition.IsZero)
            {
                return;
            }

            const int textDimension = 10;
            Render.WorldToScreen(RenderPosition.To3D(), out var wardScreenPos);

            Render.Text(Text, new Vector2(wardScreenPos.X - textDimension / 2f, wardScreenPos.Y), RenderTextFlags.Center, Color);
        }
    }
}