using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using UnityEngine;

namespace StateChecker
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class EditorStateAddon : MonoBehaviour
    {
        void Start()
        {
            if (FlightGlobals.ActiveVessel == null) return;
            
            foreach (var part in FlightGlobals.ActiveVessel.Parts)
            {
                part.AddOnMouseDown(p =>
                {
                    var diallog = new MultiOptionDialog(GetPartInfo(p), options: new DialogOption("Cancel", null, true));
                    PopupDialog.SpawnPopupDialog(diallog, false, HighLogic.Skin);
                });
            }
        }


        string GetPartInfo(Part p)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("Part info: {0} {1}", p.partInfo.name, p.partInfo.cost);
            foreach (var baseEvent in GetEvents(p))
            {
                builder.AppendFormat("{1} Part event: {0} ", baseEvent.guiName ?? baseEvent.name, Environment.NewLine);                
            }

            return builder.ToString();
        }
        

        IEnumerable<BaseEvent> GetEvents(Part p)
        {
            return p.Modules.OfType<PartModule>().SelectMany(x => x.Events.ToArray()).Where(x => x.active && x.guiActive).ToList();
        } 
    }
}
