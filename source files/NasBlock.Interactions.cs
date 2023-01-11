//refrence System.dll
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using MCGalaxy;
using MCGalaxy.Blocks;
using MCGalaxy.Events.PlayerEvents;
using MCGalaxy.Maths;
using MCGalaxy.Tasks;
using MCGalaxy.Network;
using MCGalaxy.DB;
using BlockID = System.UInt16;
using NasBlockInteraction =
    System.Action<NotAwesomeSurvival.NasPlayer, MCGalaxy.Events.PlayerEvents.MouseButton, MCGalaxy.Events.PlayerEvents.MouseAction,
    NotAwesomeSurvival.NasBlock, ushort, ushort, ushort>;
using NasBlockExistAction =
    System.Action<NotAwesomeSurvival.NasPlayer,
    NotAwesomeSurvival.NasBlock, bool, ushort, ushort, ushort>;
using NasBlockAction = System.Action<NotAwesomeSurvival.NasLevel, int, int, int>;
namespace NotAwesomeSurvival {

    public partial class NasBlock {
            
            public static string Path = "plugins/nas/";
        	public static string SavePath = Path + "playerdata/";

        public static string GetTextPath(Player p) {
            return SavePath + p.name + "text.txt";}
        	

        	
            public class Entity {
                public bool CanAccess(Player p) {
                    return lockedBy.Length == 0 || lockedBy == p.name;
                }
                [JsonIgnore] public string FormattedNameOfLocker {
                    get {
                        if (lockedBy.Length == 0) { return "no one"; }
                        Player locker = PlayerInfo.FindExact(lockedBy);
                        return locker == null ? lockedBy : locker.ColoredName;
                    } }
                public string lockedBy = "";
                public string blockText = "";
                public int strength = 0;
                public Drop drop = null;
                public int type = 0;
                public int direction = 0;
            }
            
            public class Container {
                public const int ToolLimit = 10;
                public const int BlockStackLimit = 10;
                public static readonly object locker = new object();
                public enum Type { Chest, Barrel, Crate, Gravestone, AutoCraft, Dispenser }
                public Type type;
                public string name { get { return Type.GetName(typeof(Type), type); } }
                public string description { get {
                        string desc = "%s";
                        switch (type) {
                            case NasBlock.Container.Type.Chest:
                                desc += name+"s%S can store %btools%S, with a limit of "+ToolLimit+".";
                                break;
                            case NasBlock.Container.Type.Barrel:
                            case NasBlock.Container.Type.Crate:
                            case NasBlock.Container.Type.Dispenser:
                                desc += name+"s%S can store %bblock%S stacks, with a limit of "+BlockStackLimit+".";
                                break;
                            default:
                                throw new Exception("Invalid value for Type");
                        }
                        return desc;
                    } }
                public Container() { }
                public Container(Container parent) {
                    type = parent.type;
                }
            }
            
            static NasBlockExistAction WaterExistAction() {
                return (np,nasBlock,exists,x,y,z) => {
                    if (exists) {
                        //give back barrel
                        np.inventory.SetAmount(143, 1, false, false);
                    }
                };
            }
            
        	static NasBlockExistAction LavaExistAction() {
                return (np,nasBlock,exists,x,y,z) => {
                    if (exists) {
                        //give back barrel
                        np.inventory.SetAmount(697, 1, false, false);
                    }
                };
            }
        	
        	static List<string> books = Utils.ReadAllLinesList("text/BookTitles.txt");
        	static List<string> authors = Utils.ReadAllLinesList("text/BookAuthors.txt");
        	static NasBlockInteraction BookshelfInteraction() {
        		return (np,button,action,nasBlock,x,y,z) => {
                    if (action == MouseAction.Pressed) { return; }
                    np.p.Message(books[r.Next(0,99)]+" by "+authors[r.Next(0,74)]);
                };
        	}
        	
        	
            static NasBlockInteraction CrateInteraction(string text) {
                return (np,button,action,nasBlock,x,y,z) => {
                    if (action == MouseAction.Pressed) { return; }
                    np.p.Message(text);
                };
            }
    		
        	
        	static NasBlockInteraction BedInteraction() {
                return (np,button,action,nasBlock,x,y,z) => {
                    if (action == MouseAction.Pressed || button != MouseButton.Right) { return; }
                    np.p.Message("Spawnpoint set");
                    Position coords = np.p.Pos;
                    Command.Find("tp").Use(np.p, x+" "+y+" "+z);
                    np.spawnCoords = np.p.Pos;
                    if (!np.p.Waypoints.Exists("Bed")) {np.p.Waypoints.Create("Bed", np.p);}
                    np.p.Waypoints.Update(np.p.Waypoints.Find("Bed"), np.p);
                    Command.Find("tp").Use(np.p, "-precise " + coords.X+" "+(coords.Y - 50)+" "+coords.Z);
                    np.spawnMap = np.p.level.name;
                    np.bedCoords = new int[] {x, y, z};
                    if (!(NasTimeCycle.cycleCurrentTime < 20*NasTimeCycle.hourMinutes &&
                        NasTimeCycle.cycleCurrentTime >= 7 * NasTimeCycle.hourMinutes))
                    {NasTimeCycle.cycleCurrentTime = 4200;
                    	Chat.MessageChat(np.p, "%fSay thanks to "+np.p.ColoredName+" %ffor skipping the night!", null, true);}
                    
                };
            }
        	
        	static NasBlockInteraction SmithingTableAction() {
    		return (np,button,action,nasBlock,x,y,z) => {
    		if (action == MouseAction.Pressed || button != MouseButton.Right) { return; }
    		BlockID aboveHere = np.nl.GetBlock(x, y+1, z);
    		float maxHP = np.inventory.HeldItem.prop.baseHP;
    		if (np.inventory.HeldItem.name.CaselessContains("emerald") && aboveHere == Block.FromRaw(650))
    		{
    			np.inventory.HeldItem.HP = np.inventory.HeldItem.HP + (0.4f * maxHP);
    			if (np.inventory.HeldItem.HP > maxHP) {np.inventory.HeldItem.HP = maxHP;}
    			np.nl.SetBlock(x, y+1, z, Block.Air);
    			np.p.Message("Repaired your {0}!", np.inventory.HeldItem.displayName);
    			return;
    		}
    		
    		if (np.inventory.HeldItem.name.CaselessContains("diamond") && aboveHere == Block.FromRaw(631))
    		{
    			np.inventory.HeldItem.HP = np.inventory.HeldItem.HP + (0.4f * maxHP);
    			if (np.inventory.HeldItem.HP > maxHP) {np.inventory.HeldItem.HP = maxHP;}
    			np.nl.SetBlock(x, y+1, z, Block.Air);
    			np.p.Message("Repaired your {0}!", np.inventory.HeldItem.displayName);
    			return;
    		}
    
    		if (np.inventory.HeldItem.name.CaselessContains("gold") && aboveHere == Block.FromRaw(41))
    		{
    			np.inventory.HeldItem.HP = np.inventory.HeldItem.HP + (0.4f * maxHP);
    			if (np.inventory.HeldItem.HP > maxHP) {np.inventory.HeldItem.HP = maxHP;}
    			np.nl.SetBlock(x, y+1, z, Block.Air);
    			np.p.Message("Repaired your {0}!", np.inventory.HeldItem.displayName);
    			return;
    		}
    		
    		if (np.inventory.HeldItem.name.CaselessContains("iron") && aboveHere == Block.FromRaw(42))
    		{
    			np.inventory.HeldItem.HP = np.inventory.HeldItem.HP + (0.4f * maxHP);
    			if (np.inventory.HeldItem.HP > maxHP) {np.inventory.HeldItem.HP = maxHP;}
    			np.nl.SetBlock(x, y+1, z, Block.Air);
    			np.p.Message("Repaired your {0}!", np.inventory.HeldItem.displayName);
    			return;
    		}
    		if (aboveHere == Block.FromRaw(171) && np.nl.blockEntities[x+" "+(y+1)+" "+z].CanAccess(np.p) &&  np.nl.blockEntities[x+" "+(y+1)+" "+z].blockText != "") {
    			var words = np.nl.blockEntities[x+" "+(y+1)+" "+z].blockText.Split(new[] { ':' }, 2);
    			np.inventory.HeldItem.displayName = words[1].Remove(0, 1);
    			np.nl.SetBlock(x, y+1, z, Block.Air);
    			np.nl.blockEntities.Remove(x+" "+(y+1)+" "+z);
    			np.p.Message("Changed your tool's name to {0}!", np.inventory.HeldItem.displayName);
    			return;
    		}
    		
    		np.p.Message("No valid recipes available.");
    			};
    		}
        	

			static NasBlockExistAction BeaconInteractAction() {
            return (np,nasBlock,exists,x,y,z) => {
        	if (exists) {
       		np.inventory.SetAmount(1, 1, true, true);
        	np.nl.SetBlock(x, y, z, Block.Air);
        		bool inv = np.p.invincible;

        		if (!inv) {np.p.invincible = true;}
        	Command.Find("Main").Use(np.p, "");
        	np.lastGroundedLocation = new Vec3S32(Server.mainLevel.SpawnPos.X, Server.mainLevel.SpawnPos.Y, Server.mainLevel.SpawnPos.Z);
            NasBlockChange.InvInfo invInfo = new NasBlockChange.InvInfo();
            invInfo.np = np;
            invInfo.inv = inv;
        	SchedulerTask repeaterTask = NasBlockChange.repeaterScheduler.QueueOnce(InvTask, invInfo, new TimeSpan(0, 0, 0, 1, 0));
        			}
        		};
            }

        	static void InvTask(SchedulerTask task) {
        		if (!((NasBlockChange.InvInfo)task.State).inv) {((NasBlockChange.InvInfo)task.State).np.p.invincible = false;}
        	}
			static NasBlockExistAction BedBeaconAction() {
            return (np,nasBlock,exists,x,y,z) => {
        	if (exists) {
        				if (np.isDead) {return;}
        				np.headingToBed = true;
        				np.isDead = true;
        				np.nl.SetBlock(x, y, z, Block.Air);
        				np.inventory.SetAmount(612, 1, true, true);
        				np.Die("");
        				
        			}
        		};
        	}        	
        	
        	
    		static NasBlockExistAction MessageExistAction()
    		{
		
                return (np,nasBlock,exists,x,y,z) => {
                    //you can never have enough voodoo
                    
                    lock (Container.locker) {
                        if (exists) {
                            
                           
                            if (np.nl.blockEntities.ContainsKey(x+" "+y+" "+z)) {
                                np.nl.blockEntities.Remove(x+" "+y+" "+z);
                            }
                            np.nl.blockEntities.Add(x+" "+y+" "+z, new Entity());
                            np.nl.blockEntities[x+" "+y+" "+z].lockedBy = np.p.name;
                            np.p.Message("To read the sign, right click.");
                            np.p.Message("To rewrite the sign, use the /sign command then middle click.");
                            return;
                        }
                        
                        np.nl.blockEntities.Remove(x+" "+y+" "+z);
                        //np.p.Message("You destroyed a {0}!", nasBlock.container.name);
                    }
                };
            }
            
    
    		static NasBlockInteraction StripInteraction(BlockID toThis, string checkString = "Axe") {
    		return (np,button,action,nasBlock,x,y,z) => {
    				if (action == MouseAction.Pressed || button != MouseButton.Right ) { return; }
    				if (np.inventory.HeldItem.name.Contains(checkString))
    				{
    					Item Held = np.inventory.HeldItem;
    					if (np.inventory.HeldItem.TakeDamage(1)) {
                    	np.inventory.BreakItem(ref Held);
               			 }
                		np.inventory.UpdateItemDisplay();
                		np.nl.SetBlock(x, y, z, toThis);
    					
    				}
    			};
    		}
    		
    		static NasBlockInteraction MessageInteraction() {
                return (np,button,action,nasBlock,x,y,z) => {
    				string myText = File.ReadAllText(GetTextPath(np.p));
    				if (!np.nl.blockEntities.ContainsKey(x+" "+y+" "+z)) {
    				np.nl.blockEntities.Add(x+" "+y+" "+z, new Entity());
    				np.p.Message("This sign's data got deleted. Now, it's a public sign!");
    				}
    				Entity bEntity = np.nl.blockEntities[x+" "+y+" "+z];
                    if (action == MouseAction.Pressed | button == MouseButton.Left ) { return; }
                    if (button == MouseButton.Right) { np.p.Message(bEntity.blockText);}
                    if ((button == MouseButton.Middle) && (myText != "" ) ) {
                    	if (!bEntity.CanAccess(np.p)) {return;}
                    	File.WriteAllText(GetTextPath(np.p), String.Empty);
                    	bEntity.blockText = np.p.name + " says: " + myText; 
                    	np.p.Message("Overwritten!");
                        
                    }
    				};
            }
    		static NasBlockExistAction LavaBarrelAction() {
    		return (np,nasBlock,exists,x,y,z) => {
    				if (exists) {
    					if (np.inventory.GetAmount(696) >= 5) {np.p.Message("%mYou have too many lava barrels!"); return;}
    					bool[] isLava = {false, false, false, false, false, false};
    					if (np.nl.GetBlock(x, y+1, z) == 10) {isLava[0] = true;}
    					if (np.nl.GetBlock(x+1, y, z) == 10) {isLava[1] = true;}
    					if (np.nl.GetBlock(x-1, y, z) == 10) {isLava[2] = true;}
    					if (np.nl.GetBlock(x, y, z-1) == 10) {isLava[3] = true;}
    					if (np.nl.GetBlock(x, y, z+1) == 10) {isLava[4] = true;}
    			
    					
    					if (isLava[0] || isLava[1] || isLava[2] || isLava[3]  || isLava[4] || isLava[5] )
    						{
                            np.nl.SetBlock(x, y, z, Block.Air);
                            if ((isLava[0] || isLava[5]) && IsPartOfSet(lavaSet, np.nl.GetBlock(x, y+1, z)) != -1) {np.nl.SetBlock(x, y+1, z, Block.Air);}
                            if ((isLava[1] || isLava[5]) && IsPartOfSet(lavaSet, np.nl.GetBlock(x+1, y, z)) != -1) {np.nl.SetBlock(x+1, y, z, Block.Air);}
                            if ((isLava[2] || isLava[5]) && IsPartOfSet(lavaSet, np.nl.GetBlock(x-1, y, z)) != -1) {np.nl.SetBlock(x-1, y, z, Block.Air);}
                            if ((isLava[3] || isLava[5]) && IsPartOfSet(lavaSet, np.nl.GetBlock(x, y, z-1)) != -1) {np.nl.SetBlock(x, y, z-1, Block.Air);}
                            if ((isLava[4] || isLava[5]) && IsPartOfSet(lavaSet, np.nl.GetBlock(x, y, z+1)) != -1) {np.nl.SetBlock(x, y, z+1, Block.Air);}
                            //lava barrel
                            np.inventory.SetAmount(696, 1, true, true);
                            np.inventory.DisplayHeldBlock(NasBlock.blocks[697], -1, false);
                            return;
                        }
    			
    				}
    			};
    		}
    		
    	static NasBlockExistAction SmithingAction() {
    		return (np,nasBlock,exists,x,y,z) => {
    				if (exists) {
    					np.p.Message("You placed a %bSmithing table%S!");
    					np.p.Message("Place the block you want to repair with on top.");
    					np.p.Message("Then right click with the tool you want to repair.");
                        }
    			
    				
    			};
    		}
    		
    		static NasBlockInteraction PortalInteraction() {
                return (np,button,action,nasBlock,x,y,z) => {
    				if (button != MouseButton.Right || action == MouseAction.Pressed) {return;}
    				int absoluteX, absoluteZ, lvlX = 0, lvlZ = 0;
    				string seed = "";
    				NasGen.GetSeedAndChunkOffset(np.nl.lvl.name, ref seed, ref lvlX, ref lvlZ);
    				absoluteX = lvlX * np.nl.lvl.Width;
    				absoluteZ = lvlZ * np.nl.lvl.Length;
    				absoluteX += x;
    				absoluteZ += z;
    				
    				bool nether = seed.CaselessContains("-nether");
    				double mult = nether ? 8 : (double)1/8;
    				absoluteX = (int)Math.Floor(absoluteX * mult);
    				absoluteZ = (int)Math.Floor(absoluteZ * mult);
    				int levelX = (int)Math.Floor((double)absoluteX/np.nl.lvl.Width);
    				int levelZ = (int)Math.Floor((double)absoluteZ/np.nl.lvl.Length);
    				
    				string newLevel = seed.Replace("-nether","")+(!nether?"-nether":"")+"_"+levelX+","+levelZ;
    				
    				int withX = absoluteX - levelX * np.nl.lvl.Width;
    				int withZ = absoluteZ - levelZ * np.nl.lvl.Length;
    				
    				withX = Math.Min(np.nl.lvl.Width-2, Math.Max(1, withX));
    				withZ = Math.Min(np.nl.lvl.Length-2, Math.Max(1, withZ));
    				int withY = Math.Min(245,Math.Max(32,(int)y));
    				
    				LevelActions.Load(np.p, newLevel, false);
    				Level grab = Level.Load(newLevel);
    				
    				
    				int dX = 0, dY = 0, dZ = 0;
    				double minDist = 100000;
    				bool worked = false;
    				if (grab != null) {
    				for (int offX = (nether ? -32 : -8); offX <= (nether ? 32 : 8); offX++)
    					for (int offZ = (nether ? -32 : -8); offZ <= (nether ? 32 : 8); offZ++)
    						for (int offY = 2 - withY; offY+withY <= 245; offY++) {
    							if (grab.GetBlock((ushort)(offX+withX), (ushort)(offY+withY), (ushort)(offZ+withZ)) == Block.FromRaw(457)) {
    							double tempDist = Math.Sqrt(offX*offX+offZ*offZ);
    							if (tempDist < minDist) {
    								minDist = tempDist;
    								dX = offX;
    								dY = offY;
    								dZ = offZ;
    								worked = true;
    							}
    						
    						}
    					}
    				
    				withX += dX;
    				withY += dY;
    				withZ += dZ;
    				np.p.Message(withX+" "+withY+" "+withZ);
    				dY = 0;
    				if (!worked) {
    				for (int offY = 32 - withY; offY+withY <= 245; offY++) {
    					BlockID block1 = grab.FastGetBlock((ushort)withX, (ushort)(withY+offY), (ushort)withZ);
    					BlockID block2 = grab.FastGetBlock((ushort)withX, (ushort)(withY+offY+1), (ushort)withZ);
    					if (block2 == Block.Air && (block1 == Block.FromRaw(457) || block1 == Block.Air)) {
    						dY = offY;
    						worked = true;
    						break;
    					}
    				}
    				}}
    				np.NetherTravel(newLevel, new NasPlayer.TransferInfo(np.p, levelX - lvlX, levelZ - lvlZ, withX, withY+dY, withZ));
    			};
    		}
    		
            static NasBlockExistAction ContainerExistAction() {
                return (np,nasBlock,exists,x,y,z) => {
                    //this is voodoo -- the question is, is it too much voodoo for the next 10 centuries for god's official temple?
                    if (exists && nasBlock.container.type == Container.Type.Barrel) {
                        if (
                            NasBlock.IsPartOfSet(NasBlock.waterSet, np.nl.GetBlock(x, y+1, z)) != -1 ||
                            NasBlock.IsPartOfSet(NasBlock.waterSet, np.nl.GetBlock(x+1, y, z)) != -1 ||
                            NasBlock.IsPartOfSet(NasBlock.waterSet, np.nl.GetBlock(x-1, y, z)) != -1 ||
                            NasBlock.IsPartOfSet(NasBlock.waterSet, np.nl.GetBlock(x, y, z+1)) != -1 ||
                            NasBlock.IsPartOfSet(NasBlock.waterSet, np.nl.GetBlock(x, y, z-1)) != -1
                           ) {
                            np.nl.SetBlock(x, y, z, Block.Air);
                            //water barrel
                            np.inventory.SetAmount(643, 1, true, true);
                            np.inventory.DisplayHeldBlock(NasBlock.blocks[143], -1, false);
                            return;
                        }
                    	
                    	
                    }
                    
                    lock (Container.locker) {
                        if (exists) {
                            if (nasBlock.container.type == Container.Type.Gravestone) {
                                //np.p.Message("do nothing");
                                return;
                            }
                            
                            //Entity blockEntity = new Entity(); ??????????????????????????????????????????????????????
                            if (np.nl.blockEntities.ContainsKey(x+" "+y+" "+z)) {
                                //np.p.Message("You just overrode a spot that used to contain a chest.");
                                np.nl.blockEntities.Remove(x+" "+y+" "+z);
                            }
                            np.nl.blockEntities.Add(x+" "+y+" "+z, new Entity());
                            //np.p.Message("You placed a {0}!", nasBlock.container.name);
                            np.p.Message(nasBlock.container.description);
                            np.p.Message("To insert, select what you want to store, then left click.");
                            np.p.Message("To extract, right click.");
                            np.p.Message("To inspect status, middle click.");
                            return;
                        }
                        
                        np.nl.blockEntities.Remove(x+" "+y+" "+z);
                        //np.p.Message("You destroyed a {0}!", nasBlock.container.name);
                    }
                };
            }
    		static NasBlockInteraction ChangeInteraction(BlockID toggle) {
    			return (np,button,action,nasBlock,x,y,z) => {
    				if (action == MouseAction.Pressed) { return; }
    				if (button == MouseButton.Right) {
    					if (toggle == Block.FromRaw(675) || toggle == Block.FromRaw(196))
    					{np.nl.blockEntities[x+" "+y+" "+z].strength = 15;}
    					if (toggle == Block.FromRaw(674))
    					{np.nl.blockEntities[x+" "+y+" "+z].strength = 0;}
    					np.nl.SetBlock(x, y, z, toggle); return;
    				}
    			};
    		}
    		static NasBlockInteraction AutoCraftInteraction() {
                return (np,button,action,nasBlock,x,y,z) => {
                    if (action == MouseAction.Pressed) { return; }
                    Entity bEntity = np.nl.blockEntities[x+" "+y+" "+z];
                    if (button == MouseButton.Middle) {
                    	CheckContents(np, nasBlock, bEntity);
                    	return;
                    }
                    
                    if (button == MouseButton.Left) {
                         if (np.inventory.HeldItem.name == "Key") {
                                np.p.Message("You cannot lock auto crafters.");
                                return;
                             }
                    		np.p.Message("You can right click to remove items from auto crafters.");
                         }
                    
                    if (button == MouseButton.Right) {
                          RemoveAll(np, bEntity, false);
                          return;
                    }
    			};
    		}
            static NasBlockInteraction ContainerInteraction() {
                return (np,button,action,nasBlock,x,y,z) => {
                    if (action == MouseAction.Pressed) { return; }
                    lock (Container.locker) {
                        if (np.nl.blockEntities.ContainsKey(x+" "+y+" "+z)) {
                            Entity bEntity = np.nl.blockEntities[x+" "+y+" "+z];
                            
                            if (!bEntity.CanAccess(np.p) && button != MouseButton.Middle) {
                                np.p.Message("This {0} is locked by {1}%S.", nasBlock.container.name.ToLower(), bEntity.FormattedNameOfLocker);
                                return;
                            }
                            
                            //np.p.Message("There is a blockEntity here.");
                            if (button == MouseButton.Middle) {
                                CheckContents(np, nasBlock, bEntity);
                                return;
                            }
                            
                            if (button == MouseButton.Left) {

                                if (np.inventory.HeldItem.name == "Key") {
                                    if (nasBlock.container.type == Container.Type.Gravestone || nasBlock.container.type == Container.Type.Dispenser) {
                                        np.p.Message("You cannot lock gravestones or dispensers.");
                                    }
                                    //it's already unlocked, lock it
                                    else if (bEntity.lockedBy.Length == 0) {
                                        bEntity.lockedBy = np.p.name;
                                        np.p.Message("You %flock%S the {0}. Only you can access it now.", nasBlock.container.name.ToLower());
                                        return;
                                    }
                                }
                                
                                if (nasBlock.container.type == Container.Type.Gravestone) {
                                    np.p.Message("You can right click to extract from tombstones.");
                                    return;
                                }
                                
                                if (nasBlock.container.type == Container.Type.Chest) {
                                    AddTool(np, bEntity);
                                } else {
                                    AddBlocks(np, x, y, z);
                                }
                            	np.nl.SimulateSetBlock(x, y, z);
                                return;
                            }
                            
                            if (button == MouseButton.Right) {
                                if (np.inventory.HeldItem.name == "Key") {
                                    //it's locked, unlock it
                                    if (bEntity.lockedBy.Length > 0) {
                                        bEntity.lockedBy = "";
                                        np.p.Message("You %funlock%S the {0}. Anyone can access it now.", nasBlock.container.name.ToLower());
                                        return;
                                    }
                                }
                                
                                if (nasBlock.container.type == Container.Type.Chest) {
                                    RemoveTool(np, bEntity);
                                } else if (nasBlock.container.type == Container.Type.Barrel || nasBlock.container.type == Container.Type.Dispenser) {
                                    RemoveBlocks(np, bEntity);
                                } else if (nasBlock.container.type == Container.Type.Gravestone) {
                            		RemoveAll(np, bEntity, (bEntity.lockedBy.Length == 0));
                                    bEntity.lockedBy = "";
                                }
                            	np.nl.SimulateSetBlock(x, y, z);
                                return;
                            }
                            return;
                        }
                        if (nasBlock.container.type != Container.Type.Gravestone) {
                            np.p.Message("(BUG) The data inside this {0} was lost, but you can make it functional again by %cdeleting%S then %breplacing%S it.",
                                         nasBlock.container.name.ToLower());
                    		np.nl.blockEntities.Add(x+" "+y+" "+z, new Entity());
                        }
                    }
                };
            }
            static void AddTool(NasPlayer np, Entity bEntity) {
                if (bEntity.drop != null && bEntity.drop.items.Count >= Container.ToolLimit) {
                    np.p.Message("There can only be {0} tools at most in a chest.", Container.ToolLimit);
                    return;
                }
                if (np.inventory.items[np.inventory.selectedItemIndex] == null) {
                    np.p.Message("You need to select a tool to insert it.");
                    return;
                }
                if (bEntity.drop == null) {
                    bEntity.drop = new Drop(np.inventory.items[np.inventory.selectedItemIndex]);
                } else {
                    bEntity.drop.items.Add(np.inventory.items[np.inventory.selectedItemIndex]);
                }
                np.p.Message("You put {0}%S in the chest.", np.inventory.items[np.inventory.selectedItemIndex].ColoredName);
                np.inventory.items[np.inventory.selectedItemIndex] = null;
                np.inventory.UpdateItemDisplay();
            }
            static void RemoveTool(NasPlayer np, Entity bEntity) {
                if (bEntity.drop == null) {
                    np.p.Message("There's no tools to extract.");
                    return;
                }
                Drop taken = new Drop(bEntity.drop.items[bEntity.drop.items.Count-1]);
                bool fullInv = true;
                for (int i = 0; i < Inventory.maxItems; i++) {
                if (np.inventory.items[i] == null) {
                		fullInv = false;
                	}
            	}	
                if (!fullInv) {bEntity.drop.items.RemoveAt(bEntity.drop.items.Count-1);}
                np.inventory.GetDrop(taken, true);
                if (bEntity.drop.items.Count == 0) {
                    bEntity.drop = null;
                }
            }
            
            
            
            
            static void AddBlocks(NasPlayer np, int x, int y, int z) {
                Player p = np.p;
                //p.ClientHeldBlock is server block ID
                BlockID clientBlockID = p.ConvertBlock(p.ClientHeldBlock);
                NasBlock nasBlock = NasBlock.Get(clientBlockID);
                Entity bEntity = np.nl.blockEntities[x+" "+y+" "+z];
                if (nasBlock.parentID == 0) {
                    p.Message("Select a block to store it.");
                    return;
                }
                if (!np.oldBarrel){
                np.isInserting = true;
                np.interactCoords = new int[] {x, y, z};
                p.Send(Packet.Motd(p, "-hax +thirdperson +hold horspeed=0"));
                p.Message("%ePlease enter in chat how many items you would like to put in the barrel.");}
                else {int amount = np.inventory.GetAmount(nasBlock.parentID);
                
                if (amount < 1) {
                    p.Message("You don't have any {0} to store.", nasBlock.GetName(p));
                    return;
                }
                
                if (amount > 3) { amount /= 2; }
                
                
                if (bEntity.drop == null) {
                    np.inventory.SetAmount(nasBlock.parentID, -amount, true, true);
                    bEntity.drop = new Drop(nasBlock.parentID, amount);
                    return;
                }
                foreach (BlockStack stack in bEntity.drop.blockStacks) {
                    //if a stack exists in the container already, add to that stack
                    if (stack.ID == nasBlock.parentID) {
                        np.inventory.SetAmount(nasBlock.parentID, -amount, true, true);
                        stack.amount += amount;
                        return;
                    }
                }
                
                if (bEntity.drop.blockStacks.Count >= Container.BlockStackLimit) {
                    p.Message("It can't contain more than {0} stacks of blocks.", Container.BlockStackLimit);
                    return;
                }
                np.inventory.SetAmount(nasBlock.parentID, -amount, true, true);
                bEntity.drop.blockStacks.Add(new BlockStack(nasBlock.parentID, amount));
                
                
            }
                
                
            }
            static void RemoveBlocks(NasPlayer np, Entity bEntity) {
                Player p = np.p;
                if (bEntity.drop != null && bEntity.drop.blockStacks != null) {
                    if (bEntity.drop.blockStacks.Count == 0) {
                        p.Message("%cTHERE ARE 0 BLOCK STACKS INSIDE WARNING THIS SHOULD NEVER HAPPEN IT SHOULD BE NULL INSTEAD");
                        return;
                    }
                    
                    BlockStack bs = null;
                    BlockID clientBlockID = p.ConvertBlock(p.ClientHeldBlock);
                    NasBlock nasBlock = NasBlock.Get(clientBlockID);
                    foreach (BlockStack stack in bEntity.drop.blockStacks) {
                        //if there's a stack in the container that matches what we're holding
                        if (stack.ID == nasBlock.parentID) {
                            //p.Message("found you");
                            bs = stack;
                            break;
                        }
                    }
                    if (bs == null) {

                        //p.Message("we didn't find a stack that matches held block, take the last one");
                        bs = bEntity.drop.blockStacks[bEntity.drop.blockStacks.Count-1];
                    }

                    int amount = bs.amount;
                    //if (amount > 3) { amount /= 2; }
           			 if ((np.inventory.GetAmount(696) + amount) > 5 && bs.ID == 696) {amount = 5 - np.inventory.GetAmount(696);}
                    np.inventory.SetAmount(bs.ID, amount, true, true);
                    if (amount >= bs.amount) {
                        bEntity.drop.blockStacks.Remove(bs);
                    } else {
                        bs.amount -= amount;
                    }
                    
                    if (bEntity.drop.blockStacks.Count == 0) {
                        bEntity.drop = null;
                    }
                    return;
                }
                p.Message("There's no blocks to extract.");
            }
            
            static void RemoveAll(NasPlayer np, Entity bEntity, bool message) {
                Player p = np.p;
                if (bEntity.drop != null) {
                    bEntity.drop = np.inventory.GetDrop(bEntity.drop, message, true);
                    return;
                }
                
                p.Message("There's nothing to extract.");
            }
            
            
            
            static void CheckContents(NasPlayer np, NasBlock nb, Entity blockEntity) {
                if (blockEntity.drop == null) {
                    np.p.Message("There's nothing inside.");
                }
                else {
                    if (blockEntity.drop.items != null) {
                        np.p.Message("There's {0} tool{1} inside.", blockEntity.drop.items.Count, blockEntity.drop.items.Count == 1 ? "" : "s");
                    }
                    if (blockEntity.drop.blockStacks != null) {
                        foreach (BlockStack bs in blockEntity.drop.blockStacks) {
                            np.p.Message("There's %f{0} {1}%S inside.", bs.amount, NasBlock.blocks[bs.ID].GetName(np.p));
                        }
                    }
                }
                if (nb.container.type == Container.Type.Gravestone) { return; }
                np.p.Message("%r(%fi%r)%S This {0} is %f{1}%S", nb.container.name.ToLower(), blockEntity.lockedBy.Length > 0 ? "locked" : "not locked");
            }
            
            
            
            
    		
            
            
            static NasBlockExistAction CraftingExistAction() {
                return (np,nasBlock,exists,x,y,z) => {
                    if (exists) {
                        np.p.Message("You placed a %b{0}%S!", nasBlock.station.name);
                        np.p.Message("Click to craft.");
                        np.p.Message("Right click to auto-replace recipe.");
                        np.p.Message("Left click for one-and-done.");
                        nasBlock.station.ShowArea(np, x, y, z, Color.White);
                        return;
                    }
                    //np.p.Message("You destroyed a {0}!", nasBlock.station.name);
                };
            }
    		
    		static NasBlockExistAction WireExistAction(int strength, int type) {
                return (np,nasBlock,exists,x,y,z) => {
                        lock (Container.locker) {
                        if (exists) {
                            
                           
                            if (np.nl.blockEntities.ContainsKey(x+" "+y+" "+z)) {
                                np.nl.blockEntities.Remove(x+" "+y+" "+z);
                            }
                            np.nl.blockEntities.Add(x+" "+y+" "+z, new Entity());
                            np.nl.blockEntities[x+" "+y+" "+z].strength = strength;
                            np.nl.blockEntities[x+" "+y+" "+z].type = type;
                            return;
                        }
                        
                        np.nl.blockEntities.Remove(x+" "+y+" "+z);
                        return;
                    }
        	};
        }
    		
    		static NasBlockExistAction WireBreakAction() {
                return (np,nasBlock,exists,x,y,z) => {
                        lock (Container.locker) {
                        if (!exists) {
                        np.nl.blockEntities.Remove(x+" "+y+" "+z);
                        return;}
                    }
        	};
        }
    		
    		static NasBlockExistAction AutoCraftExistAction() {
                return (np,nasBlock,exists,x,y,z) => {
                        lock (Container.locker) {
                        if (exists) {
                            
                           
                            if (np.nl.blockEntities.ContainsKey(x+" "+y+" "+z)) {
                                np.nl.blockEntities.Remove(x+" "+y+" "+z);
                            }
                            np.nl.blockEntities.Add(x+" "+y+" "+z, new Entity());
                            np.p.Message("You placed a %bAuto Crafter%S!");
                        	np.p.Message("It can craft things without user input.");
                        	np.p.Message("When powered it tries to craft whatever is above it.");
                        	np.p.Message("Click it to remove all items crafted, like a gravestone.");
                        	nasBlock.station.ShowArea(np, x, y, z, Color.White);
                            return; 
                        }
                        
                        np.nl.blockEntities.Remove(x+" "+y+" "+z);
                        return;
                    }
        	};
        }
    		
            /*static NasBlockInteraction CraftingInteraction() {
                return (np,button,action,nasBlock,x,y,z) => {
                    if (action == MouseAction.Pressed) { return; }
                    lock (Crafting.locker) {
                        Crafting.Recipe recipe = Crafting.GetRecipe(np.nl, x, y, z, nasBlock.station);
                        if (recipe == null) {
                            nasBlock.station.ShowArea(np, x, y, z, Color.Red, 500, 127);
                            return;
                        }
                        Drop dropClone = new Drop(recipe.drop);
                        
                        if (np.inventory.GetDrop(dropClone, true) != null) {
                            //non null means the player couldn't fit this drop in their inventory
                            return;
                        }
                        nasBlock.station.ShowArea(np, x, y, z, Color.LightGreen, 500);
                        bool clearCraftingArea = button == MouseButton.Left;
                        Dictionary<BlockID, int> patternCost = new Dictionary<BlockID, int>();
                        if (nasBlock.station.ori == Crafting.Station.Orientation.WE) {
                        	for (int minX = x - 1; minX < x+2; minX++) {
                        		for (int minY = y + 1; minY < y+4; minY++) {
                        			BlockID clientBlockID = NasBlock.Get(Block.ToRaw(np.nl.GetBlock(minX, minY, z))).parentID;
                        			Crafting.Recipe.FillDict(clientBlockID, ref patternCost);
                        		}
                        		}
                        }
                        if (nasBlock.station.ori == Crafting.Station.Orientation.NS) {
                        	for (int minZ = z - 1; minZ < z+2; minZ++) {
                        		for (int minY = y + 1; minY < y+4; minY++) {
                        			BlockID clientBlockID = Block.ToRaw(np.nl.GetBlock(x, minY, minZ));
                        			Crafting.Recipe.FillDict(clientBlockID, ref patternCost);
                        		}
                        		}
                        	
                        }
                        foreach (KeyValuePair<BlockID, int> pair in patternCost) {
                            if (np.inventory.GetAmount(pair.Key) < pair.Value) {
                                clearCraftingArea = true; break;
                            }
                        }
                        
                        if (clearCraftingArea) {
                            Crafting.ClearCraftingArea(np.p, x, y, z, nasBlock.station.ori, np.nl);
                        } else {
                            foreach (KeyValuePair<BlockID, int> pair in patternCost) {
                        		if (pair.Key != Block.Air) {np.inventory.SetAmount(pair.Key, -pair.Value, false);}
                            }
                        }
                    }
                };
            }*/
            
            static NasBlockInteraction CraftingInteraction() {
                return (np,button,action,nasBlock,x,y,z) => {
                    if (action == MouseAction.Pressed) { return; }
                    lock (Crafting.locker) {
                        Crafting.Recipe recipe = Crafting.GetRecipe(np.nl, x, y, z, nasBlock.station);
                        if (recipe == null) {
                            nasBlock.station.ShowArea(np, x, y, z, Color.Red, 500, 127);
                            return;
                        }
                        Drop dropClone = new Drop(recipe.drop);
                        
                        if (np.inventory.GetDrop(dropClone, true) != null) {
                            //non null means the player couldn't fit this drop in their inventory
                            return;
                        }
                        np.GiveExp(recipe.expGiven);
                        nasBlock.station.ShowArea(np, x, y, z, Color.LightGreen, 500);
                        bool clearCraftingArea = button == MouseButton.Left;
                        var patternCost = recipe.patternCost;
                        foreach (KeyValuePair<BlockID, int> pair in patternCost) {
                            if (np.inventory.GetAmount(pair.Key) < pair.Value) {
                        		if (pair.Key == 0) {continue;}
                                clearCraftingArea = true; break;
                            }
                        }
                        if (clearCraftingArea) {
                            Crafting.ClearCraftingArea(np.nl, x, y, z, nasBlock.station.ori);
                        } else {
                            foreach (KeyValuePair<BlockID, int> pair in patternCost) {
                                if (pair.Key != Block.Air) {np.inventory.SetAmount(pair.Key, -pair.Value, false);}
                            }
                        }
                    }
                };
            }
            
            static NasBlockExistAction PlantExistAction() {
                return (np,nasBlock,exists,x,y,z) => {
    				return;
                    
                };
            }
    		static BlockID[] waffleSet = {Block.Extended|542, Block.Extended|543};
            static BlockID[] breadSet = new BlockID[] { Block.Extended|640, Block.Extended|641, Block.Extended|642 };
            static BlockID[] pieSet = new BlockID[] { Block.Extended|668, Block.Extended|669, Block.Extended|670, Block.Extended|671 };
            static BlockID[] peachPieSet = new BlockID[] { Block.Extended|698, Block.Extended|699, Block.Extended|700, Block.Extended|701 };
            static NasBlockInteraction EatInteraction(BlockID[] set, int index, float healthRestored, float chewSeconds = 2) {
                return (np,button,action,nasBlock,x,y,z) => {
                    if (action == MouseAction.Pressed) { return; }
                    
                    lock (Container.locker) {
                        //if (np.HP >= NasEntity.maxHP) {
                        //    np.p.Message("You're full!");
                        //    return;
                        //}
                        
                        if (np.isChewing) {
                            //np.p.Message("You're still chewing.");
                            return;
                        }
                        np.isChewing = true;
                        SchedulerTask taskChew;
                        EatInfo eatInfo = new EatInfo();
                        eatInfo.np = np;
                        eatInfo.healthRestored = healthRestored;
                        taskChew = Server.MainScheduler.QueueOnce(CanEatAgainCallback, eatInfo, TimeSpan.FromSeconds(chewSeconds));
                        
                        
                        np.p.Message("*munch*");
                        np.p.level.BlockDB.Cache.Add(np.p, x, y, z, BlockDBFlags.ManualPlace, set[index], Block.Air);
                        if (index == set.Length-1) {
                            np.nl.SetBlock(x, y, z, Block.Air);
                            return;
                        }
                        np.nl.SetBlock(x, y, z, set[index+1]);
                    }
                };
            }
            public class EatInfo {
                public NasPlayer np;
                public float healthRestored;
            }
            static void CanEatAgainCallback(SchedulerTask task) {
                EatInfo eatInfo = (EatInfo)task.State;
                NasPlayer np = eatInfo.np;
                float healthRestored = eatInfo.healthRestored;
                
               
                float roundAdd = ((float)Math.Floor(np.HP * 2f) / 2f) - np.HP;

                np.ChangeHealth(roundAdd);
                float HPafterHeal = np.HP + healthRestored;
                if (HPafterHeal > NasEntity.maxHP) {
                    healthRestored = NasEntity.maxHP - np.HP;
                }
                if (healthRestored < 0) {np.TakeDamage(-healthRestored, NasEntity.DamageSource.None);}
                else {np.ChangeHealth(healthRestored);}
                np.isChewing = false;
                np.p.Message("*gulp*");
                if (healthRestored < 0) {np.p.Message("Oh no! It was %mPOISON! %7It tastes super good though.."); 
                	np.p.Message("%c{0} %f[{1}] HP ╝", healthRestored, np.HP);}
                else {np.p.Message("%a+{0} %f[{1}] HP ♥", healthRestored, np.HP);}
            }
        
    }

}
