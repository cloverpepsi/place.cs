using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using MCGalaxy;
using MCGalaxy.Blocks;
using BlockID = System.UInt16;
using MCGalaxy.Events.PlayerEvents;
using MCGalaxy.Network;
using MCGalaxy.Tasks;
using MCGalaxy.DB;

namespace NotAwesomeSurvival {

    public partial class NasPlayer {
		public static Scheduler savingScheduler;
        public static void Setup() {
            OnPlayerSpawningEvent.Register(OnPlayerSpawning, Priority.High);
            if (savingScheduler == null) savingScheduler = new Scheduler("SavingScheduler");
        }
        public static void TakeDown() {
            OnPlayerSpawningEvent.Unregister(OnPlayerSpawning);
        }

        static void OnPlayerSpawning(Player p, ref Position pos, ref byte yaw, ref byte pitch, bool respawning) {
            NasPlayer np = (NasPlayer)p.Extras[Nas.PlayerKey];
            np.nl = NasLevel.Get(p.level.name);
            np.SpawnPlayer(p.level, ref pos, ref yaw, ref pitch);
        }
		public void SaveAction(SchedulerTask task) {
			string jsonString;
            jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(Nas.GetSavePath(p), jsonString);
		}
        public void SpawnPlayer(Level level, ref Position spawnPos, ref byte yaw, ref byte pitch) {
            if (level.Config.Deletable && level.Config.Buildable) { return; } //not a nas map
            canDoStuffBasedOnPosition = false;
            inventory.Setup();
            
            if (isDead) {
            	if (!headingToBed) {TryDropGravestone();
            	inventory = new Inventory(p);
            	exp = 0;
            	levels = 0;
            	inventory.Setup();
            	inventory.DisplayHeldBlock(NasBlock.Default, 0);}
            	CommandData data = p.DefaultCmdData;
                data.Context = CommandContext.SendCmd;
                Orientation rot = new Orientation(Server.mainLevel.rotx, Server.mainLevel.roty);
                NasEntity.SetLocation(this, spawnMap, spawnCoords, rot);
               	Logger.Log(LogType.Debug, "Teleporting " + p.name + " to their bed!");
            		if (!headingToBed) {
            		p.SendCpeMessage(CpeMessageType.Announcement, "%cY O U  D I E D");
            		Chat.MessageChat(p, reason, null, true);
            		curFogColor = Color.Black;
            		curRenderDistance = 1;
            		HP = maxHP;
            		Air = maxAir;
            		holdingBreath = false;
            		DisplayHealth();
            		}
            		headingToBed = false;
            		isDead = false;
            	}

            
            if (!hasBeenSpawned) { SpawnPlayerFirstTime(level, ref spawnPos, ref yaw, ref pitch); return; }

            if (transferInfo != null) {
                
            	if (transferInfo.travelX == -1) {
                transferInfo.CalcNewPos();
                spawnPos = transferInfo.posBeforeMapChange;
                spawnPos.X = spawnPos.BlockX * 32 + 16;
                spawnPos.Z = spawnPos.BlockZ * 32 + 16;
            	}
            	else {
            		spawnPos = transferInfo.posBeforeMapChange;
            		spawnPos.X = transferInfo.travelX * 32 + 16;
            		spawnPos.Y = transferInfo.travelY * 32 + 51;
                	spawnPos.Z = transferInfo.travelZ * 32 + 16;
                	if (placePortal){
                		int orX = transferInfo.travelX;
                		int orY = transferInfo.travelY;
                		int orZ = transferInfo.travelZ;
                		
                		SetSafetyBlock(orX, orY-1, orZ, Block.FromRaw(162));
            			SetSafetyBlock(orX, orY+2, orZ, Block.FromRaw(162));
            			BlockID temp = nl.GetBlock(orX,orY+1,orZ);
            			if (temp != Block.Air && !nl.blockEntities.ContainsKey(orX+" "+(orY+1)+" "+orZ)) {
            				nl.SetBlock(orX, orY+1, orZ, Block.Air);
            				nl.lvl.BlockDB.Cache.Add(p, (ushort)orX, (ushort)orY, (ushort)orZ, BlockDBFlags.Drawn, temp, Block.Air);
            			}
            			temp = nl.GetBlock(orX,orY,orZ);
            			if (temp != Block.FromRaw(457) && !nl.blockEntities.ContainsKey(orX+" "+orY+" "+orZ)) {
            				nl.SetBlock(orX, orY, orZ, Block.FromRaw(457));
            				nl.lvl.BlockDB.Cache.Add(p, (ushort)orX, (ushort)orY, (ushort)orZ, BlockDBFlags.Drawn, temp, Block.FromRaw(457));
            			}
            			
            			placePortal = false; 
            		}
                	
            	}
                yaw = transferInfo.yawBeforeMapChange;
                pitch = transferInfo.pitchBeforeMapChange;
                
                atBorder = true;
                transferInfo = null;
            }
            
            		
            	
        
            
        }
		
		public void SetSafetyBlock(int x, int y, int z, BlockID block) {
			BlockID oldBlock = nl.GetBlock(x,y,z);
			if (nl.blockEntities.ContainsKey(x+" "+y+" "+z)) {return;}
			if (NasBlock.Get(Collision.ConvertToClientBlockID(oldBlock)).collideAction != NasBlock.DefaultSolidCollideAction()) {
				nl.SetBlock(x,y,z,block);
				nl.lvl.BlockDB.Cache.Add(p, (ushort)x, (ushort)y, (ushort)z, BlockDBFlags.Drawn, oldBlock, block);
				
			}
            	
        }
		
        public void SpawnPlayerFirstTime(Level level, ref Position spawnPos, ref byte yaw, ref byte pitch) {
            if (hasBeenSpawned) { return; }
            atBorder = true;
            if (!p.Model.Contains("|0.93023255813953488372093023255814")) { Command.Find("model").Use(p, "human|0.93023255813953488372093023255814"); }

            spawnPos = new Position(location.X, location.Y, location.Z);
            yaw = this.yaw;
            pitch = this.pitch;
            Logger.Log(LogType.Debug, "Teleporting " + p.name + "!");
			
            if (level.name != levelName) {
                Player.Console.Message("{0}: trying to use /goto to move to the map they logged out in", p.name.ToUpper());
                //goto will call OnPlayerSpawning again to complete the spawn
                CommandData data = p.DefaultCmdData;
                data.Context = CommandContext.SendCmd;
                p.HandleCommand("goto", levelName, data);
                return;
            }
            
            hasBeenSpawned = true;
            //p.Message("hasBeenSpawned set to {0}", hasBeenSpawned);
            Player.Console.Message("{0}: hasBeenSpawned set to {1}", p.name.ToUpper(), hasBeenSpawned);
            
        }
        
        
        [JsonIgnore] int round = 0;
        public void UpdateEnv(){
        	p.level.Config.SkyColor = NasTimeCycle.globalSkyColor;
            p.level.Config.CloudColor = NasTimeCycle.globalCloudColor;
            p.level.Config.LightColor = NasTimeCycle.globalSunColor;
        }
        
        public void DoMovement(Position next, byte yaw, byte pitch) {
            UpdateHeldBlock();
            if (canDoStuffBasedOnPosition) { UpdateAir(); }
            CheckMapCrossing(p.Pos);
            //p.Message("%gPos {0} {1} {2} %b{3}", next.FeetBlockCoords.X, next.FeetBlockCoords.Y, next.FeetBlockCoords.Z, Environment.TickCount);
            if (canDoStuffBasedOnPosition) { DoNasBlockCollideActions(next);}
            if (canDoStuffBasedOnPosition) { UpdatePosition(p.Pos, p.level.name);}
            CheckGround(p.Pos);
            UpdateCaveFog(next);
            //UpdateEnv();
            round++;
        }
        [JsonIgnore] DateTime datePositionCheckingIsAllowed = DateTime.MinValue;
        [JsonIgnore] bool placePortal = false;
        [JsonIgnore] bool canDoStuffBasedOnPosition {
            get {
                if (DateTime.UtcNow >= datePositionCheckingIsAllowed) {
                    //p.Message("canDoStuffBasedOnPosition true");
                    return true;
                }
                //p.Message("canDoStuffBasedOnPosition false");
                return false;
            }
            set {
                //if (p != null) { p.Message("canDoStuffBasedOnPosition: {0}", value); }
                if (!value) { datePositionCheckingIsAllowed = DateTime.UtcNow.AddMilliseconds(2000+p.Ping.HighestPing()); }
            }
        }
        void UpdatePosition(Position pos, string level){
        	location = new MCGalaxy.Maths.Vec3S32(pos.X, pos.Y, pos.Z);
        	levelName = level;
        	
        }
        
        
        void CheckGround(Position next) {
        	if (p.invincible) {lastGroundedLocation = new MCGalaxy.Maths.Vec3S32(next.X, next.Y, next.Z); return;}
            Position below = next;
            below.Y-= 2;
            float fallDamageMultiplier = 1f;
            if (Collision.TouchesGround(p.level, bounds, below, out fallDamageMultiplier)) {
                float fallHeight = lastGroundedLocation.Y - next.Y;
                if (!canDoStuffBasedOnPosition && fallHeight > 0 && !hasBeenSpawned) { p.Message("trying to take fall damage but cant"); }
                if (fallHeight > 0 && canDoStuffBasedOnPosition) {
                    fallHeight /= 32f;
                    fallHeight-= 4;
                    
                    if (fallHeight > 0) {
                        float damage = (int)fallHeight * 2;
                        damage /= 4;

                        //p.Message("damage is {0}", damage*fallDamageMultiplier);
                        TakeDamage(damage*fallDamageMultiplier, DamageSource.Falling);
                    }
                }
                lastGroundedLocation = new MCGalaxy.Maths.Vec3S32(next.X, next.Y, next.Z);
            }
        }
        [JsonIgnore] bool atBorder = true;
        void CheckMapCrossing(Position next) {
            if (next.BlockX <= 0) {
                TryGoMapAt(-1, 0);
                return;
            }
            if (next.BlockX >= p.level.Width - 1) {
                TryGoMapAt(1, 0);
                return;
            }

            if (next.BlockZ <= 0) {
                TryGoMapAt(0, -1);
                return;
            }
            if (next.BlockZ >= p.level.Length - 1) {
                TryGoMapAt(0, 1);
                return;
            }
            atBorder = false;
        }
        public bool TryGoMapAt(int dirX, int dirZ) {
            if (atBorder) {
                //p.Message("Can't do it because already at border");
                return false;
            }
            atBorder = true;
            int chunkOffsetX = 0, chunkOffsetZ = 0;
            string seed = "DEFAULT";
            if (!NasGen.GetSeedAndChunkOffset(p.level.name, ref seed, ref chunkOffsetX, ref chunkOffsetZ)) { return false; }
            string mapName;
            chunkOffsetX += dirX;
            chunkOffsetZ += dirZ;
            mapName = seed+"_"+chunkOffsetX + "," + chunkOffsetZ;
            if (File.Exists("plugins/nas/leveldata/" + mapName + ".json")) {
            	
            	transferInfo = new TransferInfo(p, dirX, dirZ, -1, -1, -1);
                CommandData data = p.DefaultCmdData;
                data.Context = CommandContext.SendCmd;
                p.HandleCommand("goto", mapName, data);
                return true;
            } else {
                if (NasGen.currentlyGenerating) {
                    p.Message("%cA map is already generating!");
                    return false;
                }
                GenInfo info = new GenInfo();
                info.p = p;
                info.mapName = mapName;
                info.seed = seed;
                SchedulerTask taskGenMap;
                taskGenMap = NasGen.genScheduler.QueueOnce(GenTask, info, new TimeSpan(0, 0, 5));
                return false;
            }
        }
        
        public bool NetherTravel(string map, TransferInfo trans) {
            if (atBorder) {
                //p.Message("Can't do it because already at border");
                return false;
            }
            atBorder = true;
            int chunkOffsetX = 0, chunkOffsetZ = 0;
            string seed = "DEFAULT";
            if (!NasGen.GetSeedAndChunkOffset(p.level.name, ref seed, ref chunkOffsetX, ref chunkOffsetZ)) { return false; }
            string mapName;
            mapName = map;
            NasGen.GetSeedAndChunkOffset(map, ref seed, ref chunkOffsetX, ref chunkOffsetZ);
            
            if (File.Exists("plugins/nas/leveldata/" + mapName + ".json")) {
            	
            	transferInfo = trans;
            	placePortal = true;
                CommandData data = p.DefaultCmdData;
                data.Context = CommandContext.SendCmd;
                p.HandleCommand("goto", mapName, data);
                return true;
            } else {
                if (NasGen.currentlyGenerating) {
                    p.Message("%cA map is already generating!");
                    return false;
                }
                GenInfo info = new GenInfo();
                info.p = p;
                info.mapName = mapName;
                info.seed = seed;
                SchedulerTask taskGenMap;
                taskGenMap = NasGen.genScheduler.QueueOnce(GenTask, info, new TimeSpan(0, 0, 5));
                return false;
            }
        }
        
        
        class GenInfo {
            public Player p;
            public string mapName;
            public string seed;
        }
        static void GenTask(SchedulerTask task) {
            GenInfo info = (GenInfo)task.State;
            info.p.Message("Seed is {0}", info.seed);
            Command.Find("newlvl").Use(info.p, info.mapName + " " + NasGen.mapWideness + " " + NasGen.mapTallness + " " + NasGen.mapWideness + " nasgen " + info.seed);
        }
        [JsonIgnore] public TransferInfo transferInfo = null;
        
        public class TransferInfo {
            public TransferInfo(Player p, int chunkOffsetX, int chunkOffsetZ) {
                posBeforeMapChange = p.Pos;
                yawBeforeMapChange = p.Rot.RotY;
                pitchBeforeMapChange = p.Rot.HeadX;
                this.chunkOffsetX = chunkOffsetX;
                this.chunkOffsetZ = chunkOffsetZ;
                travelX = -1;
                travelY = -1;
                travelZ = -1;
            }
        	public TransferInfo(Player p, int chunkOffsetX, int chunkOffsetZ, int x, int y, int z) {
                posBeforeMapChange = p.Pos;
                yawBeforeMapChange = p.Rot.RotY;
                pitchBeforeMapChange = p.Rot.HeadX;
                this.chunkOffsetX = chunkOffsetX;
                this.chunkOffsetZ = chunkOffsetZ;
                travelX = x;
                travelZ = z;
                travelY = y;
                
            }
            public void CalcNewPos() {
                //* 32 because its in player units
                int xOffset = chunkOffsetX * NasGen.mapWideness * 32;
                int zOffset = chunkOffsetZ * NasGen.mapWideness * 32;
                posBeforeMapChange.X -= xOffset;
                posBeforeMapChange.Z -= zOffset;
            }
            [JsonIgnore] public Position posBeforeMapChange;
            [JsonIgnore] public byte yawBeforeMapChange;
            [JsonIgnore] public byte pitchBeforeMapChange;
            [JsonIgnore] public int chunkOffsetX, chunkOffsetZ;
            [JsonIgnore] public int travelX = -1, travelY = -1, travelZ = -1;
        }

        public void UpdateCaveFog(Position next) {
            if (!NasLevel.all.ContainsKey(p.level.name)) { return; }

            const float change = 0.03125f;//0.03125f;
            if (curRenderDistance > targetRenderDistance) {
                curRenderDistance *= 1 - change;
                if (curRenderDistance < targetRenderDistance) { curRenderDistance = targetRenderDistance; }
            } else if (curRenderDistance < targetRenderDistance) {
                curRenderDistance *= 1 + change;
                if (curRenderDistance > targetRenderDistance) { curRenderDistance = targetRenderDistance; }
            }
            curFogColor = ScaleColor(curFogColor, targetFogColor);

            p.Send(Packet.EnvMapProperty(EnvProp.MaxFog, (int)curRenderDistance));
            p.Send(Packet.EnvColor(2, curFogColor.R, curFogColor.G, curFogColor.B));

            NasLevel nl = NasLevel.all[p.level.name];
            int x = next.BlockX;
            int z = next.BlockZ;
            x = Utils.Clamp(x, 0, (ushort)(p.level.Width - 1));
            z = Utils.Clamp(z, 0, (ushort)(p.level.Length - 1));
            ushort height = nl.heightmap[x, z];

            if (next.BlockCoords == p.Pos.BlockCoords) { return; }

            if (height < NasGen.oceanHeight) { height = NasGen.oceanHeight; }


            int distanceBelow = nl.biome < 0 ? 0 : height - next.BlockY;
            int expFog = 0;
            if (distanceBelow >= NasGen.diamondDepth) {
                targetRenderDistance = 128;
                targetFogColor = NasGen.diamondFogColor;
                expFog = 1;
            } else if (distanceBelow >= NasGen.goldDepth) {
                targetRenderDistance = 192;
                targetFogColor = NasGen.goldFogColor;
                expFog = 1;
            } else if (distanceBelow >= NasGen.ironDepth) {
                targetRenderDistance = 192;
                targetFogColor = NasGen.ironFogColor;
                expFog = 1;
            } else if (distanceBelow >= NasGen.coalDepth) {
                targetRenderDistance = 256;
                targetFogColor = NasGen.coalFogColor;
                expFog = 1;
            } else {
                targetRenderDistance = Server.Config.MaxFogDistance;
                targetFogColor = Color.White;
                expFog = 0;
            }
            p.Send(Packet.EnvMapProperty(EnvProp.ExpFog, expFog));
        }

        static Color ScaleColor(Color cur, Color goal) {
            byte R = ScaleChannel(cur.R, goal.R);
            byte G = ScaleChannel(cur.G, goal.G);
            byte B = ScaleChannel(cur.B, goal.B);
            return Color.FromArgb(R, G, B);
        }
        static byte ScaleChannel(byte curChannel, byte goalChannel) {
            if (curChannel > goalChannel) {
        		curChannel--;
            } else if (curChannel < goalChannel) {
                curChannel++;
            }
            return curChannel;
        }
    }

}
