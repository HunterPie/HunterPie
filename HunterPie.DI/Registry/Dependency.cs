namespace HunterPie.DI.Registry;

internal record Dependency(
    Type ConcreteType,
    Type AbstractType
);