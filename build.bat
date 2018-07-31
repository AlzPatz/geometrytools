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

%MsBuildExe% geometrytools\GeometryTools.csproj /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=diag /nr:false

cd bin\%config%\netstandard2.0
echo ls
cd ..
cd ..

%NuGet% pack "geometrytools\geometrytools.nuspec" -NoPackageAnalysis -verbosity detailed -o Build -Version %version% -p Configuration="%config%"