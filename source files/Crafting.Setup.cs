using BlockID = System.UInt16;

namespace NotAwesomeSurvival {

    public partial class Crafting {

        public static void Setup() {
			
			//EXPLANATION FOR HOW TO READ RECIPES!
			
			//the first line of the recipe shows what will be produced. if it's an item it will give the 
			//specified item with the name given. If it's a blockID it will give the blockID specified.
			
			//The recipe.pattern is a 2d list that contains what blocks are needed. Remember that the number is
			//actually the blockID. You can do /hold (blockid) ingame to see what block it is.
			
			//the recipe.usesAlternateID specifies whether or not alternate versions of the block can be used. 
			//The alternateIDs of 5 (oak planks) are birch and spruce planks, e.t.c. For 36 (white wool) it's 
			//all the other colors of wool.
			
			//If recipe.stationType = Crafting.Station.Type.Furnace, then it means the recipe must be made in a
			//furnace and NOT a crafting table.
			
			//recipe.usesParentID specifies whether or not the Parent ID of a block can be used. ParentIDS usually
			//amount to rotated versions of a block. So if a slab is in a recipe that uses parentIDS, then the slab
			//can be in any direction.
			
			//Finally, recipe.shapeless determines whether or not you can put the blocks in any order.
			//If it's true, it means the blocks shown don't have to be in the same place relative to each other.
			
			//Recipes that don't fill up all 9 slots and are NOT shapeless just mean that smaller pattern can be
			//anywhere on the crafting table, just in the same spot relative to each other.
			
			
			
            Recipe woodPickaxe = new Recipe(new Item("Wood Pickaxe"));
            woodPickaxe.usesAlternateID = true;
            woodPickaxe.pattern = new BlockID[,] {
                {  5,  5, 5 },
                {  0, 78, 0 },
                {  0, 78, 0 }
            };

            // stone tools
            Recipe stonePickaxe = new Recipe(new Item("Stone Pickaxe"));
            stonePickaxe.usesAlternateID = true;
            stonePickaxe.pattern = new BlockID[,] {
                {  1,  1, 1 },
                {  0, 78, 0 },
                {  0, 78, 0 }
            };
            Recipe stoneShovel = new Recipe(new Item("Stone Shovel"));
            stoneShovel.usesAlternateID = true;
            stoneShovel.pattern = new BlockID[,] {
                {   1 },
                {  78 },
                {  78 }
            };
            Recipe stoneAxe = new Recipe(new Item("Stone Axe"));
            stoneAxe.usesAlternateID = true;
            stoneAxe.pattern = new BlockID[,] {
                {  1,  1 },
                {  1, 78 },
                {  0, 78 }
            };

            Recipe stoneSword = new Recipe(new Item("Stone Sword"));
            stoneSword.usesAlternateID = true;
            stoneSword.pattern = new BlockID[,] {
                {  1 },
                {  1 },
                { 78 }
            };

            //iron tools
            Recipe ironPickaxe = new Recipe(new Item("Iron Pickaxe"));
            ironPickaxe.usesAlternateID = true;
            ironPickaxe.pattern = new BlockID[,] {
                { 42, 42,42 },
                {  0, 78, 0 },
                {  0, 78, 0 }
            };
            Recipe ironShovel = new Recipe(new Item("Iron Shovel"));
            ironShovel.usesAlternateID = true;
            ironShovel.pattern = new BlockID[,] {
                {  42 },
                {  78 },
                {  78 }
            };
            Recipe ironAxe = new Recipe(new Item("Iron Axe"));
            ironAxe.usesAlternateID = true;
            ironAxe.pattern = new BlockID[,] {
                { 42, 42 },
                { 42, 78 },
                {  0, 78 }
            };

            Recipe ironSword = new Recipe(new Item("Iron Sword"));
            ironSword.usesAlternateID = true;
            ironSword.pattern = new BlockID[,] {
                { 42 },
                { 42 },
                { 78 }
            };

            Recipe ironhelm = new Recipe(new Item("Iron Helmet"));
            ironhelm.pattern = new BlockID[,] {
                { 42, 42, 42 },
                { 42, 0, 42 },
            };
            
            Recipe ironchest = new Recipe(new Item("Iron Chestplate"));
            ironchest.pattern = new BlockID[,] {
                { 42, 0, 42 },
                { 42, 42, 42 },
                { 42, 42, 42 },
            };
            
            Recipe ironlegs = new Recipe(new Item("Iron Leggings"));
            ironlegs.pattern = new BlockID[,] {
                { 42, 42, 42 },
                { 42, 0, 42 },
                { 42, 0, 42 },
            };
            
            Recipe ironboots = new Recipe(new Item("Iron Boots"));
            ironboots.pattern = new BlockID[,] {
                { 42, 0, 42 },
                { 42, 0, 42 },
            };
            
            //gold tools
            Recipe goldPickaxe = new Recipe(new Item("Gold Pickaxe"));
            goldPickaxe.usesAlternateID = true;
            goldPickaxe.pattern = new BlockID[,] {
                { 41, 41,41 },
                {  0, 78, 0 },
                {  0, 78, 0 }
            };
            Recipe goldShovel = new Recipe(new Item("Gold Shovel"));
            goldShovel.usesAlternateID = true;
            goldShovel.pattern = new BlockID[,] {
                {  41 },
                {  78 },
                {  78 }
            };
            Recipe goldAxe = new Recipe(new Item("Gold Axe"));
            goldAxe.usesAlternateID = true;
            goldAxe.pattern = new BlockID[,] {
                { 41, 41 },
                { 41, 78 },
                {  0, 78 }
            };

            Recipe goldSword = new Recipe(new Item("Gold Sword"));
            goldSword.usesAlternateID = true;
            goldSword.pattern = new BlockID[,] {
                { 41 },
                { 41 },
                { 78 }
            };

            Recipe goldhelm = new Recipe(new Item("Gold Helmet"));
            goldhelm.pattern = new BlockID[,] {
                { 41, 41, 41 },
                { 41, 0, 41 },
            };
            
            Recipe goldchest = new Recipe(new Item("Gold Chestplate"));
            goldchest.pattern = new BlockID[,] {
                { 41, 0, 41 },
                { 41, 41, 41 },
                { 41, 41, 41 },
            };
            
            Recipe goldlegs = new Recipe(new Item("Gold Leggings"));
            goldlegs.pattern = new BlockID[,] {
                { 41, 41, 41 },
                { 41, 0, 41 },
                { 41, 0, 41 },
            };
            
            Recipe goldboots = new Recipe(new Item("Gold Boots"));
            goldboots.pattern = new BlockID[,] {
                { 41, 0, 41 },
                { 41, 0, 41 },
            };
            
            //diamond tools
            Recipe diamondPickaxe = new Recipe(new Item("Diamond Pickaxe"));
            diamondPickaxe.usesAlternateID = true;
            diamondPickaxe.pattern = new BlockID[,] {
                { 631, 631,631 },
                {  0, 78, 0 },
                {  0, 78, 0 }
            };
            Recipe diamondShovel = new Recipe(new Item("Diamond Shovel"));
            diamondShovel.usesAlternateID = true;
            diamondShovel.pattern = new BlockID[,] {
                {  631 },
                {  78 },
                {  78 }
            };
            Recipe diamondAxe = new Recipe(new Item("Diamond Axe"));
            diamondAxe.usesAlternateID = true;
            diamondAxe.pattern = new BlockID[,] {
                { 631, 631 },
                { 631, 78 },
                {  0, 78 }
            };

            Recipe diamondSword = new Recipe(new Item("Diamond Sword"));
            diamondSword.usesAlternateID = true;
            diamondSword.pattern = new BlockID[,] {
                { 631 },
                { 631 },
                { 78 }
            };

            
            Recipe diamondhelm = new Recipe(new Item("Diamond Helmet"));
            diamondhelm.pattern = new BlockID[,] {
                { 631, 631, 631 },
                { 631, 0, 631 },
            };
            
            Recipe diamondchest = new Recipe(new Item("Diamond Chestplate"));
            diamondchest.pattern = new BlockID[,] {
                { 631, 0, 631 },
                { 631, 631, 631 },
                { 631, 631, 631 },
            };
            
            Recipe diamondlegs = new Recipe(new Item("Diamond Leggings"));
            diamondlegs.pattern = new BlockID[,] {
                { 631, 631, 631 },
                { 631, 0, 631 },
                { 631, 0, 631 },
            };
            
            Recipe diamondboots = new Recipe(new Item("Diamond Boots"));
            diamondboots.pattern = new BlockID[,] {
                { 631, 0, 631 },
                { 631, 0, 631 },
            };
 //emerald tools
            Recipe emeraldPickaxe = new Recipe(new Item("Emerald Pickaxe"));
            emeraldPickaxe.usesAlternateID = true;
            emeraldPickaxe.pattern = new BlockID[,] {
                { 650, 650,650 },
                {  0, 78, 0 },
                {  0, 78, 0 }
            };
            
            
            Recipe emeraldShovel = new Recipe(new Item("Emerald Shovel"));
            emeraldShovel.usesAlternateID = true;
            emeraldShovel.pattern = new BlockID[,] {
                {  650 },
                {  78 },
                {  78 }
            };
            Recipe emeraldAxe = new Recipe(new Item("Emerald Axe"));
            emeraldAxe.usesAlternateID = true;
            emeraldAxe.pattern = new BlockID[,] {
                { 650, 650 },
                { 650, 78 },
                {  0, 78 }
            };

            Recipe emeraldSword = new Recipe(new Item("Emerald Sword"));
            emeraldSword.usesAlternateID = true;
            emeraldSword.pattern = new BlockID[,] {
                { 650 },
                { 650 },
                { 78 }
            };

            Recipe emeraldhelm = new Recipe(new Item("Emerald Helmet"));
            emeraldhelm.pattern = new BlockID[,] {
                { 650, 650, 650 },
                { 650, 0, 650 },
            };
            
            Recipe emeraldchest = new Recipe(new Item("Emerald Chestplate"));
            emeraldchest.pattern = new BlockID[,] {
                { 650, 0, 650 },
                { 650, 650, 650 },
                { 650, 650, 650 },
            };
            
            Recipe emeraldlegs = new Recipe(new Item("Emerald Leggings"));
            emeraldlegs.pattern = new BlockID[,] {
                { 650, 650, 650 },
                { 650, 0, 650 },
                { 650, 0, 650 },
            };
            
            Recipe emeraldboots = new Recipe(new Item("Emerald Boots"));
            emeraldboots.pattern = new BlockID[,] {
                { 650, 0, 650 },
                { 650, 0, 650 },
            };
            //wood stuff ------------------------------------------------------
            Recipe wood = new Recipe(5, 4);
            wood.usesParentID = true;
            wood.pattern = new BlockID[,] {
                {  17 }
                
              
            };
            Recipe woodFall = new Recipe(657, 4);
            woodFall.usesParentID = true;
            woodFall.shapeless = true;
            woodFall.pattern = new BlockID[,] {
                {  17, 12, 12 },
                {  12, 12, 12 },
                {  12, 12, 12 }
                
              
            };
            
            Recipe fakeDirt = new Recipe(685, 4);
            fakeDirt.shapeless = true;
            fakeDirt.pattern = new BlockID[,] {
                { 3, 55, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 },
            };
            
            Recipe fakeDirt2 = new Recipe(685, 4);
            fakeDirt2.shapeless = true;
            fakeDirt2.pattern = new BlockID[,] {
                { 3, 470, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 },
            };
            
            
            Recipe woodFall2 = new Recipe(656, 2);
            woodFall2.usesParentID = true;
            woodFall2.shapeless = true;
            woodFall2.pattern = new BlockID[,] {
                {  242, 12, 12 },
                {  12, 12, 12 },
                {  12, 12, 12 }
                
              
            };
            
            Recipe glassFall = new Recipe(655, 4);
            glassFall.shapeless = true;
            glassFall.pattern = new BlockID[,] {
                {  20, 12, 12 },
                {  12, 12, 12 },
                {  12, 12, 12 }
                
              
            };
            
            Recipe trapdoor = new Recipe(659, 3);
            trapdoor.usesAlternateID = true;
            trapdoor.pattern = new BlockID[,] {
                {  5, 5, 5 },
                {  5, 5, 5 }
            };
            
            Recipe woodSlab = new Recipe(56, 6);
            woodSlab.pattern = new BlockID[,] {
                {  5, 5, 5 }
            };
            Recipe woodWall = new Recipe(182, 6);
            woodWall.pattern = new BlockID[,] {
                {  5 },
                {  5 },
                {  5 }
            };
            Recipe woodStair = new Recipe(66, 6);
            woodStair.pattern = new BlockID[,] {
                {  5, 0, 0 },
                {  5, 5, 0 },
                {  5, 5, 5 }
            };



            Recipe woodPole = new Recipe(78, 4);
            woodPole.pattern = new BlockID[,] {
                {  5 },
                {  5 }
            };

            Recipe fenceWE = new Recipe(94, 4);
            fenceWE.pattern = new BlockID[,] {
                {  78, 79, 78 },
                {  78, 79, 78 }
            };
            Recipe fenceNS = new Recipe(94, 4);
            fenceNS.pattern = new BlockID[,] {
                {  78, 80, 78 },
                {  78, 80, 78 }
            };

            Recipe darkDoor = new Recipe(55, 2);
            darkDoor.pattern = new BlockID[,] {
                { 5, 5 },
                { 5, 5 },
                { 5, 5 }
            };
            
            Recipe board = new Recipe(168, 6);
            board.usesParentID = true;
            board.pattern = new BlockID[,] {
                {  56, 56, 56 }
            };
            Recipe boardSideways = new Recipe(524, 6);
            boardSideways.usesParentID = true;
            boardSideways.pattern = new BlockID[,] {
                {  182 },
                {  182 },
                {  182 }
            };
            
            //spruce wood stuff ------------------------------------------------------
            Recipe sprucewood = new Recipe(97, 4);
            sprucewood.usesParentID = true;
            sprucewood.pattern = new BlockID[,] {
                {  250 }
                
              
            };
              
            
            Recipe sprucewoodSlab = new Recipe(99, 6);
            sprucewoodSlab.pattern = new BlockID[,] {
                {  97, 97, 97 }
            };
            
            Recipe sprucewoodWall = new Recipe(190, 6);
            sprucewoodWall.pattern = new BlockID[,] {
                {  97 },
                {  97 },
                {  97 }
            };
            Recipe sprucewoodStair = new Recipe(266, 6);
            sprucewoodStair.pattern = new BlockID[,] {
                {  97, 0, 0 },
                {  97, 97, 0 },
                {  97, 97, 97 }
            };



            Recipe sprucewoodPole = new Recipe(252, 4);
            sprucewoodPole.pattern = new BlockID[,] {
                {  97 },
                {  97 }
            };

            Recipe sprucefenceWE = new Recipe(258, 4);
            sprucefenceWE.pattern = new BlockID[,] {
                {  252, 253, 252 },
                {  252, 253, 252 }
            };
            Recipe sprucefenceNS = new Recipe(258, 4);
            sprucefenceNS.pattern = new BlockID[,] {
                {  252, 254, 252 },
                {  252, 254, 252 }
            };

            //Recipe thirdDoor = new Recipe(470, 2);
            //thirdDoor.pattern = new BlockID[,] {
            //    { 98, 98 },
            //    { 98, 98 },
            //    { 98, 98 }
            //};
            
            
            
            //birch wood stuff ------------------------------------------------------
            Recipe birchwood = new Recipe(98, 4);
            birchwood.usesParentID = true;
            birchwood.pattern = new BlockID[,] {
                {  242 }
                
              
            };
              
            
            Recipe birchwoodSlab = new Recipe(101, 6);
            birchwoodSlab.pattern = new BlockID[,] {
                {  98, 98, 98 }
            };
            
            Recipe birchwoodWall = new Recipe(186, 6);
            birchwoodWall.pattern = new BlockID[,] {
                {  98 },
                {  98 },
                {  98 }
            };
            Recipe birchwoodStair = new Recipe(262, 6);
            birchwoodStair.pattern = new BlockID[,] {
                {  98, 0, 0 },
                {  98, 98, 0 },
                {  98, 98, 98 }
            };



            Recipe birchwoodPole = new Recipe(255, 4);
            birchwoodPole.pattern = new BlockID[,] {
                {  98 },
                {  98 }
            };

            Recipe birchfenceWE = new Recipe(260, 4);
            birchfenceWE.pattern = new BlockID[,] {
                {  255, 256, 255 },
                {  255, 256, 255 }
            };
            Recipe birchfenceNS = new Recipe(260, 4);
            birchfenceNS.pattern = new BlockID[,] {
                {  255, 257, 255 },
                {  255, 257, 255 }
            };

            Recipe lightDoor = new Recipe(470, 2);
            lightDoor.pattern = new BlockID[,] {
                { 98, 98 },
                { 98, 98 },
                { 98, 98 }
            };
            
            
            
            //chest
            Recipe chest = new Recipe(216, 1);
            chest.usesAlternateID = true;
            chest.pattern = new BlockID[,] {
                {  5,  5,  5 },
                {  5, 0, 5 },
                {  5,  5,  5 }
            };
            
            Recipe barrel = new Recipe(143, 1);
            barrel.usesAlternateID = true;
            barrel.pattern = new BlockID[,] {
                { 5, 56, 5 },
                { 5, 148, 5 },
                { 5, 57, 5 },
            };
            
            
            Recipe barrel2 = new Recipe(143, 1);
            barrel.usesAlternateID = true;
            barrel2.pattern = new BlockID[,] {
                { 150 },
                {  17 },
                { 149 }
            };
            
            Recipe auto = new Recipe(413, 1);
            auto.shapeless = true;
            auto.usesAlternateID = true;
            auto.usesParentID = true;
            auto.pattern = new BlockID[,] {
                { 672, 1, 672 },
                { 1, 17, 1 },
                { 672, 1, 672 }
            };
            
            Recipe bedbeacon = new Recipe(612, 1);
            bedbeacon.shapeless = true;
            bedbeacon.pattern = new BlockID[,] {
                { 20, 23, 20 },
                { 23, 42, 23 },
                { 20, 23, 20 }
            };
            
            Recipe smith = new Recipe(676, 1);
            smith.usesAlternateID = true;
            smith.pattern = new BlockID[,] {
                { 42, 42 },
                { 5, 5 },
                { 5, 5 }
            };
            
            Recipe tank = new Recipe(697, 1);
            tank.pattern = new BlockID[,] {
                { 690, 149, 690 },
                { 690, 0, 690 },
                { 690, 690, 690 },
            };
            
            Recipe crate = new Recipe(142, 1);
            crate.usesAlternateID = true;
            crate.pattern = new BlockID[,] {
                { 5, 5 },
                { 5, 5 }
            };
            
            Recipe cryingObs = new Recipe(457,1);
            cryingObs.shapeless = true;
            cryingObs.pattern = new BlockID[,] {
            	{ 690, 690, 690 },
                { 690, 239, 690 },
                { 690, 690, 690 },
            };

            //stone stuff ------------------------------------------------------

            Recipe stoneSlab = new Recipe(596, 6);
            stoneSlab.pattern = new BlockID[,] {
                {  1, 1, 1 }
            };
            Recipe stoneWall = new Recipe(598, 6);
            stoneWall.pattern = new BlockID[,] {
                {  1 },
                {  1 },
                {  1 }
            };
            Recipe stoneStair = new Recipe(70, 6);
            stoneStair.pattern = new BlockID[,] {
                {  1, 0, 0 },
                {  1, 1, 0 },
                {  1, 1, 1 }
            };

            
            //stonebrick
            Recipe marker = new Recipe(64, 1);
            marker.pattern = new BlockID[,] {
                {  65, 65, 65 },
                {  65,  0, 65 },
                {  65, 65, 65 }
            };
            Recipe stoneBrick = new Recipe(65, 6);
            stoneBrick.pattern = new BlockID[,] {
                {  1, 1, 0 },
                {  0, 1, 1 },
                {  1, 1, 0 }
            };
            Recipe stoneBrickSlab = new Recipe(86, 6);
            stoneBrickSlab.pattern = new BlockID[,] {
                {  65, 65, 65 }
            };
            Recipe stoneBrickWall = new Recipe(278, 6);
            stoneBrickWall.pattern = new BlockID[,] {
                {  65 },
                {  65 },
                {  65 }
            };
            Recipe stonePole = new Recipe(75, 4);
            stonePole.pattern = new BlockID[,] {
                {  65 },
                {  65 }
            };
            Recipe thinPole = new Recipe(211, 4);
            thinPole.pattern = new BlockID[,] {
                {  75 },
                {  75 }
            };
            Recipe linedStone = new Recipe(477, 4);
            linedStone.pattern = new BlockID[,] {
                {  65, 65 },
                {  65, 65 }
            };
            
            Recipe mossyCobble = new Recipe(181, 4);
            mossyCobble.pattern = new BlockID[,] {
                {  18, 162 },
                {  162, 18 }
            };
            
             Recipe mossyBricks = new Recipe(180, 4);
            mossyBricks.pattern = new BlockID[,] {
                {  181, 181 },
                {  181, 181 }
            };

            Recipe boulder = new Recipe(214, 4);
            boulder.usesAlternateID = true;
            boulder.pattern = new BlockID[,] {
                {  1 }
            };
            Recipe nub = new Recipe(194, 4);
            nub.pattern = new BlockID[,] {
                {  214 }
            };
            
            
            
            
            Recipe cobbleBrick = new Recipe(4, 4);
            cobbleBrick.pattern = new BlockID[,] {
                {  162, 162 },
                {  162, 162 }
            };
            Recipe cobbleBrickSlab = new Recipe(50, 6);
            cobbleBrickSlab.pattern = new BlockID[,] {
                {  4, 4, 4 }
            };
            Recipe cobbleBrickWall = new Recipe(133, 6);
            cobbleBrickWall.pattern = new BlockID[,] {
                {  4, 4, 4 },
                {  4, 4, 4 }
            };



            Recipe cobblestone = new Recipe(162, 4);
            cobblestone.pattern = new BlockID[,] {
                {  1, 1 },
                {  1, 1 }
            };
            Recipe cobblestoneSlab = new Recipe(163, 6);
            cobblestoneSlab.pattern = new BlockID[,] {
                {  162, 162, 162 }
            };

            Recipe sandstoneSlab = new Recipe(299, 6);
            sandstoneSlab.pattern = new BlockID[,] {
                {  52, 52, 52 }
            };
            
            Recipe furnace = new Recipe(625, 1);
            furnace.usesAlternateID = true;
            furnace.usesParentID = true;
            furnace.pattern = new BlockID[,] {
                {  1,  1, 1 },
                {  1,  0, 1 },
                {  1,  1, 1 }
            };


            Recipe concreteBlock = new Recipe(45, 4);
            concreteBlock.pattern = new BlockID[,] {
                {  4, 4 },
                {  4, 4 }
            };
            Recipe concreteSlab = new Recipe(44, 6);
            concreteSlab.pattern = new BlockID[,] {
                {  45, 45, 45 }
            };
            Recipe concreteWall = new Recipe(282, 6);
            concreteWall.pattern = new BlockID[,] {
                { 45 },
                { 45 },
                { 45 }
            };
            Recipe concreteBrick = new Recipe(549, 4);
            concreteBrick.pattern = new BlockID[,] {
                {  45, 45 },
                {  45, 45 }
            };
             Recipe sandstone = new Recipe(52, 1);
            sandstone.pattern = new BlockID[,] {
                {  12, 12 },
                {  12, 12 }
            };
            Recipe stonePlate = new Recipe(135, 6);
            stonePlate.pattern = new BlockID[,] {
                {  44, 44, 44 }
            };
            
             Recipe quartzPillar = new Recipe(63, 2);
             quartzPillar.pattern = new BlockID[,] {
                {  61 },
                {  61 }
            };
            
            
             Recipe quartzWall = new Recipe(286, 6);
             quartzWall.pattern = new BlockID[,] {
                {  61 },
                {  61 },
                {  61 }
            };
             
             Recipe quartzSlab = new Recipe(84, 6);
             quartzSlab.pattern = new BlockID[,] {
                {  61, 61, 61 },
            };
             
             Recipe quartzChis = new Recipe(235, 1);
             quartzChis.usesParentID = true;
             quartzChis.pattern = new BlockID[,] {
                 {  84 },
                 {  84 }
            };
            
            
            Recipe quartzStair = new Recipe(274, 6);
            quartzStair.pattern = new BlockID[,] {
                {  61,  0,  0 },
                {  61, 61,  0 },
                {  61, 61, 61 }
            };
             
            //upside down slab recipe
            Recipe stonePlate2 = new Recipe(135, 6);
            stonePlate2.pattern = new BlockID[,] {
                {  58, 58, 58 }
            };
            Recipe concreteStair = new Recipe(270, 6);
            concreteStair.pattern = new BlockID[,] {
                {  45,  0,  0 },
                {  45, 45,  0 },
                {  45, 45, 45 }
            };
            Recipe concreteCorner = new Recipe(480, 4);
            concreteCorner.pattern = new BlockID[,] {
                { 45 },
                { 45 }
            };



            //ore stuff
            Recipe charcoal = new Recipe(49, 2);
            charcoal.stationType = Crafting.Station.Type.Furnace;
            charcoal.shapeless = true;
            charcoal.usesParentID = true;
            charcoal.usesAlternateID = true;
            charcoal.pattern = new BlockID[,] {
                {  17, 17, 17 },
                {  17, 197, 17 },
                {  17, 17, 17 }
            };
            
            
            Recipe coalBlock = new Recipe(49, 1);
            coalBlock.shapeless = true;
            coalBlock.pattern = new BlockID[,] {
                {  197, 197, 0 },
                {  0, 0, 0 },
                {  0, 0, 0 },
            };
            Recipe hotCoals = new Recipe(239, 1);
            hotCoals.shapeless = true;
            hotCoals.pattern = new BlockID[,] {
                {  49, 49, 0 },
                {  0, 0, 0 },
                {  0, 0, 0 },
            };

            
            Recipe iron = new Recipe(42, 1);
            iron.stationType = Crafting.Station.Type.Furnace;
            iron.expGiven = 4;
            iron.shapeless = true;
            iron.pattern = new BlockID[,] {
                {  628, 197, 197 },
                {  197, 197, 197 },
                {  197, 197, 197 },
            };
            
            Recipe nugIron = new Recipe(42, 1);
            nugIron.shapeless = true;
            nugIron.pattern = new BlockID[,] {
                {  624, 624, 624 },
                {  624, 624, 624 },
                {  624, 624, 624 },
            };
            
            Recipe ironNug = new Recipe(624, 9);
            ironNug.pattern = new BlockID[,] {
            	{  42 },
            };
            
			Recipe ironRefine = new Recipe(42, 1);
            ironRefine.stationType = Crafting.Station.Type.Furnace;
            ironRefine.expGiven = 2;
            ironRefine.shapeless = true;
            ironRefine.pattern = new BlockID[,] {
                {  148, 148, 197 },
                {  148, 197, 197 },
                {  197, 197, 197 },
            };
            
            Recipe goldRefine = new Recipe(41, 1);
            goldRefine.stationType = Crafting.Station.Type.Furnace;
            goldRefine.expGiven = 3;
            goldRefine.shapeless = true;
            goldRefine.pattern = new BlockID[,] {
                {  672, 672, 49 },
                {  672, 49, 49 },
                {  49, 49, 49 },
            };
            
            //old iron
            Recipe oldIron = new Recipe(148, 3);
            oldIron.stationType = Crafting.Station.Type.Furnace;
            oldIron.expGiven = 2;
            oldIron.shapeless = true;
            oldIron.pattern = new BlockID[,] {
                {  628, 197, 0 },
                {  0, 0, 0 },
                {  0, 0, 0 },
            };
            
            Recipe oldgold = new Recipe(672, 3);
            oldgold.stationType = Crafting.Station.Type.Furnace;
            oldgold.expGiven = 3;
            oldgold.shapeless = true;
            oldgold.pattern = new BlockID[,] {
                {  629, 49, 0 },
                {  0, 0, 0 },
                {  0, 0, 0 },
            };
            
            Recipe goldWire = new Recipe(550, 32);
            goldWire.pattern = new BlockID[,] {
                { 672, 672, 672 }
            };
            
            Recipe oldIronSlab = new Recipe(149, 6);
            oldIronSlab.pattern = new BlockID[,] {
                {  148, 148, 148 }
            };
            
            Recipe oldIronWall = new Recipe(294, 6);
            oldIronWall.pattern = new BlockID[,] {
                {  148 },
                {  148 },
                {  148 }
            };
            
            Recipe tile = new Recipe(208, 4);
            tile.pattern = new BlockID[,] {
                { 21, 148 },
                { 148, 21 }
            };
            
            Recipe key = new Recipe(new Item("Key"));
            key.usesParentID = true;
            key.pattern = new BlockID[,] {
                {  294, 149, 294 },
                {  149, 148,  0  },
                {  149, 148,  0  }
            };
            
            //i = 159; //Iron fence-WE
            Recipe ironFence = new Recipe(159, 12);
            ironFence.pattern = new BlockID[,] {
                {  148, 148, 148 },
                {  148, 148, 148 }
            };
            //i = 161; //Iron cage
            Recipe ironCage = new Recipe(161, 4);
            ironCage.usesParentID = true;
            ironCage.pattern = new BlockID[,] {
                {    0, 159,   0 },
                {  159,   0, 159 },
                {    0, 159,   0 }
            };
            
            
            Recipe gold = new Recipe(41, 1);
            gold.stationType = Crafting.Station.Type.Furnace;
            gold.expGiven = 6;
            gold.shapeless = true;
            gold.pattern = new BlockID[,] {
                {  629, 49, 49 },
                {   49, 49, 49 },
                {   49, 49, 49 },
            };
            Recipe diamond = new Recipe(631, 1);
            diamond.stationType = Crafting.Station.Type.Furnace;
            diamond.expGiven = 10;
            diamond.shapeless = true;
            diamond.pattern = new BlockID[,] {
                {  630, 49, 49 },
                {  49, 49, 49 },
                {  49, 49, 49 },
            };
			Recipe emerald = new Recipe(650, 1);
            emerald.stationType = Crafting.Station.Type.Furnace;
            emerald.expGiven = 15;
            emerald.shapeless = true;
            emerald.pattern = new BlockID[,] {
                {  649, 49, 49 },
                {  49, 49, 49 },
                {  49, 49, 49 },
            };
            //glass
            Recipe glass = new Recipe(20, 1);
            glass.stationType = Crafting.Station.Type.Furnace;
            glass.pattern = new BlockID[,] {
            	{ 12 }
            };
            Recipe glassPane = new Recipe(136, 6);
            glassPane.pattern = new BlockID[,] {
                {  20, 20, 20 },
                {  20, 20, 20 }
            };
            
            Recipe oldGlass = new Recipe(203, 1);
            oldGlass.pattern = new BlockID[,] {
                { 57 },
                { 20 }
            };
            Recipe oldGlassPane = new Recipe(209, 6);
            oldGlassPane.pattern = new BlockID[,] {
                {  203, 203, 203 },
                {  203, 203, 203 }
            };
            
            Recipe newGlass = new Recipe(471, 1);
            newGlass.pattern = new BlockID[,] {
                { 150 },
                {  20 }
            };
            Recipe newGlassPane = new Recipe(472, 6);
            newGlassPane.pattern = new BlockID[,] {
                {  471, 471, 471 },
                {  471, 471, 471 }
            };
            
            Recipe rope = new Recipe(51, 3);
            rope.usesAlternateID = true;
            rope.pattern = new BlockID[,] {
                { 78 },
            	{ 78 },
                { 78 }
            };
            
            //bread
            Recipe bread = new Recipe(640, 1);
            bread.stationType = Crafting.Station.Type.Furnace;
            bread.expGiven = 4;
            bread.shapeless = true;
            bread.usesParentID = true;
            bread.pattern = new BlockID[,] {
                { 145, 145, 145 },
                {  0, 0, 0 },
                {  0, 0, 0 },
            };
            
            Recipe waffle = new Recipe(542, 1);
            waffle.stationType = Crafting.Station.Type.Furnace;
            waffle.expGiven = 4;
            waffle.shapeless = true;
            waffle.usesParentID = true;
            waffle.pattern = new BlockID[,] {
            	{ 0, 0, 0 },
            	{ 667, 0, 0 },
            	{ 145, 145, 145 },
                
            };
            
            Recipe leavesDense = new Recipe(666, 2);
            leavesDense.pattern = new BlockID[,] {
                {  18, 18 },
                {  18, 18 },
            
            };
            
            Recipe pinkLeavesDense = new Recipe(686, 2);
            pinkLeavesDense.pattern = new BlockID[,] {
            	{  103, 103 },
            	{  103,	103 }
            
            };
            
            Recipe leavesDry = new Recipe(104, 12);
            leavesDry.pattern = new BlockID[,] {
                {  18, 18, 18 },
                {  18, 25, 18 },
                {  18, 18, 18 }
            };
            
            Recipe leavesSlab = new Recipe(105, 6);
            leavesSlab.pattern = new BlockID[,] {
                {  18, 18, 18 }
            
            };
            
            Recipe pinkLeavesSlab = new Recipe(246, 6);
            pinkLeavesSlab.pattern = new BlockID[,] {
                {  103, 103, 103 }
            
            };
            Recipe orange = new Recipe(30, 2);
            orange.shapeless = true;
            orange.pattern = new BlockID[,] {
                { 27, 35, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 },
            
            };
            Recipe pink = new Recipe(138, 2);
            pink.shapeless = true;
            pink.pattern = new BlockID[,] {
                { 27, 36, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 },
            
            };
             Recipe green = new Recipe(26, 2);
            green.shapeless = true;
            green.pattern = new BlockID[,] {
                { 23, 35, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 },
            
            };
            Recipe lime = new Recipe(32, 2);
            lime.shapeless = true;
            lime.pattern = new BlockID[,] {
                { 26, 36, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 },
            
            };
            Recipe cyan = new Recipe(29, 2);
            cyan.shapeless = true;
            cyan.pattern = new BlockID[,] {
                { 23, 26, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 },
            
            };
            Recipe lightblue = new Recipe(34, 2);
            lightblue.shapeless = true;
            lightblue.pattern = new BlockID[,] {
                { 23, 36, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 },
            
            };
            Recipe purple = new Recipe(22, 2);
            purple.shapeless = true;
            purple.pattern = new BlockID[,] {
                { 23, 27, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 },
            
            };
            Recipe brown = new Recipe(25, 2);
            brown.shapeless = true;
            brown.pattern = new BlockID[,] {
                { 3, 36, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 },
            
            };
            Recipe black = new Recipe(21, 2);
            black.shapeless = true;
            black.pattern = new BlockID[,] {
                { 36, 197, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 },
            
            };
            Recipe magenta = new Recipe(200, 2);
            magenta.shapeless = true;
            magenta.pattern = new BlockID[,] {
                { 138, 22, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 },
            
            };
            Recipe gray = new Recipe(28, 2);
            gray.shapeless = true;
            gray.pattern = new BlockID[,] {
                { 21, 36, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 },
            
            };
            Recipe lightgray = new Recipe(31, 2);
            lightgray.shapeless = true;
            lightgray.pattern = new BlockID[,] {
                { 28, 36, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 },
            
            };
            
            Recipe poisonBread = new Recipe(652, 1);
            poisonBread.shapeless = true;
            poisonBread.pattern = new BlockID[,] {
                { 131, 640, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 },
            
            };
            
            
            
            Recipe poisonMushroom = new Recipe(653, 1);
            poisonMushroom.shapeless = true;
            poisonMushroom.pattern = new BlockID[,] {
                { 131, 604, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 },
            
            };
            
            Recipe poisonPie = new Recipe(654, 1);
            poisonPie.shapeless = true;
            poisonPie.pattern = new BlockID[,] {
                { 131, 668, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 },
            
            };
            
            Recipe fire = new Recipe(54, 32);
            fire.shapeless = true;
            fire.pattern = new BlockID[,] {
                { 42, 13, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 },
            
            };
            
            Recipe shears = new Recipe(new Item("Shears"));
            shears.pattern = new BlockID[,] {
                {  0, 42 },
                {  42, 0 }
            };
            
            Recipe fishing = new Recipe(new Item("Fishing Rod"));
            fishing.usesParentID = true;
            fishing.usesAlternateID = true;
            fishing.pattern = new BlockID[,] {
                {  0, 0, 78 },
                {  0, 78, 36 },
                {  78, 0, 36 }
            };
            
            Recipe die = new Recipe(236, 4);
            die.shapeless = true;
            die.pattern = new BlockID[,] {
            	{ 0, 0, 0 },
            	{ 485, 486, 487 },
            	{ 488, 489, 490 }
            };

            Recipe zero = new Recipe(484, 8);
            zero.pattern = new BlockID[,] {
            	{ 45, 27, 45 },
                { 27, 45, 27 },
                { 45, 27, 45 }
            	
            };
            
            Recipe one = new Recipe(485, 8);
            one.pattern = new BlockID[,] {
            	{ 27, 27, 45 },
                { 45, 27, 45 },
                { 27, 27, 27 }
            	
            };
            
            Recipe two = new Recipe(486, 8);
            two.pattern = new BlockID[,] {
            	{ 27, 27, 45 },
                { 45, 27, 27 },
                { 27, 27, 45 }
            	
            };
            
            Recipe three = new Recipe(487, 8);
            three.pattern = new BlockID[,] {
            	{ 27, 27, 27 },
                { 45, 27, 27 },
                { 27, 27, 27 }
            	
            };
            
            Recipe four = new Recipe(488, 8);
            four.pattern = new BlockID[,] {
            	{ 27, 45, 27 },
                { 27, 27, 27 },
                { 45, 45, 27 }
            	
            };
            
            Recipe five = new Recipe(489, 8);
            five.pattern = new BlockID[,] {
            	{ 45, 27, 27 },
                { 45, 27, 45 },
                { 27, 27, 45 }
            	
            };
            
            Recipe six = new Recipe(490, 8);
            six.pattern = new BlockID[,] {
            	{ 27, 45, 45 },
                { 27, 27, 27 },
                { 27, 27, 27 }
            	
            };
            
            Recipe seven = new Recipe(491, 8);
            seven.pattern = new BlockID[,] {
            	{ 27, 27, 27 },
                { 45, 45, 27 },
                { 45, 45, 27 }
            	
            };
            
            Recipe eight = new Recipe(492, 8);
            eight.pattern = new BlockID[,] {
            	{ 45, 27, 27 },
                { 27, 27, 27 },
                { 27, 27, 45 }
            	
            };
            
            Recipe nine = new Recipe(493, 8);
            nine.pattern = new BlockID[,] {
            	{ 27, 27, 27 },
                { 27, 27, 27 },
                { 45, 45, 27 }
            	
            };
            
            Recipe a = new Recipe(494, 8);
            a.pattern = new BlockID[,] {
            	{ 21, 21, 21 },
                { 21, 21, 21 },
                { 21, 45, 21 }
            	
            };
            
            Recipe b = new Recipe(495, 8);
            b.pattern = new BlockID[,] {
            	{ 21, 21, 45 },
                { 21, 21, 21 },
                { 21, 21, 45 }
            	
            };
            
            Recipe c = new Recipe(496, 8);
            c.pattern = new BlockID[,] {
            	{ 21, 21, 21 },
                { 21, 45, 45 },
                { 21, 21, 21 }
            	
            };
            
            Recipe d = new Recipe(497, 8);
            d.pattern = new BlockID[,] {
            	{ 21, 21, 45 },
                { 21, 45, 21 },
                { 21, 21, 45 }
            	
            };
            
            Recipe e = new Recipe(498, 8);
            e.pattern = new BlockID[,] {
            	{ 21, 21, 21 },
                { 21, 21, 45 },
                { 21, 21, 21 }
            	
            };
            
            Recipe f = new Recipe(499, 8);
            f.pattern = new BlockID[,] {
            	{ 21, 21, 21 },
                { 21, 21, 45 },
                { 21, 45, 45 }
            	
            };
            
            Recipe g = new Recipe(500, 8);
            g.pattern = new BlockID[,] {
            	{ 21, 21, 21 },
                { 21, 21, 45 },
                { 21, 21, 45 }
            	
            };
            
            Recipe h = new Recipe(501, 8);
            h.pattern = new BlockID[,] {
            	{ 21, 45, 21 },
                { 21, 21, 21 },
                { 21, 45, 21 }
            	
            };
            
            Recipe i = new Recipe(502, 8);
            i.pattern = new BlockID[,] {
            	{ 21, 21, 21 },
                { 45, 21, 45 },
                { 21, 21, 21 }
            	
            };
            
            Recipe j = new Recipe(503, 8);
            j.pattern = new BlockID[,] {
            	{ 45, 45, 21 },
                { 21, 45, 21 },
                { 21, 21, 21 }
            	
            };
            
            Recipe k = new Recipe(504, 8);
            k.pattern = new BlockID[,] {
            	{ 21, 45, 21 },
                { 21, 21, 45 },
                { 21, 45, 21 }
            	
            };
            
            Recipe l = new Recipe(505, 8);
            l.pattern = new BlockID[,] {
            	{ 21, 45, 45 },
                { 21, 45, 45 },
                { 21, 21, 21 }
            	
            };
            
            Recipe m = new Recipe(506, 8);
            m.pattern = new BlockID[,] {
            	{ 21, 45, 21 },
                { 21, 21, 21 },
                { 21, 21, 21 }
            	
            };
            
            Recipe n = new Recipe(507, 8);
            n.pattern = new BlockID[,] {
            	{ 21, 21, 21 },
                { 21, 45, 21 },
                { 21, 45, 21 }
            	
            };
            
            Recipe o = new Recipe(508, 8);
            o.pattern = new BlockID[,] {
            	{ 21, 21, 21 },
                { 21, 45, 21 },
                { 21, 21, 21 }
            	
            };
            
            Recipe p = new Recipe(509, 8);
            p.pattern = new BlockID[,] {
            	{ 21, 21, 21 },
                { 21, 21, 21 },
                { 21, 45, 45 }
            	
            };
            
            Recipe q = new Recipe(510, 8);
            q.pattern = new BlockID[,] {
            	{ 21, 21, 21 },
                { 21, 21, 21 },
                { 21, 21, 45 }
            	
            };
            
            Recipe r = new Recipe(511, 8);
            r.pattern = new BlockID[,] {
            	{ 21, 21, 21 },
                { 21, 21, 45 },
                { 21, 45, 21 }
            	
            };
            
            Recipe s = new Recipe(512, 8);
            s.pattern = new BlockID[,] {
            	{ 45, 21, 21 },
                { 21, 21, 45 },
                { 45, 21, 21 }
            	
            };
            
            Recipe t = new Recipe(513, 8);
            t.pattern = new BlockID[,] {
            	{ 21, 21, 21 },
                { 45, 21, 45 },
                { 45, 21, 45 }
            	
            };
            
            Recipe u = new Recipe(514, 8);
            u.pattern = new BlockID[,] {
            	{ 21, 45, 21 },
                { 21, 45, 21 },
                { 21, 21, 21 }
            	
            };
            
            Recipe v = new Recipe(515, 8);
            v.pattern = new BlockID[,] {
            	{ 21, 45, 21 },
                { 21, 45, 21 },
                { 45, 21, 45 }
            	
            };
            
            Recipe w = new Recipe(516, 8);
            w.pattern = new BlockID[,] {
            	{ 21, 45, 21 },
                { 21, 21, 21 },
                { 45, 21, 45 }
            	
            };
            
            Recipe x = new Recipe(517, 8);
            x.pattern = new BlockID[,] {
            	{ 21, 45, 21 },
                { 45, 21, 45 },
                { 21, 45, 21 }
            	
            };
            
            Recipe y = new Recipe(518, 8);
            y.pattern = new BlockID[,] {
            	{ 21, 45, 21 },
                { 45, 21, 45 },
                { 45, 21, 45 }
            	
            };
            
            Recipe z = new Recipe(519, 8);
            z.pattern = new BlockID[,] {
            	{ 21, 21, 45 },
                { 45, 21, 45 },
                { 45, 21, 21 }
            	
            };
            
            Recipe period = new Recipe(520, 8);
            period.pattern = new BlockID[,] {
            	{ 45, 45, 45 },
                { 45, 21, 45 },
                { 45, 45, 45 }
            	
            };
            
            Recipe exc = new Recipe(521, 8);
            exc.pattern = new BlockID[,] {
            	{ 45, 21, 45 },
                { 45, 45, 45 },
                { 45, 21, 45 }
            	
            };
            
            Recipe slash = new Recipe(522, 8);
            slash.pattern = new BlockID[,] {
            	{ 45, 45, 21 },
                { 45, 21, 45 },
                { 21, 45, 45 }
            	
            };
            
            Recipe que = new Recipe(523, 8);
            que.pattern = new BlockID[,] {
            	{ 45, 21, 21 },
                { 45, 45, 21 },
                { 45, 21, 45 }
            	
            };
            
            Recipe sign = new Recipe(171, 3);
            sign.usesAlternateID = true;
            sign.pattern = new BlockID[,] {
                {  5, 5, 5 },
                {  5, 5, 5 },
                {  0, 78, 0 }
            };
            
            Recipe bookshelf = new Recipe(132, 3);
            bookshelf.usesAlternateID = true;
            bookshelf.pattern = new BlockID[,] {
                {  5, 5, 5 },
                {  667, 667, 667 },
                {  5, 5, 5 }
            };
            
            Recipe bed = new Recipe(703, 1);
            bed.usesAlternateID = true;
            bed.pattern = new BlockID[,] {
                {  36, 36, 36 },
                {  5, 5, 5 }
            };
            
            
            Recipe pie = new Recipe(668, 1);
            pie.shapeless = true;
            pie.expGiven = 8;
            pie.usesParentID = true;
            pie.stationType = Crafting.Station.Type.Furnace;
            pie.pattern = new BlockID[,] {
                {  648, 648, 0 },
                {  667, 667, 0 },
                {  145, 145, 0 }
            };
            
            Recipe peachPie = new Recipe(698, 1);
            peachPie.shapeless = true;
            peachPie.expGiven = 8;
            peachPie.usesParentID = true;
            peachPie.stationType = Crafting.Station.Type.Furnace;
            peachPie.pattern = new BlockID[,] {
                {  702, 702, 0 },
                {  667, 667, 0 },
                {  145, 145, 0 }
            };
            
            Recipe powerSource = new Recipe(74, 1);
            powerSource.usesAlternateID = true;
            powerSource.pattern = new BlockID[,] {
                {  1, 1, 1 },
                {  1, 672, 1 },
            	{  1, 1, 1 },
            };
            
            Recipe lever = new Recipe(674, 1);
            lever.usesAlternateID = true;
            lever.pattern = new BlockID[,] {
                {  1, 1, 1 },
                {  1, 74, 1 },
            	{  1, 1, 1 },
            };
            
            Recipe pressure = new Recipe(610, 1);
            pressure.usesAlternateID = true;
            pressure.pattern = new BlockID[,] {
            	{  1, 1 }
            };
            
            Recipe spikes = new Recipe(178, 1);
            spikes.usesAlternateID = true;
            spikes.pattern = new BlockID[,] {
                {  0, 1, 0 },
            	{  1, 1, 1 },
            };
            
            Recipe obSpikes = new Recipe(476, 1);
            obSpikes.shapeless = true;
            obSpikes.pattern = new BlockID[,] {
                { 690, 178 },
            };
            
            Recipe lamp = new Recipe(687, 4);
            lamp.usesParentID = true;
            lamp.pattern = new BlockID[,] {
                {  20, 20, 20 },
                {  20, 550, 20 },
            	{  20, 20, 20 },
            };
            
            Recipe button = new Recipe(195, 1);
            button.usesParentID = true;
            button.usesAlternateID = true;
            button.pattern = new BlockID[,] {
                { 550, 550, 550 },
                { 550, 1, 550 },
            	{ 550, 550, 550 },
            };
            
            Recipe piston = new Recipe(704, 1);
            piston.usesParentID = true;
            piston.usesAlternateID = true;
            piston.pattern = new BlockID[,] {
                {  5, 5, 5 },
                {  1, 42, 1 },
            	{  1, 550, 1 },
            };
            
            Recipe dispenser = new Recipe(439, 1);
            dispenser.usesParentID = true;
            dispenser.usesAlternateID = true;
            dispenser.pattern = new BlockID[,] {
                {  1, 1, 1 },
                {  1, 0, 1 },
            	{  1, 550, 1 },
            };
            
            Recipe observer = new Recipe(415, 1);
            observer.usesParentID = true;
            observer.usesAlternateID = true;
            observer.pattern = new BlockID[,] {
            	{  1, 1, 1 },
                {  550, 550, 61 },
            	{  1, 1, 1 },
            	
            };
            Recipe repeater = new Recipe(172, 1);
            repeater.usesParentID = true;
            repeater.usesAlternateID = true;
            repeater.pattern = new BlockID[,] {
                {  1, 1, 1 },
                {  550, 550, 550 },
            	{  1, 1, 1 },
            };
            
            Recipe strongWire = new Recipe(732, 5);
            strongWire.usesParentID = true;
            strongWire.usesAlternateID = true;
            strongWire.shapeless = true;
            strongWire.pattern = new BlockID[,] {
                {  1, 550, 1 },
                {  550, 550, 550 },
                {  1, 550, 1 },
            };
            
            Recipe stickyPiston = new Recipe(678, 1);
            stickyPiston.pattern = new BlockID[,] {
                {  677 },
            	{  704 },
            };
           
           Recipe sticky = new Recipe(677, 1);
           sticky.usesAlternateID = true;
           sticky.pattern = new BlockID[,] {
            	{  6 },
            }; 
           
            
            Recipe packed = new Recipe(681, 1);
            packed.pattern = new BlockID[,] {
            	{  60, 60 },
            	{  60, 60 }
            }; 
            
            Recipe redCarpet = new Recipe(713, 3);
            redCarpet.pattern = new BlockID[,] {
            	{  27, 27 }
            };
            
            Recipe orangeCarpet = new Recipe(714, 3);
            orangeCarpet.pattern = new BlockID[,] {
            	{  30, 30 }
            };
            
            Recipe yellowCarpet = new Recipe(715, 3);
            yellowCarpet.pattern = new BlockID[,] {
            	{  35, 35 }
            };
            
            Recipe limeCarpet = new Recipe(716, 3);
            limeCarpet.pattern = new BlockID[,] {
            	{  32, 32 }
            };
            
            Recipe greenCarpet = new Recipe(717, 3);
            greenCarpet.pattern = new BlockID[,] {
            	{  26, 26 }
            };
            
            Recipe lightblueCarpet = new Recipe(718, 3);
            lightblueCarpet.pattern = new BlockID[,] {
            	{  34, 34 }
            };
            
            Recipe cyanCarpet = new Recipe(719, 3);
            cyanCarpet.pattern = new BlockID[,] {
            	{  29, 29 }
            };
            
            Recipe blueCarpet = new Recipe(720, 3);
            blueCarpet.pattern = new BlockID[,] {
            	{  23, 23 }
            };
            
            Recipe magentaCarpet = new Recipe(721, 3);
            magentaCarpet.pattern = new BlockID[,] {
            	{  200, 200 }
            };
            
            Recipe pinkCarpet = new Recipe(722, 3);
            pinkCarpet.pattern = new BlockID[,] {
            	{  138, 138 }
            };
            
            Recipe blackCarpet = new Recipe(723, 3);
            blackCarpet.pattern = new BlockID[,] {
            	{  21, 21 }
            };
            
            Recipe purpleCarpet = new Recipe(724, 3);
            purpleCarpet.pattern = new BlockID[,] {
            	{  22, 22 }
            };
            
            Recipe grayCarpet = new Recipe(725, 3);
            grayCarpet.pattern = new BlockID[,] {
            	{  28, 28 }
            };
            
            Recipe lightgrayCarpet = new Recipe(726, 3);
            lightgrayCarpet.pattern = new BlockID[,] {
            	{  31, 31 }
            };
            
            Recipe whiteCarpet = new Recipe(727, 3);
            whiteCarpet.pattern = new BlockID[,] {
            	{  36, 36 }
            };
            
            Recipe brownCarpet = new Recipe(728, 3);
            brownCarpet.pattern = new BlockID[,] {
            	{  25, 25 }
            };
            
            Recipe snowBlock = new Recipe(140, 1);
            snowBlock.pattern = new BlockID[,] {
            	{  53, 53 }
            };
            
            Recipe sponge = new Recipe(427, 3);
            sponge.usesAlternateID = true;
            sponge.pattern = new BlockID[,] {
            	{  36, 36, 36 },
            	{  36, 36, 36 },
            	{  36, 36, 36 },
            };
            
            Recipe drysponge = new Recipe(427, 1);
            drysponge.stationType = Station.Type.Furnace;
            drysponge.shapeless = true;
            drysponge.pattern = new BlockID[,] {
            	{  428, 49 }
            };
            
            Recipe cobbledDeep = new Recipe(429, 1);
            cobbledDeep.stationType = Station.Type.Furnace;
            cobbledDeep.pattern = new BlockID[,] {
            	{  430 }
            };
            
            Recipe polishedDeep = new Recipe(433, 4);
            polishedDeep.pattern = new BlockID[,] {
            	{  430, 430 },
            	{  430, 430 }
            };
            
            Recipe bricksDeep = new Recipe(436, 4);
            bricksDeep.pattern = new BlockID[,] {
            	{  433, 433 },
            	{  433, 433 }
            };
            
            Recipe tilesDeep = new Recipe(435, 4);
            tilesDeep.pattern = new BlockID[,] {
            	{  436, 436 },
            	{  436, 436 }
            };
            
            Recipe slabCobbleDeep = new Recipe(431, 6);
            slabCobbleDeep.pattern = new BlockID[,] {
            	{  430, 430, 430 },
            };
            
            Recipe slabBrickDeep = new Recipe(437, 6);
            slabBrickDeep.pattern = new BlockID[,] {
            	{  436, 436, 436 },
            };
            
            Recipe chiseledDeep = new Recipe(434, 1);
            chiseledDeep.shapeless = true;
            chiseledDeep.usesParentID = true;
            chiseledDeep.pattern = new BlockID[,] {
            	{  431, 431 },
            };
            
            Recipe bricksNether = new Recipe(155, 4);
            bricksNether.pattern = new BlockID[,] {
            	{  48, 48 },
            	{  48, 48 },
            };
            
            Recipe bricksNetherSlab = new Recipe(157, 6);
            bricksNetherSlab.pattern = new BlockID[,] {
            	{  155, 155, 155 },
            };
            
            Recipe polishedBlack = new Recipe(458, 4);
            polishedBlack.pattern = new BlockID[,] {
            	{  452, 452 },
            	{  452, 452 },
            };
            
            Recipe polishedBlackSlab = new Recipe(460, 6);
            polishedBlackSlab.pattern = new BlockID[,] {
            	{  458, 458, 458 },
            };
            
            Recipe brickBlack = new Recipe(466, 4);
            brickBlack.pattern = new BlockID[,] {
            	{  458, 458 },
            	{  458, 458 },
            };
            
            Recipe brickBlackSlab = new Recipe(468, 6);
            brickBlackSlab.pattern = new BlockID[,] {
            	{  466, 466, 466 },
            };
            
            Recipe gilded = new Recipe(469, 8);
            gilded.shapeless = true;
            gilded.pattern = new BlockID[,] {
            	{  452, 452, 452 },
            	{  452, 452, 452 },
            	{  452, 452, 672 },
            };
            
            Recipe crackedBlack = new Recipe(474, 1);
            crackedBlack.stationType = Station.Type.Furnace;
            crackedBlack.pattern = new BlockID[,] {
            	{ 466 },
            };
            
            Recipe chiseledBlack = new Recipe(475, 1);
            chiseledBlack.shapeless = true;
            chiseledBlack.usesParentID = true;
            chiseledBlack.pattern = new BlockID[,] {
            	{  460, 460 },
            };
            
            Recipe barrier = new Recipe(767, 9);
            barrier.pattern = new BlockID[,] {
            	{  7, 7, 7 },
            	{  7, 650, 7 },
            	{  7, 7, 7 },
            };
        }

    }

}
