using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Game.World.Definitions;
using HunterPie.Core.Game.World.Utils;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace HunterPie.Core.Game.World.Entities.Weapons;
public class MHWMeleeWeapon : Scannable, IWeapon, IMeleeWeapon, IEventDispatcher
{
    private int _weaponId;
    private Sharpness _sharpness;
    private int _currentSharpness;

    public Weapon Id { get; }

    public Sharpness Sharpness
    {
        get => _sharpness;
        private set
        {
            if (value != _sharpness)
            {
                _sharpness = value;
                this.Dispatch(OnSharpnessLevelChange, new SharpnessEventArgs(this));
            }
        }
    }

    public int CurrentSharpness
    {
        get => _currentSharpness;
        private set
        {
            if (value != _currentSharpness)
            {
                _currentSharpness = value;
                this.Dispatch(OnSharpnessChange, new SharpnessEventArgs(this));
            }
        }
    }

    public int[] SharpnessThresholds { get; private set; }

    public int MaxSharpness { get; private set; }

    public int Threshold { get; private set; }

    public event EventHandler<SharpnessEventArgs> OnSharpnessChange;
    public event EventHandler<SharpnessEventArgs> OnSharpnessLevelChange;

    public MHWMeleeWeapon(IProcessManager process, Weapon id) : base(process)
    {
        Id = id;
    }

    [ScannableMethod]
    private void GetWeaponSharpness()
    {
        long weaponDataPtr = _process.Memory.Read(
            AddressMap.GetAbsolute("WEAPON_DATA_ADDRESS"),
            AddressMap.Get<int[]>("WEAPON_DATA_OFFSETS")
        );

        if (weaponDataPtr.IsNullPointer())
            return;

        int weaponId = _process.Memory.Deref<int>(
            AddressMap.GetAbsolute("WEAPON_ADDRESS"),
            AddressMap.Get<int[]>("WEAPON_ID_OFFSETS")
        );

        MHWSharpnessStructure sharpness = _process.Memory.Deref<MHWSharpnessStructure>(
            AddressMap.GetAbsolute("WEAPON_ADDRESS"),
            AddressMap.Get<int[]>("WEAPON_SHARPNESS_OFFSETS")
        );

        if (SharpnessThresholds is null || _weaponId != weaponId)
        {
            long weaponSharpnessArrayPtr = _process.Memory.Read(weaponDataPtr, new[] { weaponId * sizeof(long), 0x0C });
            short[] sharpnesses = _process.Memory.Read<short>(weaponSharpnessArrayPtr, 7);

            SharpnessThresholds = sharpnesses.Select(t => (int)t)
                                             .ToArray();

            _weaponId = weaponId;
        }

        if (sharpness.Level >= Sharpness.Invalid)
            sharpness.Level = SharpnessThresholds.ToSharpnessLevel(sharpness.Sharpness);

        long gearSkillsPtr = _process.Memory.Read(
            AddressMap.GetAbsolute("ABNORMALITY_ADDRESS"),
            AddressMap.Get<int[]>("GEAR_SKILL_OFFSETS")
        );

        MHWGearSkill handicraft = _process.Memory.Read<MHWGearSkill>(gearSkillsPtr + (Marshal.SizeOf<MHWGearSkill>() * 0x36));

        int maxHits = SharpnessThresholds.MaximumSharpness(handicraft);
        Sharpness = sharpness.Level;
        MaxSharpness = CalculateMaxThreshold(sharpness.Level, SharpnessThresholds, maxHits);
        Threshold = CalculateCurrentThreshold(sharpness.Level, SharpnessThresholds);
        CurrentSharpness = sharpness.Sharpness;
    }

    private static int CalculateCurrentThreshold(Sharpness currentLevel, int[] thresholds)
    {
        Sharpness previousLevel = currentLevel - 1;

        if (previousLevel <= Sharpness.Broken)
            return 0;

        return thresholds[(int)previousLevel];
    }

    private static int CalculateMaxThreshold(Sharpness currentLevel, int[] thresholds, int maxHits)
    {
        int nextLevel = (int)currentLevel + 1;

        if (nextLevel > (int)Sharpness.Purple || thresholds[nextLevel] == 0)
            return maxHits;

        return thresholds[(int)currentLevel];
    }
}
