$libraries = @("Core", "UI", "DI", "Generators", "Integrations", "Platforms")

foreach ($library in $libraries) {
  $project = "./HunterPie.$library/"

  dotnet pack $project --configuration Release --output artifacts --v quiet --no-restore --property WarningLevel=0 /clp:ErrorsOnly

  echo "Packed HunterPie.$library.dll"
}