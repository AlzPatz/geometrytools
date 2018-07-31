@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)
 
set version=1.0.0
if not "%PackageVersion%" == "" (
   set version=%PackageVersion%
)

cd geometrytools

dotnet build 

cd ..

dir geometrytools\bin\%config%\netstandard2.0

%NuGet% pack "geometrytools\geometrytools.nuspec2" -NoPackageAnalysis -verbosity detailed -o Build -Version %version% -p Configuration="%config%"