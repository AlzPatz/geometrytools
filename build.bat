@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)
 
set version=1.0.0
if not "%PackageVersion%" == "" (
   set version=%PackageVersion%
)

mkdir geometrytools\bin
mkdir geometrytools\bin\%config%
mkdir geometrytools\bin\%config%\netstandard2.0

cd geometrytools

dotnet build 

dir geometrytools\bin\%config%\netstandard2.0

%NuGet% pack "geometrytools\geometrytools.nuspec" -NoPackageAnalysis -verbosity detailed -o Build -Version %version% -p Configuration="%config%"