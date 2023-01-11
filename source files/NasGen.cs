using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using LibNoise;
using MCGalaxy;
using BlockID = System.UInt16;
using MCGalaxy.Tasks;
using MCGalaxy.Generator;
using MCGalaxy.Generator.Foliage;

namespace NotAwesomeSurvival {

    public static class NasGen {
        public const int mapWideness = 384;
        public const int mapTallness = 256;
        public const string seed = "a";
        public const ushort oceanHeight = 60;
        public const ushort coalDepth = 4;
        public const ushort ironDepth = 16;
        public const ushort goldDepth = 30;
        public const ushort diamondDepth = 45;
        public const ushort emeraldDepth = 60;
        public const float coalChance = 1f/8f;
        public const float ironChance = 1f/16f;
        public const float goldChance = 1f/24f;
        public const float quartzChance = 1f/24f;
        public const float diamondChance = 1.5f/48f;
        public const float emeraldChance = 1.25f/48f;
        public static Color coalFogColor;
        public static Color ironFogColor;
        public static Color goldFogColor;
        public static Color diamondFogColor;
        public static Color emeraldFogColor;
        public static Scheduler genScheduler;
        public static BlockID[] stoneTypes = {Block.Sandstone, Block.Stone, 48};
        
        public static void Setup() {
            if (genScheduler == null) genScheduler = new Scheduler("MapGenScheduler");
            MapGen.Register("nasGen", GenType.Advanced, Gen, "hello?");

            coalFogColor = System.Drawing.ColorTranslator.FromHtml("#BCC9E8");
            ironFogColor = System.Drawing.ColorTranslator.FromHtml("#A1A3A8");
            goldFogColor = System.Drawing.ColorTranslator.FromHtml("#7A706A");
            diamondFogColor = System.Drawing.ColorTranslator.FromHtml("#605854");
            emeraldFogColor = System.Drawing.ColorTranslator.FromHtml("#605854");
        }
        public static void TakeDown() {

        }
        /// <summary>
        /// Returns true if seed and offsets were succesfully found
        /// </summary>
        public static bool GetSeedAndChunkOffset(string mapName, ref string seed, ref int chunkOffsetX, ref int chunkOffsetZ) {
            string[] bits = mapName.Split('_');
            if (bits.Length <= 1) { return false; }
            
            seed = bits[0];
            string[] chunks = bits[1].Split(',');
            if (chunks.Length <= 1) { return false; }
            
            if (!Int32.TryParse(chunks[0], out chunkOffsetX)) { return false; }
            if (!Int32.TryParse(chunks[1], out chunkOffsetZ)) { return false; }
            return true;
        }
        
        public static bool currentlyGenerating = false;
        static bool Gen(Player p, Level lvl, string seed) {
        	if (File.Exists("levels/" + lvl.name + ".lvl")) {
        		p.Message("Something weird happened, try going into the map again");
        		return false;
        	}
            currentlyGenerating = true;
            int offsetX = 0, offsetZ = 0;
            int chunkOffsetX = 0, chunkOffsetZ = 0;
            GetSeedAndChunkOffset(lvl.name, ref seed, ref chunkOffsetX, ref chunkOffsetZ);
            
            offsetX = chunkOffsetX * mapWideness;
            offsetZ = chunkOffsetZ * mapWideness;
            offsetX -= chunkOffsetX;
            offsetZ -= chunkOffsetZ;
            p.Message("offsetX offsetZ {0} {1}", offsetX, offsetZ);

            Perlin adjNoise = new Perlin();
            adjNoise.Seed = MapGen.MakeInt(seed);
            Random r = new Random(adjNoise.Seed);
            DateTime dateStart = DateTime.UtcNow;

            GenInstance instance = new GenInstance();
            instance.p = p;
            instance.lvl = lvl;
            instance.adjNoise = adjNoise;
            instance.offsetX = offsetX;
            instance.offsetZ = offsetZ;
            instance.r = r;
            instance.seed = seed;
            instance.biome = (new Random()).Next(0, 7);
            if (lvl.name.CaselessContains("nether")) instance.biome = -1;
            if (lvl.name.CaselessContains("test")) instance.biome = 0;
            instance.Do();

            lvl.Config.Deletable = false;
            lvl.Config.MOTD = "-hax +thirdperson maxspeed=1.5 +hold";
            lvl.Config.GrassGrow = false;
            TimeSpan timeTaken = DateTime.UtcNow.Subtract(dateStart);
            p.Message("Done in {0}", timeTaken.Shorten(true, true));

            //GotoInfo info = new GotoInfo();
            //info.p = p;
            //info.levelName = lvl.name;
            //SchedulerTask task = Server.MainScheduler.QueueOnce(Goto, info, TimeSpan.FromMilliseconds(1500));
            currentlyGenerating = false;
            return true;
        }
       
        public class GenInstance {
            public Player p;
            public Level lvl;
            public NasLevel nl;
            public Perlin adjNoise;
            public float[,] temps;
            public int offsetX, offsetZ;
            public Random r;
            public string seed;
            public int biome;
            BlockID topSoil;
            BlockID soil;

            public void Do() {
            	p.Message("Generating with biome "+biome);
                CalcTemps();
                GenTerrain();
                CalcHeightmap();
                if (biome >= 0) GenSoil();
                GenCaves();
                if (biome < 0) GenRandom();
                GenPlants();
                GenOre();
                GenWaterSources();
                if (biome >= 0) GenDungeons();
                nl.dungeons = true;
                NasLevel.Unload(lvl.name, nl);
            }

            void CalcTemps() {
            	adjNoise.OctaveCount = biome == -1 ? 7 : 2;
                if (biome == 2) lvl.Config.Weather = 2;
                if (biome < 0) {
                	lvl.Config.EdgeLevel = 30;
                	lvl.Config.HorizonBlock = Block.Lava;
                	lvl.Config.CloudsHeight = 300;
                }
            	lvl.SaveSettings();
            	if (biome < 0) {return;}
                p.Message("Calculating temperatures");
                temps = new float[lvl.Width, lvl.Length];
                for (double z = 0; z < lvl.Length; ++z) {
                    for (double x = 0; x < lvl.Width; ++x) {
                        //divide by more for bigger scale
                        double scale = 150;
                        double xVal = (x + offsetX) / scale, zVal = (z + offsetZ) / scale;
                        const double adj = 1;
                        xVal += adj;
                        zVal += adj;
                        float val = (float)adjNoise.GetValue(xVal, 0, zVal);
                        val+= 0.1f;
                        val/= 2;
                        //if (z == 0) { Player.Console.Message("temp is {0}", val); }
                        temps[(int)x, (int)z] = val;
                    }
                }
            }
            void GenTerrain() {
                p.Message("Generating terrain");
                //more frequency = smaller map scale
                adjNoise.Frequency = 0.75;
                adjNoise.OctaveCount = 5;
                DateTime dateStartLayer;
                int counter = 0;
                double width = lvl.Width, height = lvl.Height, length = lvl.Length;

                counter = 0;
                dateStartLayer = DateTime.UtcNow;
                for (double y = 0; y < height; y++) {
                    //p.Message("Starting {0} layer.", ListicleNumber((int)(y+1)));
                    for (double z = 0; z < length; ++z)
                        for (double x = 0; x < width; ++x) {
                    	//p.Message(x+" "+z);
                            //if (y < 128) {
                            //    lvl.SetTile((ushort)x, (ushort)(y), (ushort)z, Block.Stone);
                            //    continue;
                            //} else {
                            //    continue;
                            //}

                            if (y == 0 || (y == height-1 && biome < 0)) {
                                lvl.SetTile((ushort)x, (ushort)(y), (ushort)z, Block.Bedrock);
                                continue;
                            }
                            //p.Message("1");
                            if (y >= height-4 && r.Next(2) == 0 && biome < 0) {
                                lvl.SetTile((ushort)x, (ushort)(y), (ushort)z, Block.Bedrock);
                                continue;
                            }
                            //p.Message("2");
                            double threshDiv = 0;
                            
                            if (biome >= 0) {
                            	threshDiv = temps[(int)x,(int)z];
                            	threshDiv*= 1.5;
                            	if (threshDiv <= 0) { threshDiv = 0; }
                            	if (threshDiv > 1) { threshDiv = 1; }
                			}

                            //threshDiv = 1;
                            
                            
                            //double tallRandom = adjNoise.GetValue((x+offsetX)/500, 0, (z+offsetZ)/500);
                            //tallRandom*= 200;
                            //if (tallRandom <= 0.0) { tallRandom = 0.0; }
                            //else if (tallRandom > 1.0) { tallRandom = 1.0; }
                            
                            //p.Message("3");
                            double averageLandHeightAboveSeaLevel = biome == -1 ? 10 : 1;// - (6*tallRandom);
                            double minimumFlatness = biome == -1 ? 0 : 5;
                            double maxFlatnessAdded = biome == -1 ? 80 : 28;
                            
                            //multiply by more to more strictly follow halfway under = solid, above = air
                            double threshold =
                                (((y + (oceanHeight - averageLandHeightAboveSeaLevel)) / (height)) - 0.5)
                                * (minimumFlatness + (maxFlatnessAdded * threshDiv)); //4.5f
                            //threshold = 0;
                            
                            if (threshold < -1.5) {
                            	if (biome == 1) {
                            		lvl.SetTile((ushort)x, (ushort)(y), (ushort)z, Block.Sandstone);}
                            	else {
                            		if (biome < 0) {
                            			lvl.SetTile((ushort)x, (ushort)(y), (ushort)z, 48);
                            		}
                            		else {lvl.SetTile((ushort)x, (ushort)(y), (ushort)z, Block.Stone);}
                                continue;
                            	}
                            }
                            if (threshold > 1.5) { continue; }
							//p.Message("4");
                            //divide y by less for more "layers"
                            
                            double xVal = (x + offsetX) / 200, yVal = y / (250 + (biome == -1 ? 40 : 150 * threshDiv)), zVal = (z + offsetZ) / 200;
                            const double shrink = 2;
                            xVal *= shrink;
                            yVal *= shrink;
                            zVal *= shrink;
                            const double adj = 1;
                            xVal += adj;
                            yVal += adj;
                            zVal += adj;
                            double value = adjNoise.GetValue(xVal, yVal, zVal);
                            //if (counter % (256*256) == 0) {
                            //    Thread.Sleep(10);
                            //}
                            //counter++;

                            //p.Message("5");


                            if (value > threshold || (biome == 6 && y < oceanHeight - 10)) {
                            	if (biome == 1) {lvl.SetTile((ushort)x, (ushort)(y), (ushort)z, Block.Sandstone);} else {
                            	if (biome < 0) { lvl.SetTile((ushort)x, (ushort)(y), (ushort)z, 48); } else {
                            	lvl.SetTile((ushort)x, (ushort)(y), (ushort)z, Block.Stone);}}
                            } 
                            else if (y < oceanHeight) {
                            	if (biome == 1) { lvl.SetTile((ushort)x, (ushort)(y), (ushort)z, Block.Sand);} else {
                            	if (y == (oceanHeight - 1) && biome == 2) { lvl.SetTile((ushort)x, (ushort)(y), (ushort)z, Block.Ice); } else {
                            	if (biome < 0 && y < oceanHeight/2) {
                            				lvl.SetTile((ushort)x, (ushort)(y), (ushort)z, Block.Lava);
                            			}
                            	else if (biome >= 0) lvl.SetTile((ushort)x, (ushort)(y), (ushort)z, Block.Water);}}
                            }
                           	//p.Message("6");
                    }
                    TimeSpan span = DateTime.UtcNow.Subtract(dateStartLayer);
                    if (span > TimeSpan.FromSeconds(10)) {
                        p.Message("Initial gen {0}% complete.", (int)((y / height) * 100));
                        dateStartLayer = DateTime.UtcNow;
                    }
                }
                p.Message("Initial gen 100% complete.");



            }
            void CalcHeightmap() {
                p.Message("Calculating heightmap");
                nl = new NasLevel();
                nl.heightmap = new ushort[lvl.Width, lvl.Length];
                for (ushort z = 0; z < lvl.Length; ++z)
                    for (ushort x = 0; x < lvl.Width; ++x) {
                        //         skip bedrock
                        for (ushort y = 1; y < lvl.Height; ++y) {
                            BlockID curBlock = lvl.FastGetBlock(x, y, z);
                            if (NasBlock.IsPartOfSet(stoneTypes, curBlock) == -1) {
                                nl.heightmap[x, z] = (ushort)(y - 1);
                                break;
                            }
                        }
                    }
                nl.lvl = lvl;
                nl.biome = biome;
                //NasLevel.all.Add(lvl.name, nl);
            }
            void GenSoil() {
                int width = lvl.Width, height = lvl.Height, length = lvl.Length;
                p.Message("Now creating soil.");
                adjNoise.Seed = MapGen.MakeInt(seed + "soil");
                adjNoise.Frequency = 1;
                adjNoise.OctaveCount = 6;

                for (int y = 0; y < height - 1; y++)
                    for (int z = 0; z < length; ++z)
                        for (int x = 0; x < width; ++x) {
                	if (biome == 1) {soil = Block.Sand;}
                	else {soil = Block.Dirt;}

                	if (NasBlock.IsPartOfSet(stoneTypes, lvl.FastGetBlock((ushort)x, (ushort)y, (ushort)z)) != -1 && (
                		NasBlock.IsPartOfSet(stoneTypes, lvl.FastGetBlock((ushort)x, (ushort)(y+1), (ushort)z)) == -1)
                                && ShouldThereBeSoil(x, y, z)
                               ) {
                                
                                soil = GetSoilType(x, z);
                                if (y <= oceanHeight - 12 && biome != 6) {
                                    soil = Block.Gravel;
                                } else if ((y <= oceanHeight && biome != 6)) {
                                    soil = Block.Sand;
                                }
                                int startY = y;
                                for (int yCol = startY; yCol > startY - 2 - r.Next(0, 2); yCol--) {
                                    if (yCol < 0) { break; }
                                    if (lvl.FastGetBlock((ushort)x, (ushort)(yCol), (ushort)z) == Block.Stone || lvl.FastGetBlock((ushort)x, (ushort)(yCol), (ushort)z) == Block.Sandstone) {
                                        lvl.SetBlock((ushort)x, (ushort)(yCol), (ushort)z, soil);
                                    }
                                }
                            }
                        }
            }
            bool ShouldThereBeSoil(int x, int y, int z) {
                if (
                    IsNeighborLowEnough(x, y, z,-1, 0) ||
                    IsNeighborLowEnough(x, y, z, 1, 0) ||
                    IsNeighborLowEnough(x, y, z, 0,-1) ||
                    IsNeighborLowEnough(x, y, z, 0, 1))
                {
                    return false;
                }
                return true;
            }
            bool IsNeighborLowEnough(int x, int y, int z, int offX, int offZ) {
                int neighborX = x+offX;
                int neighborZ = z+offZ;
                if (neighborX >= lvl.Width  || neighborX < 0 ||
                    neighborZ >= lvl.Length || neighborZ < 0
                   ) { return false; }
                for (int i = 0; i < 4; i++) {
                    if (!lvl.IsAirAt((ushort)neighborX, (ushort)(y-i), (ushort)neighborZ)) {
                        return false;
                    }
                }
                return true;
            }
            void GenCaves() {
                int width = lvl.Width, height = lvl.Height, length = lvl.Length;

                p.Message("Now creating caves");
                adjNoise.Seed = MapGen.MakeInt(seed + "cave");
                adjNoise.Frequency = 1; //more frequency = smaller map scale
                adjNoise.OctaveCount = 2;

                int counter = 0;
                DateTime dateStartLayer = DateTime.UtcNow;
                for (double y = 0; y < height; y++) {
                    //p.Message("Starting {0} layer.", ListicleNumber((int)(y+1)));
                    for (double z = 0; z < length; ++z)
                        for (double x = 0; x < width; ++x) {
                            double threshold = 0.55;
                            int caveHeight = biome < 0 ? height : nl.heightmap[(int)x, (int)z] - 7;
                            if (y > caveHeight) {
                                threshold += 0.05 * (y - (caveHeight));
                            }
                            if (threshold > 1.5) { continue; }
                            bool tryCave = false;
                            BlockID thisBlock = lvl.FastGetBlock((ushort)x, (ushort)(y), (ushort)z);
                            if (thisBlock == Block.Stone || thisBlock == Block.Dirt || thisBlock == Block.Sandstone || thisBlock == 48) { tryCave = true; }
                            if (!tryCave) {
                                continue;
                            }

                            //divide y by less for more "layers"
                            double xVal = (x + offsetX) / 15, yVal = y / 7, zVal = (z + offsetZ) / 15;
                            const double adj = 1;
                            xVal += adj;
                            yVal += adj;
                            zVal += adj;
                            double value = adjNoise.GetValue(xVal, yVal, zVal);

                            //if (counter % (256*256) == 0) {
                            //    Thread.Sleep(10);
                            //}
                            counter++;

                            if (value > threshold) {
                                if (y <= 4) {
                                    lvl.SetTile((ushort)x, (ushort)(y), (ushort)z, Block.Lava);
                                } else {
                                    lvl.SetTile((ushort)x, (ushort)(y), (ushort)z, Block.Air);
                                }
                            }
                        }
                	
                
                    
                    TimeSpan span = DateTime.UtcNow.Subtract(dateStartLayer);
                    if (span > TimeSpan.FromSeconds(10)) {
                        p.Message("Cave gen {0}% complete.", (int)((y / height) * 100));
                        dateStartLayer = DateTime.UtcNow;
                    }
                }
                p.Message("Cave gen 100% complete.");
            }
            
           void GenRandom() {
                int width = lvl.Width, height = lvl.Height, length = lvl.Length;

                p.Message("Now creating random patches");
                adjNoise.Seed = MapGen.MakeInt(seed + "random");
                adjNoise.Frequency = 1; //more frequency = smaller map scale
                adjNoise.OctaveCount = 2;
                adjNoise.Persistence = 0.25;

                int counter = 0;
                DateTime dateStartLayer = DateTime.UtcNow;
                for (double y = 0; y < height; y++) {
                    //p.Message("Starting {0} layer.", ListicleNumber((int)(y+1)));
                    for (double z = 0; z < length; ++z)
                        for (double x = 0; x < width; ++x) {
                            double threshold = 0.7;
                            bool tryPlace = false;
                            BlockID thisBlock = lvl.FastGetBlock((ushort)x, (ushort)(y), (ushort)z);
                            if (thisBlock == Block.Stone || thisBlock == Block.Dirt || thisBlock == Block.Sandstone || thisBlock == 48) { tryPlace = true; }
                            if (!tryPlace) {
                                continue;
                            }

                            //divide y by less for more "layers"
                            double xVal = (x + offsetX) / 35, yVal = y / 35 , zVal = (z + offsetZ) / 35;
                            const double adj = 1;
                            xVal += adj;
                            yVal += adj;
                            zVal += adj;
                            double value = adjNoise.GetValue(xVal, yVal, zVal);

                            //if (counter % (256*256) == 0) {
                            //    Thread.Sleep(10);
                            //}
                            counter++;

                            if (value > threshold) {
                            	lvl.SetBlock((ushort)x, (ushort)(y), (ushort)z, Block.FromRaw(451));
                            }
                        }
                	
                
                    
                    TimeSpan span = DateTime.UtcNow.Subtract(dateStartLayer);
                    if (span > TimeSpan.FromSeconds(10)) {
                        p.Message("Random gen {0}% complete.", (int)((y / height) * 100));
                        dateStartLayer = DateTime.UtcNow;
                    }
                }
                p.Message("Random gen 100% complete.");
            }
            
            void GenPlants() {
                p.Message("Now creating foliage");
                if (biome < 0) {
				for (ushort y = 0; y < (ushort)(lvl.Height - 1); y++)
                    for (ushort z = 0; z < lvl.Length; ++z)
                        for (ushort x = 0; x < lvl.Width; ++x) {     
					if (lvl.FastGetBlock(x,(ushort)(y+1),z) == Block.Air && lvl.FastGetBlock(x,y,z) == 48) {
						 if (r.Next(0, 320) == 0) lvl.SetTile(x,(ushort)(y+1),z,Block.Fire);
						 if (r.Next(0, 320) == 0) lvl.SetBlock(x,(ushort)(y+1),z,Block.FromRaw(456));
					}
					}
            		return;
            	}
                adjNoise.Seed = MapGen.MakeInt(seed + "tree");
                adjNoise.Frequency = 1;
                adjNoise.OctaveCount = 1;

                for (int y = 0; y < (ushort)(lvl.Height - 1); y++)
                    for (int z = 0; z < lvl.Length; ++z)
                        for (int x = 0; x < lvl.Width; ++x) {
                            topSoil = Block.Extended|129; //Block.Grass;
                            if (biome == 1) {topSoil = Block.Sand;}
                            if (biome == 2) {topSoil = Block.FromRaw(139);}
                            if ((lvl.FastGetBlock((ushort)x, (ushort)y, (ushort)z) == Block.Dirt || (lvl.FastGetBlock((ushort)x, (ushort)y, (ushort)z) == Block.Sand && biome == 1)) &&
                                lvl.FastGetBlock((ushort)x, (ushort)(y + 1), (ushort)z) == Block.Air) {
                            	if (((r.Next(0, 50) == 0 && biome != 3 && biome != 4 && biome != 6)||(r.Next(0, 15) == 0 && (biome == 3 || biome == 4 || biome == 6)))&& lvl.IsAirAt((ushort)x, (ushort)(y + 10), (ushort)z)) {
                                   
                                    double xVal = ((double)x + offsetX) / 200, yVal = (double)y / 130, zVal = ((double)z + offsetZ) / 200;
                                    const double adj = 1;
                                    xVal += adj;
                                    yVal += adj;
                                    zVal += adj;
                                    double value = adjNoise.GetValue(xVal, yVal, zVal);
                                    
                                    if (value > r.NextDouble() || biome == 3 || biome == 4 || biome == 6) {
                                        GenTree((ushort)x, (ushort)(y+1), (ushort)z);
                                    } else if (r.Next(0, 20) == 0) {
                                        GenTree((ushort)x, (ushort)(y+1), (ushort)z);
                                    }
                            	} else if (biome != 1) {
                            		
                            		if (r.Next(0, 10) == 0) {
                                    //tallgrass 40 wettallgrass Block.Extended|130
                                    lvl.SetBlock((ushort)x, (ushort)(y+1), (ushort)z, Block.Extended|130);}
                            		else {if (biome == 2) {
                                    lvl.SetBlock((ushort)x, (ushort)(y+1), (ushort)z, Block.Snow);}
                                    if (biome == 5) {
                                    	if (r.Next(0, 2) == 0) {
                        				int flowerChance = r.Next(0, 10);
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
                                    	}
                        				}}}}}
                                    }
                                    }
                            	}
                                
                                lvl.SetBlock((ushort)x, (ushort)(y), (ushort)z, topSoil);
                            }
                        }
                p.Message("Foliage gen complete.");
                if (biome != 6) return;
                for (int z = 0; z < lvl.Length; ++z)
                    for (int x = 0; x < lvl.Width; ++x) {
                	if (NasBlock.IsPartOfSet(NasBlock.waterSet, lvl.FastGetBlock((ushort)x, oceanHeight-1, (ushort)z)) != -1) {
                		if (lvl.FastGetBlock((ushort)x, oceanHeight, (ushort)z) != 0) continue;
                		if (r.NextDouble() <= 0.05) 
                			lvl.SetBlock((ushort)x, oceanHeight, (ushort)z, Block.FromRaw(449));
                	}
                }
                
            }
            void GenTree(ushort x, ushort y, ushort z) {
            	if (biome == 3) {
            		if (r.Next(3) == 0) return;
            		NasTree.GenBirchTree(nl, r, x, y, z);
            		return;
            	}
            	if (biome == 4) {
            		if (r.Next(3) == 0) return;
            		NasTree.GenOakTree(nl, r, x, y, z);
            		return;
            	}
            	 if (biome == 6) {
            		if (r.Next(2) != 0) return;
            		if (y > oceanHeight + 6) return;
            		NasTree.GenSwampTree(nl, r, x, y, z);
            		return;
            	}
            	if (biome == 1) {
            	if (r.Next(5) == 0) {
                lvl.SetBlock((ushort)x, (ushort)(y), (ushort)z, Block.Extended|106);
                lvl.SetBlock((ushort)x, (ushort)(y+1), (ushort)z, Block.Extended|106);
                lvl.SetBlock((ushort)x, (ushort)(y+2), (ushort)z, Block.Extended|106);
                return;
            		}
            	}
            	else {
            	if (biome == 2) {
            		NasTree.GenSpruceTree(nl, r, x, y, z);
            	}
            	else {
            	topSoil = Block.Dirt;
                if (r.Next(5) == 0)
                {
                    NasTree.GenBirchTree(nl, r, x, y, z);
                }
                else
                {
                    NasTree.GenOakTree(nl, r, x, y, z);
                }
            		}
            	}
        }
            
            

            
            BlockID GetSoilType(int x, int z) {
                if (biome == 1) {
                    return Block.Sand;
                }
                return Block.Dirt;
            }
            
            void GenOre() {
                for (int y = 0; y < (ushort)lvl.Height - 1; y++)
                    for (int z = 0; z < lvl.Length; ++z)
                        for (int x = 0; x < lvl.Width; ++x) {
                            BlockID curBlock = lvl.FastGetBlock((ushort)x, (ushort)(y), (ushort)z);
                            if (NasBlock.IsPartOfSet(stoneTypes, curBlock) == -1) {continue;}
                            if (biome >= 0) {
                            TryGenOre(x, y, z, ironDepth, ironChance, 628, 3);
                            TryGenOre(x, y, z, goldDepth, goldChance, 629, 3);
                            TryGenOre(x, y, z, diamondDepth, diamondChance, 630, 2);
                            TryGenOre(x, y, z, emeraldDepth, emeraldChance, 649, 1);
                            TryGenOre(x, y, z, coalDepth, coalChance, 627, r.Next(3, 4), 0.5);
                            }
                            if (biome == 1) {TryGenOre(x, y, z, coalDepth, quartzChance, 586, 3);}
                            if (biome < 0) {
                            	TryGenOre(x, y, z, -1000, coalChance, 454, 3);
                            	TryGenOre(x, y, z, -1000, goldChance, 455, 2);
                            }
                        }
                
                for (ushort xPl = 0; xPl <= 383; xPl++) {
                		for (ushort yPl = 0; yPl <= 20; yPl++) {
                			for (ushort zPl = 0; zPl <= 383; zPl++) {
                			if (yPl <= 10 || r.Next(yPl-9) == 0) {
                				if (lvl.FastGetBlock(xPl, yPl, zPl) == Block.Stone || lvl.FastGetBlock(xPl, yPl, zPl) == 48){
                				lvl.SetBlock(xPl, yPl, zPl, biome >= 0 ? Block.FromRaw(429) : Block.FromRaw(452));
                				}
                			}
                		}
                	}
                }
            }
            bool TryGenOre(int x, int y, int z, int oreDepth, float oreChance, BlockID oreID, int size = 0, double vsf = 0.4) {
                double chance = (double)(oreChance / 100);
                int height = nl.heightmap[x, z];
                if (height < oceanHeight) { height = oceanHeight; }
                int hmbyhttdfttrh = lvl.Height - height;
                hmbyhttdfttrh += oreDepth;

                if (y <= lvl.Height - hmbyhttdfttrh
                    && r.NextDouble() <= chance
                   ) {
                    //if (r.NextDouble() > 0.5) {
                    //    if (!BlockExposed(lvl, x, y, z)) { return false; }
                    //}
                    
                    GenerateOreCluster(x, y, z, oreID, size, vsf);
                    return true;
                }
                return false;
            }

            void GenerateOreCluster(int x, int y, int z, BlockID oreID, int iteration, double chance = 0.4){
            	if (x < 0 || y < 0 || z < 0 || x > mapWideness - 1 || z > mapWideness - 1 || y > mapTallness - 1) 
            	{return;}
            	BlockID hereBlock = lvl.FastGetBlock((ushort)x, (ushort)y, (ushort)z);
            	if (hereBlock == Block.Stone || hereBlock == Block.Sandstone || hereBlock == 48) {
            		lvl.SetBlock((ushort)x, (ushort)y, (ushort)z, Block.FromRaw(oreID));
            	}
            	else {return;}
            	iteration--;
            	if (iteration == 0) {return;}
            	Random rng = new Random();
            	if (rng.NextDouble() <= chance) {GenerateOreCluster(x+1, y, z, oreID, iteration);}
            	if (rng.NextDouble() <= chance) {GenerateOreCluster(x-1, y, z, oreID, iteration);}
            	if (rng.NextDouble() <= chance) {GenerateOreCluster(x, y+1, z, oreID, iteration);}
            	if (rng.NextDouble() <= chance) {GenerateOreCluster(x, y-1, z, oreID, iteration);}
            	if (rng.NextDouble() <= chance) {GenerateOreCluster(x, y, z+1, oreID, iteration);}
            	if (rng.NextDouble() <= chance) {GenerateOreCluster(x, y, z-1, oreID, iteration);}
            	
            }
            
            
            
            void GenWaterSources() {
            	if (biome == 6) {
            		lvl.CustomBlockDefs[8] = BlockDefinition.GlobalDefs[8].Copy();
            		lvl.CustomBlockDefs[8].Name = "#Water";
            		lvl.CustomBlockDefs[8].FogR = 72;
            		lvl.CustomBlockDefs[8].FogG = 94;
            		lvl.CustomBlockDefs[8].FogB = 24;
            		lvl.CustomBlockDefs[Block.FromRaw(129)] = BlockDefinition.GlobalDefs[Block.FromRaw(129)].Copy();
            		lvl.CustomBlockDefs[Block.FromRaw(129)].Name = "#Grass";
            		lvl.CustomBlockDefs[Block.FromRaw(129)].FogR = 176;
            		lvl.CustomBlockDefs[Block.FromRaw(129)].FogG = 191;
            		lvl.CustomBlockDefs[Block.FromRaw(129)].FogB = 176;
            		lvl.CustomBlockDefs[3] = BlockDefinition.GlobalDefs[3].Copy();
            		lvl.CustomBlockDefs[3].Name = "#Dirt";
            		lvl.CustomBlockDefs[3].FogR = 176;
            		lvl.CustomBlockDefs[3].FogG = 191;
            		lvl.CustomBlockDefs[3].FogB = 176;
            		lvl.CustomBlockDefs[Block.FromRaw(130)] = BlockDefinition.GlobalDefs[Block.FromRaw(130)].Copy();
            		lvl.CustomBlockDefs[Block.FromRaw(130)].Name = "#Tall grass";
            		lvl.CustomBlockDefs[Block.FromRaw(130)].FogR = 176;
            		lvl.CustomBlockDefs[Block.FromRaw(130)].FogG = 191;
            		lvl.CustomBlockDefs[Block.FromRaw(130)].FogB = 176;
            		BlockDefinition.Save(false, lvl);
            	}
                for (int y = 0; y < lvl.Height - 1; y++)
                    for (int z = 0; z < lvl.Length; ++z)
                        for (int x = 0; x < lvl.Width; ++x) {
                            BlockID curBlock = lvl.FastGetBlock((ushort)x, (ushort)(y), (ushort)z);
                            
                            if (curBlock == Block.Lava) {
                            	if (BlockExposed2(x,y,z)) nl.blocksThatMustBeDisturbed.Add(new NasLevel.BlockLocation(x,y,z));
                            }
                            
                            if (NasBlock.IsPartOfSet(stoneTypes, curBlock) == -1) { continue; }
                            if (r.NextDouble() < 0.00025) {
                                if (BlockExposed(x, y, z)) {
                                    if (NasBlock.IsPartOfSet(stoneTypes, lvl.FastGetBlock((ushort)x, (ushort)(y+1), (ushort)z)) == -1) { continue; }
                                    //Player.Console.Message("Generating water source");
                                    lvl.SetTile((ushort)x, (ushort)y, (ushort)z, (byte)(biome < 0 ? 10 : 9));
                                    nl.blocksThatMustBeDisturbed.Add(new NasLevel.BlockLocation(x, y, z));
                                }
                            }
                        }
            }
            
            bool BlockExposed(int x, int y, int z) {
                if (lvl.IsAirAt((ushort)(x + 1), (ushort)y, (ushort)z)) { return true; }
                if (lvl.IsAirAt((ushort)(x - 1), (ushort)y, (ushort)z)) { return true; }
                if (lvl.IsAirAt((ushort)x, (ushort)(y + 1), (ushort)z)) { return true; }
                if (lvl.IsAirAt((ushort)x, (ushort)(y - 1), (ushort)z)) { return true; }
                if (lvl.IsAirAt((ushort)x, (ushort)y, (ushort)(z + 1))) { return true; }
                if (lvl.IsAirAt((ushort)x, (ushort)y, (ushort)(z - 1))) { return true; }
                return false;
            }
            bool BlockExposed2(int x, int y, int z) {
                if (lvl.IsAirAt((ushort)(x + 1), (ushort)y, (ushort)z)) { return true; }
                if (lvl.IsAirAt((ushort)(x - 1), (ushort)y, (ushort)z)) { return true; }
                if (lvl.IsAirAt((ushort)x, (ushort)y, (ushort)(z + 1))) { return true; }
                if (lvl.IsAirAt((ushort)x, (ushort)y, (ushort)(z - 1))) { return true; }
                return false;
            }
            
            void GenDungeons() {
            	p.Message("Generating structures");
            	int dungeonCount = r.Next(3,6);
            	for (int done = 0; done <= dungeonCount; done++) {
            		GenerateDungeon(r, lvl, nl);
            	}
            	
            }
            
            public static void GenerateDungeon(Random rng, Level level, NasLevel nsl) {
            	int genX = rng.Next(10, NasGen.mapWideness - 10);
            	int genZ = rng.Next(10, NasGen.mapWideness - 10);
            	int genY = rng.Next(10, Math.Max(nsl.heightmap[genX, genZ] - 10, 10));
            	
            	
            	for (int dx = 0; dx < 9; dx++)
            		for (int dy = 0; dy < 7; dy++)
            			for (int dz = 0; dz < 9; dz++) {
            		if (rng.Next(0,3) == 0) level.SetBlock((ushort)(genX+dx), (ushort)(genY+dy), (ushort)(genZ+dz), Block.FromRaw(180));
            		else level.SetTile((ushort)(genX+dx), (ushort)(genY+dy), (ushort)(genZ+dz), Block.StoneBrick);
            	}
            	for (int dx = 1; dx < 8; dx++)
            		for (int dy = 2; dy < 6; dy++)
            			for (int dz = 1; dz < 8; dz++) {
            		level.SetTile((ushort)(genX+dx), (ushort)(genY+dy), (ushort)(genZ+dz), Block.Air);
            	}
            	
            	int dungeonType = rng.Next(0,6);
            	
            	
            	
            	if (dungeonType == 0) {
            		
            		for (int dx = 2; dx < 7; dx++)
            			for (int dz = 2; dz < 7; dz++) {
            			level.SetBlock((ushort)(genX+dx), (ushort)(genY+2), (ushort)(genZ+dz), Block.FromRaw(476));
            		}
            		level.SetTile((ushort)(genX+3), (ushort)(genY+2), (ushort)(genZ+4), Block.Air);
            		level.SetTile((ushort)(genX+5), (ushort)(genY+2), (ushort)(genZ+4), Block.Air);
            		level.SetTile((ushort)(genX+4), (ushort)(genY+2), (ushort)(genZ+3), Block.Air);
            		level.SetTile((ushort)(genX+4), (ushort)(genY+2), (ushort)(genZ+5), Block.Air);
            		level.SetTile((ushort)(genX+3), (ushort)(genY+1), (ushort)(genZ+4), Block.Air);
            		level.SetTile((ushort)(genX+5), (ushort)(genY+1), (ushort)(genZ+4), Block.Air);
            		level.SetTile((ushort)(genX+4), (ushort)(genY+1), (ushort)(genZ+3), Block.Air);
            		level.SetTile((ushort)(genX+4), (ushort)(genY+1), (ushort)(genZ+5), Block.Air);
            		
            		level.SetTile((ushort)(genX+4), (ushort)(genY+4), (ushort)(genZ+4), Block.StoneBrick);
            		level.SetTile((ushort)(genX+4), (ushort)(genY+5), (ushort)(genZ+4), Block.Lava);
            		nsl.blocksThatMustBeDisturbed.Add(new NasLevel.BlockLocation(genX+4, genY+5, genZ+4));
            		
            		GenLoot(genX+4, genY+2, genZ+4, level, rng, nsl);
            		return;
            	}
            	
            	if (dungeonType == 1) {
            		
            		level.SetTile((ushort)(genX+2), (ushort)(genY+1), (ushort)(genZ+2), 48);
            		level.SetTile((ushort)(genX+2), (ushort)(genY+1), (ushort)(genZ+6), 48);
            		level.SetTile((ushort)(genX+6), (ushort)(genY+1), (ushort)(genZ+2), 48);
            		level.SetTile((ushort)(genX+6), (ushort)(genY+1), (ushort)(genZ+6), 48);
            		level.SetBlock((ushort)(genX+2), (ushort)(genY+1), (ushort)(genZ+4), Block.FromRaw(469));
            		level.SetBlock((ushort)(genX+6), (ushort)(genY+1), (ushort)(genZ+4), Block.FromRaw(469));
            		level.SetBlock((ushort)(genX+4), (ushort)(genY+1), (ushort)(genZ+2), Block.FromRaw(469));
            		level.SetBlock((ushort)(genX+4), (ushort)(genY+1), (ushort)(genZ+6), Block.FromRaw(469));
            		
            		level.SetBlock((ushort)(genX+4), (ushort)(genY+2), (ushort)(genZ+4), Block.FromRaw(457));
            		
            		/*for (int dx = 1; dx < 8; dx++)
            			for (int dz = 1; dz < 8; dz++) {
            			if (rng.Next(8) == 0) {
            				level.SetBlock((ushort)(genX+dx), (ushort)(genY+5), (ushort)(genZ+dz), Block.FromRaw(107));
            				if (rng.Next(2) == 0) {
            					level.SetBlock((ushort)(genX+dx), (ushort)(genY+4), (ushort)(genZ+dz), Block.FromRaw(107));
            				}
            			}
            		}*/
            		
            		GenLoot(genX+1, genY+2, genZ+1, level, rng, nsl);
            		GenLoot(genX+7, genY+2, genZ+7, level, rng, nsl);
            		return;
            	}
            	
            	if (dungeonType == 2) {
            		
            		for (int dx = 1; dx < 8; dx++)
            			for (int dz = 1; dz < 8; dz++) {
            			level.SetTile((ushort)(genX+dx), (ushort)(genY+1), (ushort)(genZ+dz), Block.Lava);
            		}
            		for (int dx = 1; dx < 8; dx++)
            			for (int dz = 1; dz < 8; dz++) {
            			level.SetBlock((ushort)(genX+dx), (ushort)(genY+2), (ushort)(genZ+dz), (rng.Next(2) == 0)? Block.StoneBrick : Block.FromRaw(685));
            		}
            		
            		GenLoot(genX+4, genY+3, genZ+4, level, rng, nsl);
            		return;
            	}
            	
            	if (dungeonType == 3) {
            		for (int count = 0; count < 4; count++) {
            			int dx = rng.Next(1,8);
						int dz = rng.Next(1,8);
						level.SetBlock((ushort)(genX+dx), (ushort)(genY+2), (ushort)(genZ+dz), Block.FromRaw(604));
            		}
            		
            		for (int count = 0; count < 4; count++) {
            			int dx = rng.Next(1,8);
						int dz = rng.Next(1,8);
						level.SetBlock((ushort)(genX+dx), (ushort)(genY+2), (ushort)(genZ+dz), Block.FromRaw(653));
            		}
            		
            		GenLoot(genX+4, genY+2, genZ+4, level, rng, nsl);
            		return;
            	}
            	
            	if (dungeonType == 4) {
            		
            		for (int dx = 1; dx < 8; dx++)
            			for (int dy = 1; dy < 6; dy++)
            				for (int dz = 1; dz < 8; dz++) {
            		if (rng.Next(8) == 0) level.SetTile((ushort)(genX+dx), (ushort)(genY+dy), (ushort)(genZ+dz), Block.Lava);
            		else level.SetTile((ushort)(genX+dx), (ushort)(genY+dy), (ushort)(genZ+dz), Block.Stone);
            	}
            		
            		GenLoot(genX+4, genY+1, genZ+4, level, rng, nsl);
            		return;
            	}
            	
            	if (dungeonType == 5) {
            		
            		for (int dx = 1; dx < 8; dx++)
            			for (int dz = 1; dz < 8; dz++) {
            			level.SetBlock((ushort)(genX+dx), (ushort)(genY+1), (ushort)(genZ+dz), Block.FromRaw(129));
            		}
            		
            		level.SetBlock((ushort)(genX+4), (ushort)(genY+3), (ushort)(genZ+4), Block.FromRaw(171));
            		NasBlock.Entity bEntity = new NasBlock.Entity();
            		bEntity.blockText = "&mCongratulations. You touched grass.";
            		if (!nsl.blockEntities.ContainsKey((genX+4)+" "+(genY+3)+" "+(genZ+4))) nsl.blockEntities.Add((genX+4)+" "+(genY+3)+" "+(genZ+4), bEntity);
            		
            		GenLoot(genX+4, genY+2, genZ+4, level, rng, nsl);
            		return;
            	}
            	
            }
            
            static void GenLoot(int x, int y, int z, Level level, Random rng, NasLevel nsl) {
            	
            	level.SetBlock((ushort)x, (ushort)y, (ushort)z, Block.FromRaw(647));
            	
            	NasBlock.Entity bEntity = new NasBlock.Entity();
            	
            	bEntity.drop = new Drop(41, rng.Next(1,5)); //gold
            	bEntity.drop.blockStacks.Add(new BlockStack(729, rng.Next(0,3)));
            	
            	if (rng.Next(2) == 0) bEntity.drop.blockStacks.Add(new BlockStack(631, rng.Next(1,3))); //dia
            	if (rng.Next(4) == 0) bEntity.drop.blockStacks.Add(new BlockStack(650)); //ems
            	
            	
            	if (rng.Next(3) == 0) bEntity.drop.blockStacks.Add(new BlockStack(478)); //gapple
            	if (rng.Next(3) == 0) bEntity.drop.blockStacks.Add(new BlockStack(204)); //monitor
            	if (nsl.blockEntities.ContainsKey(x+" "+y+" "+z)) {
                        nsl.blockEntities.Remove(x+" "+y+" "+z);
                  }
            	nsl.blockEntities.Add(x+" "+y+" "+z, bEntity);
            }
            
            
            
        }
        
        //public class Biome {
        //    BlockID topSoil;
        //    BlockID soil;
        //    Tree treeType;
        //    BlockID treeLeaves;
        //    BlockID treeTrunk;
        //}



        //class GotoInfo {
        //    public Player p;
        //    public string levelName;
        //}
        //static void Goto(SchedulerTask task) {
        //    GotoInfo info = (GotoInfo)task.State;
        //    Command.Find("goto").Use(info.p, info.levelName);
        //}
    }

}
