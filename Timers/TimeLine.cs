using System.Collections.Generic;
using UnityEngine;

namespace GS.Timers
{
    public class TimeLine
    {
        public LinkedList<TimeLineData> data = new LinkedList<TimeLineData>();

        public void Init()
        {
            data = new LinkedList<TimeLineData>();
        }

        public void Record(Transform _target, bool first = false)
        {
            if ( first ) { data.AddFirst(new TimeLineData(_target)); }
            else { data.AddLast(new TimeLineData(_target)); }
        }

        public void Forget(bool first = false)
        {
            if ( first ) { data.RemoveFirst(); }
            else { data.RemoveLast(); }
        }
    }
}