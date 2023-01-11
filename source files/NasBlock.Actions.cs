using System;
using System.Collections.Generic;
using MCGalaxy;
using MCGalaxy.Blocks;
using MCGalaxy.Maths;
using MCGalaxy.Network;
using MCGalaxy.Tasks;
using BlockID = System.UInt16;
using NasBlockAction = System.Action<NotAwesomeSurvival.NasLevel, NotAwesomeSurvival.NasBlock, int, int, int>;
using NasBlockExistAction =
    System.Action<NotAwesomeSurvival.NasPlayer,
    NotAwesomeSurvival.NasBlock, bool, ushort, ushort, ushort>;
namespace NotAwesomeSurvival {

    public partial class NasBlock {
        static NasBlockAction FloodAction(BlockID[] set) {
            return (nl,nasBlock,x,y,z) => {
                if (CanInfiniteFloodKillThis(nl, x, y-1, z, set) ) {
                    nl.SetBlock(x, y-1, z, set[LiquidInfiniteIndex]);
                    return;
                }
                if (CanInfiniteFloodKillThis(nl, x+1, y, z, set) ) {
                    nl.SetBlock(x+1, y, z, set[LiquidInfiniteIndex]);
                }
                if (CanInfiniteFloodKillThis(nl, x-1, y, z, set) ) {
                    nl.SetBlock(x-1, y, z, set[LiquidInfiniteIndex]);
                }
                if (CanInfiniteFloodKillThis(nl, x, y, z+1, set) ) {
                    nl.SetBlock(x, y, z+1, set[LiquidInfiniteIndex]);
                }
                if (CanInfiniteFloodKillThis(nl, x, y, z-1, set) ) {
                    nl.SetBlock(x, y, z-1, set[LiquidInfiniteIndex]);
                }
            };
        }
        static bool CanInfiniteFloodKillThis(NasLevel nl, int x, int y, int z, BlockID[] set) {
            BlockID here = nl.GetBlock(x, y, z);
            if (CanPhysicsKillThis(here) || IsPartOfSet(set, here) > LiquidInfiniteIndex) { return true; }
            return false;
        }
        
        public static BlockID[] blocksPhysicsCanKill = new BlockID[] {
            0,
            37,
            38,
            39,
            40,
            Block.Fire,
            Block.Extended|96,
            Block.Extended|130,
            Block.Extended|651,
            Block.FromRaw(644), 
            Block.FromRaw(645), 
            Block.FromRaw(646), 
            Block.FromRaw(461),
            Block.Snow
        };
        public static bool CanPhysicsKillThis(BlockID block) {
            for (int i = 0; i < blocksPhysicsCanKill.Length; i++) {
                if (block == blocksPhysicsCanKill[i]) { return true; }
            }
            return false;
        }
        public static bool IsThisLiquid(BlockID block) {
            if (IsPartOfSet(waterSet, block) != -1) { return true; }
            return false;
        }
        
        static int LiquidInfiniteIndex = 0;
        static int LiquidSourceIndex = 1;
        static int LiquidWaterfallIndex = 2;
        /// <summary>
        /// First ID is the infinite-flood version of the liquid, second is the source, third is waterfall, the rest are heights from tallest to shortest
        /// </summary>
        public static BlockID[] waterSet = new BlockID[] { 8, 9, Block.Extended|639,
            Block.Extended|632,
            Block.Extended|633,
            Block.Extended|634,
            Block.Extended|635,
            Block.Extended|636,
            Block.Extended|637,
            Block.Extended|638 };
        

        
        public static BlockID[] lavaSet = new BlockID[] { 11, 10, Block.Extended|695,
            Block.Extended|691,
            Block.Extended|692,
            Block.Extended|693,
            Block.Extended|694 };
        
        /// <summary>
        /// Check if the given block exists within the given set.
        /// </summary>
        /// <returns>The index of the set that the block is at
        /// or -1 if the block does not exist within the set.
        /// </returns>
        public static int IsPartOfSet(BlockID[] set, BlockID block) {
            for (int i = 0; i < set.Length; i++) {
                if (set[i] == block) { return i; }
            }
            return -1;
        }
        /// <summary>
        /// returns -1 if not part of the set, spreadIndex+1 if air(or block liquids kill), otherwise the index into the set
        /// </summary>
        static int CanReplaceBlockAt(NasLevel nl, int x, int y, int z, BlockID[] set, int spreadIndex) {
            BlockID hereBlock = nl.GetBlock(x, y, z);
            if (nl.GetBlock(x, y-1, z) == Block.FromRaw(703)) {return -1;}
            if (CanPhysicsKillThis(hereBlock)) { return spreadIndex+1; }
            int hereIndex = IsPartOfSet(set, hereBlock);
            return hereIndex;
        }
        static bool CanLiquidLive(NasLevel nl, BlockID[] set, int index, int x, int y, int z) {
            BlockID neighbor = nl.GetBlock(x, y, z);
            if (neighbor == set[index-1] ||
                neighbor == set[LiquidSourceIndex] ||
                neighbor == set[LiquidWaterfallIndex]
               ) {
                return true;
            }
            return false;
        }
        
        static NasBlockAction LimitedFloodAction(BlockID[] set, int index) {
            return (nl,nasBlock,x,y,z) => {
        		if (y >= 200 && set == waterSet) {nl.SetBlock(x, y, z, Block.Ice); return;}
        		if (nl.biome < 0 && set == waterSet) {nl.SetBlock(x,y,z,Block.Air); return;}
        		BlockID hereBlock = nl.GetBlock(x, y, z); 
    				
        		BlockID[] aboveHere = {nl.GetBlock(x, y+1, z), nl.GetBlock(x+1, y, z), nl.GetBlock(x-1, y, z), nl.GetBlock(x, y, z+1), nl.GetBlock(x, y, z-1) };
        		
        		if ((IsPartOfSet(lavaSet, hereBlock)) != -1) {
        		if (((IsPartOfSet(waterSet, aboveHere[0])) != -1)||
					((IsPartOfSet(waterSet, aboveHere[1])) != -1)||		   
					((IsPartOfSet(waterSet, aboveHere[2])) != -1)||	
					((IsPartOfSet(waterSet, aboveHere[3])) != -1)||	
					((IsPartOfSet(waterSet, aboveHere[4])) != -1))
					{

    				if (hereBlock == 10 || hereBlock == 11) {
    				
        			nl.SetBlock(x, y, z, Block.Extended|690); 
        			return;
    					}
    				
        			nl.SetBlock(x, y, z, Block.Extended|162); 
        			return;
    					}
    					
    					}
        		
        		//Step one -- Check if we need to drain
                if (index > LiquidSourceIndex) {
                    //it's not a source block
                    
                    if (index == LiquidWaterfallIndex) {
                        //it's a waterfall -- see if it needs to die
                        if (IsPartOfSet(set, aboveHere[0]) == -1) {
                            //nl.lvl.Message("killing waterfall");
                            nl.SetBlock(x, y, z, Block.Air);
                            return;
                        }
                    } else {
                        //it's not a waterfall -- see if it needs to diewa
                        if (!(CanLiquidLive(nl, set, index, x+1, y, z) ||
                            CanLiquidLive(nl, set, index, x-1, y, z) ||
                            CanLiquidLive(nl, set, index, x, y, z+1) ||
                            CanLiquidLive(nl, set, index, x, y, z-1)) ) {
                            
                            //nl.lvl.Message("killing liquid");
                            nl.SetBlock(x, y, z, Block.Air);
                            return;
                        }
                    }
                }
                
                //Step two -- Do the actual flooding
                if (set == waterSet && hereBlock == set[3]) {
       
                	int borders = 0;
                	if (nl.GetBlock(x+1, y, z) == set[LiquidSourceIndex]) {borders++;}
                	if (nl.GetBlock(x-1, y, z) == set[LiquidSourceIndex]) {borders++;}
                	if (nl.GetBlock(x, y, z+1) == set[LiquidSourceIndex]) {borders++;}
                	if (nl.GetBlock(x, y, z-1) == set[LiquidSourceIndex]) {borders++;}
                	if (borders > 1) {nl.SetBlock(x, y, z, set[LiquidSourceIndex]); return;}
                }
                BlockID below = nl.GetBlock(x, y-1, z);
                int belowIndex = IsPartOfSet(set, below);
                if (CanPhysicsKillThis(below) || belowIndex != -1) {
                    //don't override infinite source, source, or waterfall with a waterfall
                    if (!CanPhysicsKillThis(below) && belowIndex <= LiquidWaterfallIndex) { return; }
                    
                    //nl.lvl.Message("setting waterfall");
                    nl.SetBlock(x, y-1, z, set[LiquidWaterfallIndex]);
                    return;
                }
                
                if (index == set.Length-1) {
                    //it's the end of the stream -- no need to flood further
                    return;
                }
                
                int spreadIndex = (index < LiquidWaterfallIndex+1) ? LiquidWaterfallIndex+1 : index+1;
                BlockID spreadBlock = set[spreadIndex];
                
                bool posX;
                bool negX;
                bool posZ;
                bool negZ;
                CanFlowInDirection(nl, x, y, z, set, spreadIndex,
                                   out posX,
                                   out negX,
                                   out posZ,
                                   out negZ);
                
                if (posX) {
                    nl.SetBlock(x+1, y, z, spreadBlock);
                }
                if (negX) {
                    nl.SetBlock(x-1, y, z, spreadBlock);
                }
                if (posZ) {
                    nl.SetBlock(x, y, z+1, spreadBlock);
                }
                if (negZ) {
                    nl.SetBlock(x, y, z-1, spreadBlock);
                }
                
                
            };
        }
        /// <summary>
        /// 
        /// 
        /// </summary>
        static void CanFlowInDirection(NasLevel nl, int x, int y, int z,
                                       BlockID[] set, int spreadIndex,
                                       out bool xPos,
                                       out bool xNeg,
                                       out bool zPos,
                                       out bool zNeg
                                      ) {
            xPos = true;
            xNeg = true;
            zPos = true;
            zNeg = true;
            
            bool xBlockedPos = false;
            bool xBlockedNeg = false;
            bool zBlockedPos = false;
            bool zBlockedNeg = false;
            
            
            int originalHoleDistance;
            List<Vec3S32> holes = HolesInRange(nl, x, y, z, 4, set, out originalHoleDistance);
            if (holes.Count > 0) {
                CloserToAHole(x, y, z,  1,  0, originalHoleDistance, holes, ref xPos);
                CloserToAHole(x, y, z, -1,  0, originalHoleDistance, holes, ref xNeg);
                CloserToAHole(x, y, z,  0,  1, originalHoleDistance, holes, ref zPos);
                CloserToAHole(x, y, z,  0, -1, originalHoleDistance, holes, ref zNeg);
            }
            
            int neighborIndex1 = CanReplaceBlockAt(nl, x+1, y, z, set, spreadIndex);
            int neighborIndex2 = CanReplaceBlockAt(nl, x-1, y, z, set, spreadIndex);
            int neighborIndex3 = CanReplaceBlockAt(nl, x, y, z+1, set, spreadIndex);
            int neighborIndex4 = CanReplaceBlockAt(nl, x, y, z-1, set, spreadIndex);
            
            if (neighborIndex1 == -1) {
                xBlockedPos = true;
            }
            if (neighborIndex2 == -1) {
                xBlockedNeg = true;
            }
            if (neighborIndex3 == -1) {
                zBlockedPos = true;
            }
            if (neighborIndex4 == -1) {
                zBlockedNeg = true;
            }
            xPos = xPos && !xBlockedPos;
            xNeg = xNeg && !xBlockedNeg;
            zPos = zPos && !zBlockedPos;
            zNeg = zNeg && !zBlockedNeg;
            
            if (!(xPos || xNeg || zPos || zNeg)) { //no water can be spread
                //allow any to spread that were not blocked by solid blocks before
                xPos = !xBlockedPos;
                xNeg = !xBlockedNeg;
                zPos = !zBlockedPos;
                zNeg = !zBlockedNeg;
                
            }
            //make it not spread if the neighbor is taller
            xPos = xPos && neighborIndex1 > spreadIndex;
            xNeg = xNeg && neighborIndex2 > spreadIndex;
            zPos = zPos && neighborIndex3 > spreadIndex;
            zNeg = zNeg && neighborIndex4 > spreadIndex;
            
            
        }
        static void CloserToAHole(int x, int y, int z, int xDiff, int zDiff, int originalHoleDistance, List<Vec3S32> holes, ref bool canFlowDir) {
            x += xDiff;
            z += zDiff;
            foreach (var hole in holes) {
                int dist = Math.Abs(x - hole.X) + Math.Abs(z - hole.Z);
                if (dist < originalHoleDistance) {
                    canFlowDir = true; return;
                }
            }
            canFlowDir = false;
        }
        
        
        public class FloodSim {
            NasLevel nl;
            int xO;
            int yO;
            int zO;
            int totalDistance;
            BlockID[] liquidSet;
            bool[,] waterAtSpot;
            int widthAndHeight;
            
            List<Vec3S32> holes;
            int distanceHolesWereFoundAt;
            
            
            public FloodSim(NasLevel nl, int xO, int yO, int zO, int totalDistance, BlockID[] set) {
                this.nl = nl;
                this.xO = xO;
                this.yO = yO;
                this.zO = zO;
                this.totalDistance = totalDistance;
                this.liquidSet = set;
                waterAtSpot = new bool[totalDistance*2+1,totalDistance*2+1];
                widthAndHeight = waterAtSpot.GetLength(0);
                
                holes = new List<Vec3S32>();
                distanceHolesWereFoundAt = totalDistance;
            }
            public List<Vec3S32> GetHoles(out int distance) {
                //place water in the center
                Flood(xO, zO, true);
                TryFlood(xO+1, yO, zO);
                TryFlood(xO-1, yO, zO);
                TryFlood(xO,   yO, zO+1);
                TryFlood(xO,   yO, zO-1);
                
                distance = distanceHolesWereFoundAt;
                return holes;
            }
            void TryFlood(int x, int y, int z) {
                int distanceFromCenter = Math.Abs(x - xO) + Math.Abs(z - zO);
                //this spot is out of bounds? quit
                if (distanceFromCenter > totalDistance) {
                    return;
                }
                //this spot has been flooded already? quit
                if (AlreadyFlooded(x, z)) {
                    return;
                }
                
                BlockID here = nl.GetBlock(x, y, z);
                //can't flood into this spot? quit
                if (!(CanPhysicsKillThis(here) || IsPartOfSet(liquidSet, here) != -1) ) {
                    return;
                }
                BlockID below = nl.GetBlock(x, y-1, z);
                //if there's a hole here
                if (CanPhysicsKillThis(below) || IsPartOfSet(liquidSet, below) != -1) {
                    if (distanceFromCenter < distanceHolesWereFoundAt) {
                        holes.Clear();
                        holes.Add(new Vec3S32(x, y-1, z));
                        distanceHolesWereFoundAt = distanceFromCenter;
                    } else if (distanceFromCenter == distanceHolesWereFoundAt) {
                        holes.Add(new Vec3S32(x, y-1, z));
                    }
                }
                Flood(x, z, true);
                TryFlood(x+1, y, z);
                TryFlood(x-1, y, z);
                TryFlood(x, y, z+1);
                TryFlood(x, y, z-1);
            }
            bool AlreadyFlooded(int x, int z) {
                int xI = x - xO;
                int zI = z - zO;
                xI += totalDistance;
                zI += totalDistance;
                //both dimensions are the same 
                if (
                    xI >= widthAndHeight ||
                    zI >= widthAndHeight ||
                    xI <  0 ||
                    zI <  0
                   ) {
                    return false;
                }
                return waterAtSpot[xI,zI];
            }
            void Flood(int x, int z, bool value) {
                int xI = x - xO;
                int zI = z - zO;
                xI += totalDistance;
                zI += totalDistance;
                
                waterAtSpot[xI,zI] = value;
            }
        }
        public static List<Vec3S32> HolesInRange(NasLevel nl, int x, int y, int z, int totalDistance, BlockID[] set, out int distance) {
            FloodSim sim = new FloodSim(nl, x, y, z, totalDistance, set);
            return sim.GetHoles(out distance);
        }
        
        static NasBlockAction FallingBlockAction(BlockID serverBlockID) {
            return (nl,nasBlock,x,y,z) => {
                BlockID blockUnder = nl.GetBlock(x, y-1, z);
                if (nl.GetBlock(x, y-2, z) == Block.FromRaw(703)) {return;}
                if (CanPhysicsKillThis(blockUnder) || IsPartOfSet(waterSet, blockUnder) != -1) {
                    nl.SetBlock(x, y, z, Block.Air);
                    nl.SetBlock(x, y-1, z, serverBlockID);
                }
            };
        }
        static NasBlockAction GrassBlockAction(BlockID grass, BlockID dirt) {
            return (nl,nasBlock,x,y,z) => {
        		if (grass == Block.FromRaw(139) && nl.biome != 2) 
        		{nl.SetBlock(x, y, z, Block.FromRaw(129));}
                BlockID aboveHere = nl.GetBlock(x, y+1, z);
                if (!nl.lvl.LightPasses(aboveHere)) {
                    nl.SetBlock(x, y, z, dirt);
                }
            };
        }
        
        
     
        
        static BlockID[] grassSet = new BlockID[] { Block.Grass, Block.Extended|119, Block.Extended|129, Block.Extended|139};
        static BlockID[] tallGrassSet = new BlockID[] { 40, Block.Extended|120, Block.Extended|130, Block.Extended|130};
        static NasBlockAction DirtBlockAction(BlockID[] grassSet, BlockID dirt) {
            return (nl,nasBlock,x,y,z) => {
                BlockID aboveHere = nl.GetBlock(x, y+1, z);
                if (!nl.lvl.LightPasses(aboveHere)) {
                    //nl.lvl.Message("Can't grow since solid above");
                    return;
                }
                
                for (int xOff = -1; xOff <= 1; xOff++)
                    for (int yOff = -1; yOff <= 1; yOff++)
                        for (int zOff = -1; zOff <= 1; zOff++)
                {
                    if (xOff == 0 && yOff == -1 && zOff == 0) { continue; }
                    BlockID neighbor = nl.GetBlock(x+xOff, y+yOff, z+zOff);
                    int setIndex = IsPartOfSet(grassSet, neighbor);
                    if (setIndex == 3 && nl.biome != 2) {setIndex = 2;}
                    if (setIndex != 3 && nl.biome == 2 && setIndex != -1) {setIndex = 3;}
                    if (setIndex == -1) { continue; }
                    nl.SetBlock(x, y, z, grassSet[setIndex], true);
                    if (nl.GetBlock(x, y+1, z) == Block.Air) {
                    if (r.Next(0, 20) == 0) {nl.SetBlock((ushort)x, (ushort)(y+1), (ushort)z, Block.Extended|130);}
                    else {
                    int flowerChance = r.Next(0, 100);
                    if (flowerChance == 0 ) {nl.SetBlock(x, y+1, z, Block.Extended|96);}
                    else {
                    if (flowerChance == 1 ) {nl.SetBlock(x, y+1, z, 37);}
                    else {
                    if (flowerChance == 2 ) {nl.SetBlock(x, y+1, z, 38);}
                    else {
                    if (flowerChance == 3 ) {nl.SetBlock(x, y+1, z, Block.Extended|651);}
                    else {
                    if (flowerChance == 4 ) { if (r.Next(0, 20) == 0) {nl.SetBlock(x, y+1, z, Block.Extended|604);}}
                    else {
                    if (flowerChance == 5 ) {nl.SetBlock(x, y+1, z, Block.Extended|201);}
                    else {if (nl.biome == 2) {nl.SetBlock(x, y+1, z, Block.Snow);}}
                                    	}}
                    }}}
                    	}
                        }
                    }
                return;
            };
        }
		private static Random CoordRandom(int x, int y, int z) {
			string rndString = x+" "+y+" "+z;
			return new Random(rndString.GetHashCode()); 
		}
        
        
        static NasBlockAction ObserverActivateAction(int type) {
        	return (nl,nasBlock,x,y,z) => {
        		if (!nl.blockEntities.ContainsKey(x+" "+y+" "+z)){
        			nl.blockEntities.Add(x+" "+y+" "+z, new Entity());
        			nl.blockEntities[x+" "+y+" "+z].type = type;
        		}
                nl.blockEntities[x+" "+y+" "+z].strength = 15;
        		nl.SetBlock(x, y, z, (ushort)(nl.lvl.FastGetBlock((ushort)x, (ushort)y, (ushort)z) + 6));
        	};
        }
        
        static NasBlockAction ObserverDeactivateAction(int type) {
        	return (nl,nasBlock,x,y,z) => {
        		if (!nl.blockEntities.ContainsKey(x+" "+y+" "+z)){
        			nl.blockEntities.Add(x+" "+y+" "+z, new Entity());
        			nl.blockEntities[x+" "+y+" "+z].type = type;
        		}
                nl.blockEntities[x+" "+y+" "+z].strength = 0;
        		nl.SetBlock(x, y, z, (ushort)(nl.lvl.FastGetBlock((ushort)x, (ushort)y, (ushort)z) - 6));
        	};
        }
        
        
        static BlockID[] logSet = new BlockID[] { 15, 16, 17, Block.Extended|144, Block.Extended|242, Block.Extended|656, Block.Extended|657, Block.Extended|240, Block.Extended|241, Block.Extended|248, Block.Extended|249, Block.Extended|250,};
        static NasBlockAction LeafBlockAction(BlockID[] logSet, BlockID leaf) {
            return (nl,nasBlock,x,y,z) => {
                bool canLive = false;
                int iteration = 1;
                IsThereLog(nl, x+1, y,   z,   leaf, iteration, ref canLive);
                IsThereLog(nl, x,   y+1, z,   leaf, iteration, ref canLive);
                IsThereLog(nl, x,   y,   z+1, leaf, iteration, ref canLive);
                IsThereLog(nl, x-1, y,   z,   leaf, iteration, ref canLive);
                IsThereLog(nl, x,   y-1, z,   leaf, iteration, ref canLive);
                IsThereLog(nl, x,   y,   z-1, leaf, iteration, ref canLive);
                if (canLive) {
                    //Player.Console.Message("It can live!");
                    return;
                }
                nl.SetBlock(x, y, z, Block.Air);
                if (r.Next(0, 384) == 0 && CanPhysicsKillThis(nl.GetBlock(x, y-1, z)) ) {
                	if (leaf == Block.Leaves) {nl.SetBlock(x, y-1, z, Block.FromRaw(648));}
                	if (leaf == Block.FromRaw(103)) {nl.SetBlock(x, y-1, z, Block.FromRaw(702));}
                }
            };
        }
        
        
        static NasBlockAction GrowAction(BlockID grow) {
        return (nl,nasBlock,x,y,z) => {
        		if (grow == Block.FromRaw(667)) {
                    if (Block.Sand == nl.GetBlock(x, y-1, z))
                {
                    if (!((nl.GetBlock(x-1, y-1, z) == Block.Water )||
                          (nl.GetBlock(x+1, y-1, z) == Block.Water )||
                          (nl.GetBlock(x, y-1, z+1) == Block.Water )||
                          (nl.GetBlock(x, y-1, z-1) == Block.Water )||
                          (nl.GetBlock(x-1, y-1, z) == 9 )||
                          (nl.GetBlock(x+1, y-1, z) == 9 )||
                          (nl.GetBlock(x, y-1, z+1) == 9 )||
                          (nl.GetBlock(x, y-1, z-1) == 9 )))
                    {return;}
                }
                else
                {
                    if (!((nl.GetBlock(x-1, y-2, z) == Block.Water )||
                          (nl.GetBlock(x+1, y-2, z) == Block.Water )||
                          (nl.GetBlock(x, y-2, z+1) == Block.Water )||
                          (nl.GetBlock(x, y-2, z-1) == Block.Water )||
                          (nl.GetBlock(x-1, y-2, z) == 9 )||
                          (nl.GetBlock(x+1, y-2, z) == 9 )||
                          (nl.GetBlock(x, y-2, z+1) == 9 )||
                          (nl.GetBlock(x, y-2, z-1) == 9 )))
                    {return;}
                }
        		}
        		if (!((grow == nl.GetBlock(x, y-1, z) && nl.GetBlock(x, y-2, z) == Block.Sand) || nl.GetBlock(x, y-1, z) == Block.Sand))
                { return; }
                
                if (((nl.GetBlock(x, y-1, z) == grow) && (nl.GetBlock(x, y-2, z) == grow)) | (nl.GetBlock(x, y+1, z) != Block.Air)) {return;}
                
                nl.SetBlock(x, y+1, z, grow);
            };
        }
        
        static NasBlockAction VineGrowAction(BlockID grow) {
        	return (nl,nasBlock,x,y,z) => {
        		if (nl.GetBlock(x,y+1,z) == grow && nl.GetBlock(x,y+2,z) == grow) return;
        		if (nl.GetBlock(x,y-1,z) == Block.Air) 
        		nl.SetBlock(x,y-1,z,grow);
        	};
        }
        
        static NasBlockAction VineDeathAction() {
        	return (nl,nasBlock,x,y,z) => {
        		if (IsPartOfSet(blocksPhysicsCanKill,nl.GetBlock(x,y+1,z)) != -1)
        		nl.SetBlock(x,y,z,Block.Air);
        	};
        }
        
        static NasBlockAction LilyAction() {
        	return (nl,nasBlock,x,y,z) => {
        		if (IsPartOfSet(waterSet,nl.GetBlock(x,y-1,z)) == -1) 
        			nl.SetBlock(x,y,z,Block.Air);
        	};
        }
        
        static void IsThereLog(NasLevel nl, int x, int y, int z, BlockID leaf, int iteration, ref bool canLive) {
            if (canLive) { return; }
            BlockID hereBlock = nl.GetBlock(x, y, z);
            if (IsPartOfSet(logSet, hereBlock) != -1) {
                canLive = true;
                return;
            }
            if (hereBlock != leaf) { return; }
            if ((iteration >= 10 && leaf == Block.FromRaw(104)) || (iteration >= 5 && leaf != Block.FromRaw(104)))  { return; }
            iteration++;
            IsThereLog(nl, x+1, y,   z,   leaf, iteration, ref canLive);
            IsThereLog(nl, x,   y+1, z,   leaf, iteration, ref canLive);
            IsThereLog(nl, x,   y,   z+1, leaf, iteration, ref canLive);
            IsThereLog(nl, x-1, y,   z,   leaf, iteration, ref canLive);
            IsThereLog(nl, x,   y-1, z,   leaf, iteration, ref canLive);
            IsThereLog(nl, x,   y,   z-1, leaf, iteration, ref canLive);
        }
        
        public static BlockID[] infinifire = new BlockID[] {
        Block.Extended|647,
        Block.Extended|690,
		48        
        
        };
        
        static NasBlockAction FireAction() {
        	return (nl,nasBlock,x,y,z) => {
        		if (IsPartOfSet(infinifire, nl.GetBlock(x, y-1, z)) == -1)
        		{
        			if (r.Next(0, 8) == 0) {nl.SetBlock(x, y, z, Block.FromRaw(131));}
        			else {nl.SetBlock(x, y, z, Block.Air);}
        		}
        	
        	};
        }
        
           static NasBlockAction LampAction(BlockID on, BlockID off, BlockID me) {
        	return (nl,nasBlock,x,y,z) => {
        			Entity[] b = new Entity[6];
        			if (nl.blockEntities.ContainsKey(x+" "+(y-1)+" "+z)) 
        			{b[0] = nl.blockEntities[x+" "+(y-1)+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey((x+1)+" "+y+" "+z))
        			{b[1] = nl.blockEntities[(x+1)+" "+y+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey((x-1)+" "+y+" "+z))
        			{b[2] = nl.blockEntities[(x-1)+" "+y+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z+1)))
        			{b[3] = nl.blockEntities[x+" "+y+" "+(z+1)];}
        			
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z-1)))
        			{b[4] = nl.blockEntities[x+" "+y+" "+(z-1)];}
        			
        			if (nl.blockEntities.ContainsKey(x+" "+(y+1)+" "+z)) 
        			{b[5] = nl.blockEntities[x+" "+(y+1)+" "+z];}
        			
        		bool powered = (
        				((b[5] != null) && (b[5].strength > 0 && (b[5].type == 1 || b[5].type == 4 || b[5].type == 5 || b[5].type == 12))) ||
        				((b[0] != null) && (b[0].strength > 0 && (b[0].type == 1 || b[0].type == 4 || b[0].type == 6 || b[0].type == 12))) ||
        				((b[1] != null) && (b[1].strength > 0 && (b[1].type == 0 || b[1].type == 4 || b[1].type == 10|| b[1].type == 11))) ||
        				((b[2] != null) && (b[2].strength > 0 && (b[2].type == 0 || b[2].type == 4 || b[2].type == 8 || b[2].type == 11))) ||
        				((b[3] != null) && (b[3].strength > 0 && (b[3].type == 2 || b[3].type == 4 || b[3].type == 7 || b[3].type == 13))) ||
        				((b[4] != null) && (b[4].strength > 0 && (b[4].type == 2 || b[4].type == 4 || b[4].type == 9 || b[4].type == 13)))
        			);
        			if (powered) {
        				if (off == me)
        				{nl.SetBlock(x, y, z, on);}
        			}
        		else {
        				if (on == me)
        				{nl.SetBlock(x, y, z, off);}
        			}
        	};
        }
        
        static NasBlockAction UnrefinedGoldAction(BlockID on, BlockID off, BlockID me) {
        	return (nl,nasBlock,x,y,z) => {
        			Entity[] b = new Entity[6];
        			if (nl.blockEntities.ContainsKey(x+" "+(y-1)+" "+z)) 
        			{b[0] = nl.blockEntities[x+" "+(y-1)+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey((x+1)+" "+y+" "+z))
        			{b[1] = nl.blockEntities[(x+1)+" "+y+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey((x-1)+" "+y+" "+z))
        			{b[2] = nl.blockEntities[(x-1)+" "+y+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z+1)))
        			{b[3] = nl.blockEntities[x+" "+y+" "+(z+1)];}
        			
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z-1)))
        			{b[4] = nl.blockEntities[x+" "+y+" "+(z-1)];}
        			
        			if (nl.blockEntities.ContainsKey(x+" "+(y+1)+" "+z)) 
        			{b[5] = nl.blockEntities[x+" "+(y+1)+" "+z];}
        			
        		bool powered = (
        				((b[5] != null) && (b[5].strength > 0 && ( b[5].type == 5))) ||
        				((b[0] != null) && (b[0].strength > 0 && ( b[0].type == 6))) ||
        				((b[1] != null) && (b[1].strength > 0 && ( b[1].type == 10))) ||
        				((b[2] != null) && (b[2].strength > 0 && ( b[2].type == 8))) ||
        				((b[3] != null) && (b[3].strength > 0 && ( b[3].type == 7))) ||
        				((b[4] != null) && (b[4].strength > 0 && ( b[4].type == 9)))
        			);
        			if (powered) {
        				if (off == me)
        				{nl.SetBlock(x, y, z, on);
        				nl.blockEntities[x+" "+y+" "+z].strength = 15;
        				}
        			}
        		else {
        				if (on == me)
        				{nl.SetBlock(x, y, z, off);
        				nl.blockEntities[x+" "+y+" "+z].strength = 0;}
        			}
        	};
        }
        
        static NasBlockAction SidewaysPistonAction(string type, string axis, int dir, BlockID[] pistonSet, bool sticky = false) {
        	return (nl,nasBlock,x,y,z) => {
        		int changeX = 0;
        		int changeZ = 0;
        		int changeY = 0;
        		BlockID[] dontpush = {0, 0, 0, 0, Block.FromRaw(680), Block.FromRaw(706), Block.FromRaw(709), Block.FromRaw(712)};
        		if (axis == "x") {
        			changeX = dir;
        			changeZ = 0;
        			dontpush[0] = Block.FromRaw(391);
        			dontpush[1] = Block.FromRaw(397);
        			dontpush[2] = Block.FromRaw(403);
        			dontpush[3] = Block.FromRaw(409);
        		}
        		
        		if (axis == "z") {
        			changeZ = dir;
        			changeX = 0;
        			dontpush[0] = Block.FromRaw(394);
        			dontpush[1] = Block.FromRaw(400);
        			dontpush[2] = Block.FromRaw(406);
        			dontpush[3] = Block.FromRaw(412);
        		}
        		
        		if (type == "off") {
        		Entity[] b = new Entity[6];
        			
        			if (nl.blockEntities.ContainsKey(x+" "+(y+1)+" "+z))
        			{b[0] = nl.blockEntities[x+" "+(y+1)+" "+z];}
        			
        		    if (nl.blockEntities.ContainsKey(x+" "+(y-1)+" "+z))
        			{b[1] = nl.blockEntities[x+" "+(y-1)+" "+z];}
        		
        		    if (nl.blockEntities.ContainsKey((x+1)+" "+y+" "+z) && (changeX != 1))
        			{b[2] = nl.blockEntities[(x+1)+" "+y+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey((x-1)+" "+y+" "+z) && (changeX != -1))
        			{b[3] = nl.blockEntities[(x-1)+" "+y+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z+1)) && (changeZ != 1))
        			{b[4] = nl.blockEntities[x+" "+y+" "+(z+1)];}
        			
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z-1)) && (changeZ != -1))
        			{b[5] = nl.blockEntities[x+" "+y+" "+(z-1)];}
        			
        		if (
        				((b[0] != null) && (b[0].strength > 0 && (b[0].type == 1 || b[0].type == 4 || b[0].type == 5 || b[0].type == 12))) ||
        				((b[1] != null) && (b[1].strength > 0 && (b[1].type == 1 || b[1].type == 4 || b[1].type == 6 || b[1].type == 12))) ||
        				((b[2] != null) && (b[2].strength > 0 && (b[2].type == 0 || b[2].type == 4 || b[2].type == 10|| b[2].type == 11))) ||
        				((b[3] != null) && (b[3].strength > 0 && (b[3].type == 0 || b[3].type == 4 || b[3].type == 8 || b[3].type == 11))) ||
        				((b[4] != null) && (b[4].strength > 0 && (b[4].type == 2 || b[4].type == 4 || b[4].type == 7 || b[4].type == 13))) ||
        				((b[5] != null) && (b[5].strength > 0 && (b[5].type == 2 || b[5].type == 4 || b[5].type == 9 || b[5].type == 13)))
        			)
        			{
        			BlockID[] above = { 
        					TurnValid(nl.GetBlock((x+changeX), (y+changeY), (z+changeZ))), 0, 0, 0, 0, 0, 0};
        				int push = 0;
        				if (above[0] == Block.Air || above[0] == 8) {push = 0;}
        				else {
        				if (IsPartOfSet(unpushable, above[0]) != -1 || IsPartOfSet(dontpush, above[0]) != -1) {return;}
        					above[1] = TurnValid(nl.GetBlock((x+2*changeX), (y+2*changeY), (z+2*changeZ)));
        				if (above[1] == Block.Air || above[1] == 8) {push = 1;}
        				else {
        				if (IsPartOfSet(unpushable, above[1]) != -1 || IsPartOfSet(dontpush, above[1]) != -1) {return;}
        					above[2] = TurnValid(nl.GetBlock((x+3*changeX), (y+3*changeY), (z+3*changeZ)));
        				if (above[2] == Block.Air || above[2] == 8) {push = 2;}
        				else {
        				if (IsPartOfSet(unpushable, above[2]) != -1 || IsPartOfSet(dontpush, above[2]) != -1) {return;}
        					above[3] = TurnValid(nl.GetBlock((x+4*changeX), (y+4*changeY), (z+4*changeZ)));
        				if (above[3] == Block.Air || above[3] == 8) {push = 3;}
        				else {
        				if (IsPartOfSet(unpushable, above[3]) != -1 || IsPartOfSet(dontpush, above[3]) != -1) {return;}
        					above[4] = TurnValid(nl.GetBlock((x+5*changeX), (y+5*changeY), (z+5*changeZ)));
        				if (above[4] == Block.Air || above[4] == 8) {push = 4;}
        				else {
        				if (IsPartOfSet(unpushable, above[4]) != -1 || IsPartOfSet(dontpush, above[4]) != -1) {return;}
        					above[5] = TurnValid(nl.GetBlock((x+6*changeX), (y+6*changeY), (z+6*changeZ)));
        				if (above[5] == Block.Air || above[5] == 8) {push = 5;}
        				else {
        				if (IsPartOfSet(unpushable, above[5]) != -1 || IsPartOfSet(dontpush, above[5]) != -1) {return;}
        					above[6] = TurnValid(nl.GetBlock((x+7*changeX), (y+7*changeY), (z+7*changeZ)));
        				if (above[6] == Block.Air || above[6] == 8) {push = 6;}
        				else {return;}
        				}}}}}}
        			
        			if (push >= 6) {
        				nl.SetBlock(x+(7*changeX), y+(7*changeY), z+(7*changeZ), above[5]);
        				if (nl.blockEntities.ContainsKey((x+(6*changeX))+" "+(y+(6*changeY))+" "+(z+(6*changeZ))))
        			{nl.blockEntities.Remove((x+(6*changeX))+" "+(y+(6*changeY))+" "+(z+(6*changeZ)));}
        			}
        			if (push >= 5) {
        				nl.SetBlock(x+(6*changeX), y+(6*changeY), z+(6*changeZ), above[4]);
        				if (nl.blockEntities.ContainsKey((x+(5*changeX))+" "+(y+(5*changeY))+" "+(z+(5*changeZ))))
        			{nl.blockEntities.Remove((x+(5*changeX))+" "+(y+(5*changeY))+" "+(z+(5*changeZ)));}
        			}
        			if (push >= 4) {
        				nl.SetBlock(x+(5*changeX), y+(5*changeY), z+(5*changeZ), above[3]);
        				if (nl.blockEntities.ContainsKey((x+(4*changeX))+" "+(y+(4*changeY))+" "+(z+(4*changeZ))))
        			{nl.blockEntities.Remove((x+(4*changeX))+" "+(y+(4*changeY))+" "+(z+(4*changeZ)));}
        			}
        			if (push >= 3) {
        				nl.SetBlock(x+(4*changeX), y+(4*changeY), z+(4*changeZ), above[2]);
        				if (nl.blockEntities.ContainsKey((x+(3*changeX))+" "+(y+(3*changeY))+" "+(z+(3*changeZ))))
        			{nl.blockEntities.Remove((x+(3*changeX))+" "+(y+(3*changeY))+" "+(z+(3*changeZ)));}
        			}
        			if (push >= 2) {
        				nl.SetBlock(x+(3*changeX), y+(3*changeY), z+(3*changeZ), above[1]);
        				if (nl.blockEntities.ContainsKey((x+(2*changeX))+" "+(y+(2*changeY))+" "+(z+(2*changeZ))))
        			{nl.blockEntities.Remove((x+(2*changeX))+" "+(y+(2*changeY))+" "+(z+(2*changeZ)));}
        			}
        			if (push >= 1) {
        				nl.SetBlock(x+(2*changeX), y+(2*changeY), z+(2*changeZ), above[0]);
        				if (nl.blockEntities.ContainsKey((x+(changeX))+" "+(y+(changeY))+" "+(z+(changeZ))))
        				{nl.blockEntities.Remove((x+(changeX))+" "+(y+(changeY))+" "+(z+(changeZ)));}
        			}
        			nl.SetBlock(x+changeX, y+changeY, z+changeZ, pistonSet[2]);
        			nl.SetBlock(x, y, z, pistonSet[1]);
        			
        			Player[] players = PlayerInfo.Online.Items;
        			for (int i = 0; i < players.Length; i++) {
					Player who = players[i];
					Vec3F32 posH = who.Pos.BlockCoords;
					if (who.Pos.FeetBlockCoords == new Vec3S32((x+(push+1)*changeX), (y+(push+1)*changeY), (z+(push+1)*changeZ)) || who.Pos.BlockCoords == new Vec3S32((x+(push+1)*changeX), (y+(push+1)*changeY), (z+(push+1)*changeZ)))
					{
						if (Get(Collision.ConvertToClientBlockID(nl.GetBlock((int)posH.X+changeX, (int)posH.Y+changeY, (int)posH.Z+changeZ))).collideAction != DefaultSolidCollideAction()){
						Position posit = who.Pos;
						posit.X += changeX * 32;
						posit.Y += changeY * 32;
						posit.Z += changeZ * 32;
						who.SendPosition(posit, new Orientation(who.Rot.RotY, who.Rot.HeadX));
						}
		            } 
						
					}
        			
        		}
        	}
        		if (type == "body") {
        		Entity[] b = new Entity[6];
        			
        			if (nl.blockEntities.ContainsKey(x+" "+(y+1)+" "+z))
        			{b[0] = nl.blockEntities[x+" "+(y+1)+" "+z];}
        			
        		    if (nl.blockEntities.ContainsKey(x+" "+(y-1)+" "+z))
        			{b[1] = nl.blockEntities[x+" "+(y-1)+" "+z];}
        		
        		    if (nl.blockEntities.ContainsKey((x+1)+" "+y+" "+z) && (changeX != 1))
        			{b[2] = nl.blockEntities[(x+1)+" "+y+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey((x-1)+" "+y+" "+z) && (changeX != -1))
        			{b[3] = nl.blockEntities[(x-1)+" "+y+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z+1)) && (changeZ != 1))
        			{b[4] = nl.blockEntities[x+" "+y+" "+(z+1)];}
        			
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z-1)) && (changeZ != -1))
        			{b[5] = nl.blockEntities[x+" "+y+" "+(z-1)];}
        			
        			
        		if (!(
        				((b[0] != null) && (b[0].strength > 0 && (b[0].type == 1 || b[0].type == 4 || b[0].type == 5 || b[0].type == 12))) ||
        				((b[1] != null) && (b[1].strength > 0 && (b[1].type == 1 || b[1].type == 4 || b[1].type == 6 || b[1].type == 12))) ||
        				((b[2] != null) && (b[2].strength > 0 && (b[2].type == 0 || b[2].type == 4 || b[2].type == 10|| b[2].type == 11))) ||
        				((b[3] != null) && (b[3].strength > 0 && (b[3].type == 0 || b[3].type == 4 || b[3].type == 8 || b[3].type == 11))) ||
        				((b[4] != null) && (b[4].strength > 0 && (b[4].type == 2 || b[4].type == 4 || b[4].type == 7 || b[4].type == 13))) ||
        				((b[5] != null) && (b[5].strength > 0 && (b[5].type == 2 || b[5].type == 4 || b[5].type == 9 || b[5].type == 13)))
        			))
        			{
        				if (!sticky) {
        				nl.SetBlock(x, y, z, pistonSet[0]);
        				nl.SetBlock(x+(1*changeX), y+(1*changeY), z+(1*changeZ), Block.Air);
        				return;
        				}
        				else {
        				nl.SetBlock(x, y, z, pistonSet[0]);
        				BlockID pullback = TurnValid(nl.GetBlock(x+(2*changeX), y+(2*changeY), z+(2*changeZ)));
        				if (IsPartOfSet(unpushable, pullback) == -1 && IsPartOfSet(dontpush, pullback) == -1 && pullback != 8) {
        					nl.SetBlock(x+(1*changeX), y+(1*changeY), z+(1*changeZ), pullback);
        					nl.SetBlock(x+(2*changeX), y+(2*changeY), z+(2*changeZ), Block.Air);
        					if (nl.blockEntities.ContainsKey((x+(2*changeX))+" "+(y+(2*changeY))+" "+(z+(2*changeZ))))
        					{nl.blockEntities.Remove((x+(2*changeX))+" "+(y+(2*changeY))+" "+(z+(2*changeZ)));}
        					return;
        				}
        				return;
        			}
        			}
        			if (nl.GetBlock(x+changeX, y+changeY, z+changeZ) != pistonSet[2])
        			{nl.SetBlock(x+changeX, y+changeY, z+changeZ, pistonSet[2]);}
        			
        			
        		}
        		
        		if (type == "head") {
        			if (nl.GetBlock(x-changeX, y-changeY, z-changeZ) != pistonSet[1])
        		{nl.SetBlock(x, y, z, Block.Air);}
        		}
        		
        		};
        }
        
        static NasBlockAction PistonAction(string type, int changeX, int changeY, int changeZ, BlockID[] pistonSet) {
        	return (nl,nasBlock,x,y,z) => {
        	BlockID[] dontpush = {Block.FromRaw(391),
        			Block.FromRaw(397),
        			Block.FromRaw(403),
        			Block.FromRaw(409),
        			Block.FromRaw(394),
        			Block.FromRaw(400),
        			Block.FromRaw(406),
        			Block.FromRaw(412),
        		};
        		if (type == "off") {
        		Entity[] b = new Entity[5];
        		if (nl.blockEntities.ContainsKey(x+" "+(y-changeY)+" "+z))
        			{b[0] = nl.blockEntities[x+" "+(y-changeY)+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey((x+1)+" "+y+" "+z))
        			{b[1] = nl.blockEntities[(x+1)+" "+y+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey((x-1)+" "+y+" "+z))
        			{b[2] = nl.blockEntities[(x-1)+" "+y+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z+1)))
        			{b[3] = nl.blockEntities[x+" "+y+" "+(z+1)];}
        			
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z-1)))
        			{b[4] = nl.blockEntities[x+" "+y+" "+(z-1)];}
        			
        			
        		if (
        				((b[0] != null) && (b[0].strength > 0 && (b[0].type == 1 || b[0].type == 4 || b[0].type == 12 || ((changeY == 1 && b[0].type == 6) || (changeY == -1 && b[0].type == 5))))) ||
        				((b[1] != null) && (b[1].strength > 0 && (b[1].type == 0 || b[1].type == 4 || b[1].type == 10|| b[1].type == 11))) ||
        				((b[2] != null) && (b[2].strength > 0 && (b[2].type == 0 || b[2].type == 4 || b[2].type == 8 || b[2].type == 11))) ||
        				((b[3] != null) && (b[3].strength > 0 && (b[3].type == 2 || b[3].type == 4 || b[3].type == 7 || b[3].type == 13))) ||
        				((b[4] != null) && (b[4].strength > 0 && (b[4].type == 2 || b[4].type == 4 || b[4].type == 9 || b[4].type == 13)))
        			)
        			{
        			BlockID[] above = { 
        					TurnValid(nl.GetBlock((x+changeX), (y+changeY), (z+changeZ))), 0, 0, 0, 0, 0, 0};
        				int push = 0;
        				if (above[0] == Block.Air || above[0] == 8) {push = 0;}
        				else {
        				if (IsPartOfSet(unpushable, above[0]) != -1  || IsPartOfSet(dontpush, above[0]) != -1) {return;}
        				above[1] = TurnValid(nl.GetBlock((x+2*changeX), (y+2*changeY), (z+2*changeZ)));
        				if (above[1] == Block.Air || above[1] == 8) {push = 1;}
        				else {
        				if (IsPartOfSet(unpushable, above[1]) != -1  || IsPartOfSet(dontpush, above[1]) != -1) {return;}
        					above[2] = TurnValid(nl.GetBlock((x+3*changeX), (y+3*changeY), (z+3*changeZ)));
        				if (above[2] == Block.Air || above[2] == 8) {push = 2;}
        				else {
        				if (IsPartOfSet(unpushable, above[2]) != -1  || IsPartOfSet(dontpush, above[2]) != -1) {return;}
        					above[3] = TurnValid(nl.GetBlock((x+4*changeX), (y+4*changeY), (z+4*changeZ)));
        				if (above[3] == Block.Air || above[3] == 8) {push = 3;}
        				else {
        				if (IsPartOfSet(unpushable, above[3]) != -1  || IsPartOfSet(dontpush, above[3]) != -1) {return;}
        					above[4] = TurnValid(nl.GetBlock((x+5*changeX), (y+5*changeY), (z+5*changeZ)));
        				if (above[4] == Block.Air || above[4] == 8) {push = 4;}
        				else {
        				if (IsPartOfSet(unpushable, above[4]) != -1  || IsPartOfSet(dontpush, above[4]) != -1) {return;}
        					above[5] = TurnValid(nl.GetBlock((x+6*changeX), (y+6*changeY), (z+6*changeZ)));
        				if (above[5] == Block.Air || above[5] == 8) {push = 5;}
        				else {
        				if (IsPartOfSet(unpushable, above[5]) != -1  || IsPartOfSet(dontpush, above[5]) != -1) {return;}
        					above[6] = TurnValid(nl.GetBlock((x+7*changeX), (y+7*changeY), (z+7*changeZ)));
        				if (above[6] == Block.Air || above[6] == 8) {push = 6;}
        				else {return;}
        				}}}}}}
        			
        			if (push >= 6) {
        				nl.SetBlock(x, y+(7*changeY), z, above[5]);
        				if (nl.blockEntities.ContainsKey(x+" "+(y+(6*changeY))+" "+z))
        			{nl.blockEntities.Remove(x+" "+(y+(6*changeY))+" "+z);}
        			}
        			if (push >= 5) {
        				nl.SetBlock(x, y+(6*changeY), z, above[4]);
        				if (nl.blockEntities.ContainsKey(x+" "+(y+(5*changeY))+" "+z))
        			{nl.blockEntities.Remove(x+" "+(y+(5*changeY))+" "+z);}
        			}
        			if (push >= 4) {
        				nl.SetBlock(x, y+(5*changeY), z, above[3]);
        				if (nl.blockEntities.ContainsKey(x+" "+(y+(4*changeY))+" "+z))
        			{nl.blockEntities.Remove(x+" "+(y+(4*changeY))+" "+z);}
        			}
        			if (push >= 3) {
        				nl.SetBlock(x, y+(4*changeY), z, above[2]);
        				if (nl.blockEntities.ContainsKey(x+" "+(y+(3*changeY))+" "+z))
        			{nl.blockEntities.Remove(x+" "+(y+(3*changeY))+" "+z);}
        			}
        			if (push >= 2) {
        				nl.SetBlock(x, y+(3*changeY), z, above[1]);
        				if (nl.blockEntities.ContainsKey(x+" "+(y+(2*changeY))+" "+z))
        			{nl.blockEntities.Remove(x+" "+(y+(2*changeY))+" "+z);}
        			}
        			if (push >= 1) {
        				nl.SetBlock(x, y+(2*changeY), z, above[0]);
        				if (nl.blockEntities.ContainsKey(x+" "+(y+(1*changeY))+" "+z))
        			{nl.blockEntities.Remove(x+" "+(y+(1*changeY))+" "+z);}
        			}
        			nl.SetBlock(x, y+changeY, z, pistonSet[2]);
        			nl.SetBlock(x, y, z, pistonSet[1]);
        			Player[] players = PlayerInfo.Online.Items;
        			for (int i = 0; i < players.Length; i++) {
					Player who = players[i];
					Vec3F32 posH = who.Pos.BlockCoords;
					if (who.Pos.FeetBlockCoords == new Vec3S32((x+(push+1)*changeX), (y+(push+1)*changeY), (z+(push+1)*changeZ)) || who.Pos.BlockCoords == new Vec3S32((x+(push+1)*changeX), (y+(push+1)*changeY), (z+(push+1)*changeZ)))
					{
						if (Get(Collision.ConvertToClientBlockID(nl.GetBlock((int)posH.X+changeX, (int)posH.Y+changeY, (int)posH.Z+changeZ))).collideAction != DefaultSolidCollideAction()){
						Position posit = who.Pos;
						posit.X += changeX * 32;
						posit.Y += changeY * 32;
						posit.Z += changeZ * 32;
						who.SendPosition(posit, new Orientation(who.Rot.RotY, who.Rot.HeadX));
						} }
        			}
        		}
        	}
        		if (type == "body") {
        		Entity[] b = new Entity[5];
        		if (nl.blockEntities.ContainsKey(x+" "+(y-changeY)+" "+z))
        			{b[0] = nl.blockEntities[x+" "+(y-changeY)+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey((x+1)+" "+y+" "+z))
        			{b[1] = nl.blockEntities[(x+1)+" "+y+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey((x-1)+" "+y+" "+z))
        			{b[2] = nl.blockEntities[(x-1)+" "+y+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z+1)))
        			{b[3] = nl.blockEntities[x+" "+y+" "+(z+1)];}
        			
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z-1)))
        			{b[4] = nl.blockEntities[x+" "+y+" "+(z-1)];}
        			
        			
        		if (!(
        				((b[0] != null) && (b[0].strength > 0 && (b[0].type == 1 || b[0].type == 4 || b[0].type == 12 || ((changeY == 1 && b[0].type == 6) || (changeY == -1 && b[0].type == 5))))) ||
        				((b[1] != null) && (b[1].strength > 0 && (b[1].type == 0 || b[1].type == 4 || b[1].type == 10 || b[1].type == 11))) ||
        				((b[2] != null) && (b[2].strength > 0 && (b[2].type == 0 || b[2].type == 4 || b[2].type == 8 || b[2].type == 11))) ||
        				((b[3] != null) && (b[3].strength > 0 && (b[3].type == 2 || b[3].type == 4 || b[3].type == 7 || b[3].type == 13))) ||
        				((b[4] != null) && (b[4].strength > 0 && (b[4].type == 2 || b[4].type == 4 || b[4].type == 9 || b[4].type == 13)))
        			))
        			{
        				nl.SetBlock(x, y, z, pistonSet[0]);
        				
        				return;
        			}
        			if (nl.GetBlock(x, y+changeY, z) != pistonSet[2])
        			{nl.SetBlock(x, y+changeY, z, pistonSet[2]);}
        			
        			
        		}
        		
        		if (type == "head") {
        			if (nl.GetBlock(x, y-changeY, z) != pistonSet[1])
        		{nl.SetBlock(x, y, z, Block.Air);}
        		}
        		
        		};
        }
        
        
        
        
        public static BlockID[] pistonUp = {
        Block.FromRaw(704),
        Block.FromRaw(705),
        Block.FromRaw(706)
        };
        
        public static BlockID[] stickyPistonUp = {
        Block.FromRaw(678),
        Block.FromRaw(679),
        Block.FromRaw(680)
        };
        
        public static BlockID[] pistonDown = {
        Block.FromRaw(707),
        Block.FromRaw(708),
        Block.FromRaw(709)
        };
        
        public static BlockID[] stickyPistonDown = {
        Block.FromRaw(710),
        Block.FromRaw(711),
        Block.FromRaw(712)
        };
        
        public static BlockID[] pistonNorth = {
        Block.FromRaw(389),
        Block.FromRaw(390),
        Block.FromRaw(391)
        };
        
        public static BlockID[] pistonEast = {
        Block.FromRaw(392),
        Block.FromRaw(393),
        Block.FromRaw(394)
        };
        
        public static BlockID[] pistonSouth = {
        Block.FromRaw(395),
        Block.FromRaw(396),
        Block.FromRaw(397)
        };
        
        public static BlockID[] pistonWest = {
        Block.FromRaw(398),
        Block.FromRaw(399),
        Block.FromRaw(400)
        };
        
        public static BlockID[] stickyPistonNorth = {
        Block.FromRaw(401),
        Block.FromRaw(402),
        Block.FromRaw(403)
        };
        
        public static BlockID[] stickyPistonEast = {
        Block.FromRaw(404),
        Block.FromRaw(405),
        Block.FromRaw(406)
        };
        
        public static BlockID[] stickyPistonSouth = {
        Block.FromRaw(407),
        Block.FromRaw(408),
        Block.FromRaw(409)
        };
        
        public static BlockID[] stickyPistonWest = {
        Block.FromRaw(410),
        Block.FromRaw(411),
        Block.FromRaw(412)
        };
        
        static NasBlockAction StickyPistonAction(string type, int changeX, int changeY, int changeZ, BlockID[] pistonSet) {
        	return (nl,nasBlock,x,y,z) => {
        		BlockID[] dontpush = {Block.FromRaw(391),
        			Block.FromRaw(397),
        			Block.FromRaw(403),
        			Block.FromRaw(409),
        			Block.FromRaw(394),
        			Block.FromRaw(400),
        			Block.FromRaw(406),
        			Block.FromRaw(412),
        		};
        		Entity[] b = new Entity[5];
        		if (nl.blockEntities.ContainsKey(x+" "+(y-changeY)+" "+z))
        			{b[0] = nl.blockEntities[x+" "+(y-changeY)+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey((x+1)+" "+y+" "+z))
        			{b[1] = nl.blockEntities[(x+1)+" "+y+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey((x-1)+" "+y+" "+z))
        			{b[2] = nl.blockEntities[(x-1)+" "+y+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z+1)))
        			{b[3] = nl.blockEntities[x+" "+y+" "+(z+1)];}
        			
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z-1)))
        			{b[4] = nl.blockEntities[x+" "+y+" "+(z-1)];}
        		if (type == "off") {
        		
        			
        			
        		if (
        				((b[0] != null) && (b[0].strength > 0 && (b[0].type == 1 || b[0].type == 4 || b[0].type == 12 || ((changeY == 1 && b[0].type == 6) || (changeY == -1 && b[0].type == 5))))) ||
        				((b[1] != null) && (b[1].strength > 0 && (b[1].type == 0 || b[1].type == 4 || b[1].type == 10|| b[1].type == 11))) ||
        				((b[2] != null) && (b[2].strength > 0 && (b[2].type == 0 || b[2].type == 4 || b[2].type == 8 || b[2].type == 11))) ||
        				((b[3] != null) && (b[3].strength > 0 && (b[3].type == 2 || b[3].type == 4 || b[3].type == 7 || b[3].type == 13))) ||
        				((b[4] != null) && (b[4].strength > 0 && (b[4].type == 2 || b[4].type == 4 || b[4].type == 9 || b[4].type == 13)))
        			)
        			{
        			BlockID[] above = { 
        					TurnValid(nl.GetBlock((x+changeX), (y+changeY), (z+changeZ))), 0, 0, 0, 0, 0, 0};
        				int push = 0;
        				if (above[0] == Block.Air || above[0] == 8) {push = 0;}
        				else {
        				if (IsPartOfSet(unpushable, above[0]) != -1  || IsPartOfSet(dontpush, above[0]) != -1) {return;}
        				above[1] = TurnValid(nl.GetBlock((x+2*changeX), (y+2*changeY), (z+2*changeZ)));
        				if (above[1] == Block.Air || above[1] == 8) {push = 1;}
        				else {
        				if (IsPartOfSet(unpushable, above[1]) != -1  || IsPartOfSet(dontpush, above[1]) != -1) {return;}
        				above[2] = TurnValid(nl.GetBlock((x+3*changeX), (y+3*changeY), (z+3*changeZ)));
        				if (above[2] == Block.Air || above[2] == 8) {push = 2;}
        				else {
        				if (IsPartOfSet(unpushable, above[2]) != -1  || IsPartOfSet(dontpush, above[2]) != -1) {return;}
        				above[3] = TurnValid(nl.GetBlock((x+4*changeX), (y+4*changeY), (z+4*changeZ)));
        				if (above[3] == Block.Air || above[3] == 8) {push = 3;}
        				else {
        				if (IsPartOfSet(unpushable, above[3]) != -1  || IsPartOfSet(dontpush, above[3]) != -1) {return;}
        				above[4] = TurnValid(nl.GetBlock((x+5*changeX), (y+5*changeY), (z+5*changeZ)));
        				if (above[4] == Block.Air || above[4] == 8) {push = 4;}
        				else {
        				if (IsPartOfSet(unpushable, above[4]) != -1  || IsPartOfSet(dontpush, above[4]) != -1) {return;}
        				above[5] = TurnValid(nl.GetBlock((x+6*changeX), (y+6*changeY), (z+6*changeZ)));
        				if (above[5] == Block.Air || above[5] == 8) {push = 5;}
        				else {
        				if (IsPartOfSet(unpushable, above[5]) != -1  || IsPartOfSet(dontpush, above[5]) != -1) {return;}
        				above[6] = TurnValid(nl.GetBlock((x+7*changeX), (y+7*changeY), (z+7*changeZ)));
        				if (above[6] == Block.Air || above[6] == 8) {push = 6;}
        				else {return;}
        				}}}}}}
        			
        			if (push >= 6) {
        				nl.SetBlock(x, y+(7*changeY), z, above[5]);
        				if (nl.blockEntities.ContainsKey(x+" "+(y+(6*changeY))+" "+z))
        			{nl.blockEntities.Remove(x+" "+(y+(6*changeY))+" "+z);}
        			}
        			if (push >= 5) {
        				nl.SetBlock(x, y+(6*changeY), z, above[4]);
        				if (nl.blockEntities.ContainsKey(x+" "+(y+(5*changeY))+" "+z))
        			{nl.blockEntities.Remove(x+" "+(y+(5*changeY))+" "+z);}
        			}
        			if (push >= 4) {
        				nl.SetBlock(x, y+(5*changeY), z, above[3]);
        				if (nl.blockEntities.ContainsKey(x+" "+(y+(4*changeY))+" "+z))
        			{nl.blockEntities.Remove(x+" "+(y+(4*changeY))+" "+z);}
        			}
        			if (push >= 3) {
        				nl.SetBlock(x, y+(4*changeY), z, above[2]);
        				if (nl.blockEntities.ContainsKey(x+" "+(y+(3*changeY))+" "+z))
        			{nl.blockEntities.Remove(x+" "+(y+(3*changeY))+" "+z);}
        			}
        			if (push >= 2) {
        				nl.SetBlock(x, y+(3*changeY), z, above[1]);
        				if (nl.blockEntities.ContainsKey(x+" "+(y+(2*changeY))+" "+z))
        			{nl.blockEntities.Remove(x+" "+(y+(2*changeY))+" "+z);}
        			}
        			if (push >= 1) {
        				nl.SetBlock(x, y+(2*changeY), z, above[0]);
        				if (nl.blockEntities.ContainsKey(x+" "+(y+(1*changeY))+" "+z))
        			{nl.blockEntities.Remove(x+" "+(y+(1*changeY))+" "+z);}
        			}
        			nl.SetBlock(x, y+changeY, z, pistonSet[2]);
        			nl.SetBlock(x, y, z, pistonSet[1]);
        			
        			Player[] players = PlayerInfo.Online.Items;
        			for (int i = 0; i < players.Length; i++) {
					Player who = players[i];
					Vec3F32 posH = who.Pos.BlockCoords;
					if (who.Pos.FeetBlockCoords == new Vec3S32((x+(push+1)*changeX), (y+(push+1)*changeY), (z+(push+1)*changeZ)) || who.Pos.BlockCoords == new Vec3S32((x+(push+1)*changeX), (y+(push+1)*changeY), (z+(push+1)*changeZ)))
					{
						if (Get(Collision.ConvertToClientBlockID(nl.GetBlock((int)posH.X+changeX, (int)posH.Y+changeY, (int)posH.Z+changeZ))).collideAction != DefaultSolidCollideAction()){
						Position posit = who.Pos;
						posit.X += changeX * 32;
						posit.Y += changeY * 32;
						posit.Z += changeZ * 32;
						who.SendPosition(posit, new Orientation(who.Rot.RotY, who.Rot.HeadX));
						}
		            } 
        			}
        		}
        	}
        		if (type == "body") {
        			
        		if (!(
        				((b[0] != null) && (b[0].strength > 0 && (b[0].type == 1 || b[0].type == 4 || b[0].type == 12 || ((changeY == 1 && b[0].type == 6) || (changeY == -1 && b[0].type == 5))))) ||
        				((b[1] != null) && (b[1].strength > 0 && (b[1].type == 0 || b[1].type == 4 || b[1].type == 10|| b[1].type == 11))) ||
        				((b[2] != null) && (b[2].strength > 0 && (b[2].type == 0 || b[2].type == 4 || b[2].type == 8 || b[2].type == 11))) ||
        				((b[3] != null) && (b[3].strength > 0 && (b[3].type == 2 || b[3].type == 4 || b[3].type == 7 || b[3].type == 13))) ||
        				((b[4] != null) && (b[4].strength > 0 && (b[4].type == 2 || b[4].type == 4 || b[4].type == 9 || b[4].type == 13)))
        			))
        			{
        				nl.SetBlock(x, y, z, pistonSet[0]);
        				BlockID pullback = TurnValid(nl.GetBlock(x, y+(2*changeY), z));
        			if (IsPartOfSet(unpushable, pullback) == -1  && IsPartOfSet(dontpush, pullback) == -1 && pullback != 8) {
        			nl.SetBlock(x, y+(1*changeY), z, pullback);
        			nl.SetBlock(x, y+(2*changeY), z, Block.Air);
        			if (nl.blockEntities.ContainsKey(x+" "+(y+(2*changeY))+" "+z))
        			{nl.blockEntities.Remove(x+" "+(y+(2*changeY))+" "+z);}
        			
        			}
        			return;
        			}
        			if (nl.GetBlock(x, y+changeY, z) != pistonSet[2])
        			{nl.SetBlock(x, y+changeY, z, pistonSet[2]);}
        		}
        		
        		if (type == "head") {
        			if (nl.GetBlock(x, y-changeY, z) != pistonSet[1])
        		{nl.SetBlock(x, y, z, Block.Air);}
        		}
        		
        		};
        }
        static bool ConvertBody(BlockID block, BlockID[] set, out BlockID returnedBlock) {
        returnedBlock = block;
        if (block == set[1]) {
        	returnedBlock = set[0];
        	return true;
        	}
        return false;
        }
        
        static BlockID TurnValid(BlockID block){
        	BlockID returnedBlock = block;
        	if (ConvertBody(block, pistonUp, out returnedBlock)) return returnedBlock;
        	if (ConvertBody(block, pistonDown, out returnedBlock)) return returnedBlock;
        	if (ConvertBody(block, pistonNorth, out returnedBlock)) return returnedBlock;
        	if (ConvertBody(block, pistonEast, out returnedBlock)) return returnedBlock;
        	if (ConvertBody(block, pistonSouth, out returnedBlock)) return returnedBlock;
        	if (ConvertBody(block, pistonWest, out returnedBlock)) return returnedBlock;
        	if (ConvertBody(block, stickyPistonUp, out returnedBlock)) return returnedBlock;
        	if (ConvertBody(block, stickyPistonDown, out returnedBlock)) return returnedBlock;
        	if (ConvertBody(block, stickyPistonNorth, out returnedBlock)) return returnedBlock;
        	if (ConvertBody(block, stickyPistonEast, out returnedBlock)) return returnedBlock;
        	if (ConvertBody(block, stickyPistonSouth, out returnedBlock)) return returnedBlock;
        	if (ConvertBody(block, stickyPistonWest, out returnedBlock)) return returnedBlock;
        	return returnedBlock;
        }
        
        
        static BlockID[] unpushable = {
        	Block.FromRaw(690),
        	Block.FromRaw(647), 
        	Block.FromRaw(216), 
        	Block.FromRaw(217), 
        	Block.FromRaw(218), 
        	Block.FromRaw(219),
        	Block.FromRaw(602), 
        	Block.FromRaw(603), 
        	Block.FromRaw(143),
        	Block.FromRaw(171),
        	Block.FromRaw(54),
        	Block.FromRaw(703),
        	Block.FromRaw(7),
        	Block.FromRaw(767),
        	Block.FromRaw(674),
        	Block.FromRaw(675),
        	Block.FromRaw(195),
        	Block.FromRaw(196),
        	Block.FromRaw(172),
        	Block.FromRaw(173),
        	Block.FromRaw(174),
        	Block.FromRaw(175),
        	Block.FromRaw(176),
        	Block.FromRaw(177),
        	Block.FromRaw(612),
        	Block.FromRaw(613),
        	Block.FromRaw(614),
        	Block.FromRaw(615),
        	Block.FromRaw(616),
        	Block.FromRaw(617),
        	Block.FromRaw(413),
        	Block.FromRaw(414),
        	Block.FromRaw(439),
        	Block.FromRaw(440),
        	Block.FromRaw(441),
        	Block.FromRaw(442),
        	Block.FromRaw(443),
        	Block.FromRaw(444),
        	Block.FromRaw(673),
        	Block.FromRaw(457),
        };
        
        static int CanIPush(NasLevel nl, int x, int y, int z, BlockID[] above) {
        	if (above[0] == Block.Air || above[0] == 8) {return 0;}
        	if (IsPartOfSet(unpushable, above[0]) != -1) {return -1;}
        	if (above[1] == Block.Air || above[1] == 8) {return 1;}
        	if (IsPartOfSet(unpushable, above[1]) != -1) {return -1;}
        	if (above[2] == Block.Air || above[2] == 8) {return 2;}
        	if (IsPartOfSet(unpushable, above[2]) != -1) {return -1;}
        	if (above[3] == Block.Air || above[3] == 8) {return 3;}
        	if (IsPartOfSet(unpushable, above[3]) != -1) {return -1;}
        	if (above[4] == Block.Air || above[4] == 8) {return 4;}
        	if (IsPartOfSet(unpushable, above[4]) != -1) {return -1;}
        	if (above[5] == Block.Air || above[5] == 8) {return 5;}
        	if (IsPartOfSet(unpushable, above[5]) != -1) {return -1;}
        	if (above[6] == Block.Air || above[6] == 8) {return 6;}
        	if (IsPartOfSet(unpushable, above[6]) != -1) {return -1;}
        	{return -1;}
        }
        
        public static BlockID[] wireSetActive = {
        Block.Extended|683,
        Block.Extended|682,
        Block.Extended|684
        };
        
        public static BlockID[] wireSetInactive = {
        Block.Extended|551,
        Block.Extended|550,
        Block.Extended|552
        };
        
        public static BlockID[] fixedWireSetInactive = {
        Block.Extended|732,
        Block.Extended|733,
        Block.Extended|734
        };
        
        
        public static BlockID[] fixedWireSetActive = {
        Block.Extended|735,
        Block.Extended|736,
        Block.Extended|737
        };
        

        public static BlockID[] repeaterSetActive = {
        Block.Extended|613,
        Block.Extended|614,
        Block.Extended|615,
        Block.Extended|616,
        Block.Extended|617,
        Block.Extended|618,
        };
        
        public static BlockID[] repeaterSetInactive = {
        Block.Extended|172,
        Block.Extended|173,
        Block.Extended|174,
        Block.Extended|175,
        Block.Extended|176,
        Block.Extended|177,
        };
        
        
        static NasBlockAction PowerSourceAction(int direction) {
        	return (nl,nasBlock,x,y,z) => {
        	if (!nl.blockEntities.ContainsKey(x+" "+y+" "+z)) 
        		{
        		nl.blockEntities.Add(x+" "+y+" "+z, new Entity());
                nl.blockEntities[x+" "+y+" "+z].strength = 15;
                nl.blockEntities[x+" "+y+" "+z].type = direction;
                nl.SimulateSetBlock(x, y, z);
        		}
        	};
        	
        }
        
        static NasBlockAction WireAction(BlockID[] actSet, BlockID[] inactSet, int direction, BlockID hereBlock) {
            return (nl,nasBlock,x,y,z) => {
        		
        		int type = 0;
        		if (IsPartOfSet(actSet, hereBlock) != -1)
        		{type = IsPartOfSet(actSet, hereBlock);}
        		else {type = IsPartOfSet(inactSet, hereBlock);}
        		if (actSet == fixedWireSetActive) {type += 11;}
        		if (!nl.blockEntities.ContainsKey(x+" "+y+" "+z)) 
        		{
        		nl.blockEntities.Add(x+" "+y+" "+z, new Entity());
                nl.blockEntities[x+" "+y+" "+z].strength = 0;
                nl.blockEntities[x+" "+y+" "+z].type = type;
        		}
        		
        		Entity b = nl.blockEntities[x+" "+y+" "+z];
        		Entity strength1 = new Entity();
        		Entity strength2 = new Entity();
        		int strength0 = b.strength;
        		if (direction == 0) {
        			if (nl.blockEntities.ContainsKey((x+1)+" "+y+" "+z)) 
        			{
        				Entity bEntity = nl.blockEntities[(x+1)+" "+y+" "+z];
        				int checkType = bEntity.type;
        				if (checkType < 5 || checkType == 10 || checkType == 11)
        				     strength1 = bEntity;
        			}
        			if (nl.blockEntities.ContainsKey((x-1)+" "+y+" "+z)) 
        			{
        				Entity bEntity = nl.blockEntities[(x-1)+" "+y+" "+z];
        				int checkType = bEntity.type;
        				if (checkType < 5 || checkType == 8 || checkType == 11)  
        				     strength2 = bEntity;
        			}
        			
        		}
        		if (direction == 1) {
        			if (nl.blockEntities.ContainsKey(x+" "+(y+1)+" "+z)) 
        			{
        				Entity bEntity = nl.blockEntities[x+" "+(y+1)+" "+z];
        				int checkType = bEntity.type;
        				if (checkType <= 5 || checkType == 12)
        				     strength1 = bEntity;
        			}
        			if (nl.blockEntities.ContainsKey(x+" "+(y-1)+" "+z))
        			{
        				Entity bEntity = nl.blockEntities[x+" "+(y-1)+" "+z];
        				int checkType = bEntity.type;
        				if (checkType < 5 || checkType == 6 || checkType == 12)
        				     strength2 = bEntity;
        			}
        			
        		}
        		if (direction == 2) {
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z+1)))
        			{
        				Entity bEntity = nl.blockEntities[x+" "+y+" "+(z+1)];
        				int checkType = bEntity.type;
        				if (checkType < 5 || checkType == 7 || checkType == 13)
        				     strength1 = bEntity;
        			}
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z-1)))
        			{
        				Entity bEntity = nl.blockEntities[x+" "+y+" "+(z-1)];
        				int checkType = bEntity.type;
        				if (checkType < 5 || checkType == 9 || checkType == 13)
        				     strength2 = bEntity;
        			}
        			
        		}
        		
        		if (strength1.strength >= strength2.strength) 
        		{
        			
        			b.strength = strength1.strength - 1;
        			//b.direction = 1;
        		}
        		else
        		{
        			b.strength = strength2.strength - 1;
        		}
        		
        		if (b.strength <= 0) {
        		b.strength = 0;
        		b.direction = 0;
        		if (IsPartOfSet(actSet, hereBlock) != -1)
        		    {
        			nl.FastSetBlock(x, y, z, inactSet[IsPartOfSet(actSet, hereBlock)]);
        			
        		    }
        		else {
        			if (strength0 != b.strength)
        			{nl.DisturbBlocks(x, y, z);}
        		}
        		}
        		else {
        		if (IsPartOfSet(actSet, hereBlock) == -1)
        		    {
        			nl.FastSetBlock(x, y, z, actSet[IsPartOfSet(inactSet, hereBlock)]);
        		    }
        		else {
        			if (strength0 != b.strength)
        			{nl.DisturbBlocks(x, y, z);}
        		}
        		}
        		
            };
        }
       
        static NasBlockAction PressurePlateAction() {
        	return (nl,nasBlock,x,y,z) => {
        		bool stoodOn = false;
        		Player[] players = PlayerInfo.Online.Items;
        		for (int i = 0; i < players.Length; i++) {
				Player who = players[i];
				if ((who.Pos.FeetBlockCoords == new Vec3S32(x, y, z) || who.Pos.FeetBlockCoords == new Vec3S32(x, y+1, z)) && who.level == nl.lvl )
					stoodOn = true;
        		}
        		if (!stoodOn) {
        		nl.SetBlock(x, y, z, Block.FromRaw(610));
        		nl.blockEntities[x+" "+y+" "+z].strength = 0;
        		}
        		else {nl.SimulateSetBlock(x, y, z);}
        	};
        }
        
        static NasBlockAction RepeaterAction(int direction, BlockID hereBlock) {
            return (nl,nasBlock,x,y,z) => {
        		
        		int type = 0;
        		if (IsPartOfSet(repeaterSetActive, hereBlock) != -1)
        		{type = 1;}
        		else {type = 0;}
        		if (!nl.blockEntities.ContainsKey(x+" "+y+" "+z)) 
        		{
        		nl.blockEntities.Add(x+" "+y+" "+z, new Entity());
                nl.blockEntities[x+" "+y+" "+z].strength = 0;
                nl.blockEntities[x+" "+y+" "+z].type = direction;
        		}
        		Entity b = nl.blockEntities[x+" "+y+" "+z];
        		Entity strength1 = new Entity();
        		if (direction == 5) {
        			if (nl.blockEntities.ContainsKey(x+" "+(y+1)+" "+z)){
        				strength1 = nl.blockEntities[x+" "+(y+1)+" "+z];
        			}
        		}
        		if (direction == 6) {
        			if (nl.blockEntities.ContainsKey(x+" "+(y-1)+" "+z)){
        				strength1 = nl.blockEntities[x+" "+(y-1)+" "+z];
        			}
        		}
        		if (direction == 7) {
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z+1))){
        				strength1 = nl.blockEntities[x+" "+y+" "+(z+1)];
        			}
        		}
        		if (direction == 8) {
        			if (nl.blockEntities.ContainsKey((x-1)+" "+y+" "+z)){
        				strength1 = nl.blockEntities[(x-1)+" "+y+" "+z];
        			}
        		}
        		if (direction == 9) {
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z-1))){
        				strength1 = nl.blockEntities[x+" "+y+" "+(z-1)];
        			}
        		}
        		if (direction == 10) {
        			if (nl.blockEntities.ContainsKey((x+1)+" "+y+" "+z)){
        				strength1 = nl.blockEntities[(x+1)+" "+y+" "+z];
        			}
        		}
        		NasLevel.QueuedBlockUpdate qb = new NasLevel.QueuedBlockUpdate();
            	qb.x = x;
	            qb.y = y;
	            qb.z = z;
	            float seconds = 0.4f;
	            qb.date = DateTime.UtcNow + TimeSpan.FromSeconds(seconds);
	            qb.date = qb.date.Floor(TimeSpan.FromMilliseconds(100));
	            qb.nb = nasBlock;
	            qb.da = ContRepeaterTask(type, strength1, direction);
	            nl.tickQueue.Enqueue(qb, qb.date);
            };
        }
        		
        static NasBlockAction ContRepeaterTask(int type, Entity strength1, int direction) {
				return (nl,nasBlock,x,y,z) => {
        		Entity b = nl.blockEntities[x+" "+y+" "+z];
        		
        		if (!(strength1.type < 5 || strength1.type == b.type || ((strength1.type == 11 && (b.type == 10 || b.type == 8)) || 
        		      (strength1.type == 12 && (b.type == 5 || b.type == 6)) ||
        		      (strength1.type == 13 && (b.type == 9 || b.type == 7))))) {strength1.strength = 0;}
        		if (type == 0 && strength1.strength > 0 ){
        			nl.SetBlock(x, y, z, repeaterSetActive[direction-5]);
        			b.strength = 15;
        		}
        		if (type == 1 && strength1.strength == 0){
        			nl.SetBlock(x, y, z, repeaterSetInactive[direction-5]);
        			b.strength = 0;
        		}};
        }
        
        static NasBlockAction TurnOffAction() {
        return (nl,nasBlock,x,y,z) => {
        		nl.SetBlock(x, y, z, Block.FromRaw(195));
        		nl.blockEntities[x+" "+y+" "+z].strength = 0;
        	};
        	
        }
        
        static NasBlockAction DispenserAction(int changeX, int changeY, int changeZ) {
        	return (nl,nasBlock,x,y,z) => {
        		
        		
        		Entity[] b = new Entity[6];
        			
        			if (nl.blockEntities.ContainsKey(x+" "+(y+1)+" "+z) && (changeY != 1))
        			{b[0] = nl.blockEntities[x+" "+(y+1)+" "+z];}
        			
        		    if (nl.blockEntities.ContainsKey(x+" "+(y-1)+" "+z) && (changeY != -1))
        			{b[1] = nl.blockEntities[x+" "+(y-1)+" "+z];}
        		
        		    if (nl.blockEntities.ContainsKey((x+1)+" "+y+" "+z) && (changeX != 1))
        			{b[2] = nl.blockEntities[(x+1)+" "+y+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey((x-1)+" "+y+" "+z) && (changeX != -1))
        			{b[3] = nl.blockEntities[(x-1)+" "+y+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z+1)) && (changeZ != 1))
        			{b[4] = nl.blockEntities[x+" "+y+" "+(z+1)];}
        			
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z-1)) && (changeZ != -1))
        			{b[5] = nl.blockEntities[x+" "+y+" "+(z-1)];}
        		
        			bool powered = (
        				((b[0] != null) && (b[0].strength > 0 && (b[0].type == 1 || b[0].type == 4 || b[0].type == 5 || b[0].type == 12))) ||
        				((b[1] != null) && (b[1].strength > 0 && (b[1].type == 1 || b[1].type == 4 || b[1].type == 6 || b[1].type == 12))) ||
        				((b[2] != null) && (b[2].strength > 0 && (b[2].type == 0 || b[2].type == 4 || b[2].type == 10|| b[2].type == 11))) ||
        				((b[3] != null) && (b[3].strength > 0 && (b[3].type == 0 || b[3].type == 4 || b[3].type == 8 || b[3].type == 11))) ||
        				((b[4] != null) && (b[4].strength > 0 && (b[4].type == 2 || b[4].type == 4 || b[4].type == 7 || b[4].type == 13))) ||
        				((b[5] != null) && (b[5].strength > 0 && (b[5].type == 2 || b[5].type == 4 || b[5].type == 9 || b[5].type == 13)))
        			);
        			if (!powered) 
        			{nl.blockEntities[x+" "+y+" "+z].type = 0; return;}
        			if (nl.blockEntities[x+" "+y+" "+z].type == 1) {return;}
        			nl.blockEntities[x+" "+y+" "+z].type = 1;
        			{
        				BlockID checkBlock = nl.GetBlock(x+changeX, y+changeY, z+changeZ);
        				if (!CanPhysicsKillThis(checkBlock) && IsPartOfSet(waterSet, checkBlock) == -1 && IsPartOfSet(lavaSet, checkBlock) == -1) {return;}
        				Entity bEntity = nl.blockEntities[x+" "+y+" "+z];
        				if (bEntity.drop == null || bEntity.drop.blockStacks == null) {return;}
        				BlockStack bs = bEntity.drop.blockStacks[bEntity.drop.blockStacks.Count-1];
        				if (bs.ID == Block.Bedrock) {return;}
        				BlockID clientBlockID = bs.ID;
        				BlockID addedBlockID = 0;
        				if (clientBlockID == 643) {clientBlockID = 9; addedBlockID = 143;}
        				else {if (clientBlockID == 696) {clientBlockID = 10; addedBlockID = 697;}
        				else {if (clientBlockID == 143 && IsPartOfSet(waterSet, checkBlock) != -1) {clientBlockID = 0; addedBlockID = 643;}
        				else {if (clientBlockID == 697 && checkBlock == 10) {clientBlockID = 0; addedBlockID = 696;}}}}
        				bs.amount -= 1;
        				if (bs.amount == 0) {
        					bEntity.drop.blockStacks.Remove(bs);
        				}
        				if (bEntity.drop.blockStacks.Count == 0) {
                        bEntity.drop = null;
                    	}
        				if (addedBlockID == 0) {nl.SetBlock(x+changeX, y+changeY, z+changeZ, Block.FromRaw(clientBlockID)); 
        					if (Get(bs.ID).container != null) {nl.blockEntities.Add(((x+changeX)+" "+(y+changeY)+" "+(z+changeZ)), new Entity());} return;}
        				if (bEntity.drop == null) {
        				//Logger.Log(LogType.Debug, "Dispenser: Placing {0}, putting {1} back in", clientBlockID, addedBlockID);
        				nl.SetBlock(x+changeX, y+changeY, z+changeZ, Block.FromRaw(clientBlockID));
	                    bEntity.drop = new Drop(addedBlockID);
	                    return;
	                	}
	                	foreach (BlockStack stack in bEntity.drop.blockStacks) {
	                    if (stack.ID == addedBlockID) {
	                        if (addedBlockID != 0) stack.amount += 1;
	                        nl.SetBlock(x+changeX, y+changeY, z+changeZ, Block.FromRaw(clientBlockID));
	                        if (Get(bs.ID).container != null) {nl.blockEntities.Add(((x+changeX)+" "+(y+changeY)+" "+(z+changeZ)), new Entity());}
	                        return;
		                    }
		                }
		                
		                if (bEntity.drop.blockStacks.Count >= Container.BlockStackLimit) {
		                    return;
		                }
        				if (addedBlockID != 0) bEntity.drop.blockStacks.Add(new BlockStack(addedBlockID));
	                    nl.SetBlock(x+changeX, y+changeY, z+changeZ, Block.FromRaw(clientBlockID));
	                    if (Get(bs.ID).container != null) {nl.blockEntities.Add(((x+changeX)+" "+(y+changeY)+" "+(z+changeZ)), new Entity());}
        
        				
        				
        			}
        	};
        }
        
        static NasBlockAction SpongeAction() {
        	return (nl,nasBlock,x,y,z) => {
        		bool absorbed = false;
        		for (int xOff = -3; xOff <= 3; xOff++) {
        			for (int yOff = -3; yOff <= 3; yOff++) {
                        for (int zOff = -3; zOff <= 3; zOff++)
                {
                        if (IsPartOfSet(waterSet, nl.GetBlock(x + xOff, y + yOff, z + zOff)) != -1){
                        		nl.SetBlock(x + xOff, y + yOff, z + zOff, Block.Air); absorbed = true;
                        }
                        
                  }}}
        		if (absorbed) {nl.SetBlock(x, y, z, Block.FromRaw(428));}
        		
            };
        }
        
        static NasBlockAction NeedsSupportAction() {
            return (nl,nasBlock,x,y,z) => {
                IsSupported(nl, x, y, z);
            };
        }
        
        
        static NasBlockAction GenericPlantAction() {
            return (nl,nasBlock,x,y,z) => {
                GenericPlantSurvived(nl, x, y, z);
            };
        }
        
        public static BlockID[] leafSet = new BlockID[] { Block.Leaves };
        static NasBlockAction OakSaplingAction() {
            return (nl,nasBlock,x,y,z) => {
                if (!GenericPlantSurvived(nl, x, y, z)) { return; }
                nl.SetBlock(x, y, z, Block.Air);
                NasTree.GenOakTree(nl, r, x, y, z, true);
            };
        }
        
        static NasBlockAction BirchSaplingAction() {
            return (nl,nasBlock,x,y,z) => {
                if (!GenericPlantSurvived(nl, x, y, z)) { return; }
                nl.SetBlock(x, y, z, Block.Air);
                NasTree.GenBirchTree(nl, r, x, y, z, true);
            };
        }
        
       static NasBlockAction SwampSaplingAction() {
            return (nl,nasBlock,x,y,z) => {
                if (!GenericPlantSurvived(nl, x, y, z)) { return; }
                nl.SetBlock(x, y, z, Block.Air);
                NasTree.GenSwampTree(nl, r, x, y, z, true);
            };
        }
        
        static NasBlockAction SpruceSaplingAction() {
            return (nl,nasBlock,x,y,z) => {
                if (!GenericPlantSurvived(nl, x, y, z)) { return; }
                nl.SetBlock(x, y, z, Block.Air);
                NasTree.GenSpruceTree(nl, r, x, y, z, true);
            };
        }
        
        static BlockID[] wheatSet = new BlockID[] { Block.FromRaw(644), Block.FromRaw(645), Block.FromRaw(646), Block.FromRaw(461) };
        static BlockID[] ironSet = new BlockID[] { Block.FromRaw(729), Block.FromRaw(730), Block.FromRaw(731), Block.FromRaw(479) };
        static NasBlockAction CropAction(BlockID[] cropSet, int index) {
            return (nl,nasBlock,x,y,z) => {
                if (!CropSurvived(nl, x, y, z)) { return; }
                if (index+1 >= cropSet.Length) { return; }
                nl.SetBlock(x, y, z, cropSet[index+1]);
            };
        }
        
        static NasBlockAction IronCropAction(BlockID[] cropSet, int index) {
            return (nl,nasBlock,x,y,z) => {
                if (!IronCropSurvived(nl, x, y, z)) { return; }
                if (index+1 >= cropSet.Length) { return; }
                nl.SetBlock(x, y, z, cropSet[index+1]);
            };
        }
        
        static NasBlockAction AutoCraftingAction() {
                return (nl,nasBlock,x,y,z) => {
        			Entity[] b = new Entity[5];
        			if (nl.blockEntities.ContainsKey(x+" "+(y-1)+" "+z)) 
        			{b[0] = nl.blockEntities[x+" "+(y-1)+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey((x+1)+" "+y+" "+z))
        			{b[1] = nl.blockEntities[(x+1)+" "+y+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey((x-1)+" "+y+" "+z))
        			{b[2] = nl.blockEntities[(x-1)+" "+y+" "+z];}
        			
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z+1)))
        			{b[3] = nl.blockEntities[x+" "+y+" "+(z+1)];}
        			
        			if (nl.blockEntities.ContainsKey(x+" "+y+" "+(z-1)))
        			{b[4] = nl.blockEntities[x+" "+y+" "+(z-1)];}

        			
        		bool powered = (
        				((b[0] != null) && (b[0].strength > 0 && (b[0].type == 1 || b[0].type == 4 || b[0].type == 6 || b[0].type == 12))) ||
        				((b[1] != null) && (b[1].strength > 0 && (b[1].type == 0 || b[1].type == 4 || b[1].type == 10|| b[1].type == 11))) ||
        				((b[2] != null) && (b[2].strength > 0 && (b[2].type == 0 || b[2].type == 4 || b[2].type == 8 || b[2].type == 11))) ||
        				((b[3] != null) && (b[3].strength > 0 && (b[3].type == 2 || b[3].type == 4 || b[3].type == 7 || b[3].type == 13))) ||
        				((b[4] != null) && (b[4].strength > 0 && (b[4].type == 2 || b[4].type == 4 || b[4].type == 9 || b[4].type == 13)))
        			);
        			if (!powered) 
        			{nl.blockEntities[x+" "+y+" "+z].type = 0; return;}
        			if (nl.blockEntities[x+" "+y+" "+z].type == 1) {return;}
        			nl.blockEntities[x+" "+y+" "+z].type = 1;
                    lock (Crafting.locker) {
        			Crafting.Recipe recipe = Crafting.GetRecipe(nl, (ushort)x, (ushort)y, (ushort)z, nasBlock.station);
                        if (recipe == null) {
                            return;
                        }
                        Drop dropClone = new Drop(recipe.drop);
                        
                        Crafting.ClearCraftingArea(nl, (ushort)x, (ushort)y, (ushort)z, nasBlock.station.ori);
                        Entity bEntity = nl.blockEntities[x+" "+y+" "+z];
                        if (bEntity.drop == null) {
                    	bEntity.drop = dropClone;
                    	return;
                			}
                        if (dropClone.items != null) {
                        	foreach (Item tool in bEntity.drop.items){
                        		bEntity.drop.items.Add(tool);
                        	}
                        }
                        if (dropClone.blockStacks != null) {
                        bool exists = false;
                        foreach (BlockStack stack in dropClone.blockStacks) {
                        	exists = false;
                        	foreach (BlockStack otherStack in bEntity.drop.blockStacks) {
                        		if (stack.ID == otherStack.ID) {
                        			otherStack.amount += stack.amount;
                        			exists = true;
                        		}}
                        	if (!exists) {bEntity.drop.blockStacks.Add(new BlockStack(stack.ID, stack.amount));}
                        
                        }}
                        }
                    
                };
            }
        
        static bool IsSupported(NasLevel nl, int x, int y, int z) {
            BlockID below = nl.GetBlock(x, y-1, z);
            if (CanPhysicsKillThis(below)) {
                nl.SetBlock(x, y, z, Block.Air);
                return false;
            }
            return true;
        }
        static bool GenericPlantSurvived(NasLevel nl, int x, int y, int z) {
            if (!IsSupported(nl, x, y, z)) { return false; }
            if (!CanPlantsLiveOn(nl.GetBlock(x, y-1, z))) {
                nl.SetBlock(x, y, z, 39);
                return false;
            }
            return true;
        }
        static bool CropSurvived(NasLevel nl, int x, int y, int z) {
            if (!IsSupported(nl, x, y, z)) { return false; }
            if (nl.biome < 0) { return false; }
            if (IsPartOfSet(soilForPlants, nl.GetBlock(x, y-1, z)) == -1 ) {
                nl.SetBlock(x, y, z, 39);
                return false;
            }
            return true;
        }
        
        static bool IronCropSurvived(NasLevel nl, int x, int y, int z) {
            if (!IsSupported(nl, x, y, z)) { return false; }
            if (nl.biome >= 0) { return false; }
            if (IsPartOfSet(soilForIron, nl.GetBlock(x, y-1, z)) == -1 || IsPartOfSet(lavaSet, nl.GetBlock(x, y-2, z)) == -1) {
                nl.SetBlock(x, y, z, 39);
                return false;
            }
            return true;
        }
        
        static BlockID[] soilForPlants = new BlockID[] { Block.Dirt, Block.Extended|144, Block.Extended|685 };
        static BlockID[] soilForIron = new BlockID[] { 48, Block.Extended|452, Block.Extended|451 };
        static bool CanPlantsLiveOn(BlockID block) {
            if (IsPartOfSet(soilForPlants, block) != -1 || IsPartOfSet(grassSet, block) != -1) {
                return true;
            }
            return false;
        }
	  static NasBlockAction OnSoil(BlockID soil) {	
       return (nl,nasBlock,x,y,z) => { 
        		if (!((nl.GetBlock(x, y, z) == nl.GetBlock(x, y-1, z)) | (nl.GetBlock(x, y-1, z) == soil)))
        		{nl.SetBlock(x, y, z, Block.Extended|39); }
        	};
        }
        
    }

}
