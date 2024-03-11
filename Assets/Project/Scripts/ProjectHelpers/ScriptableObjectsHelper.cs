namespace Popeye.ProjectHelpers
{
    public static class ScriptableObjectsHelper
    {
        private const string ROOT = "Popeye/";
        public const string CORE_ASSETS_PATH = ROOT + "Core/";
        
        public const string PLAYER_ANCHOR_ASSETS_PATH = ROOT + "PlayerAnchor/";
        public const string ANCHOR_ASSETS_PATH = PLAYER_ANCHOR_ASSETS_PATH + "Anchor/";
        public const string ANCHORCHAIN_ASSETS_PATH = ANCHOR_ASSETS_PATH + "Chain/";
        public const string PLAYER_ASSETS_PATH = PLAYER_ANCHOR_ASSETS_PATH + "Player/";

        public const string ENEMIES_ASSET_PATH = ROOT + "Enemies/";
        public const string ENEMYHINTS_ASSET_PATH =ENEMIES_ASSET_PATH + "Hints/";
        public const string HAZARDS_ASSET_PATH = ENEMIES_ASSET_PATH + "Hazards/";
        
        public const string PLAYERCONTROLLER_ASSETS_PATH = ROOT + "PlayerController/";
        public const string CAMERA_ASSETS_PATH = ROOT + "Camera/";
        public const string AUTOAIM_ASSETS_PATH = ROOT + "AutoAim/";
        
        
        public const string GAMESTATE_ASSETS_PATH = ROOT + "GameState/";
        
        public const string WORLDELEMENTS_ASSETS_PATH = ROOT + "WorldElements/";
        public const string WALLBUILDER_ASSETS_PATH = WORLDELEMENTS_ASSETS_PATH + "WallBuilder/";
        public const string GRIDMOVEMENT_ASSETS_PATH = WORLDELEMENTS_ASSETS_PATH + "GridMovement/";
        
        
        public const string TIME_ASSETS_PATH = ROOT + "Time/";
        public const string HITSTOP_ASSETS_PATH = TIME_ASSETS_PATH + "HitStop/";
        
        public const string VFX_ASSETS_PATH = ROOT + "VFX/";
        
        public const string SCENELOADING_ASSETS_PATH = ROOT + "SceneLoading/";
        
        
        public const string ID_ASSETS_PATH = ROOT + "ID/";
        public const string OBJECTTYPE_ASSETS_PATH = ROOT + "ObjectType/";
        
        
        public const string COLLISIONS_PATH = ROOT + "Collisions/";
        public const string COMBATSYSTEM_PATH = ROOT + "CombatSystem/";
        
        
        public const string SOUNDSYSTEM_ASSETS_PATH = ROOT + "SoundSystem/";
        
        
        public const string EDITORUTILITIES_ASSETS_PATH = ROOT + "EditorUtilities/";
        public const string TEXTUTILITIES_ASSETS_PATH = ROOT + "TextUtilities/";
    }
}