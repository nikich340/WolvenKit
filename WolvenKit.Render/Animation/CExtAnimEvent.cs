using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WolvenKit.Render.Animation
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    class CExtAnimEvent
    {
        //public string TYPE;
        //public string eventName;//<property Name = "eventName" Type="CName" />
        //public float? startTime;//<property Name = "startTime" Type="Float" />
        //public bool? reportToScript;//<property Name = "reportToScript" Type="Bool" />
        //public float? reportToScriptMinWeight; //<property Name = "reportToScriptMinWeight" Type="Float" />
        //public string animationName;//<property Name = "animationName" Type="CName" />
        //public string soundEventName;//<property Name = "soundEventName" Type="StringAnsi" />
        //public float? maxDistance;//<property Name = "maxDistance" Type="Float" />
        //public string bone;//<property Name = "bone" Type="CName" />
        //public List<string> switchesToUpdate;//<property Name = "switchesToUpdate" Type="array:2,0,StringAnsi" />
        //public List<string> parametersToUpdate;//<property Name = "parametersToUpdate" Type="array:2,0,StringAnsi" />
        //public bool? filter;//<property Name = "filter" Type="Bool" />
        //public float? filterCooldown;//<property Name = "filterCooldown" Type="Float" />
        //public bool? useDistanceParameter;//<property Name = "useDistanceParameter" Type="Bool" />
        //public float? speed;//<property Name = "speed" Type="Float" />
        //public float? decelDist;//<property Name = "decelDist" Type="Float" />

        //public float? duration; // <property Name = "duration" Type="Float" />
        //public bool? alwaysFiresEnd; // <property Name = "alwaysFiresEnd" Type="Bool" />
        
        //public SEnumVariant enumVariant; // <property Name="enumVariant" Type="SEnumVariant" />

        //public string effectName; //<property Name="effectName" Type="CName" />

        //public CPreAttackEventData data; //<property Name="data" Type="CPreAttackEventData" />



        ////CExtAnimItemSyncEvent" BaseClass="CExtAnimEvent">
        //public string equipSlot; //<property Name = "equipSlot" Type="CName" />
        //public string holdSlot; //<property Name = "holdSlot" Type="CName" />
        //public string action; //<property Name = "action" Type="EItemLatentAction" />


        ////<class Name="CExtAnimOnSlopeEvent" BaseClass="CExtAnimDurationEvent">
        //public float? slopeAngle;//    float? slopeAngle;<property Name = "slopeAngle" Type="Float" />


        ////<class Name="CExtAnimMaterialBasedFxEvent" BaseClass="CExtAnimEvent">
        //public bool? vfxKickup; //    <property Name = "vfxKickup" Type="Bool" />
        //public bool? vfxFootstep;//    <property Name = "vfxFootstep" Type="Bool" />
        ////</class>

        ////<class Name="CEASMultiValueEvent" BaseClass="CExtAnimScriptDurationEvent">
        //public string callback;//<property Name = "callback" Type="CName" />
        ////</class>

        ////<class Name="CEASMultiValueSimpleEvent" BaseClass="CExtAnimScriptEvent">
        //public SMultiValue SMultiValue;//<property Name = "properties" Type="SMultiValue" />
        ////</class>

        ////<class Name="CEASSlideToTargetEvent" BaseClass="CExtAnimScriptDurationEvent">
        //public SSlideToTargetEventProps properties;//<property Name = "properties" Type="SSlideToTargetEventProps" />
        ////</class>

        ////<class Name="CEdEntitySetupListParam" BaseClass="CGameplayEntityParam">
        //public bool? wasIncluded;//<property Name = "wasIncluded" Type="Bool" />
        //string name;//<property Name = "name" Type="String" />
        //public bool? overrideInherited;//<property Name = "overrideInherited" Type="Bool" />
        //public List<string> effectors;//<property Name = "effectors" Type="array:2,0,handle:IEdEntitySetupEffector" />
        //public bool? detachFromTemplate;//<property Name = "detachFromTemplate" Type="Bool" />
        ////</class>

        //public bool? translation;//<property Name = "translation" Type="Bool" />
        //public bool? rotation;//<property Name = "rotation" Type="Bool" />
        //public bool? toCollision;//<property Name = "toCollision" Type="Bool" />

        ////<class Name="CExtAnimAttackEvent" BaseClass="CExtAnimEvent">
        //public string soundAttackType;//<property Name = "soundAttackType" Type="CName" />
        ////</class>

        ////<class Name="CExtAnimCutsceneBodyPartEvent" BaseClass="CExtAnimEvent">
        //public string appearance;//<property Name = "appearance" Type="CName" />
        ////</class>

        ////<class Name="CExtAnimCutsceneBokehDofBlendEvent" BaseClass="CExtAnimDurationEvent">
        //public SBokehDofParams bokehDofParamsStart;//<property Name = "bokehDofParamsStart" Type="SBokehDofParams" />
        //public SBokehDofParams bokehDofParamsEnd;//<property Name = "bokehDofParamsEnd" Type="SBokehDofParams" />
        ////</class>

        ////<class Name="CExtAnimCutsceneBokehDofEvent" BaseClass="CExtAnimEvent">
        //public SBokehDofParams bokehDofParams;//<property Name = "bokehDofParams" Type="SBokehDofParams" />
        ////</class>

        ////<class Name="CExtAnimCutsceneBreakEvent" BaseClass="CExtAnimEvent">
        //public bool? iAmHackDoNotUseMeInGame;//<property Name = "iAmHackDoNotUseMeInGame" Type="Bool" />
        ////</class>

        ////<class Name="CExtAnimCutsceneDisableClothEvent" BaseClass="CExtAnimEvent">
        //public float? weight;//<property Name = "weight" Type="Float" />
        //public float? blendTime;//<property Name = "blendTime" Type="Float" />
        ////</class>

        ////<class Name="CExtAnimCutsceneEffectEvent" BaseClass="CExtAnimDurationEvent">
        //public string effect;//<property Name = "effect" Type="CName" />
        //public TagList tag;//<property Name = "tag" Type="TagList" />
        //public string template;//<property Name = "template" Type="soft:CEntityTemplate" />
        //public Vector spawnPosMS; //<property Name = "spawnPosMS" Type="Vector" />
        //public EulerAngles spawnRotMS;//<property Name = "spawnRotMS" Type="EulerAngles" />
        ////</class>

        ////<class Name="CExtAnimCutsceneEnvironmentEvent" BaseClass="CExtAnimEvent">
        //public bool? stabilizeBlending;//<property Name = "stabilizeBlending" Type="Bool" />
        //public bool? instantEyeAdaptation;//<property Name = "instantEyeAdaptation" Type="Bool" />
        //public bool? instantDissolve;//<property Name = "instantDissolve" Type="Bool" />
        //public bool? forceSetupLocalEnvironments;//<property Name = "forceSetupLocalEnvironments" Type="Bool" />
        //public bool? forceSetupGlobalEnvironments;//<property Name = "forceSetupGlobalEnvironments" Type="Bool" />
        //public string environmentName;//<property Name = "environmentName" Type="String" />
        //public bool? environmentActivate;//<property Name = "environmentActivate" Type="Bool" />
        //public bool? forceNoOtherEnvironments;//<property Name = "forceNoOtherEnvironments" Type="Bool" />
        ////</class>


        ////<class Name="CExtAnimCutsceneFadeEvent" BaseClass="CExtAnimEvent">
        //[JsonProperty("in")]
        //public bool? In;//<property Name = "in" Type="Bool" />
        //public ColorE color;//<property Name = "color" Type="Color" />
        ////</class>

        ////<class Name="CExtAnimCutsceneHideEntityEvent" BaseClass="CExtAnimCutsceneEvent">
        //public string entTohideTag;//<property Name = "entTohideTag" Type="CName" />
        ////</class>

        ////<class Name="CExtAnimCutsceneLightEvent" BaseClass="CExtAnimEvent">
        ////<property Name = "tag" Type="TagList" />
        //public bool? isEnabled;//<property Name = "isEnabled" Type="Bool" />
        //public float? radius;//<property Name = "radius" Type="Float" />
        //public float? brightness;//<property Name = "brightness" Type="Float" />
        //public SLightFlickering lightFlickering;//<property Name = "lightFlickering" Type="SLightFlickering" />
        ////</class>

        ////<class Name="CExtAnimCutsceneQuestEvent" BaseClass="CExtAnimEvent">
        //public string cutsceneName;//<property Name = "cutsceneName" Type="String" />
        ////</class>

        ////<class Name="CExtAnimCutsceneResetClothAndDangleEvent" BaseClass="CExtAnimEvent">
        //public bool? forceRelaxedState;//<property Name = "forceRelaxedState" Type="Bool" />
        ////</class>

        ////<class Name="CExtAnimCutsceneSetClippingPlanesEvent" BaseClass="CExtAnimEvent">
        //public string nearPlaneDistance;//<property Name = "nearPlaneDistance" Type="ENearPlaneDistance" />
        //public string farPlaneDistance;//<property Name = "farPlaneDistance" Type="EFarPlaneDistance" />
        //public SCustomClippingPlanes customPlaneDistance;//<property Name = "customPlaneDistance" Type="SCustomClippingPlanes" />
        ////</class>

        ////<class Name="CExtAnimCutsceneSlowMoEvent" BaseClass="CExtAnimCutsceneDurationEvent">
        //public bool? enabled;//<property Name = "enabled" Type="Bool" />
        //public float? factor;//<property Name = "factor" Type="Float" />
        //public bool? useWeightCurve;//<property Name = "useWeightCurve" Type="Bool" />
        //public SCurveData weightCurve;//<property Name = "weightCurve" Type="SCurveData" />
        ////</class>

        ////<class Name="CExtAnimCutsceneSoundEvent" BaseClass="CExtAnimEvent">
        //public bool? useMaterialInfo;//<property Name = "useMaterialInfo" Type="Bool" />
        ////</class>

        ////<class Name="CExtAnimCutsceneSurfaceEffect" BaseClass="CExtAnimCutsceneEvent">
        //public ESceneEventSurfacePostFXType type;//<property Name = "type" Type="ESceneEventSurfacePostFXType" />
        //public bool? worldPos;//<property Name = "worldPos" Type="Bool" />
        //public Vector position;//<property Name = "position" Type="Vector" />
        //public float? fadeInTime;//<property Name = "fadeInTime" Type="Float" />
        //public float? fadeOutTime;//<property Name = "fadeOutTime" Type="Float" />
        //public float? durationTime;//<property Name = "durationTime" Type="Float" />
        ////</class>

        ////<class Name="CExtAnimDialogKeyPoseDuration" BaseClass="CExtAnimDurationEvent">
        //public bool? transition;//<property Name = "transition" Type="Bool" />
        //public bool? keyPose;//<property Name = "keyPose" Type="Bool" />
        ////</class>

        ////<class Name="CExtAnimEventsFile" BaseClass="CResource">
        //public string requiredSfxTag;//<property Name = "requiredSfxTag" Type="CName" />
        ////</class>


        ////<class Name="CExtAnimFootstepEvent" BaseClass="CExtAnimSoundEvent">
        //public bool? fx;//<property Name = "fx" Type="Bool" />
        //public string customFxName;//<property Name = "customFxName" Type="CName" />
        ////</class>

        ////<class Name="CExtAnimGameplayMimicEvent" BaseClass="CExtAnimDurationEvent">
        //public string animation;//<property Name = "animation" Type="CName" />
        ////</class>

        ////<class Name="CExtAnimHitEvent" BaseClass="CExtAnimEvent">
        //uint? hitLevel;//<property Name = "hitLevel" Type="Uint32" />
        ////</class>

        ////<class Name="CExtAnimItemAnimationEvent" BaseClass="CExtAnimEvent">
        //public string itemCategory;//<property Name = "itemCategory" Type="CName" />
        //public string itemAnimationName;//<property Name = "itemAnimationName" Type="CName" />
        ////</class>

        ////<class Name="CExtAnimItemBehaviorEvent" BaseClass="CExtAnimEvent">
        //[JsonProperty("event")]
        //public string Event;//<property Name = "event" Type="CName" />
        ////</class>

        ////<class Name="CExtAnimItemEffectDurationEvent" BaseClass="CExtAnimDurationEvent">
        //public string itemSlot;//<property Name = "itemSlot" Type="CName" />
        ////</class>

        ////<class Name="CExtAnimItemEvent" BaseClass="CExtAnimEvent">
        //public string category;//<property Name = "category" Type="CName" />
        //public string itemName_optional;//<property Name = "itemName_optional" Type="CName" />
        //public string ignoreItemsWithTag;//<property Name = "ignoreItemsWithTag" Type="CName" />
        //public string itemGetting;//<property Name = "itemGetting" Type="EGettingItem" />
        ////</class>


        ////<class Name="CExtAnimItemSyncWithCorrectionEvent" BaseClass="CExtAnimDurationEvent">
        //public string correctionBone;//<property Name = "correctionBone" Type="CName" />
        ////</class>

        ////<class Name="CExtAnimLocationAdjustmentEvent" BaseClass="CExtAnimDurationEvent">
        //public string locationAdjustmentVar;//<property Name = "locationAdjustmentVar" Type="CName" />
        //public string adjustmentActiveVar;//<property Name = "adjustmentActiveVar" Type="CName" />
        ////</class>

        ////<class Name="CExtAnimLookAtEvent" BaseClass="CExtAnimDurationEvent">
        //public string level;//<property Name = "level" Type="ELookAtLevel" />
        ////</class>

        ////<class Name="CExtAnimMorphEvent" BaseClass="CExtAnimDurationEvent">
        //public string morphComponentId;//<property Name = "morphComponentId" Type="CName" />
        //public bool? invertWeight;//<property Name = "invertWeight" Type="Bool" />
        //public bool? useCurve;//<property Name = "useCurve" Type="Bool" />
        //public SCurveData curve;//<property Name = "curve" Type="SCurveData" />
        ////</class>

        ////<class Name="CExtAnimProjectileEvent" BaseClass="CExtAnimEvent">
        //public string spell;//<property Name = "spell" Type="handle:CEntityTemplate" />
        //public string castPosition;//<property Name = "castPosition" Type="EProjectileCastPosition" />
        //public string boneName;//<property Name = "boneName" Type="CName" />
        ////</class>

        ////<class Name="CExtAnimRaiseEventEvent" BaseClass="CExtAnimEvent">
        //public string eventToBeRaisedName;//<property Name = "eventToBeRaisedName" Type="CName" />
        //public bool? forceRaiseEvent;//<property Name = "forceRaiseEvent" Type="Bool" />
        ////</class>


        ////<class Name="CExtAnimReattachItemEvent" BaseClass="CExtAnimDurationEvent">
        //public string item;//<property Name = "item" Type="CName" />
        //public string targetSlot;//<property Name = "targetSlot" Type="CName" />
        ////</class>

        ////<class Name="CExtAnimRotationAdjustmentEvent" BaseClass="CExtAnimDurationEvent">
        //public string rotationAdjustmentVar;//<property Name = "rotationAdjustmentVar" Type="CName" />
        ////</class>

        ////<class Name="CExtAnimRotationAdjustmentLocationBasedEvent" BaseClass="CExtAnimDurationEvent">
        //public string targetLocationVar;//<property Name = "targetLocationVar" Type="CName" />
        ////</class>


        ////<class Name="CFistfightMinigame" BaseClass="CMinigame">
        //public string fightAreaTag;//<property Name = "fightAreaTag" Type="CName" />
        //public string playerPosTag;//<property Name = "playerPosTag" Type="CName" />
        //public bool? toTheDeath;//<property Name = "toTheDeath" Type="Bool" />
        //public bool? endsWithBlackscreen;//<property Name = "endsWithBlackscreen" Type="Bool" />
        //public List<CFistfightOpponent> enemies;//<property Name = "enemies" Type="array:2,0,CFistfightOpponent" />
        ////</class>

    }

    //public class CFistfightOpponent
    //{
    //    public string npcTag;//<property Name = "npcTag" Type="CName" />
    //    public string startingPosTag;//<property Name = "startingPosTag" Type="CName" />
    //}

    //public class ESceneEventSurfacePostFXType
    //{
    //}

    //public class SCurveData
    //{
    //}

    //public class SCustomClippingPlanes
    //{
    //}

    //public class SLightFlickering
    //{
    //}

    //public class EulerAngles
    //{
    //}

    //public class TagList
    //{
    //}

    //public class ColorE
    //{
    //}

    //public class SBokehDofParams
    //{
    //}

    //public class SMultiValue
    //{
    //}

    //public class SSlideToTargetEventProps
    //{
    //}

    //class SEnumVariant
    //{
    //    public string enumType;
    //    public int? enumValue;
    //}

    //[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    //class CPreAttackEventData
    //{
    //    public string attackName;//" Type="CName" />
    //    public string weaponSlot;//" Type="CName" />
    //    public int? hitReactionType;//" Type="Int32" />
    //    public string rangeName;//" Type="CName" />
    //    public bool? Damage_Friendly;//" Type="Bool" />
    //    public bool? Damage_Neutral;//" Type="Bool" />
    //    public bool? Damage_Hostile;//" Type="Bool" />
    //    public bool? Can_Parry_Attack;//" Type="Bool" />
    //    public string hitFX;//" Type="CName" />
    //    public string hitBackFX;//" Type="CName" />
    //    public string hitParriedFX;//" Type="CName" />
    //    public string hitBackParriedFX;//" Type="CName" />
    //    public int? swingType;//" Type="Int32" />
    //    public int? swingDir;//" Type="Int32" />
    //    public string soundAttackType;//" Type="CName" />
    //    public bool? canBeDodged;//" Type="Bool" />
    //    public string cameraAnimOnMissedHit;//" Type="CName" />
    //}

}
