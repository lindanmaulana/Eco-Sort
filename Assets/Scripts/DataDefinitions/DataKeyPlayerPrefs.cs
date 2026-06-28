using UnityEngine;


// ==========================================
// STRUCTURED PLAYERPREFS KEYS (Anti-Typo)
// ==========================================

public static class DataKeyPlayerPrefs
{
    // Sakelar Pemain Baru / Lama
    public const string INVENTORY_SAVED = "InventorySaved";


    
    // Gudang Data Utama (JSON)
    public const string INVENTORY_DATA  = "InventoryData";


    
    // Mata Uang Game
    public const string USER_COINS      = "UserCoins";
    public const int USER_COINS_DEFAULT = 50;


    
    // Slot Tong yang Sedang Dipakai (Equipped)
    public const string EQUIP_ORGANIC   = "User_Equipped_Bin_Organic";
    public const string EQUIP_INORGANIC = "User_Equipped_Bin_Inorganic";
    public const string EQUIP_B3        = "User_Equipped_Bin_B3";


    // Slot Audio Setting
    public const string SETTING_MUSIC_BACKGROUND   = "User_Setting_Music_Background";
    public const string SETTING_SOUND_BACKGROUND   = "User_Setting_Sound_Background"; 
    public const string SETTING_VIBRATE_BACKGROUND = "User_Setting_Vibrate_Background"; 
};