#!/bin/bash

source ~/aoc_envars

mkdir Day$1

cat > Day$1/Day$1.csproj << EOF
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net7.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <PublishSingleFile>True</PublishSingleFile>
    <AssemblyName>day$1</AssemblyName>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\Shared\Shared.csproj" />
  </ItemGroup>

  <ItemGroup>
    <None Update="sample.txt">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
    <None Update="input.txt">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
  </ItemGroup>

</Project>
EOF

cat > Day$1/Program.cs << EOF
Console.WriteLine("AOC - Day $1\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToArray();

    Console.WriteLine($"Part 1:");
    Console.WriteLine($"Part 2:\n");
}
EOF

curl -H "Cookie: session=$AOC__SESSION__TOKEN" "https://adventofcode.com/2020/day/$1/input" -o Day$1/input.txt
touch Day$1/sample.txt

dotnet sln AoC.2020.sln add Day$1/Day$1.csproj
