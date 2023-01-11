using System;
using System.Drawing;
using LibNoise;
using MCGalaxy;
using BlockID = System.UInt16;
using MCGalaxy.Tasks;
using MCGalaxy.Generator;
using MCGalaxy.Generator.Foliage;

namespace NotAwesomeSurvival {

    public static class NasTree {
        public static void Setup() {
            
        }
        public static void GenOakTree(NasLevel nl, Random r, int x, int y, int z, bool broadcastChange = false)
        {
            Level lvl = nl.lvl;

            Tree oak;
            oak = new OakTree();

            oak.SetData(r, r.Next(0, 8));
            PlaceBlocks(lvl, oak, x, y, z, broadcastChange);
        }
        
        public static void GenSwampTree(NasLevel nl, Random r, int x, int y, int z, bool broadcastChange = false)
        {
            Level lvl = nl.lvl;

            Tree swamp;
            swamp = new SwampTree();
            swamp.SetData(r, r.Next(4, 8));
            PlaceBlocks(lvl, swamp, x, y, z, broadcastChange);
        }

        public static void GenBirchTree(NasLevel nl, Random r, int x, int y, int z, bool broadcastChange = false)
        {
            Level lvl = nl.lvl;

            Tree birch;
            birch = new BirchTree();
            birch.SetData(r, r.Next(5, 8));
            PlaceBlocks(lvl, birch, x, y, z, broadcastChange);
        }
        
        public static void GenSpruceTree(NasLevel nl, Random r, int x, int y, int z, bool broadcastChange = false)
        {
            Level lvl = nl.lvl;

            Tree spruce;
            spruce = new SpruceTree();

            spruce.SetData(r, r.Next(0, 8));
            PlaceBlocks(lvl, spruce, x, y, z, broadcastChange);
        }
        
        private static void PlaceBlocks(Level lvl, Tree tree, int x, int y, int z, bool broadcastChange) {
            tree.Generate((ushort)x, (ushort)(y), (ushort)z, (X, Y, Z, block) =>
            {
        	    NasLevel nl = NasLevel.Get(lvl.name);
                BlockID here = lvl.GetBlock(X, Y, Z);
                if (NasBlock.CanPhysicsKillThis(here) || NasBlock.IsPartOfSet(NasBlock.leafSet, here) != -1)
                {
                    lvl.SetBlock(X, Y, Z, block); // Thanks Unk!
                    if (broadcastChange)
                    {
                        lvl.BroadcastChange(X, Y, Z, block);
                      	
                    }
                }
            });
        }
    }

}