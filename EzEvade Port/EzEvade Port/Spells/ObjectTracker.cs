namespace EzEvade_Port.Spells
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Helpers;
    using Utils;

    public class ObjectTrackerInfo
    {
        public Vector3 Direction;
        public string Name;
        public GameObject Obj;
        public Dictionary<int, GameObject> ObjList = new Dictionary<int, GameObject>();
        public int OwnerNetworkId;
        public Vector3 Position;
        public float Timestamp;
        public bool UsePosition;

        public ObjectTrackerInfo(GameObject obj)
        {
            Obj = obj;
            Name = obj.Name;
            Timestamp = Environment.TickCount;
        }

        public ObjectTrackerInfo(GameObject obj, string name)
        {
            Obj = obj;
            Name = name;
            Timestamp = Environment.TickCount;
        }

        public ObjectTrackerInfo(string name, Vector3 position)
        {
            Name = name;
            UsePosition = true;
            Position = position;

            Timestamp = Environment.TickCount;
        }
    }

    public static class ObjectTracker
    {
        public static Dictionary<int, ObjectTrackerInfo> ObjTracker = new Dictionary<int, ObjectTrackerInfo>();
        public static int ObjTrackerId;
        private static bool _loaded;

        static ObjectTracker()
        {
            GameObject.OnCreate += HiuCreate_ObjectTracker;
            _loaded = true;
        }

        public static void HuiTrackerForceLoad()
        {
            if (_loaded)
            {
                return;
            }

            GameObject.OnCreate += HiuCreate_ObjectTracker;
            _loaded = true;
        }

        public static void AddObjTrackerPosition(string name, Vector3 position, float timeExpires)
        {
            ObjTracker.Add(ObjTrackerId, new ObjectTrackerInfo(name, position));

            DelayAction.Add((int) timeExpires, () => ObjTracker.Remove(ObjTrackerId));

            ObjTrackerId += 1;
        }

        private static void HiuCreate_ObjectTracker(GameObject obj)
        {
            if (ObjTracker.ContainsKey(obj.NetworkId))
            {
                return;
            }

            var minion = obj as Obj_AI_Minion;
            if (minion == null || !minion.CheckTeam() || !minion.UnitSkinName.ToLower().Contains("testcube"))
            {
                return;
            }

            ObjTracker.Add(obj.NetworkId, new ObjectTrackerInfo(obj, "hiu"));
            DelayAction.Add(250, () => ObjTracker.Remove(obj.NetworkId));
        }

        private static void HiuDelete_ObjectTracker(GameObject obj, EventArgs args)
        {
            if (ObjTracker.ContainsKey(obj.NetworkId))
            {
                ObjTracker.Remove(obj.NetworkId);
            }
        }

        public static Vector2 GetLastHiuOrientation()
        {
            var objList = ObjTracker.Values.Where(o => o.Name == "hiu");
            var sortedObjList = objList.OrderByDescending(o => o.Timestamp);

            var count = sortedObjList.Count();
            if (count < 2)
            {
                return Vector2.Zero;
            }
            var pos1 = sortedObjList.First().Obj.Position;
            var pos2 = sortedObjList.ElementAt(1).Obj.Position;

            return (pos2.To2D() - pos1.To2D()).Normalized();
        }
    }
}