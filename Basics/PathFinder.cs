using UnityEngine;

namespace GS.Basics
{
    public abstract class PathFinder
    {
        protected GS.Data.PathData[] route;

        public abstract GS.Data.PathData GetPath(Vector3 _from);
        public abstract GS.Data.PathData GetNearestPath(Vector3 _from);
        public abstract GS.Data.PathData[] GetRoute();

        public abstract void SetRoute(GS.Data.PathData[] _route);
        public abstract void PlanRoute(Vector3 _from, Vector3 _to);
        public abstract void PlanRouteUsingPath(Vector3 _from, Vector3 _to, GS.Data.PathData _path);
        public abstract void PlanRouteUsingRoute(Vector3 _from, Vector3 _to, GS.Data.PathData[] _route);

        public abstract void AddPathPoint(GS.Data.PathData _point);
        public abstract void RemovePathPoint(int _index);

        public abstract float GetDistanceToTravel(Vector3 _from);
        public abstract float GetDistanceTraveled(Vector3 _from);
        public abstract float GetRouteLength();
    }
}