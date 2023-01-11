using System;
using System.Collections.Generic;

namespace NotAwesomeSurvival {

    public partial class ItemProp {
        public static void Setup() {
			
			Dictionary<string,bool> nothing = new Dictionary<string,bool>();
			
			Dictionary<string,bool> toolEnchants = new Dictionary<string,bool>(){
				{"Efficiency",true},
				{"Fortune",true},
				{"Mending",true},
				{"Silk Touch",true},
				{"Unbreaking",true},
			};
			
			Dictionary<string,bool> swordEnchants = new Dictionary<string,bool>(){
				{"Knockback",true},
				{"Mending",true},
				{"Sharpness",true},
				{"Unbreaking",true},
			};
			
			Dictionary<string,bool> bootEnchants = new Dictionary<string,bool>(){
				{"Feather Falling",true},
				{"Mending",true},
				{"Protection",true},
				{"Thorns",true},
				{"Unbreaking",true},
			};
			
			Dictionary<string,bool> helmetEnchants = new Dictionary<string,bool>(){
				{"Aqua Affinity",true},
				{"Mending",true},
				{"Protection",true},
				{"Respiration",true},
				{"Thorns",true},
				{"Unbreaking",true},
			};
			
			Dictionary<string,bool> armorEnchants = new Dictionary<string,bool>(){
				{"Mending",true},
				{"Protection",true},
				{"Thorns",true},
				{"Unbreaking",true},
			};
			

			
            ItemProp fist = new ItemProp("Fist|f|¬", nothing, NasBlock.Material.None, 0, 0);
            fist.baseHP = Int32.MaxValue;
            Item.Fist = new Item("Fist");
            
            ItemProp key = new ItemProp("Key|f|σ", nothing, NasBlock.Material.None, 0, 0);
            key.baseHP = Int32.MaxValue;
            
            ItemProp fishing = new ItemProp("Fishing Rod|f|δ", nothing, NasBlock.Material.None, 0, 0);
			fishing.baseHP = baseHPconst * 7;
            fishing.damage = 0f;
			fishing.knockback = -1f;
			
            ItemProp shears = new ItemProp("Shears|f|µ", nothing, NasBlock.Material.Organic, 0.75f, 1);
			shears.baseHP = baseHPconst * 8;
            ItemProp woodPick = new ItemProp("Wood Pickaxe|s|ß", toolEnchants, NasBlock.Material.Stone, 0.0f, 1);
            woodPick.baseHP = 12;

            ItemProp stonePick = new ItemProp("Stone Pickaxe|7|ß", toolEnchants, NasBlock.Material.Stone, 0.75f, 1);
            ItemProp stoneShovel = new ItemProp("Stone Shovel|7|Γ", toolEnchants, NasBlock.Material.Earth, 0.50f, 1);
            ItemProp stoneAxe = new ItemProp("Stone Axe|7|π", toolEnchants, NasBlock.Material.Wood, 0.60f, 1);
            stoneAxe.materialsEffectiveAgainst.Add(NasBlock.Material.Leaves);
            ItemProp stoneSword = new ItemProp("Stone Sword|7|α", swordEnchants, NasBlock.Material.Leaves, 0.50f, 1);
            stoneSword.damage = 2.5f;
			stoneSword.recharge = 750;
            const int ironBaseHP = baseHPconst * 8;
            ItemProp ironPick = new ItemProp("Iron Pickaxe|f|ß", toolEnchants, NasBlock.Material.Stone, 0.85f, 3);
            ItemProp ironShovel = new ItemProp("Iron Shovel|f|Γ", toolEnchants, NasBlock.Material.Earth, 0.60f, 3);
            ItemProp ironAxe = new ItemProp("Iron Axe|f|π", toolEnchants, NasBlock.Material.Wood, 0.75f, 3);
            ironAxe.materialsEffectiveAgainst.Add(NasBlock.Material.Leaves);
            ItemProp ironSword = new ItemProp("Iron Sword|f|α",swordEnchants, NasBlock.Material.Leaves, 0.75f, 3);
            ironSword.damage = 3f;
            ironSword.recharge = 750;
            ironSword.knockback = 1.25f;
            ironPick.baseHP = ironBaseHP;
            ironShovel.baseHP = ironBaseHP;
            ironAxe.baseHP = ironBaseHP;
            ironSword.baseHP = ironBaseHP;

            const int goldBaseHP = baseHPconst * 64;
            ItemProp goldPick = new ItemProp("Gold Pickaxe|6|ß", toolEnchants, NasBlock.Material.Stone, 0.90f, 3);
            ItemProp goldShovel = new ItemProp("Gold Shovel|6|Γ", toolEnchants, NasBlock.Material.Earth, 0.85f, 3);
            ItemProp goldAxe = new ItemProp("Gold Axe|6|π", toolEnchants, NasBlock.Material.Wood, 0.90f, 3);
            goldAxe.materialsEffectiveAgainst.Add(NasBlock.Material.Leaves);
            ItemProp goldSword = new ItemProp("Gold Sword|6|α", swordEnchants, NasBlock.Material.Leaves, 0.85f, 3);
            goldSword.damage = 3f;
            goldSword.recharge = 750;
            goldSword.knockback = 1.25f;
            goldPick.baseHP = goldBaseHP;
            goldShovel.baseHP = goldBaseHP;
            goldAxe.baseHP = goldBaseHP;
            goldSword.baseHP = goldBaseHP;

            const int diamondBaseHP = baseHPconst * 128;
            ItemProp diamondPick = new ItemProp("Diamond Pickaxe|b|ß", toolEnchants, NasBlock.Material.Stone, 0.95f, 4);
            ItemProp diamondShovel = new ItemProp("Diamond Shovel|b|Γ", toolEnchants, NasBlock.Material.Earth, 1f, 4);
            ItemProp diamondAxe = new ItemProp("Diamond Axe|b|π", toolEnchants, NasBlock.Material.Wood, 0.95f, 4);
            diamondAxe.materialsEffectiveAgainst.Add(NasBlock.Material.Leaves);
            ItemProp diamondSword = new ItemProp("Diamond Sword|b|α", swordEnchants, NasBlock.Material.Leaves, 1f, 4);
            diamondSword.damage = 3.5f;
            diamondSword.recharge = 750;
            diamondSword.knockback = 1.25f;
            diamondPick.baseHP = diamondBaseHP;
            diamondShovel.baseHP = diamondBaseHP;
            diamondAxe.baseHP = diamondBaseHP;
            diamondSword.baseHP = diamondBaseHP;

			const int emeraldBaseHP = baseHPconst * 192;
            ItemProp emeraldPick = new ItemProp("Emerald Pickaxe|2|ß", toolEnchants, NasBlock.Material.Stone, 0.975f, 5);
            emeraldPick.allowedEnchants["Efficiency"] = true;
            ItemProp emeraldShovel = new ItemProp("Emerald Shovel|2|Γ", toolEnchants, NasBlock.Material.Earth, 1f, 5);
            ItemProp emeraldAxe = new ItemProp("Emerald Axe|2|π", toolEnchants, NasBlock.Material.Wood, 0.975f, 5);
            emeraldAxe.materialsEffectiveAgainst.Add(NasBlock.Material.Leaves);
            ItemProp emeraldSword = new ItemProp("Emerald Sword|2|α", swordEnchants, NasBlock.Material.Leaves, 1f, 5);
            emeraldSword.damage = 4f;
            emeraldSword.recharge = 750;
            emeraldSword.knockback = 1.25f;
            emeraldPick.baseHP = emeraldBaseHP;
            emeraldShovel.baseHP = emeraldBaseHP;
            emeraldAxe.baseHP = emeraldBaseHP;
            emeraldSword.baseHP = emeraldBaseHP;
            
            ItemProp bedrockPick = new ItemProp("Bedrock Pickaxe|m|╟", nothing, NasBlock.Material.None, -1f, 5);
            bedrockPick.baseHP = Int32.MaxValue;
            
            ItemProp etheriumPick = new ItemProp("Etherium Pickaxe|h|ß", toolEnchants, NasBlock.Material.Stone, 1f, 6);
            etheriumPick.materialsEffectiveAgainst.Add(NasBlock.Material.Leaves);
            etheriumPick.materialsEffectiveAgainst.Add(NasBlock.Material.Glass);
            etheriumPick.materialsEffectiveAgainst.Add(NasBlock.Material.Wood);
            etheriumPick.baseHP = Int32.MaxValue;
            
            ItemProp bedrockSword = new ItemProp("Bedrock Sword|0|α", swordEnchants, NasBlock.Material.Leaves, 1f, 6);
            bedrockSword.damage = 50f;
            bedrockSword.knockback = 2f;
            bedrockSword.recharge = 3000;
            bedrockSword.baseHP = Int32.MaxValue;
            
            ItemProp ironHelmet = new ItemProp("Iron Helmet|f|τ", helmetEnchants, NasBlock.Material.None, 0, 0);
            ItemProp ironChest = new ItemProp("Iron Chestplate|f|Φ",armorEnchants, NasBlock.Material.None, 0, 0);
            ItemProp ironLegs = new ItemProp("Iron Leggings|f|Θ",armorEnchants, NasBlock.Material.None, 0, 0);
            ItemProp ironBoots = new ItemProp("Iron Boots|f|Ω", bootEnchants, NasBlock.Material.None, 0, 0);
            ironHelmet.baseHP = ironBaseHP/2f;
            ironChest.baseHP = ironBaseHP/2f;
            ironLegs.baseHP = ironBaseHP/2f;
            ironBoots.baseHP = ironBaseHP/2f;
            ironHelmet.armor = 2f;
            ironChest.armor = 5f;
            ironLegs.armor = 4f;
            ironBoots.armor = 1f;
            
            ItemProp goldHelmet = new ItemProp("Gold Helmet|6|τ", helmetEnchants, NasBlock.Material.None, 0, 0);
            ItemProp goldChest = new ItemProp("Gold Chestplate|6|Φ",armorEnchants, NasBlock.Material.None, 0, 0);
            ItemProp goldLegs = new ItemProp("Gold Leggings|6|Θ",armorEnchants, NasBlock.Material.None, 0, 0);
            ItemProp goldBoots = new ItemProp("Gold Boots|6|Ω", bootEnchants, NasBlock.Material.None, 0, 0);
            goldHelmet.baseHP = goldBaseHP/2f;
            goldChest.baseHP = goldBaseHP/2f;
            goldLegs.baseHP = goldBaseHP/2f;
            goldBoots.baseHP = goldBaseHP/2f;
            goldHelmet.armor = 2f;
            goldChest.armor = 6f;
            goldLegs.armor = 5f;
            goldBoots.armor = 2f;
            
            
            ItemProp diamondHelmet = new ItemProp("Diamond Helmet|b|τ", helmetEnchants, NasBlock.Material.None, 0, 0);
            ItemProp diamondChest = new ItemProp("Diamond Chestplate|b|Φ",armorEnchants, NasBlock.Material.None, 0, 0);
            ItemProp diamondLegs = new ItemProp("Diamond Leggings|b|Θ",armorEnchants, NasBlock.Material.None, 0, 0);
            ItemProp diamondBoots = new ItemProp("Diamond Boots|b|Ω", bootEnchants, NasBlock.Material.None, 0, 0);
            diamondHelmet.baseHP = diamondBaseHP/2f;
            diamondChest.baseHP = diamondBaseHP/2f;
            diamondLegs.baseHP = diamondBaseHP/2f;
            diamondBoots.baseHP = diamondBaseHP/2f;
            diamondHelmet.armor = 3f;
            diamondChest.armor = 7f;
            diamondLegs.armor = 5f;
            diamondBoots.armor = 3f;
            
            ItemProp emeraldHelmet = new ItemProp("Emerald Helmet|2|τ", helmetEnchants, NasBlock.Material.None, 0, 0);
            ItemProp emeraldChest = new ItemProp("Emerald Chestplate|2|Φ",armorEnchants, NasBlock.Material.None, 0, 0);
            ItemProp emeraldLegs = new ItemProp("Emerald Leggings|2|Θ",armorEnchants, NasBlock.Material.None, 0, 0);
            ItemProp emeraldBoots = new ItemProp("Emerald Boots|2|Ω", bootEnchants, NasBlock.Material.None, 0, 0);
            emeraldHelmet.baseHP = emeraldBaseHP/2f;
            emeraldChest.baseHP = emeraldBaseHP/2f;
            emeraldLegs.baseHP = emeraldBaseHP/2f;
            emeraldBoots.baseHP = emeraldBaseHP/2f;
            emeraldHelmet.armor = 3f;
            emeraldChest.armor = 8f;
            emeraldLegs.armor = 6f;
            emeraldBoots.armor = 3f;
        }
    }

}
