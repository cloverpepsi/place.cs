using System;
using System.Collections.Generic;
using MCGalaxy;
using MCGalaxy.Blocks;
using BlockID = System.UInt16;

namespace NotAwesomeSurvival {

	
	
	
    public partial class NasBlock {
		
		
        static Random r = new Random();
        public static void Setup() {
            DefaultDurabilities[(int)Material.None] = 1;
            DefaultDurabilities[(int)Material.Gas] = 0;
            DefaultDurabilities[(int)Material.Stone] = 16;
            DefaultDurabilities[(int)Material.Earth] = 5;
            DefaultDurabilities[(int)Material.Wood] = 8;
            DefaultDurabilities[(int)Material.Plant] = 1;
            DefaultDurabilities[(int)Material.Leaves] = 3;
            DefaultDurabilities[(int)Material.Organic] = 5;
            DefaultDurabilities[(int)Material.Glass] = 3;
            DefaultDurabilities[(int)Material.Metal] = 32;

            Default = new NasBlock(0, Material.Earth);
            Default.collideAction = AirCollideAction();
            
            const float fallSpeed = 0.325f;

            BlockID i;
            
            i = 8; //active 
            blocks[i] = new NasBlock(i, Material.Liquid, Int32.MaxValue);
            blocks[i].disturbDelayMin = 1f;
            blocks[i].disturbDelayMax = 5f;
            blocks[i].disturbedAction = FloodAction(waterSet);
            blocks[i].collideAction = LiquidCollideAction();
            
            
            
            
            
            const float waterDisturbDelayMin = 0.5f;
            const float waterDisturbDelayMax = 0.5f;
            
            i = 643; //Water barrel
            blocks[i] = new NasBlock(i, Material.Wood, DefaultDurabilities[(int)Material.Wood]*2);
            blocks[i].childIDs = new List<BlockID>();
            blocks[i].childIDs.Add(9);
            
            i = 9; //still water
            blocks[i] = new NasBlock(i, Material.Liquid, Int32.MaxValue);
            blocks[i].existAction = WaterExistAction();
            blocks[i].disturbDelayMin = waterDisturbDelayMin;
            blocks[i].disturbDelayMax = waterDisturbDelayMax;
            blocks[i].disturbedAction = LimitedFloodAction(waterSet, 1);
            blocks[i].collideAction = LiquidCollideAction();
            blocks[i].parentID = 643;
            

            
            i = 632; //water flows
            blocks[i] = new NasBlock(i, Material.Liquid, Int32.MaxValue);
            blocks[i].disturbDelayMin = waterDisturbDelayMin;
            blocks[i].disturbDelayMax = waterDisturbDelayMax;
            blocks[i].disturbedAction = LimitedFloodAction(waterSet, 3);
            blocks[i].collideAction = LiquidCollideAction();
            i++;
            blocks[i] = new NasBlock(i, Material.Liquid, Int32.MaxValue);
            blocks[i].disturbDelayMin = waterDisturbDelayMin;
            blocks[i].disturbDelayMax = waterDisturbDelayMax;
            blocks[i].disturbedAction = LimitedFloodAction(waterSet, 4);
            blocks[i].collideAction = LiquidCollideAction();
            i++;
            blocks[i] = new NasBlock(i, Material.Liquid, Int32.MaxValue);
            blocks[i].disturbDelayMin = waterDisturbDelayMin;
            blocks[i].disturbDelayMax = waterDisturbDelayMax;
            blocks[i].disturbedAction = LimitedFloodAction(waterSet, 5);
            blocks[i].collideAction = LiquidCollideAction();
            i++;
            blocks[i] = new NasBlock(i, Material.Liquid, Int32.MaxValue);
            blocks[i].disturbDelayMin = waterDisturbDelayMin;
            blocks[i].disturbDelayMax = waterDisturbDelayMax;
            blocks[i].disturbedAction = LimitedFloodAction(waterSet, 6);
            blocks[i].collideAction = LiquidCollideAction();
            i++;
            blocks[i] = new NasBlock(i, Material.Liquid, Int32.MaxValue);
            blocks[i].disturbDelayMin = waterDisturbDelayMin;
            blocks[i].disturbDelayMax = waterDisturbDelayMax;
            blocks[i].disturbedAction = LimitedFloodAction(waterSet, 7);
            blocks[i].collideAction = LiquidCollideAction();
            i++;
            blocks[i] = new NasBlock(i, Material.Liquid, Int32.MaxValue);
            blocks[i].disturbDelayMin = waterDisturbDelayMin;
            blocks[i].disturbDelayMax = waterDisturbDelayMax;
            blocks[i].disturbedAction = LimitedFloodAction(waterSet, 8);
            blocks[i].collideAction = LiquidCollideAction();
            i++;
            blocks[i] = new NasBlock(i, Material.Liquid, Int32.MaxValue);
            blocks[i].disturbDelayMin = waterDisturbDelayMin;
            blocks[i].disturbDelayMax = waterDisturbDelayMax;
            blocks[i].disturbedAction = LimitedFloodAction(waterSet, 9);
            blocks[i].collideAction = LiquidCollideAction();
            i = 639; //waterfall
            blocks[i] = new NasBlock(i, Material.Liquid, Int32.MaxValue);
            blocks[i].disturbDelayMin = fallSpeed;
            blocks[i].disturbDelayMax = fallSpeed;
            blocks[i].disturbedAction = LimitedFloodAction(waterSet, 2);
            blocks[i].collideAction = LiquidCollideAction();
            
            float lavaDisturbDelayMin = 1.5f;
            float lavaDisturbDelayMax = 1.5f;
            
           
            i = 10; //Active lava
            blocks[i] = new NasBlock(i, Material.Liquid, Int32.MaxValue);
            blocks[i].existAction = LavaExistAction();
			blocks[i].disturbDelayMin = lavaDisturbDelayMin;
            blocks[i].disturbDelayMax = lavaDisturbDelayMax;
            blocks[i].disturbedAction = LimitedFloodAction(lavaSet, 1);
            blocks[i].collideAction = LavaCollideAction();
            blocks[i].parentID = 696;
            
            
            i = 691; //lava flows
            blocks[i] = new NasBlock(i, Material.Liquid, Int32.MaxValue);
			blocks[i].disturbDelayMin = lavaDisturbDelayMin;
            blocks[i].disturbDelayMax = lavaDisturbDelayMax;
            blocks[i].disturbedAction = LimitedFloodAction(lavaSet, 3);
            blocks[i].collideAction = LavaCollideAction();
            i++;
            blocks[i] = new NasBlock(i, Material.Liquid, Int32.MaxValue);
			blocks[i].disturbDelayMin = lavaDisturbDelayMin;
            blocks[i].disturbDelayMax = lavaDisturbDelayMax;
            blocks[i].disturbedAction = LimitedFloodAction(lavaSet, 4);
            blocks[i].collideAction = LavaCollideAction();
            i++;
            blocks[i] = new NasBlock(i, Material.Liquid, Int32.MaxValue);
			blocks[i].disturbDelayMin = lavaDisturbDelayMin;
            blocks[i].disturbDelayMax = lavaDisturbDelayMax;
            blocks[i].disturbedAction = LimitedFloodAction(lavaSet, 5);
            blocks[i].collideAction = LavaCollideAction();
            i++;
            blocks[i] = new NasBlock(i, Material.Liquid, Int32.MaxValue);
			blocks[i].disturbDelayMin = lavaDisturbDelayMin;
            blocks[i].disturbDelayMax = lavaDisturbDelayMax;
            blocks[i].disturbedAction = LimitedFloodAction(lavaSet, 6);
            blocks[i].collideAction = LavaCollideAction();
            i = 695; //lavafall
            blocks[i] = new NasBlock(i, Material.Liquid, Int32.MaxValue);
			blocks[i].disturbDelayMin = lavaDisturbDelayMin;
            blocks[i].disturbDelayMax = lavaDisturbDelayMax;
            blocks[i].disturbedAction = LimitedFloodAction(lavaSet, 2);
            blocks[i].collideAction = LavaCollideAction();
            
            
            
            
            
            i = 11; //Lava
            blocks[i] = new NasBlock(i, Material.Liquid, Int32.MaxValue);
            blocks[i].collideAction = LavaCollideAction();
            blocks[i].disturbedAction = FloodAction(new BlockID[] {11});
            
            
            i = 1; //Stone
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            i = 596; //Stone slab
            blocks[i] = new NasBlock(i++, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i] = new NasBlock(i, blocks[596]);
            i = 598; //Stone wall
            blocks[i] = new NasBlock(i++, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i] = new NasBlock(i++, blocks[598]);
            blocks[i] = new NasBlock(i++, blocks[598]);
            blocks[i] = new NasBlock(i++, blocks[598]);
            i = 70; //Stone stair (lower)
            blocks[i] = new NasBlock(i++, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i] = new NasBlock(i++, blocks[70]);
            blocks[i] = new NasBlock(i++, blocks[70]);
            blocks[i] = new NasBlock(i++, blocks[70]);
            i = 579; //Stone stair (upper)
            blocks[i] = new NasBlock(i++, blocks[70]);
            blocks[i] = new NasBlock(i++, blocks[70]);
            blocks[i] = new NasBlock(i++, blocks[70]);
            blocks[i] = new NasBlock(i++, blocks[70]);

            i = 162; //Cobblestone
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            i = 181; //Mossy Cobblestone
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            i = 163; //Cobblestone-U (next is D)
            blocks[i] = new NasBlock(i++, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i] = new NasBlock(i, blocks[163]);
            
            const int stonebrickDurMulti = 2;
            i = 64; //Marker
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone]*stonebrickDurMulti, 1);
            i = 65; //Stone brick
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone]*stonebrickDurMulti, 1);
            i = 180; //Mossy stone brick
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone]*stonebrickDurMulti, 1);
            i = 86; //Stone brick slab
            blocks[i] = new NasBlock(i++, Material.Stone, DefaultDurabilities[(int)Material.Stone]*stonebrickDurMulti, 1);
            blocks[i] = new NasBlock(i++, blocks[86]);
            i = 75; //Stone pole
            blocks[i] = new NasBlock(i++, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i] = new NasBlock(i++, blocks[75]);
            blocks[i] = new NasBlock(i++, blocks[75]);
            i = 278; //Stone brick wall-N
            blocks[i] = new NasBlock(i++, Material.Stone, DefaultDurabilities[(int)Material.Stone]*stonebrickDurMulti, 1);
            blocks[i] = new NasBlock(i++, blocks[278]);
            blocks[i] = new NasBlock(i++, blocks[278]);
            blocks[i] = new NasBlock(i++, blocks[278]);
            i = 477; //Lined stone
            blocks[i] = new NasBlock(i++, Material.Stone, DefaultDurabilities[(int)Material.Stone]*stonebrickDurMulti, 1);
            
            i = 211; //Thin pole
            blocks[i] = new NasBlock(i++, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i] = new NasBlock(i++, blocks[211]);
            blocks[i] = new NasBlock(i++, blocks[211]);
            
            i = 214; //Boulder
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone]/2, 0);
            i = 194; //Nub
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone]/2, 0);
            
            i = 236; //unseen head
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone]/2, 0);
            
            //oh boy it's nether blocks 
        	i = 48; //Netherrack
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone]/2, 1);
         	i = 155; //Nether bricks
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);           
            i = 157; //Nether slab-D
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            i = 156; //Nether slab-U
            blocks[i] = new NasBlock(i, blocks[157]);
            
            i = 452; //Blackstone
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1); 
            blocks[i].alternateID = 1;
            
            i = 458; //polished blackstone + slabs
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);    
            i = 460;
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);    
            i--;
          	blocks[i] = new NasBlock(i, blocks[460]);
          	
          	i = 466; //bricks blackstone + slabs
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);    
            i = 468;
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);    
            i--;
          	blocks[i] = new NasBlock(i, blocks[468]);
            
          	i = 469; //Gilded blackstone
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1); 
            
            i = 474; //Cracked blackstone bricks
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1); 
            
            i = 475; // blackstone chiseled
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1); 
            
            const float grassDelayMin = 10;
            const float grassDelayMax = 60;
            i = 2; //Grass
            blocks[i] = new NasBlock(i, Material.Earth);
            blocks[i].disturbDelayMin = grassDelayMin;
            blocks[i].disturbDelayMax = grassDelayMax;
            blocks[i].disturbedAction = GrassBlockAction(Block.Grass, Block.Dirt);
            blocks[i].dropHandler = (NasPlayer, dropID) => {
                Drop grassDrop = new Drop();
                grassDrop.blockStacks = new List<BlockStack>();
                grassDrop.blockStacks.Add(new BlockStack(3, 1));
                return grassDrop;
                
            };
            
            i = 129; //Wet grass
            blocks[i] = new NasBlock(i, Material.Earth);
            blocks[i].disturbDelayMin = grassDelayMin;
            blocks[i].disturbDelayMax = grassDelayMax;
            blocks[i].disturbedAction = GrassBlockAction(Block.Extended|129, Block.Dirt);
            blocks[i].interaction = StripInteraction(Block.FromRaw(547), "Shovel");
            blocks[i].dropHandler = (NasPlayer, dropID) => {
                if (NasPlayer.inventory.HeldItem.name == "Shears")
                {return new Drop(129, 1);}
                {return new Drop(3, 1);}
            };
            
            i = 139; //Snowy grass
            blocks[i] = new NasBlock(i, Material.Earth);
            blocks[i].disturbedAction = GrassBlockAction(Block.Extended|139, Block.Dirt);
            blocks[i].interaction = StripInteraction(Block.FromRaw(547), "Shovel");
            blocks[i].dropHandler = (NasPlayer, dropID) => {
                if (NasPlayer.inventory.HeldItem.name == "Shears")
                {return new Drop(139, 1);}
                {return new Drop(3, 1);}
            };
            
            i = 547; //Grass path
            blocks[i] = new NasBlock(i, Material.Earth);
            blocks[i].disturbDelayMin = 10f;
            blocks[i].disturbDelayMax = 20f;
            blocks[i].disturbedAction = GrassBlockAction(Block.Extended|129, Block.Dirt);
            blocks[i].dropHandler = (NasPlayer, dropID) => {
                if (NasPlayer.inventory.HeldItem.name == "Shears")
                {return new Drop(547, 1);}
                {return new Drop(3, 1);}
            };

            i = 3; //Dirt
            blocks[i] = new NasBlock(i, Material.Earth);
            blocks[i].disturbDelayMin = grassDelayMin;
            blocks[i].disturbDelayMax = grassDelayMax;
            blocks[i].interaction = StripInteraction(Block.FromRaw(547), "Shovel");
            blocks[i].disturbedAction = DirtBlockAction(grassSet, Block.Dirt);

            i = 685; //Dirt (fake)
            blocks[i] = new NasBlock(i, Material.Earth);
            blocks[i].collideAction = AirCollideAction();
            
            i = 4; //Cobblebrick
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            i = 50; //Cobble brick-D
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            i = 59; //Cobble brick-U
            blocks[i] = new NasBlock(i, blocks[50]);
            i = 133; //Cobble brick wall
            blocks[i] = new NasBlock(i++, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i] = new NasBlock(i, blocks[133]);
//BIRCH STUFF~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            i = 98; //Wood
            blocks[i] = new NasBlock(i, Material.Wood);
            blocks[i].alternateID = 5;
            i = 101; //Wood slab-U
            blocks[i] = new NasBlock(i, Material.Wood);
            blocks[i].alternateID = 56;
            i = 102; //Wood slab-D
            blocks[i] = new NasBlock(i, blocks[101]);
            blocks[i].alternateID = 57;
            i = 186; //Wood wall
            blocks[i] = new NasBlock(i++, Material.Wood);
            blocks[i - 1].alternateID = 182;
            blocks[i] = new NasBlock(i++, blocks[186]);
            blocks[i - 1].alternateID = 183;
            blocks[i] = new NasBlock(i++, blocks[186]);
            blocks[i - 1].alternateID = 184;
            blocks[i] = new NasBlock(i++, blocks[186]);
            blocks[i - 1].alternateID = 185;
            i = 262; //Wood stair (lower)
            blocks[i] = new NasBlock(i++, Material.Wood);
            blocks[i - 1].alternateID = 66;
            blocks[i] = new NasBlock(i++, blocks[262]);
            blocks[i - 1].alternateID = 67;
            blocks[i] = new NasBlock(i++, blocks[262]);
            blocks[i - 1].alternateID = 68;
            blocks[i] = new NasBlock(i++, blocks[262]);
            blocks[i - 1].alternateID = 69;
            i = 575; //Wood stair (upper)
            blocks[i] = new NasBlock(i++, blocks[262]);
            blocks[i - 1].alternateID = 567;
            blocks[i] = new NasBlock(i++, blocks[262]);
            blocks[i - 1].alternateID = 568;
            blocks[i] = new NasBlock(i++, blocks[262]);
            blocks[i - 1].alternateID = 569;
            blocks[i] = new NasBlock(i++, blocks[262]);
            blocks[i - 1].alternateID = 570;
            i = 255; //Wood pole
            blocks[i] = new NasBlock(i++, Material.Wood);
            blocks[i - 1].alternateID = 78;
            blocks[i] = new NasBlock(i++, blocks[255]);
            blocks[i - 1].alternateID = 79;
            blocks[i] = new NasBlock(i++, blocks[255]);
            blocks[i - 1].alternateID = 80;
            i = 260; //Fence (wood)
            blocks[i] = new NasBlock(i++, Material.Wood);
            blocks[i] = new NasBlock(i, blocks[260]);
            
            
            

            i = 243; //Gnarly (Log)
            blocks[i] = new NasBlock(i, Material.Wood);
            i = 242; //Log-UD
            blocks[i] = new NasBlock(i, Material.Wood);
            blocks[i].alternateID = 17;
            blocks[i].interaction = StripInteraction(Block.FromRaw(546));
            i = 240; //Log-WE
            blocks[i] = new NasBlock(i, blocks[242]);
            blocks[i].alternateID = 15;
            blocks[i].interaction = StripInteraction(Block.FromRaw(544));
            i = 241; //Log-NS
            blocks[i] = new NasBlock(i, blocks[242]);
            blocks[i].alternateID = 16;
            blocks[i].interaction = StripInteraction(Block.FromRaw(545));
            
            i = 546; //Stripped-UD
            blocks[i] = new NasBlock(i, Material.Wood);
            i = 544; //Log-WE
            blocks[i] = new NasBlock(i, blocks[546]);
            i = 545; //Log-NS
            blocks[i] = new NasBlock(i, blocks[546]);
            
            
//SPRUCE STUFF~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            i = 97; //Wood
            blocks[i] = new NasBlock(i, Material.Wood);
            blocks[i].alternateID = 5;
            i = 99; //Wood slab-U
            blocks[i] = new NasBlock(i, Material.Wood);
            blocks[i].alternateID = 56;
            i = 100; //Wood slab-D
            blocks[i] = new NasBlock(i, blocks[99]);
            blocks[i].alternateID = 57;
            i = 190; //Wood wall
            blocks[i] = new NasBlock(i++, Material.Wood);
            blocks[i - 1].alternateID = 182;
            blocks[i] = new NasBlock(i++, blocks[190]);
            blocks[i - 1].alternateID = 183;
            blocks[i] = new NasBlock(i++, blocks[190]);
            blocks[i - 1].alternateID = 184;
            blocks[i] = new NasBlock(i++, blocks[190]);
            blocks[i - 1].alternateID = 185;
            i = 266; //Wood stair (lower)
            blocks[i] = new NasBlock(i++, Material.Wood);
            blocks[i - 1].alternateID = 66;
            blocks[i] = new NasBlock(i++, blocks[266]);
            blocks[i - 1].alternateID = 67;
            blocks[i] = new NasBlock(i++, blocks[266]);
            blocks[i - 1].alternateID = 68;
            blocks[i] = new NasBlock(i++, blocks[266]);
            blocks[i - 1].alternateID = 69;
            i = 571; //Wood stair (upper)
            blocks[i] = new NasBlock(i++, blocks[266]);
            blocks[i - 1].alternateID = 567;
            blocks[i] = new NasBlock(i++, blocks[266]);
            blocks[i - 1].alternateID = 568;
            blocks[i] = new NasBlock(i++, blocks[266]);
            blocks[i - 1].alternateID = 569;
            blocks[i] = new NasBlock(i++, blocks[266]);
            blocks[i - 1].alternateID = 570;
            i = 252; //Wood pole
            blocks[i] = new NasBlock(i++, Material.Wood);
            blocks[i - 1].alternateID = 78;
            blocks[i] = new NasBlock(i++, blocks[252]);
            blocks[i - 1].alternateID = 79;
            blocks[i] = new NasBlock(i++, blocks[252]);
            blocks[i - 1].alternateID = 80;
            i = 258; //Fence (wood)
            blocks[i] = new NasBlock(i++, Material.Wood);
            blocks[i] = new NasBlock(i, blocks[258]);
            
            
            

            i = 251; //Gnarly (Log)
            blocks[i] = new NasBlock(i, Material.Wood);
            i = 250; //Log-UD
            blocks[i] = new NasBlock(i, Material.Wood);
            blocks[i].alternateID = 17;
            blocks[i].interaction = StripInteraction(Block.FromRaw(621));
            i = 248; //Log-WE
            blocks[i] = new NasBlock(i, blocks[250]);
            blocks[i].alternateID = 15;
            blocks[i].interaction = StripInteraction(Block.FromRaw(619));
            i = 249; //Log-NS
            blocks[i] = new NasBlock(i, blocks[250]);
            blocks[i].alternateID = 16;
            blocks[i].interaction = StripInteraction(Block.FromRaw(620));
            
            i = 621; //Stripped-UD
            blocks[i] = new NasBlock(i, Material.Wood);
            i = 619; //Log-WE
            blocks[i] = new NasBlock(i, blocks[621]);
            i = 620; //Log-NS
            blocks[i] = new NasBlock(i, blocks[621]);
            
    //OAK STUFF ~~~~~~~~~    
            i = 5; //Wood
            blocks[i] = new NasBlock(i, Material.Wood);
            i = 56; //Wood slab-U
            blocks[i] = new NasBlock(i, Material.Wood);
            i = 57; //Wood slab-D
            blocks[i] = new NasBlock(i, blocks[56]);
            i = 182; //Wood wall
            blocks[i] = new NasBlock(i++, Material.Wood);
            blocks[i] = new NasBlock(i++, blocks[182]);
            blocks[i] = new NasBlock(i++, blocks[182]);
            blocks[i] = new NasBlock(i++, blocks[182]);
            i = 66; //Wood stair (lower)
            blocks[i] = new NasBlock(i++, Material.Wood);
            blocks[i] = new NasBlock(i++, blocks[66]);
            blocks[i] = new NasBlock(i++, blocks[66]);
            blocks[i] = new NasBlock(i++, blocks[66]);
            i = 567; //Wood stair (upper)
            blocks[i] = new NasBlock(i++, blocks[66]);
            blocks[i] = new NasBlock(i++, blocks[66]);
            blocks[i] = new NasBlock(i++, blocks[66]);
            blocks[i] = new NasBlock(i++, blocks[66]);
            i = 78; //Wood pole
            blocks[i] = new NasBlock(i++, Material.Wood);
            blocks[i] = new NasBlock(i++, blocks[78]);
            blocks[i] = new NasBlock(i++, blocks[78]);
            i = 94; //Fence (wood)
            blocks[i] = new NasBlock(i++, Material.Wood);
            blocks[i] = new NasBlock(i, blocks[94]);
            
            i = 168; //Wood board
            blocks[i] = new NasBlock(i++, Material.Wood);
            i = 524; //Board (sideways)
            blocks[i] = new NasBlock(i++, Material.Wood);
            blocks[i] = new NasBlock(i++, blocks[524]);
            blocks[i] = new NasBlock(i++, blocks[524]);
            blocks[i] = new NasBlock(i++, blocks[524]);
            
           
            i = 17; //Log-UD
            blocks[i] = new NasBlock(i, Material.Wood);
            blocks[i].interaction = StripInteraction(Block.FromRaw(585));
            i = 15; //Log-WE
            blocks[i] = new NasBlock(i, blocks[17]);
            blocks[i].interaction = StripInteraction(Block.FromRaw(583));
            i = 16; //Log-NS
            blocks[i] = new NasBlock(i, blocks[17]);
            blocks[i].interaction = StripInteraction(Block.FromRaw(584));
            
            i = 585; //Stripped-UD
            blocks[i] = new NasBlock(i, Material.Wood);
            i = 583; //Log-WE
            blocks[i] = new NasBlock(i, blocks[585]);
            i = 584; //Log-NS
            blocks[i] = new NasBlock(i, blocks[585]);
            
            i = 676; //smithing
            blocks[i] = new NasBlock(i, Material.Wood);
            blocks[i].existAction = SmithingAction();
            blocks[i].interaction = SmithingTableAction();
            
            
            
            i = 657; //Falling Log
            blocks[i] = new NasBlock(i, Material.Wood);
            blocks[i].disturbDelayMin = fallSpeed;
            blocks[i].disturbDelayMax = fallSpeed;
            blocks[i].disturbedAction = FallingBlockAction(Block.FromRaw(i));
			i = 656; //Falling Log 2
            blocks[i] = new NasBlock(i, Material.Wood);
            blocks[i].disturbDelayMin = 0.75f;
            blocks[i].disturbDelayMax = 0.75f;
            blocks[i].disturbedAction = FallingBlockAction(Block.FromRaw(i));

            const float treeDelayMin = 30f;
            const float treeDelayMax = 60f;
            i = 6; //Sapling
            blocks[i] = new NasBlock(i, Material.Plant);
            blocks[i].disturbDelayMin = treeDelayMin;
            blocks[i].disturbDelayMax = treeDelayMax;
            blocks[i].disturbedAction = OakSaplingAction();

            i = 154; //Sapling birch
            blocks[i] = new NasBlock(i, Material.Plant);
            blocks[i].disturbDelayMin = treeDelayMin;
            blocks[i].disturbDelayMax = treeDelayMax;
            blocks[i].disturbedAction = BirchSaplingAction();
            blocks[i].alternateID = 6;
            
            i = 450; //Sapling swamp
            blocks[i] = new NasBlock(i, Material.Plant);
            blocks[i].disturbDelayMin = treeDelayMin;
            blocks[i].disturbDelayMax = treeDelayMax;
            blocks[i].disturbedAction = SwampSaplingAction();
            blocks[i].alternateID = 6;
            
            i = 689; //Sapling spruce
            blocks[i] = new NasBlock(i, Material.Plant);
            blocks[i].disturbDelayMin = treeDelayMin;
            blocks[i].disturbDelayMax = treeDelayMax;
            blocks[i].disturbedAction = SpruceSaplingAction();
            blocks[i].alternateID = 6;
            
            i = 7; //Bedrock
            blocks[i] = new NasBlock(i, Material.Stone, int.MaxValue, 5);
            
            i = 767; //Barrier
            blocks[i] = new NasBlock(i, Material.Stone, int.MaxValue-1, 6);
            blocks[i].collideAction = AirCollideAction();
            
            i = 673; //spawnbedrock
            blocks[i] = new NasBlock(i, Material.Stone, int.MaxValue, 5);
            
            i = 690; //Obsidian
            blocks[i] = new NasBlock(i, Material.Stone, 512, 3);
            i = 457; //Obsidian
            blocks[i] = new NasBlock(i, Material.Stone, 512, 3);
            blocks[i].interaction = PortalInteraction();

            
            i = 659;
            blocks[i] = new NasBlock(i, Material.Wood);
            blocks[i].interaction = ChangeInteraction(Block.FromRaw(658));
            blocks[i].dropHandler = CustomDrop(659, 1);
            i = 658; //trapdoors
            blocks[i] = new NasBlock(i, blocks[659]);
            blocks[i].interaction = ChangeInteraction(Block.FromRaw(659));
            blocks[i].dropHandler = CustomDrop(659, 1);
            i = 660;
            blocks[i] = new NasBlock(i, blocks[659]);
            blocks[i].interaction = ChangeInteraction(Block.FromRaw(661));
            blocks[i].dropHandler = CustomDrop(659, 1);
            i = 661;
            blocks[i] = new NasBlock(i, blocks[659]);
            blocks[i].interaction = ChangeInteraction(Block.FromRaw(660));
            blocks[i].dropHandler = CustomDrop(659, 1);
            i = 662;
            blocks[i] = new NasBlock(i, blocks[659]);
            blocks[i].interaction = ChangeInteraction(Block.FromRaw(663));
            blocks[i].dropHandler = CustomDrop(659, 1);
            i = 663;
            blocks[i] = new NasBlock(i, blocks[659]);
            blocks[i].interaction = ChangeInteraction(Block.FromRaw(662));
            blocks[i].dropHandler = CustomDrop(659, 1);
            i = 664;
            blocks[i] = new NasBlock(i, blocks[659]);
            blocks[i].interaction = ChangeInteraction(Block.FromRaw(665));
            blocks[i].dropHandler = CustomDrop(659, 1);
            i = 665;
            blocks[i] = new NasBlock(i, blocks[659]);
            blocks[i].interaction = ChangeInteraction(Block.FromRaw(664));
            blocks[i].dropHandler = CustomDrop(659, 1);
           
            	
            i = 12; //Sand
            blocks[i] = new NasBlock(i, Material.Earth, 3);
            blocks[i].disturbDelayMin = fallSpeed;
            blocks[i].disturbDelayMax = fallSpeed;
            blocks[i].disturbedAction = FallingBlockAction(Block.Sand);

            i = 451; //Soul sand
            blocks[i] = new NasBlock(i, Material.Earth, 3);
            blocks[i].disturbDelayMin = fallSpeed;
            blocks[i].disturbDelayMax = fallSpeed;
            
            i = 13; //Gravel
            blocks[i] = new NasBlock(i, Material.Earth);
            blocks[i].disturbDelayMin = 0.7f;
            blocks[i].disturbDelayMax = 0.7f;
            blocks[i].disturbedAction = FallingBlockAction(Block.Gravel);

            const float leafShrivelDelayMin = 0.2f;
            const float leafShrivelDelayMax = 0.4f;
            i = 18; //Leaves
            blocks[i] = new NasBlock(i, Material.Leaves);
            blocks[i].disturbedAction = LeafBlockAction(logSet, Block.Leaves);
            blocks[i].disturbDelayMin = leafShrivelDelayMin;
            blocks[i].disturbDelayMax = leafShrivelDelayMax;
            blocks[i].dropHandler = (NasPlayer, dropID) => {
                Drop drop = new Drop(18, 1);
                
                int rand = r.Next(0, 8);
                if (rand == 0) { //16 in 128 chance (1 in 8 chance) of sapling
                    drop.blockStacks.Add(new BlockStack(6, 1));
                } else {
                    drop = new Drop(18, 1);
                }
                return drop;
            };
            
            
            i = 103; //Pink leaves
            blocks[i] = new NasBlock(i, Material.Leaves);
            blocks[i].disturbedAction = LeafBlockAction(logSet, Block.Extended|103);
            blocks[i].disturbDelayMin = leafShrivelDelayMin;
            blocks[i].disturbDelayMax = leafShrivelDelayMax;
            blocks[i].dropHandler = (NasPlayer, dropID) => {
                Drop drop = new Drop(103, 1);
                
                int rand = r.Next(0, 8);
                if (rand == 0) { 
                    drop.blockStacks.Add(new BlockStack(154, 1));
                } else {
                    drop = new Drop(103, 1);
                }
                return drop;
            };
            
            
            i = 666; //dense Leaves
            blocks[i] = new NasBlock(i, Material.Leaves);
            blocks[i].disturbedAction = LeafBlockAction(logSet, Block.FromRaw(i));
            blocks[i].disturbDelayMin = 1f;
            blocks[i].disturbDelayMax = 1.5f;
            i = 686; //dense Leaves
            blocks[i] = new NasBlock(i, Material.Leaves);
            blocks[i].disturbedAction = LeafBlockAction(logSet, Block.FromRaw(i));
            blocks[i].disturbDelayMin = 1f;
            blocks[i].disturbDelayMax = 1.5f;
            
            i = 105; //Leaves slab
            blocks[i] = new NasBlock(i, Material.Leaves);
            
            i = 246; //pink Leaves slab
            blocks[i] = new NasBlock(i, Material.Leaves);
            
            i = 19; //Leaves spruce
            blocks[i] = new NasBlock(i, Material.Leaves);
            blocks[i].disturbedAction = LeafBlockAction(logSet, Block.Sponge);
            blocks[i].disturbDelayMin = leafShrivelDelayMin;
            blocks[i].disturbDelayMax = leafShrivelDelayMax;
            blocks[i].dropHandler = (NasPlayer, dropID) => {
                Drop drop = new Drop(19, 1);
                
                int rand = r.Next(0, 8);
                if (rand == 0) { 
                    drop.blockStacks.Add(new BlockStack(689, 1));
                } else {
                    drop = new Drop(19, 1);
                }
                return drop;
            };

            i = 20; //Glass
            blocks[i] = new NasBlock(i, Material.Glass);
            i = 136; //Glass pane
            blocks[i] = new NasBlock(i++, Material.Glass);
            blocks[i] = new NasBlock(i, blocks[136]);
            
            i = 687; //Lamp
            blocks[i] = new NasBlock(i, Material.Glass);
            blocks[i].disturbedAction = LampAction(Block.FromRaw(688), Block.FromRaw(687), Block.FromRaw(687));
            i++;
            blocks[i] = new NasBlock(i, Material.Glass);
            blocks[i].disturbedAction = LampAction(Block.FromRaw(688), Block.FromRaw(687), Block.FromRaw(688));
            blocks[i].dropHandler = CustomDrop(687, 1);
            
            i = 178; //Spikes
            blocks[i] = new NasBlock(i, Material.Stone);
            blocks[i].disturbedAction = LampAction(Block.FromRaw(179), Block.FromRaw(178), Block.FromRaw(178));
            i = 179;
            blocks[i] = new NasBlock(i, Material.Stone);
            blocks[i].disturbedAction = LampAction(Block.FromRaw(179), Block.FromRaw(178), Block.FromRaw(179));
            blocks[i].collideAction = SpikeCollideAction();
            blocks[i].dropHandler = CustomDrop(178, 1);
            i = 476; //Obsidian spikes
            blocks[i] = new NasBlock(i, Material.Stone, 128, 4);
            blocks[i].collideAction = SpikeCollideAction();
            
            
            
            i = 203; //Old glass
            blocks[i] = new NasBlock(i, Material.Glass);
            i = 209; //Old pane
            blocks[i] = new NasBlock(i++, Material.Glass);
            blocks[i] = new NasBlock(i, blocks[209]);
            
            i = 471; //New glass
            blocks[i] = new NasBlock(i, Material.Glass);
            i = 472; //New pane
            blocks[i] = new NasBlock(i++, Material.Glass);
            blocks[i] = new NasBlock(i, blocks[472]);
            

            i = 37; //Dandelion
            blocks[i] = new NasBlock(i, Material.Plant);
            blocks[i].dropHandler = (NasPlayer, dropID) => {
                if (NasPlayer.inventory.HeldItem.name == "Shears")
                {return new Drop(37, 1); }
                else {Drop yellowDrop = new Drop(35, 1); return (r.Next(0, 2) == 0) ? yellowDrop : null;}
            };
            blocks[i].disturbedAction = GenericPlantAction();

            i = 38; //Rose
            blocks[i] = new NasBlock(i, Material.Plant, 1);
            blocks[i].dropHandler = (NasPlayer, dropID) => {
                if (NasPlayer.inventory.HeldItem.name == "Shears")
                {return new Drop(38, 1); }
                else {Drop redDrop = new Drop(27, 1); return (r.Next(0, 2) == 0) ? redDrop : null;}
            };
            blocks[i].disturbedAction = GenericPlantAction();
            i = 39; //Dead shrub
            blocks[i] = new NasBlock(i, Material.Plant);
            blocks[i].disturbedAction = NeedsSupportAction();
            
            i = 40; //Tall grass
            blocks[i] = new NasBlock(i, Material.Plant);
            blocks[i].disturbedAction = GenericPlantAction();
            i = 130; //Wet tall grass
            blocks[i] = new NasBlock(i, Material.Plant);
            blocks[i].dropHandler = (NasPlayer, dropID) => {
                if (NasPlayer.inventory.HeldItem.name == "Shears")
                {return new Drop(130, 1); }
                else {Drop wheatDrop = new Drop(644, 1); return (r.Next(0, 8) == 0) ? wheatDrop : null;}
            };
            blocks[i].disturbedAction = GenericPlantAction();

            i = 41; //Gold
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 2);
            i = 42; //Iron
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            i = 631; //Diamond
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 3);
            i = 650; //Emerald
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 4);
			
            i = 148; //Old iron
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            i = 208;
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
           
            i = 149; //Old iron slab
            blocks[i] = new NasBlock(i++, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i] = new NasBlock(i, blocks[149]);
            i = 294; //Old iron wall
            blocks[i] = new NasBlock(i++, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i] = new NasBlock(i++, blocks[294]);
            blocks[i] = new NasBlock(i++, blocks[294]);
            blocks[i] = new NasBlock(i, blocks[294]);
            
            i = 159; //Iron fence-WE
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].collideAction = AirCollideAction();
            i++;
            blocks[i] = new NasBlock(i, blocks[159]);
            i = 161; //Iron cage
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].collideAction = AirCollideAction();
            

            i = 44; //Concrete slab-D
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone] / 2, 1);
            blocks[i].damageDoneToTool = 0.5f;
            i = 58; //Concrete slab-U
            blocks[i] = new NasBlock(i, blocks[44]);
            i = 43; //Double Concrete slab
            blocks[i] = new NasBlock(i, blocks[44]);
            blocks[i].durability = DefaultDurabilities[(int)Material.Stone];
            blocks[i].dropHandler = (NasPlayer, dropID) => { return new Drop(dropID, 2); };
            blocks[i].resourceCost = 2;
            blocks[i].damageDoneToTool = 1f;
            i = 45; //Concrete block
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            i = 282; //Concrete wall
            blocks[i] = new NasBlock(i++, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i] = new NasBlock(i++, blocks[282]);
            blocks[i] = new NasBlock(i++, blocks[282]);
            blocks[i] = new NasBlock(i++, blocks[282]);
            i = 549; //Concrete bricks[sic]
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            i = 135; //Stone plate
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone] / 2, 1);
            i = 270; //Concrete stairs
            blocks[i] = new NasBlock(i++, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i] = new NasBlock(i++, blocks[270]);
            blocks[i] = new NasBlock(i++, blocks[270]);
            blocks[i] = new NasBlock(i, blocks[270]);
            i = 587; //upper
            blocks[i] = new NasBlock(i++, blocks[270]);
            blocks[i] = new NasBlock(i++, blocks[270]);
            blocks[i] = new NasBlock(i++, blocks[270]);
            blocks[i] = new NasBlock(i, blocks[270]);
            i = 480; //Concrete corner
            blocks[i] = new NasBlock(i++, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i] = new NasBlock(i++, blocks[480]);
            blocks[i] = new NasBlock(i++, blocks[480]);
            blocks[i] = new NasBlock(i, blocks[480]);


            
            
            i = 453; //Obsidian slab-D
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            i = 298; //Obsidian slab-U
            blocks[i] = new NasBlock(i, blocks[453]);
            i = 49; //Coal block
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);

            i = 51; //Rope
            blocks[i] = new NasBlock(i, Material.Wood);

            i = 52; //Sandstone
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            i = 299; //Sandstone slab
            blocks[i] = new NasBlock(i++, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i] = new NasBlock(i, blocks[299]);

            i = 53; //Snow (layer)
            blocks[i] = new NasBlock(i, Material.Earth);
            blocks[i].disturbedAction = NeedsSupportAction();
			
            i = 140; //snow (block)
            blocks[i] = new NasBlock(i, Material.Earth);
            
            i = 677; //Sticky
            blocks[i] = new NasBlock(i, Material.Organic, 1);
            
            i = 54; //Fire
            blocks[i] = new NasBlock(i, Material.None);
            blocks[i].disturbDelayMin = 10f;
            blocks[i].disturbDelayMax = 15f;
            blocks[i].disturbedAction = FireAction();
            blocks[i].dropHandler = (NasPlayer, dropID) => {
                Drop fireDrop = new Drop(131, 1);
                return (r.Next(0, 100) == 0) ? fireDrop : null;
            };
            blocks[i].collideAction = FireCollideAction();

            i = 55; //Dark door
            blocks[i] = new NasBlock(i, Material.Wood);

            i = 470; //light door
            blocks[i] = new NasBlock(i, Material.Wood);
            
            i = 131; //ash
            blocks[i] = new NasBlock(i, Material.Plant);
            blocks[i].disturbDelayMin = fallSpeed;
            blocks[i].disturbDelayMax = fallSpeed;
            blocks[i].disturbedAction = FallingBlockAction(Block.FromRaw(i));
            
            i = 60; //Ice
            blocks[i] = new NasBlock(i, Material.Stone, 8);

            i = 681; //Packed Ice
            blocks[i] = new NasBlock(i, Material.Stone, 12);
            
            i = 61; //Quartz
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);

            i = 62; //Lamp
            blocks[i] = new NasBlock(i, Material.Glass);

            i = 63; //Pillar-UD
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            i = 166;
            blocks[i] = new NasBlock(i, blocks[63]);
            i = 167;
            blocks[i] = new NasBlock(i, blocks[63]);
            i = 235; //chiseled quartz
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            
			i = 84; //Quartz slab-D
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            i = 85; //Quartz slab-U
            blocks[i] = new NasBlock(i, blocks[84]);

            i = 286; //Quartz wall
            blocks[i] = new NasBlock(i++, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i] = new NasBlock(i++, blocks[286]);
            blocks[i] = new NasBlock(i++, blocks[286]);
            blocks[i] = new NasBlock(i++, blocks[286]);
            
            i = 274; //Quartz stair
            blocks[i] = new NasBlock(i++, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i] = new NasBlock(i++, blocks[274]);
            blocks[i] = new NasBlock(i++, blocks[274]);
            blocks[i] = new NasBlock(i++, blocks[274]);
            i = 591;
            blocks[i] = new NasBlock(i++, blocks[274]);
            blocks[i] = new NasBlock(i++, blocks[274]);
            blocks[i] = new NasBlock(i++, blocks[274]);
            blocks[i] = new NasBlock(i++, blocks[274]);
            
            i = 104; //Dry leaves
            blocks[i] = new NasBlock(i, Material.Leaves);
            blocks[i].disturbedAction = LeafBlockAction(logSet, Block.FromRaw(i));
            blocks[i].disturbDelayMin = leafShrivelDelayMin;
            blocks[i].disturbDelayMax = leafShrivelDelayMax;

            
            //Crafting table
            i = 198;
            blocks[i] = new NasBlock(i, blocks[17]);
            blocks[i].station = new Crafting.Station();
            blocks[i].station.name = "Crafting Table";
            blocks[i].station.type = Crafting.Station.Type.Normal;
            blocks[i].station.ori = Crafting.Station.Orientation.NS;
            blocks[i].existAction = CraftingExistAction();
            blocks[i].interaction = CraftingInteraction();
            i = 199;
            blocks[i] = new NasBlock(i, blocks[198]);
            blocks[i].station.ori = Crafting.Station.Orientation.WE;

            i = 413;//autocraft
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i].station = new Crafting.Station();
            blocks[i].station.name = "Auto Crafter";
            blocks[i].station.type = Crafting.Station.Type.Normal;
            blocks[i].station.ori = Crafting.Station.Orientation.NS;
            blocks[i].existAction = AutoCraftExistAction();
            blocks[i].interaction = AutoCraftInteraction();
            blocks[i].disturbedAction = AutoCraftingAction();
            blocks[i].container = new Container();
            blocks[i].container.type = Container.Type.AutoCraft;
            i = 414;
            blocks[i] = new NasBlock(i, blocks[413]);
            blocks[i].station.ori = Crafting.Station.Orientation.WE;
            
            
            
            i = 462; //birch crafting table
            blocks[i] = new NasBlock(i, blocks[242]);
            blocks[i].station = new Crafting.Station();
            blocks[i].station.name = "Crafting Table";
            blocks[i].station.type = Crafting.Station.Type.Normal;
            blocks[i].station.ori = Crafting.Station.Orientation.NS;
            blocks[i].existAction = CraftingExistAction();
            blocks[i].interaction = CraftingInteraction();
            blocks[i].alternateID = 198;
            i = 463;
            blocks[i] = new NasBlock(i, blocks[462]);
            blocks[i].station.ori = Crafting.Station.Orientation.WE;
            blocks[i].alternateID = 199;
            
            i = 464; //spruce crafting table
            blocks[i] = new NasBlock(i, blocks[250]);
            blocks[i].station = new Crafting.Station();
            blocks[i].station.name = "Crafting Table";
            blocks[i].station.type = Crafting.Station.Type.Normal;
            blocks[i].station.ori = Crafting.Station.Orientation.NS;
            blocks[i].existAction = CraftingExistAction();
            blocks[i].interaction = CraftingInteraction();
            blocks[i].alternateID = 198;
            i = 465;
            blocks[i] = new NasBlock(i, blocks[464]);
            blocks[i].station.ori = Crafting.Station.Orientation.WE;
            blocks[i].alternateID = 199;
            
            //Furnace
            i = 625;
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i].station = new Crafting.Station();
            blocks[i].station.name = "Furnace";
            blocks[i].station.type = Crafting.Station.Type.Furnace;
            blocks[i].station.ori = Crafting.Station.Orientation.WE;
            blocks[i].existAction = CraftingExistAction();
            blocks[i].interaction = CraftingInteraction();
            i = 626;
            blocks[i] = new NasBlock(i, blocks[625]);
            blocks[i].station.ori = Crafting.Station.Orientation.NS;

            i = 239; //hotcoals
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i].collideAction = FireCollideAction();

            i = 204; //Monitor-S
            blocks[i] = new NasBlock(i++, Material.Metal, 3);
            blocks[i] = new NasBlock(i++, blocks[204]);
            blocks[i] = new NasBlock(i++, blocks[204]);
            blocks[i] = new NasBlock(i, blocks[204]);

            
            
            i = 142; //Crate
            blocks[i] = new NasBlock(i, Material.Wood, DefaultDurabilities[(int)Material.Wood]*2);
            blocks[i].interaction = CrateInteraction("You can't open it. It's just for decoration.");
            
            i = 132; //bookshelf
            blocks[i] = new NasBlock(i, Material.Wood, DefaultDurabilities[(int)Material.Wood]*2);
            blocks[i].interaction = BookshelfInteraction();
            
            i = 143; //Barrel
            blocks[i] = new NasBlock(i, Material.Wood, DefaultDurabilities[(int)Material.Wood]*2);
            blocks[i].container = new Container();
            blocks[i].container.type = Container.Type.Barrel;
            blocks[i].existAction = ContainerExistAction();
            blocks[i].interaction = ContainerInteraction();
            i = 602; //Barrel (sideways)
            blocks[i] = new NasBlock(i++, blocks[143]);
            blocks[i] = new NasBlock(i, blocks[143]);
            
            
            i = 216; //Chest-S
            blocks[i] = new NasBlock(i, Material.Wood, DefaultDurabilities[(int)Material.Wood]*2);
            blocks[i].container = new Container();
            blocks[i].container.type = Container.Type.Chest;
            blocks[i].existAction = ContainerExistAction();
            blocks[i].interaction = ContainerInteraction();
            i++;
            blocks[i] = new NasBlock(i, blocks[216]);
            i++;
            blocks[i] = new NasBlock(i, blocks[216]);
            i++;
            blocks[i] = new NasBlock(i, blocks[216]);
            
            
            i = 647; //Gravestone
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 0);
            blocks[i].container = new Container();
            blocks[i].container.type = Container.Type.Gravestone;
            blocks[i].existAction = ContainerExistAction();
            blocks[i].interaction = ContainerInteraction();
            
            
			i = 586; //Quartz ore
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone] + 2, 1);
            blocks[i].dropHandler = CustomDrop(61, 1);
            blocks[i].expGivenMin = 4;
            blocks[i].expGivenMax = 10;
            
            i = 454; //Nether quartz ore
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone] + 2, 1);
            blocks[i].dropHandler = CustomDrop(61, 1);
            blocks[i].expGivenMin = 4;
            blocks[i].expGivenMax = 10;
            
            i = 455; //Nether gold ore
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone] + 2, 1);
            blocks[i].dropHandler = CustomDrop(672, 1);
            blocks[i].expGivenMin = 6;
            blocks[i].expGivenMax = 12;
            
            i = 627; //Coal ore
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone] + 2, 1);
            blocks[i].dropHandler = (NasPlayer, dropID) => {return new Drop(197, r.Next(2, 5));};
            blocks[i].expGivenMin = 0;
            blocks[i].expGivenMax = 4;
            
            i = 197; //Chunk of coal
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone] + 2, 1);
            
            i = 628; //Iron ore
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone] + 4, 1);
            //blocks[i].expGivenMin = 0;
            //blocks[i].expGivenMax = 4;
            
            i = 629; //Gold ore
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone] + 6, 2);
            //blocks[i].expGivenMin = 1;
            //blocks[i].expGivenMax = 6;
            
            i = 630; //Diamond ore
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone] + 6, 3);
            //blocks[i].expGivenMin = 3;
            //blocks[i].expGivenMax = 8;
            
            i = 649; //Emerald ore
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone] + 7, 4);
            //blocks[i].expGivenMin = 4;
            //blocks[i].expGivenMax = 9;
            
            //from 20 to 40 minutes (avg 30)
            const float wheatTotalSeconds = 20f * 60f;
            const float wheatMaxAddedSeconds = 20f * 60f;
            const float wheatGrowMin = (wheatTotalSeconds)/3f;
            const float wheatGrowMax = (wheatTotalSeconds + wheatMaxAddedSeconds)/3f;
            i = 644; //Wheat (baby)
            blocks[i] = new NasBlock(i, Material.Plant);
            blocks[i].disturbedAction = CropAction(wheatSet, 0);
            blocks[i].disturbDelayMin = wheatGrowMin;
            blocks[i].disturbDelayMax = wheatGrowMax;
            blocks[i].dropHandler = CustomDrop(644, 1);
            i = 645;
            blocks[i] = new NasBlock(i, Material.Plant);
            blocks[i].disturbedAction = CropAction(wheatSet, 1);
            blocks[i].disturbDelayMin = wheatGrowMin;
            blocks[i].disturbDelayMax = wheatGrowMax;
            blocks[i].dropHandler = CustomDrop(644, 1);
            i = 646;
            blocks[i] = new NasBlock(i, Material.Plant);
            blocks[i].disturbedAction = CropAction(wheatSet, 2);
            blocks[i].disturbDelayMin = wheatGrowMin;
            blocks[i].disturbDelayMax = wheatGrowMax;
            blocks[i].dropHandler = CustomDrop(644, 1);
            i = 461; //Wheat (full grown)
            blocks[i] = new NasBlock(i, Material.Plant);
            blocks[i].disturbedAction = CropAction(wheatSet, 3);
            blocks[i].dropHandler = (NasPlayer, dropID) => {
                if (r.Next(0, 2) == 0) {
                    return new Drop(145, 1);
                }
                return new Drop(644, r.Next(2, 5));
                
                //Drop drop = new Drop();
                //drop.blockStacks = new List<BlockStack>();
                //drop.blockStacks.Add(new BlockStack(644, r.Next(1, 4)));
                //drop.blockStacks.Add(new BlockStack(145, 1));
                //return drop;
            };
            
            i = 624; //iron nug
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbDelayMin = fallSpeed/2f;
            blocks[i].disturbDelayMax = fallSpeed/2f;
            blocks[i].disturbedAction = FallingBlockAction(Block.FromRaw(i));
            
            i = 729; //Iron (baby)
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbedAction = IronCropAction(ironSet, 0);
            blocks[i].disturbDelayMin = wheatGrowMin*2;
            blocks[i].disturbDelayMax = wheatGrowMax*2;
            blocks[i].dropHandler = CustomDrop(729, 1);
            i = 730;
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbedAction = IronCropAction(ironSet, 1);
            blocks[i].disturbDelayMin = wheatGrowMin*2;
            blocks[i].disturbDelayMax = wheatGrowMax*2;
            blocks[i].dropHandler = CustomDrop(729, 1);
            i = 731;
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbedAction = IronCropAction(ironSet, 2);
            blocks[i].disturbDelayMin = wheatGrowMin*2;
            blocks[i].disturbDelayMax = wheatGrowMax*2;
            blocks[i].dropHandler = CustomDrop(729, 1);
            i = 479; //Iron (full grown)
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbedAction = IronCropAction(ironSet, 3);
            blocks[i].dropHandler = (NasPlayer, dropID) => {
            	Drop finalDrop = new Drop(729, r.Next(1,4));
                if (r.Next(0, 2) == 0) {
            		finalDrop.blockStacks.Add(new BlockStack(624, 1));
                }
            	return finalDrop;
                //Drop drop = new Drop();
                //drop.blockStacks = new List<BlockStack>();
                //drop.blockStacks.Add(new BlockStack(644, r.Next(1, 4)));
                //drop.blockStacks.Add(new BlockStack(145, 1));
                //return drop;
            };
            
            const float sugarTotalSeconds = 10f * 60f;
            const float sugarMaxAddedSeconds = 15f * 60f;
            const float sugarGrowMin = (sugarTotalSeconds)/2f;
            const float sugarGrowMax = (sugarTotalSeconds + sugarMaxAddedSeconds)/2f;
            
            i = 667; //sugarcane
            blocks[i] = new NasBlock(i, Material.Plant);
            blocks[i].disturbDelayMin = sugarGrowMin;
            blocks[i].disturbDelayMax = sugarGrowMax;
            blocks[i].disturbedAction = GrowAction(Block.FromRaw(i));
            
            i = 106; //cactus
            blocks[i] = new NasBlock(i, Material.Plant);
            blocks[i].disturbDelayMin = sugarGrowMin;
            blocks[i].disturbDelayMax = sugarGrowMax;
            blocks[i].disturbedAction = GrowAction(Block.FromRaw(i));
            
            i = 107; //vines
            blocks[i] = new NasBlock(i, Material.Plant);
            blocks[i].disturbDelayMin = sugarGrowMin/5f;
            blocks[i].disturbDelayMax = sugarGrowMax/5f;
            blocks[i].disturbedAction = VineGrowAction(Block.FromRaw(i));
            blocks[i].instantAction = VineDeathAction();
            	
            i = 146; //Swamp leaves
            blocks[i] = new NasBlock(i, Material.Leaves);
            blocks[i].disturbedAction = LeafBlockAction(logSet, Block.FromRaw(146));
            blocks[i].disturbDelayMin = leafShrivelDelayMin;
            blocks[i].disturbDelayMax = leafShrivelDelayMax;
            blocks[i].dropHandler = (NasPlayer, dropID) => {
                Drop drop = new Drop(146, 1);
                
                int rand = r.Next(0, 8);
                if (rand == 0) { //16 in 128 chance (1 in 8 chance) of sapling
                    drop.blockStacks.Add(new BlockStack(450, 1));
                } else {
                    drop = new Drop(146, 1);
                }
                return drop;
            };
            
            i = 449; //lilypad
            blocks[i] = new NasBlock(i, Material.Plant);
            blocks[i].disturbedAction = LilyAction();
            
            
            i = 171; //sign
            blocks[i] = new NasBlock(i, Material.None, DefaultDurabilities[(int)Material.Wood]);
            blocks[i].existAction = MessageExistAction();
            blocks[i].interaction = MessageInteraction();
            
            i = 145; //hay
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].fallDamageMultiplier = 0.1f;
            i = 622;
            blocks[i] = new NasBlock(i++, blocks[145]);
            blocks[i] = new NasBlock(i++, blocks[145]);
            
            
            const float breadRestore = 1f;
            i = 640; //Loaf of bread
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].interaction = EatInteraction(breadSet, 0, breadRestore*2);
            i++;
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].interaction = EatInteraction(breadSet, 1, breadRestore);
            i++;
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].interaction = EatInteraction(breadSet, 2, breadRestore);
            
            const float pieRestore = 2.5f;
            i = 542; //Waffle
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].interaction = EatInteraction(waffleSet, 0, pieRestore);
            i++;
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].interaction = EatInteraction(waffleSet, 1, pieRestore);
            
            i = 668; //Pie
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].interaction = EatInteraction(pieSet, 0, pieRestore);
            i++;
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].interaction = EatInteraction(pieSet, 1, pieRestore);
            i++;
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].interaction = EatInteraction(pieSet, 2, pieRestore);
            i++;
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].interaction = EatInteraction(pieSet, 3, pieRestore);
            
            i = 698; //Peach pie
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].interaction = EatInteraction(peachPieSet, 0, pieRestore);
            i++;
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].interaction = EatInteraction(peachPieSet, 1, pieRestore);
            i++;
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].interaction = EatInteraction(peachPieSet, 2, pieRestore);
            i++;
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].interaction = EatInteraction(peachPieSet, 3, pieRestore);
            
            
            
            i = 654; //Poison pie and bread
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].interaction = EatInteraction(new BlockID[] { Block.FromRaw(648) } , 0, -6f);
            
            i = 652;
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].interaction = EatInteraction(new BlockID[] { Block.FromRaw(648) } , 0, -5f);
            
            i = 648; //Apple
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].disturbDelayMin = fallSpeed;
            blocks[i].disturbDelayMax = fallSpeed;
            blocks[i].disturbedAction = FallingBlockAction(Block.FromRaw(i));
            blocks[i].interaction = EatInteraction(new BlockID[] { Block.FromRaw(648) } , 0, 1f);
            
            i = 702; //Peach
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].disturbDelayMin = fallSpeed;
            blocks[i].disturbDelayMax = fallSpeed;
            blocks[i].disturbedAction = FallingBlockAction(Block.FromRaw(i));
            blocks[i].interaction = EatInteraction(new BlockID[] { Block.FromRaw(702) } , 0, 1f);
            
            i = 478; //Gapple
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].disturbDelayMin = fallSpeed/2f;
            blocks[i].disturbDelayMax = fallSpeed/2f;
            blocks[i].disturbedAction = FallingBlockAction(Block.FromRaw(i));
            blocks[i].interaction = EatInteraction(new BlockID[] { Block.FromRaw(478) } , 0, 10f, 0.5f);
            
            i = 36; //white
            blocks[i] = new NasBlock(i, Material.Organic, 4);
          
            i = 27; //red
            blocks[i] = new NasBlock(i, Material.Organic, 4);
            blocks[i].alternateID = 36;
            
            i = 35; //yellow
            blocks[i] = new NasBlock(i, Material.Organic, 4);
            blocks[i].alternateID = 36;
            
            i = 30; //orange
            blocks[i] = new NasBlock(i, Material.Organic, 4);
            blocks[i].alternateID = 36;
            
            i = 138; //pink
            blocks[i] = new NasBlock(i, Material.Organic, 4);
            blocks[i].alternateID = 36;
            
            i = 23; //blue
            blocks[i] = new NasBlock(i, Material.Organic, 4);
            blocks[i].alternateID = 36;
            
            i = 29; //cyan
            blocks[i] = new NasBlock(i, Material.Organic, 4);
            blocks[i].alternateID = 36;
            
            i = 34; //light blue
            blocks[i] = new NasBlock(i, Material.Organic, 4);
            blocks[i].alternateID = 36;
            
            i = 26; //green
            blocks[i] = new NasBlock(i, Material.Organic, 4);
            blocks[i].alternateID = 36;
            
            i = 32; //lime
            blocks[i] = new NasBlock(i, Material.Organic, 4);
            blocks[i].alternateID = 36;
            
            i = 200; //purple
            blocks[i] = new NasBlock(i, Material.Organic, 4);
            blocks[i].alternateID = 36;
            
            i = 22; //darkpurple
            blocks[i] = new NasBlock(i, Material.Organic, 4);
            blocks[i].alternateID = 36;
            
            i = 21; //black
            blocks[i] = new NasBlock(i, Material.Organic, 4);
            blocks[i].alternateID = 36;
            
            i = 25; //brown
            blocks[i] = new NasBlock(i, Material.Organic, 4);
            blocks[i].alternateID = 36;
            
            i = 31; //lightgray
            blocks[i] = new NasBlock(i, Material.Organic, 4);
            blocks[i].alternateID = 36;
            
            i = 28; //gray
            blocks[i] = new NasBlock(i, Material.Organic, 4);
            blocks[i].alternateID = 36;
            
            i = 703; //bed
            blocks[i] = new NasBlock(i, Material.Organic, 4);
            blocks[i].interaction = BedInteraction();
            
            i = 96; //daisy
            blocks[i] = new NasBlock(i, Material.Plant);
            blocks[i].dropHandler = (NasPlayer, dropID) => {
                if (NasPlayer.inventory.HeldItem.name == "Shears")
                {return new Drop(96, 1); }
                else {Drop whiteDrop = new Drop(36, 1); return (r.Next(0, 2) == 0) ? whiteDrop : null;}
            };
            blocks[i].disturbedAction = GenericPlantAction();
            
            i = 651; //cornflower
            blocks[i] = new NasBlock(i, Material.Plant);
            blocks[i].dropHandler = (NasPlayer, dropID) => {
                if (NasPlayer.inventory.HeldItem.name == "Shears")
                {return new Drop(651, 1); }
                else {Drop blueDrop = new Drop(23, 1); return (r.Next(0, 3) == 0) ? blueDrop : null;}
            };
            blocks[i].disturbedAction = GenericPlantAction();
            
            i = 201; //Daisy
            blocks[i] = new NasBlock(i, Material.Plant);
            blocks[i].dropHandler = (NasPlayer, dropID) => {
                if (NasPlayer.inventory.HeldItem.name == "Shears")
                {return new Drop(201, 1); }
                else {Drop pinkDrop = new Drop(138, 1); return (r.Next(0, 2) == 0) ? pinkDrop : null;}
            };
            blocks[i].disturbedAction = GenericPlantAction();
            
            
            i = 604; //Red mushroom
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].interaction = EatInteraction(new BlockID[] { Block.FromRaw(604) } , 0, 1f);
            i = 456; //Brown mushroom
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].interaction = EatInteraction(new BlockID[] { Block.FromRaw(604) } , 0, 1f);
            i = 653; //Poison Mushroom
            blocks[i] = new NasBlock(i, Material.Organic, 3);
            blocks[i].interaction = EatInteraction(new BlockID[] { Block.FromRaw(604) } , 0, -2.5f);
            
            for (i = 484; i < 524; i++) 
            {blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);}
            
            for (i = 713; i <= 728; i++) 
            {blocks[i] = new NasBlock(i, Material.Organic, 2);}
            
            i = 655; //glass gravity
            blocks[i] = new NasBlock(i, Material.Glass);
            blocks[i].disturbDelayMin = fallSpeed;
            blocks[i].disturbDelayMax = fallSpeed;
            blocks[i].disturbedAction = FallingBlockAction(Block.FromRaw(i));
            blocks[i].collideAction = AirCollideAction();
			
            i = 696; //tank of lava
            blocks[i] = new NasBlock(i, Material.Stone, 384, 4);
            blocks[i].childIDs = new List<BlockID>();
            blocks[i].childIDs.Add(10);
            
            i = 697; 
            blocks[i] = new NasBlock(i, Material.Stone, 384, 4);
            blocks[i].existAction = LavaBarrelAction();
            
            i = 672; //unrefined gold
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].existAction = WireExistAction(0, 4);
            blocks[i].disturbedAction = UnrefinedGoldAction(Block.FromRaw(237), Block.FromRaw(672), Block.FromRaw(672));
            
            i = 237;
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].existAction = WireBreakAction();
            blocks[i].disturbedAction = UnrefinedGoldAction(Block.FromRaw(237), Block.FromRaw(672), Block.FromRaw(237));
            blocks[i].dropHandler = CustomDrop(672, 1);
            
            i = 674;//lever
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].interaction = ChangeInteraction(Block.FromRaw(675));
            blocks[i].existAction = WireExistAction(0, 4);
            i = 675;
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].interaction = ChangeInteraction(Block.FromRaw(674));
            blocks[i].dropHandler = CustomDrop(674, 1);
            blocks[i].existAction = WireExistAction(15, 4);
            
            
            i = 74; //power source 
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbedAction = PowerSourceAction(4);
            blocks[i].existAction = WireExistAction(15, 4);
            
            i = 195;//button
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].interaction = ChangeInteraction(Block.FromRaw(196));
            blocks[i].existAction = WireExistAction(0, 4);
            i = 196;
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbDelayMin = 1.2f;
            blocks[i].disturbDelayMax = 1.2f;
            blocks[i].disturbedAction = TurnOffAction();
            blocks[i].dropHandler = CustomDrop(195, 1);
            blocks[i].existAction = WireExistAction(15, 4);
            

            i = 704; //piston
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbDelayMin = 0.125f;
            blocks[i].disturbDelayMax = 0.125f;
            blocks[i].disturbedAction = PistonAction("off", 0, 1, 0, pistonUp);
            
            i = 705; //piston body
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbDelayMin = 0.125f;
            blocks[i].disturbDelayMax = 0.125f;
            blocks[i].disturbedAction = PistonAction("body", 0, 1, 0, pistonUp);
            blocks[i].dropHandler = CustomDrop(704, 1);
            
            i = 706; //piston head
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbedAction = PistonAction("head", 0, 1, 0, pistonUp);
			blocks[i].dropHandler = (NasPlayer, dropID) => {
            blocks[i].collideAction = AirCollideAction();
                Drop pistonDrop = new Drop(1, 1);
                return (1 == 0) ? pistonDrop : null;};
            
            i = 707; //piston
            blocks[i] = new NasBlock(i, blocks[704]);
            blocks[i].disturbedAction = PistonAction("off", 0, -1, 0, pistonDown);
            
            i = 708; //piston body
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbDelayMin = 0.125f;
            blocks[i].disturbDelayMax = 0.125f;
            blocks[i].disturbedAction = PistonAction("body", 0, -1, 0, pistonDown);
            blocks[i].dropHandler = CustomDrop(704, 1);
            
            i = 709; //piston head
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbedAction = PistonAction("head", 0, -1, 0, pistonDown);
			blocks[i].dropHandler = (NasPlayer, dropID) => {
            blocks[i].collideAction = AirCollideAction();
                Drop pistonDrop = new Drop(1, 1);
                return (1 == 0) ? pistonDrop : null;};
            
            
            i = 678; //Spiston
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbDelayMin = 0.125f;
            blocks[i].disturbDelayMax = 0.125f;
            blocks[i].disturbedAction = StickyPistonAction("off", 0, 1, 0, stickyPistonUp);
            
            i = 679; //Spiston body
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbDelayMin = 0.125f;
            blocks[i].disturbDelayMax = 0.125f;
            blocks[i].disturbedAction = StickyPistonAction("body", 0, 1, 0, stickyPistonUp);
            blocks[i].dropHandler = CustomDrop(678, 1);
            
            i = 680; //Spiston head
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbedAction = StickyPistonAction("head", 0, 1, 0, stickyPistonUp);
			blocks[i].dropHandler = (NasPlayer, dropID) => {
            blocks[i].collideAction = AirCollideAction();
                Drop pistonDrop = new Drop(1, 1);
                return (1 == 0) ? pistonDrop : null;};
            
            
            i = 710; //Spiston
            blocks[i] = new NasBlock(i, blocks[678]);
            blocks[i].disturbedAction = StickyPistonAction("off", 0, -1, 0, stickyPistonDown);
            
            i = 711; //Spiston body
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbDelayMin = 0.125f;
            blocks[i].disturbDelayMax = 0.125f;
            blocks[i].disturbedAction = StickyPistonAction("body", 0, -1, 0, stickyPistonDown);
            blocks[i].dropHandler = CustomDrop(678, 1);
            
            i = 712; //Spiston head
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbedAction = StickyPistonAction("head", 0, -1, 0, stickyPistonDown);
			blocks[i].dropHandler = (NasPlayer, dropID) => {
            blocks[i].collideAction = AirCollideAction();
                Drop pistonDrop = new Drop(1, 1);
                return (1 == 0) ? pistonDrop : null;};

            DefinePiston(389, pistonNorth, "z", 1, 704);
            DefinePiston(392, pistonEast, "x", -1, 704);
            DefinePiston(395, pistonSouth, "z", -1, 704);
            DefinePiston(398, pistonWest, "x", 1, 704);
            
            DefinePiston(401, stickyPistonNorth, "z", 1, 678, true);
            DefinePiston(404, stickyPistonEast, "x", -1, 678, true);
            DefinePiston(407, stickyPistonSouth, "z", -1, 678, true);
            DefinePiston(410, stickyPistonWest, "x", 1, 678, true);
            
            i = 609; //Beacon
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].existAction = BeaconInteractAction();
            
            i = 445; //Beacon
            blocks[i] = new NasBlock(i, blocks[1]);
            blocks[i].existAction = BeaconInteractAction();
            blocks[i].durability = DefaultDurabilities[(int)Material.Metal];
            
            i = 612; //Bed beacon
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].existAction = BedBeaconAction();
            
            i = 550; //wiring
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].existAction = WireExistAction(0, 1);
            blocks[i].disturbedAction = WireAction(wireSetActive, wireSetInactive, 1, Block.FromRaw(550));
            i++; 
            blocks[i] = new NasBlock(i, blocks[550]);
            blocks[i].existAction = WireExistAction(0, 0);
            blocks[i].disturbedAction = WireAction(wireSetActive, wireSetInactive, 0, Block.FromRaw(551));
            i++; 
            blocks[i] = new NasBlock(i, blocks[550]);
            blocks[i].existAction = WireExistAction(0, 2);
            blocks[i].disturbedAction = WireAction(wireSetActive, wireSetInactive, 2, Block.FromRaw(552));
            
            i = 682; //wiring
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbedAction = WireAction(wireSetActive, wireSetInactive, 1, Block.FromRaw(682));
            blocks[i].existAction = WireBreakAction();
            blocks[i].dropHandler = CustomDrop(550, 1);
            i++; 
            blocks[i] = new NasBlock(i, blocks[682]);
            blocks[i].disturbedAction = WireAction(wireSetActive, wireSetInactive, 0, Block.FromRaw(683));
            i++; 
            blocks[i] = new NasBlock(i, blocks[682]);
            blocks[i].disturbedAction = WireAction(wireSetActive, wireSetInactive, 2, Block.FromRaw(684));
            
            i = 732; //fixed wiring
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].existAction = WireExistAction(0, 12);
            blocks[i].disturbedAction = WireAction(fixedWireSetActive, fixedWireSetInactive, 1, Block.FromRaw(732));
            i++; 
            blocks[i] = new NasBlock(i, blocks[732]);
            blocks[i].existAction = WireExistAction(0, 11);
            blocks[i].disturbedAction = WireAction(fixedWireSetActive, fixedWireSetInactive, 0, Block.FromRaw(733));
            i++; 
            blocks[i] = new NasBlock(i, blocks[732]);
            blocks[i].existAction = WireExistAction(0, 13);
            blocks[i].disturbedAction = WireAction(fixedWireSetActive, fixedWireSetInactive, 2, Block.FromRaw(734));
            i++;  // powered fixedf
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbedAction = WireAction(fixedWireSetActive, fixedWireSetInactive, 1, Block.FromRaw(735));
            blocks[i].existAction = WireBreakAction();
            blocks[i].dropHandler = CustomDrop(732, 1);
            i++; 
            blocks[i] = new NasBlock(i, blocks[735]);
            blocks[i].disturbedAction = WireAction(fixedWireSetActive, fixedWireSetInactive, 0, Block.FromRaw(736));
            i++; 
            blocks[i] = new NasBlock(i, blocks[735]);
            blocks[i].disturbedAction = WireAction(fixedWireSetActive, fixedWireSetInactive,  2, Block.FromRaw(737));
            
            
            i = 172; // repeater
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].existAction = WireExistAction(0, 5);
            blocks[i].disturbDelayMin = 0f;
            blocks[i].disturbDelayMax = 0f;
            blocks[i].disturbedAction = RepeaterAction(5, Block.FromRaw(172));
            i++;
            blocks[i] = new NasBlock(i, blocks[172]);
            blocks[i].existAction = WireExistAction(0, 6);
            blocks[i].disturbDelayMin = 0f;
            blocks[i].disturbDelayMax = 0f;
            blocks[i].disturbedAction = RepeaterAction(6, Block.FromRaw(173));
            i++;
            blocks[i] = new NasBlock(i, blocks[172]);
            blocks[i].existAction = WireExistAction(0, 7);
            blocks[i].disturbDelayMin = 0f;
            blocks[i].disturbDelayMax = 0f;
            blocks[i].disturbedAction = RepeaterAction(7, Block.FromRaw(174));
            i++;
            blocks[i] = new NasBlock(i, blocks[172]);
            blocks[i].existAction = WireExistAction(0, 8);
            blocks[i].disturbDelayMin = 0f;
            blocks[i].disturbDelayMax = 0f;
            blocks[i].disturbedAction = RepeaterAction(8, Block.FromRaw(175));
            i++;
            blocks[i] = new NasBlock(i, blocks[172]);
            blocks[i].existAction = WireExistAction(0, 9);
            blocks[i].disturbDelayMin = 0f;
            blocks[i].disturbDelayMax = 0f;
            blocks[i].disturbedAction = RepeaterAction(9, Block.FromRaw(176));
            i++;
            blocks[i] = new NasBlock(i, blocks[172]);
            blocks[i].existAction = WireExistAction(0, 10);
            blocks[i].disturbDelayMin = 0f;
            blocks[i].disturbDelayMax = 0f;
            blocks[i].disturbedAction = RepeaterAction(10, Block.FromRaw(177));
            
            i = 613; //yes this too
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].existAction = WireBreakAction();
            blocks[i].disturbDelayMin = 0f;
            blocks[i].disturbDelayMax = 0f;
            blocks[i].disturbedAction = RepeaterAction(5, Block.FromRaw(613));
            blocks[i].dropHandler = CustomDrop(172, 1);
            i++;
            blocks[i] = new NasBlock(i, blocks[613]);
            blocks[i].disturbDelayMin = 0f;
            blocks[i].disturbDelayMax = 0f;
            blocks[i].disturbedAction = RepeaterAction(6, Block.FromRaw(614));
            blocks[i].dropHandler = CustomDrop(172, 1);
            i++;
            blocks[i] = new NasBlock(i, blocks[613]);
            blocks[i].disturbDelayMin = 0f;
            blocks[i].disturbDelayMax = 0f;
            blocks[i].disturbedAction = RepeaterAction(7, Block.FromRaw(615));
            blocks[i].dropHandler = CustomDrop(172, 1);
            i++;
            blocks[i] = new NasBlock(i, blocks[613]);
            blocks[i].disturbDelayMin = 0f;
            blocks[i].disturbDelayMax = 0f;
            blocks[i].disturbedAction = RepeaterAction(8, Block.FromRaw(616));
            blocks[i].dropHandler = CustomDrop(172, 1);
            i++;
            blocks[i] = new NasBlock(i, blocks[613]);
            blocks[i].disturbDelayMin = 0f;
            blocks[i].disturbDelayMax = 0f;
            blocks[i].disturbedAction = RepeaterAction(9, Block.FromRaw(617));
            blocks[i].dropHandler = CustomDrop(172, 1);
            i++;
            blocks[i] = new NasBlock(i, blocks[613]);
            blocks[i].disturbDelayMin = 0f;
            blocks[i].disturbDelayMax = 0f;
            blocks[i].disturbedAction = RepeaterAction(10, Block.FromRaw(618));
            blocks[i].dropHandler = CustomDrop(172, 1);
            
            i = 610; //pressure plates
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i].existAction = WireExistAction(0, 4);
            blocks[i].collideAction = PressureCollideAction();
        	
            i = 611;
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i].existAction = WireBreakAction();
            blocks[i].disturbDelayMin = 1f;
            blocks[i].disturbDelayMax = 1f;
            blocks[i].disturbedAction = PressurePlateAction();
            blocks[i].dropHandler = CustomDrop(610, 1);
            
            
            i = 415; //observers
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i].existAction = WireExistAction(0, 9);
            blocks[i].disturbDelayMin = 0.2f;
            blocks[i].disturbDelayMax = 0.2f;
            blocks[i].disturbedAction = ObserverActivateAction(9);
            i++;
            blocks[i] = new NasBlock(i, blocks[415]);
            blocks[i].existAction = WireExistAction(0, 10);
            blocks[i].disturbDelayMin = 0.2f;
            blocks[i].disturbDelayMax = 0.2f;
            blocks[i].disturbedAction = ObserverActivateAction(10);
            i++;
            blocks[i] = new NasBlock(i, blocks[415]);
            blocks[i].existAction = WireExistAction(0, 7);
            blocks[i].disturbDelayMin = 0.2f;
            blocks[i].disturbDelayMax = 0.2f;
            blocks[i].disturbedAction = ObserverActivateAction(7);
            i++;
            blocks[i] = new NasBlock(i, blocks[415]);
            blocks[i].existAction = WireExistAction(0, 8);
            blocks[i].disturbDelayMin = 0.2f;
            blocks[i].disturbDelayMax = 0.2f;
            blocks[i].disturbedAction = ObserverActivateAction(8);
            i++;
            blocks[i] = new NasBlock(i, blocks[415]);
            blocks[i].existAction = WireExistAction(0, 5);
            blocks[i].disturbDelayMin = 0.2f;
            blocks[i].disturbDelayMax = 0.2f;
            blocks[i].disturbedAction = ObserverActivateAction(5);
            i++;
            blocks[i] = new NasBlock(i, blocks[415]);
            blocks[i].existAction = WireExistAction(0, 6);
            blocks[i].disturbDelayMin = 0.2f;
            blocks[i].disturbDelayMax = 0.2f;
            blocks[i].disturbedAction = ObserverActivateAction(6);
            
            i = 421;
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i].existAction = WireBreakAction();
            blocks[i].disturbDelayMin = 0.2f;
            blocks[i].disturbDelayMax = 0.2f;
            blocks[i].disturbedAction = ObserverDeactivateAction(9);
            blocks[i].dropHandler = CustomDrop(415, 1);
            i++;
            blocks[i] = new NasBlock(i, blocks[421]);
            blocks[i].existAction = WireBreakAction();
            blocks[i].disturbDelayMin = 0.2f;
            blocks[i].disturbDelayMax = 0.2f;
            blocks[i].disturbedAction = ObserverDeactivateAction(10);
            blocks[i].dropHandler = CustomDrop(415, 1);
            i++;
            blocks[i] = new NasBlock(i, blocks[421]);
            blocks[i].existAction = WireBreakAction();
            blocks[i].disturbDelayMin = 0.2f;
            blocks[i].disturbDelayMax = 0.2f;
            blocks[i].disturbedAction = ObserverDeactivateAction(7);
            blocks[i].dropHandler = CustomDrop(415, 1);
            i++;
            blocks[i] = new NasBlock(i, blocks[421]);
            blocks[i].existAction = WireBreakAction();
            blocks[i].disturbDelayMin = 0.2f;
            blocks[i].disturbDelayMax = 0.2f;
            blocks[i].disturbedAction = ObserverDeactivateAction(8);
            blocks[i].dropHandler = CustomDrop(415, 1);
            i++;
            blocks[i] = new NasBlock(i, blocks[421]);
            blocks[i].existAction = WireBreakAction();
            blocks[i].disturbDelayMin = 0.2f;
            blocks[i].disturbDelayMax = 0.2f;
            blocks[i].disturbedAction = ObserverDeactivateAction(5);
            blocks[i].dropHandler = CustomDrop(415, 1);
            i++;
            blocks[i] = new NasBlock(i, blocks[421]);
            blocks[i].existAction = WireBreakAction();
            blocks[i].disturbDelayMin = 0.2f;
            blocks[i].disturbDelayMax = 0.2f;
            blocks[i].disturbedAction = ObserverDeactivateAction(6);
            blocks[i].dropHandler = CustomDrop(415, 1);
            
            i = 427; //Sponge
            blocks[i] = new NasBlock(i, Material.Leaves);
            blocks[i].disturbedAction = SpongeAction();
            i++;
            blocks[i] = new NasBlock(i, Material.Leaves);
            
            i = 429; //deepslate
            blocks[i] = new NasBlock(i, Material.Stone, 24, 1);
            blocks[i].dropHandler = CustomDrop(430, 1);
            i++;
            blocks[i] = new NasBlock(i, Material.Stone, 24, 1);
            blocks[i].alternateID = 1;
            i = 431;
            blocks[i] = new NasBlock(i, Material.Stone, 24, 1);
            i++;
            blocks[i] = new NasBlock(i, blocks[431]);
            
            i = 433;
            blocks[i] = new NasBlock(i, Material.Stone, 24, 1);
            i++;
            blocks[i] = new NasBlock(i, Material.Stone, 24, 1);
            i++;
            blocks[i] = new NasBlock(i, Material.Stone, 24, 1);
            i++;
            blocks[i] = new NasBlock(i, Material.Stone, 24, 1);
            
            i = 437;
            blocks[i] = new NasBlock(i, Material.Stone, 24, 1);
            i++;
            blocks[i] = new NasBlock(i, blocks[437]);
            
            i = 439; //dispenser
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Stone], 1);
            blocks[i].existAction = ContainerExistAction();
            blocks[i].disturbedAction = DispenserAction(0, 0, 1);
            blocks[i].interaction = ContainerInteraction();
            blocks[i].container = new Container();
            blocks[i].container.type = Container.Type.Dispenser;
            i++;
            blocks[i] = new NasBlock(i, blocks[439]);
            blocks[i].disturbedAction = DispenserAction(-1, 0, 0);
            i++;
            blocks[i] = new NasBlock(i, blocks[439]);
            blocks[i].disturbedAction = DispenserAction(0, 0, -1);
            i++;
            blocks[i] = new NasBlock(i, blocks[439]);
            blocks[i].disturbedAction = DispenserAction(1, 0, 0);
            i++;
            blocks[i] = new NasBlock(i, blocks[439]);
            blocks[i].disturbedAction = DispenserAction(0, -1, 0);
            i++;
            blocks[i] = new NasBlock(i, blocks[439]);
            blocks[i].disturbedAction = DispenserAction(0, 1, 0);
            
        }
        static Func<NasPlayer, BlockID, Drop> CustomDrop(BlockID clientBlockID, int amount) {
            return (NasPlayer, dropID) => { return new Drop(clientBlockID, amount); };
        }
        static void DefinePiston(ushort startID, BlockID[] set, string axis, int direction, int parent, bool sticky = false) {
        	ushort i = startID;
            blocks[i] = new NasBlock(i, blocks[parent]);
            blocks[i].disturbedAction = SidewaysPistonAction("off", axis, direction, set, sticky);
            i++;
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbDelayMin = 0.125f;
            blocks[i].disturbDelayMax = 0.125f;
            blocks[i].disturbedAction = SidewaysPistonAction("body", axis, direction, set, sticky);
            blocks[i].dropHandler = CustomDrop((ushort)parent, 1);
            i++;
            blocks[i] = new NasBlock(i, Material.Stone, DefaultDurabilities[(int)Material.Metal], 1);
            blocks[i].disturbedAction = SidewaysPistonAction("head", axis, direction, set, sticky);
            blocks[i].collideAction = AirCollideAction();
			blocks[i].dropHandler = (NasPlayer, dropID) => {
                Drop pistonDrop = new Drop(1, 1);
                return (1 == 0) ? pistonDrop : null;};
        }
        
    } //class NasBlock

}
