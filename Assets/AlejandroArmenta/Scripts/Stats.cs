
using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class item_stats
{
    public int BarCount;
    public float[] Ratios = new float[4];
};

public enum custom_texture_type
{
    CustomTexture_Consumible,
    CustomTexture_Head, 
    CustomTexture_Torso,
    CustomTexture_Feet,
};

public class custom_scroll
{
    public custom_texture_type type;
    public GameObject ScrollPref;
    public Scrollbar scrollBar;

    public GameObject Content;
    public uint ItemCount;
    //public uint [] itemIndices_;
};

public class custom_user_info
{
   public uint user_index;
   public uint [] AddonList;
   public uint [] ConsumibleList;
};

public class custom_item_user_info
{
    uint ItemCount;
    uint CustomState;
}

public class custom_state
{
    public GameObject MainMenu;
    public GameObject MainMenuContent;
    public Scrollbar MainMenuScrollbar;

    public int CurrentScrollIndex;
    //public Scrollbar[] ScrollBars = new Scrollbar[Stats.MAX_NUM_SCROLLS];

    public uint StatCount;
    public item_stats[] Stats_ = new item_stats[4096];

    public bool IntemsInitialized;
    public custom_assets[] UserItems;
    public uint ScrollCount;
    public custom_scroll[] Scrolls = new custom_scroll[Stats.MAX_NUM_SCROLLS];

    //TODO(Alex): Expand this functionality
    public uint CurrentPlayerIndex;
    public bool AddingPlayerItems;
    public uint PlayerItemCount;
#if false
    public uint UserCount;
    public custom_user_info [] UserInfos = new custom_user_info[4096];
#endif
}

public enum custom_asset_type
{
    CustomAssetType_Null,
    CustomAssetType_Model,
    CustomAssetType_Texture,
};

public class custom_texture
{
    public custom_texture_type type;
    public Sprite Data;
	public uint StatIndex;
	//public item_stats Stats_;
};

public class custom_assets
{
    public custom_asset_type type;
    public custom_texture Texture;
    public GameObject Model;
};

//NOTE(Alex): You can use this enum to select item_specific naming
public enum sprite_item_type
{
    SpriteAddon_Customizable = 'C',
    SpriteAddon_Head = 'H',
    SpriteAddon_Torso = 'T',
    SpriteAddon_Feet= 'F', 
};

public enum sprite_item_name
{
    SpriteItem_Morfina,
    SpriteItem_Espinaca,
    SpriteItem_Pesas,
    SpriteItem_Vodka,
    SpriteItem_Pata,
    SpriteItem_Treboles,
    SpriteItem_OxidoNitrico,
    SpriteItem_Cafe,
    SpriteItem_Tennis,
    SpriteItem_VitaminaC,
    SpriteItem_Caldo,
    SpriteItem_Aceite,
    SpriteItem_Ritalin,
    SpriteItem_Te,
    SpriteItem_Porro,

    SpriteItem_CustomizableCount,
    
    SpriteItem_CascoAgua,
    SpriteItem_Cuernos,
    SpriteItem_Sportacus,
    SpriteItem_CascoMaya,
    SpriteItem_Bigote,
    SpriteItem_Picos,
    SpriteItem_Poncho,
    SpriteItem_Sombrero,
    SpriteItem_Fez,
    SpriteItem_CollarMaya,
    SpriteItem_Dross,
    SpriteItem_Hawaii,
    SpriteItem_Cocos,
    
    SpriteItem_Count,
    SpriteItem_AddOnCount = SpriteItem_Count - SpriteItem_CustomizableCount,
};

//NOTE(Alex): Stats ---------------------------------------------------------
public class Stats : MonoBehaviour
{
    public static uint INVALID_PLAYER = 0xffffffff;
    public static uint MAX_NUM_SCROLLS = 128;
    public DisplayControl DC;
    public GameObject ImagePref;
    public GameObject ScrollPref;
    public RectTransform[] Rects;

    public custom_state CState;

#if false
    public void AddButtonsToViewPort(ref item_list List)
    {
        DC.Assert(List.ItemCount <= List.Items.Length);
        for (int ItemIndex = 0;
            ItemIndex < List.ItemCount;
            ++ItemIndex)
        {
            List.Items[ItemIndex] = new item_stats();
            item_stats Bar = List.Items[ItemIndex];
            Bar.BarCount = 4;
            for (int BarIndex = 0;
                BarIndex < Bar.BarCount;
                ++BarIndex)
            {
                Bar.BarRatios[BarIndex] = UnityEngine.Random.Range(0, 200.0f);
            }

            Image ItemImg = Instantiate(pref);
            InputTest Test = (InputTest)ItemImg.GetComponent("InputTest");// as InputTest;
            Test.ItemIndex = ItemIndex;

            ItemImg.transform.SetParent(GameObject.Find("Content").transform, false);
        }
    }
#endif
#if false
    public void InitList(item_list List)
    {
        DC.Assert(List.ItemCount <= List.Items.Length);
        for (int ItemIndex = 0;
            ItemIndex < List.ItemCount;
            ++ItemIndex)
        {
            //NOTE(Alex): This could add the item stats 
                        
        }
    }
#endif


    public void DrawItemStats(int ItemIndex)
    {
        item_stats Item = CState.Stats_[ItemIndex]; 
        for (int BarIndex = 0;
                BarIndex < Item.BarCount;
                ++BarIndex)
        {
            float Bar = Item.Ratios[BarIndex];
            Rects[BarIndex].sizeDelta = new Vector2(Bar, Rects[BarIndex].sizeDelta.y);
        }
    }

    private void 
    AddScrollOfType(ref custom_state CState, custom_texture_type type)
    {
        //DC.Assert(CState.ScrollCount <= Stats.MAX_NUM_SCROLLS);
        CState.Scrolls[(uint)type] = new custom_scroll();
        custom_scroll Temp = CState.Scrolls[(uint)type];
        Temp.type = type;
        Temp.ScrollPref = Instantiate(ScrollPref);
        if(Temp.ScrollPref)
        {
            Scrollbar[] Bars = Temp.ScrollPref.transform.GetComponentsInChildren<Scrollbar>();
            Temp.scrollBar = Bars[0];
            Temp.ScrollPref.transform.SetParent(CState.MainMenuContent.transform, false);
        }

        CState.ScrollCount++;
    }

    //TODO(Alex): Add Scroll Bar Selector
    //public void Scroll(custom_scroll_bar Scroll_bar, int Sign)

    public int Scroll(ref Scrollbar scrollbar, uint ItemCount, int Sign)
    {
        int Result = 0;
        float DispScrollRatio = 0;
        if (CState.ScrollCount > 1)
        {
            DispScrollRatio = 1.0f / (float)(ItemCount - 1);
        }

        //TODO(Alex): Change Sign so it its not passed as parameter
        if ((Sign == 1) || (Sign == -1))
        {
            scrollbar.value += Sign * DispScrollRatio;
            Result = Sign;
        }
        else
        {
            //DC.Assert(Convert.ToBoolean("Sign Has to be a Normalized signed integer"));
        }

        return Result;
    }

    public void SwitchRows(ref custom_state CState, int Sign)
    {
        custom_scroll CustomScroll = CState.Scrolls[CState.CurrentScrollIndex];
        if(CustomScroll.scrollBar)
        {
            Scroll(ref CustomScroll.scrollBar, CustomScroll.ItemCount, Sign);
        }
    }


    public void SwitchColumns(ref custom_state CState, int Sign)
    {
        int Value = Scroll(ref CState.MainMenuScrollbar, CState.ScrollCount, Sign);
        int Temp = CState.CurrentScrollIndex + Value;
        CState.CurrentScrollIndex = (Temp < 0) ? CState.CurrentScrollIndex : (Temp >= CState.ScrollCount) ? CState.CurrentScrollIndex : Temp;
    }

    //TODO(Alex): Does  SceneManager.LoadScene calls the start call?
#if (false)
    public void
        OnSceneChanged()
    {

    }
#endif

    //TODO(Alex):A way to get suffixes and prefixes
    public bool StringHasPrefix(string String, string Prefix)
    {
        bool Result = String.StartsWith(Prefix);
        return Result;
    }

    public bool StringHasSuffix(string String, string Suffix)
    {
        bool Result = String.EndsWith(Suffix); 
        return Result;
    }
    public List<custom_assets> 
        FindAllAssetsForUser(custom_state CState, uint PlayerIndex)
    {
        //NOTE(Alex): This counts for the models and the sprites
        /*I guess it will probably be better to have an array and thats it!*/
        List<custom_assets> Result = new List<custom_assets>();
        custom_user_info User = new custom_user_info();

        //NOTE(Alex): We will get the assets that belongs the user
        //Texture[] textures  = Resources.FindObjectsOfTypeAll<Texture>();
        Sprite[] textures = Resources.LoadAll<Sprite>("CustomData/CustomItemsAssets");
        GameObject[] Models = Resources.LoadAll<GameObject>("CustomData/CustomItemsAssets");

        string PlayerCountS = "P" + PlayerIndex + "_Count";
        uint PCount = (uint)PlayerPrefs.GetInt(PlayerCountS);
        
        for (uint Index = 0;
            Index < PCount;
            ++Index)
        {
            string Key = "P" + PlayerIndex + "_" + Index.ToString();
            string AssetName = PlayerPrefs.GetString(Key);
            //NOTE(Alex): "Sprite_0_C"
            if (StringHasPrefix(AssetName, "Sprite"))
            {
                bool AssetFound = false;
                for (uint TextureIndex = 0;
                    TextureIndex < textures.Length;
                    ++TextureIndex)
                {
                    Sprite SourceTexture = textures[TextureIndex];
                    if (AssetName.Contains(SourceTexture.name))
                    {
                        AssetFound = true;
                        custom_assets Asset = new custom_assets();
                        Asset.Texture = new custom_texture();
                        Asset.Texture.Data = SourceTexture;
						Asset.type = custom_asset_type.CustomAssetType_Texture;
						
						//NOTE(Alex): Init Stats HERE!
						AddStatToList(CState, ref Asset.Texture);
                        
                        if(StringHasSuffix(AssetName, "C"))
                        {
                            Asset.Texture.type = custom_texture_type.CustomTexture_Consumible;
                        }
                        else if (StringHasSuffix(AssetName, "F"))
                        {
                            Asset.Texture.type = custom_texture_type.CustomTexture_Feet;
                        }
                        else if (StringHasSuffix(AssetName, "T"))
                        {
                            Asset.Texture.type = custom_texture_type.CustomTexture_Torso;
                        }
                        else if (StringHasSuffix(AssetName, "H"))
                        {
                            Asset.Texture.type = custom_texture_type.CustomTexture_Head;
                        }

                        Result.Add(Asset);
                    }
                    if (AssetFound)
                    {
                        break;
                    }
                    else
                    {
                        //TODO(Alex): Look for Assets similar to this to expand or expose default asset 
                    }
                }
            }
            else if (StringHasPrefix(AssetName, "Model"))
            {
                //TODO(Alex): Models
#if false
                bool AssetFound = false;
                for (uint TextureIndex = 0;
                    TextureIndex < textures.Length;
                    ++TextureIndex)
                {
                    if (textures[TextureIndex].name == AssetName)
                    {
                        AssetFound = true;

                    }
                }

                if (!AssetFound)
                {
                    //TODO(Alex): Look for Assets similar to this to expand or expose default asset 
                    
                }
#endif
            }
            else
            {
                //NOTE(Alex): Invalid custom_user_asset
            }
        }

        return Result;
    }

    Transform 
    FindTransformInArray(Transform [] Array, string name)
    {
        Transform Result = null;

        for (uint Index = 0;
            Index < Array.Length;
            ++Index)
        {
            Transform Test = Array[Index];
            if(Test.name == name)
            {
                Result = Test;
            }
        }

        return Result;
    }

    public bool
    CustomAddUserItemsToScrolls(custom_state CState, custom_assets [] UserItems)
    {
        bool Result = false;
        for (uint Index = 0;
            Index < UserItems.Length;
            ++Index)
        {
            custom_assets Asset = UserItems[Index];
            if(Asset.type == custom_asset_type.CustomAssetType_Texture)
            {
                custom_scroll Scroll = CState.Scrolls[(uint)Asset.Texture.type];
                Transform [] Transforms = Scroll.ScrollPref.GetComponentsInChildren<Transform>();
                Transform Content = FindTransformInArray(Transforms, "Content");
                if (Content)
                {
                    GameObject Temp = Instantiate(ImagePref);
                    Image Img = Temp.GetComponent<Image>();
                    RectTransform ImgTransform = Temp.GetComponent<RectTransform>();
                    Img.sprite = Asset.Texture.Data;
                    ImgTransform.sizeDelta = new Vector2(400.0f, 400.0f);

                    //InputTest TestInput = (InputTest)this.GetComponent("InputTest");// CState.MainMenu.GetComponent("InputTest");// as InputTest
                    //TODO(Alex): Do we want to keep this here?
                    InputTest TestInput = (InputTest)Temp.GetComponent("InputTest");
                    TestInput.ItemIndex = (int)Asset.Texture.StatIndex;
                    Temp.transform.SetParent(Content, false);

                    Scroll.ItemCount++;
                    Result = true;
                }
            }
        }

        return Result;
    }
	
#if true
	//NOTE(Alex): Item Stat Initialization
    void 
	AddStatToList(custom_state CState, ref custom_texture Txt)
	{
		//DC.Assert(CState.StatCount <= CState.Stats_.Length);
		
		Txt.StatIndex = CState.StatCount++;
		item_stats Stat = CState.Stats_[Txt.StatIndex] = new item_stats();
		Stat.BarCount = 4;
        for (int BarIndex = 0;
            BarIndex < Stat.BarCount;
            ++BarIndex)
        {
            Stat.Ratios[BarIndex] = UnityEngine.Random.Range(0, 200.0f);
        }
	}
#endif
#if true
    void Update()
    {
        //NOTE(Alex): This is ridicoulous!
        if (!CState.IntemsInitialized)
        {
            if (CState.MainMenuScrollbar.value != 0)
            {
                CState.MainMenuScrollbar.value = 0;
                CState.IntemsInitialized = true;
            }
        }
        		
    }
#endif

    void BEGIN_PLAYER_ITEMS(custom_state CState, uint PlayerIndex)
    {
        CState.CurrentPlayerIndex = PlayerIndex;
        CState.AddingPlayerItems = true;
    }
    void ADD_PLAYER_ITEMS(custom_state CState, sprite_item_name ItemName, sprite_item_type ItemType)
    {
        if(CState.AddingPlayerItems)
        {
            uint ItemNameV = (uint)ItemName;
            char ItemTypeV = (char)ItemType;
            PlayerPrefs.SetString("P" + CState.CurrentPlayerIndex.ToString() + "_" + (CState.PlayerItemCount++).ToString(), "Sprite_" + ItemNameV.ToString() + "_" + ItemTypeV.ToString());
        }
    }

    void END_PLAYER_ITEMS(custom_state CState)
    {
        PlayerPrefs.SetInt("P"+ CState.CurrentPlayerIndex.ToString() +"_Count", (int)CState.PlayerItemCount);
        CState.AddingPlayerItems = false;
        CState.PlayerItemCount = 0;
        CState.CurrentPlayerIndex = INVALID_PLAYER;

        PlayerPrefs.Save();
    }


    void Start()
    {
        CState = new custom_state();
        CState.MainMenu = GameObject.Find("MainMenu");

        /*
         We have a list of items and a list of assets, we probably want to leave, 
         */

        if(CState.MainMenu)
        {
            CState.MainMenuScrollbar = GetComponentInChildren<Scrollbar>();
            
            //NOTE(Alex): These means that the MaiMenuPref always has a MainMeuContent inside
            CState.MainMenuContent = GameObject.Find("MainMenuContent");
            if(CState.MainMenuContent)
            {
                //TODO(Alex): You are here
                //TODO(Alex): Change this so they are not loaded dinamycally, since a reference to objects 
                //are needed to exist so that we can add attach them dinamically
                AddScrollOfType(ref CState, custom_texture_type.CustomTexture_Consumible);
                AddScrollOfType(ref CState, custom_texture_type.CustomTexture_Head);
                AddScrollOfType(ref CState, custom_texture_type.CustomTexture_Torso);
                AddScrollOfType(ref CState, custom_texture_type.CustomTexture_Feet);
            }


            //InitItemList(CState, 5);

            //TODO(Alex): We will probably need to have a virtual allocator so that we 
            //could push assets in and out from memory
            //NOTE(Alex): We only load bitmaps and models

            //NOTE(Alex): Add Items in this way!
            uint PlayerIndex = 0;
            BEGIN_PLAYER_ITEMS(CState, PlayerIndex++);
            ADD_PLAYER_ITEMS(CState, sprite_item_name.SpriteItem_Morfina, sprite_item_type.SpriteAddon_Customizable);
            ADD_PLAYER_ITEMS(CState, sprite_item_name.SpriteItem_Espinaca, sprite_item_type.SpriteAddon_Feet);
            ADD_PLAYER_ITEMS(CState, sprite_item_name.SpriteItem_Pesas, sprite_item_type.SpriteAddon_Feet);
            ADD_PLAYER_ITEMS(CState, sprite_item_name.SpriteItem_Vodka, sprite_item_type.SpriteAddon_Head);
            END_PLAYER_ITEMS(CState);

#if false
            BEGIN_PLAYER_ITEMS(CState, PlayerIndex++);
            ADD_PLAYER_ITEMS(CState, sprite_item_name.SpriteItem_Cocos, sprite_item_type.SpriteAddon_Customizable);
            ADD_PLAYER_ITEMS(CState, sprite_item_name.SpriteItem_CollarMaya, sprite_item_type.SpriteAddon_Torso);
            ADD_PLAYER_ITEMS(CState, sprite_item_name.SpriteItem_Dross, sprite_item_type.SpriteAddon_Head);
            ADD_PLAYER_ITEMS(CState, sprite_item_name.SpriteItem_Fez, sprite_item_type.SpriteAddon_Feet);
            ADD_PLAYER_ITEMS(CState, sprite_item_name.SpriteItem_Hawaii, sprite_item_type.SpriteAddon_Feet);
            END_PLAYER_ITEMS(CState);
#endif

            PlayerIndex = 0;
            //NOTE(Alex): User Test index = 0;
            //TODO(Alex): Check for non repetitive values 
            //TODO(Alex): Write this to a file!
            CState.UserItems = FindAllAssetsForUser(CState, PlayerIndex).ToArray();
            CustomAddUserItemsToScrolls(CState, CState.UserItems);
        }
    }

}
