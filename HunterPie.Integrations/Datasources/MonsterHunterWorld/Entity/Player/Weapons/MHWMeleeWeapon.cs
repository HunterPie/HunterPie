using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Player.Skills;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.Common.Entity.Player.Weapons;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Utils;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player.Weapons;
public class MHWMeleeWeapon : CommonMeleeWeapon
{
    private readonly ISkillService _skillService;
    private int _weaponId;
    private Sharpness _sharpness;
    private int _currentSharpness;
    private int _threshold;

    public override Weapon Id { get; }

    public override Sharpness Sharpness
    {
        get => _sharpness;
        protected set
        {
            if (value != _sharpness)
            {
                _sharpness = value;
                this.Dispatch(_onSharpnessLevelChange, new SharpnessEventArgs(this));
            }
        }
    }

    public override int CurrentSharpness
    {
        get => _currentSharpness;
        protected set
        {
            if (value != _currentSharpness)
            {
                _currentSharpness = value;
                this.Dispatch(_onSharpnessChange, new SharpnessEventArgs(this));
            }
        }
    }

    public override int[]? SharpnessThresholds { get; protected set; }

    public override int MaxSharpness { get; protected set; }

    public override int Threshold
    {
        get => _threshold;
        protected set
        {
            if (value == _threshold)
                return;

            _threshold = value;
            this.Dispatch(_onSharpnessChange, new SharpnessEventArgs(this));
        }
    }

    public MHWMeleeWeapon(ISkillService skillService, IProcessManager process, Weapon id) : base(process)
    {
        _skillService = skillService;
        Id = id;
    }

    [ScannableMethod]
    protected void GetWeaponSharpness()
    {
        long weaponDataPtr = Process.Memory.Read(
            AddressMap.GetAbsolute("WEAPON_DATA_ADDRESS"),
            AddressMap.Get<int[]>("WEAPON_DATA_OFFSETS")
        );

        if (weaponDataPtr.IsNullPointer())
            return;

        int weaponId = Process.Memory.Deref<int>(
            AddressMap.GetAbsolute("WEAPON_ADDRESS"),
            AddressMap.Get<int[]>("WEAPON_ID_OFFSETS")
        );

        MHWSharpnessStructure sharpness = Process.Memory.Deref<MHWSharpnessStructure>(
            AddressMap.GetAbsolute("WEAPON_ADDRESS"),
            AddressMap.Get<int[]>("WEAPON_SHARPNESS_OFFSETS")
        );

        if (sharpness.Level is >= Sharpness.Invalid or < Sharpness.Red)
            return;

        if (SharpnessThresholds is null || _weaponId != weaponId)
        {
            long weaponSharpnessArrayPtr = Process.Memory.Read(weaponDataPtr, new[] { weaponId * sizeof(long), 0x0C });
            short[] sharpnesses = Process.Memory.Read<short>(weaponSharpnessArrayPtr, 7);

            SharpnessThresholds = sharpnesses.Select(t => (int)t)
                                             .ToArray();

            _weaponId = weaponId;
        }

        sharpness.Level = SharpnessThresholds.ToSharpnessLevel(sharpness.Sharpness);

        Skill handicraft = _skillService.GetSkillBy(54);

        int maxHits = SharpnessThresholds.MaximumSharpness(handicraft);
        Sharpness = sharpness.Level;
        MaxSharpness = CalculateMaxThreshold(sharpness.Level, SharpnessThresholds, maxHits);
        Threshold = CalculateCurrentThreshold(sharpness.Level, SharpnessThresholds);
        CurrentSharpness = sharpness.Sharpness;
    }
}
