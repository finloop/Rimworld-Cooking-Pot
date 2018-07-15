using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;

namespace CookingPot
{
    [StaticConstructorOnStartup]
    public class Building_CookingPot : Building
    {

        #region Variables
        public static Graphic[] graphic = null;
        private const int arraysize = 6;
        private string graphicpathAdditionWoNumber = "_frame";

        private int activeGraphicFrame = 0;
        private int ticksSinceUpdategraphic;
        private int updateAnimationEveryXTicks = 10;
        #endregion

        #region Setup Work
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            Log.Warning("SPAWNEDDDDDDDDDDDDDDDDDDDDDDDDD!");
            base.SpawnSetup(map, respawningAfterLoad);
        }

        public override void ExposeData()
        {
            base.ExposeData();
        }

        private void UpdateGraphics()
        {
            if (graphic != null && graphic.Length > 0 && graphic[0] != null)
                return;

            graphic = new Graphic_Single[arraysize];
            int indexOf_frame = def.graphicData.texPath.ToLower().LastIndexOf(graphicpathAdditionWoNumber);
            string graphicRealPathBase = def.graphicData.texPath.Remove(indexOf_frame);

            for(int i = 0; i < arraysize; i++)
            {
                string graphicRealPath = graphicRealPathBase + graphicpathAdditionWoNumber + (i + 1).ToString();

                graphic[i] = GraphicDatabase.Get<Graphic_Single>(graphicRealPath, def.graphic.Shader, def.graphic.drawSize, def.graphic.Color, def.graphic.ColorTwo);
            }

        }

        #endregion

        #region Destroy

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            base.Destroy(mode);
        }
        #endregion

        #region Ticker

        public override void Tick()
        {
            DoTickerWork(1);

            base.Tick();
        }

        private void DoTickerWork(int ticks)
        {
            if (Map == null)
                return;

            ticksSinceUpdategraphic += ticks;
            if(ticksSinceUpdategraphic >= updateAnimationEveryXTicks)
            {
                ticksSinceUpdategraphic = 0;
                activeGraphicFrame++;
                if (activeGraphicFrame >= arraysize)
                    activeGraphicFrame = 0;
                Map.mapDrawer.MapMeshDirty(Position, MapMeshFlag.Things, true, false);
            }
        }
        #endregion

        public override Graphic Graphic
        {
            get
            {
                if (graphic == null || graphic[0] == null)
                {
                    UpdateGraphics();
                    Log.Message("graphics null lub graphics[0] jest null");
                    if (graphic == null || graphic[0] == null)
                        return base.Graphic;
                }

                if (graphic[activeGraphicFrame] != null)
                {
                    Log.Message("Frame 1 not lodaed!!!!!!!!!!!!!!");
                    return graphic[activeGraphicFrame];
                }
                    

                return base.Graphic;
            }
        }


    }
}
