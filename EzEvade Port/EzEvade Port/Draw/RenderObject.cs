namespace EzEvade_Port.Draw
{
    using System;
    using System.Collections.Generic;
    using Utils;

    abstract class RenderObject
    {
        public float EndTime = 0;
        public float StartTime = 0;

        public abstract void Draw();
    }

    class RenderObjects
    {
        private static readonly List<RenderObject> Objects = new List<RenderObject>();

        static RenderObjects()
        {
            Aimtec.Render.OnPresent += Render_OnPresent;
        }

        private static void Render_OnPresent()
        {
            Render();
        }

        private static void Render()
        {
            foreach (var obj in Objects)
            {
                if (obj.EndTime - Environment.TickCount > 0)
                {
                    obj.Draw();
                }
                else
                {
                    DelayAction.Add(1, () => Objects.Remove(obj));
                }
            }
        }

        public static void Add(RenderObject obj)
        {
            Objects.Add(obj);
        }
    }
}