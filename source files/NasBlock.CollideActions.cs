using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using MCGalaxy;
using MCGalaxy.Blocks;
using MCGalaxy.Events.PlayerEvents;
using MCGalaxy.Maths;
using MCGalaxy.Tasks;
using BlockID = System.UInt16;
using NasBlockCollideAction =
    System.Action<NotAwesomeSurvival.NasEntity,
    NotAwesomeSurvival.NasBlock, bool, ushort, ushort, ushort>;

namespace NotAwesomeSurvival {

    public partial class NasBlock {
            
            
            public static NasBlockCollideAction DefaultSolidCollideAction() {
                return (ne,nasBlock,headSurrounded,x,y,z) => {
                    if (headSurrounded) {
                        //if (ne.GetType() == typeof(NasPlayer)) {
                        //    NasPlayer np = (NasPlayer)ne;
                        //    np.p.Message("head surrounded @ {0} {1} {2}", x, y, z);
                        //}
                        ne.TakeDamage(1.5f, NasEntity.DamageSource.Suffocating);
                        
                    }
                    
                };
            }
            
            public static NasBlockCollideAction LavaCollideAction() {
                return (ne,nasBlock,headSurrounded,x,y,z) => {
                    if (headSurrounded) {
                        ne.holdingBreath = true;
                    }
                    ne.TakeDamage(1.5f, NasEntity.DamageSource.Suffocating, "@p %cmelted in lava.");
                };
            }
            
    		public static NasBlockCollideAction FireCollideAction() {
                return (ne,nasBlock,headSurrounded,x,y,z) => {
                    ne.TakeDamage(0.25f, NasEntity.DamageSource.None, "@p %cburned up");
                };
            }
    		
    		public static NasBlockCollideAction SpikeCollideAction() {
                return (ne,nasBlock,headSurrounded,x,y,z) => {
                    ne.TakeDamage(3f, NasEntity.DamageSource.None, "@p %cgot impaled");
                };
            }
    		
    		public static NasBlockCollideAction PressureCollideAction() {
                return (ne,nasBlock,headSurrounded,x,y,z) => {
    				ne.nl.SetBlock(x, y, z, Block.FromRaw(611));
    				ne.nl.blockEntities[x+" "+y+" "+z].strength = 15;
                };
            }
            
            public static NasBlockCollideAction LiquidCollideAction() {
                return (ne,nasBlock,headSurrounded,x,y,z) => {
                    if (headSurrounded) {
                        ne.holdingBreath = true;
                    }
                };
            }
            public static NasBlockCollideAction AirCollideAction() {
                return (ne,nasBlock,headSurrounded,x,y,z) => {
                    if (headSurrounded) {
                        ne.holdingBreath = false;
                    }
                };
            }
        
    }

}
