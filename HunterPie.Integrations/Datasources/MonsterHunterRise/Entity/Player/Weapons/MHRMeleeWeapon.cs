using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Utils;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player.Weapons;
public class MHRMeleeWeapon : Scannable, IWeapon, IMeleeWeapon, IEventDispatcher
{
    private long _weaponSharpnessPointer;
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

    public int Threshold { get; set; }

    public event EventHandler<SharpnessEventArgs> OnSharpnessChange;
    public event EventHandler<SharpnessEventArgs> OnSharpnessLevelChange;

    public MHRMeleeWeapon(IProcessManager process, Weapon id) : base(process)
    {
        Id = id;
    }

    [ScannableMethod]
    private void GetWeaponSharpness()
    {
        long sharpnessArrayPtr = Process.Memory.Read(
            AddressMap.GetAbsolute("SHARPNESS_ADDRESS"),
            AddressMap.Get<int[]>("SHARPNESS_ARRAY_OFFSETS")
        );

        if (sharpnessArrayPtr.IsNullPointer())
            return;

        MHRSharpnessStructure structure = Process.Memory.Deref<MHRSharpnessStructure>(
            AddressMap.GetAbsolute("SHARPNESS_ADDRESS"),
            AddressMap.Get<int[]>("SHARPNESS_OFFSETS")
        );

        if (SharpnessThresholds is null || _weaponSharpnessPointer != sharpnessArrayPtr)
        {
            int[] sharpnessValues = Process.Memory.ReadArray<int>(sharpnessArrayPtr);
            SharpnessThresholds = CalculateThresholds(sharpnessValues);
            _weaponSharpnessPointer = sharpnessArrayPtr;
        }

        Sharpness = structure.Level;
        MaxSharpness = CalculateMaxThreshold(structure.Level, SharpnessThresholds, structure.MaxHits);
        Threshold = CalculateCurrentThreshold(structure.Level, SharpnessThresholds);
        CurrentSharpness = structure.Hits;
    }

    private static int[] CalculateThresholds(int[] sharpnesses)
    {
        int[] thresholds = new int[sharpnesses.Length];

        int sum = 0;
        for (int i = 0; i < sharpnesses.Length; i++)
        {
            if (sharpnesses[i] == 0)
                continue;

            sum += sharpnesses[i];
            thresholds[i] = sum;
        }

        return thresholds;
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
