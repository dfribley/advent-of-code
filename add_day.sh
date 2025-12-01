#!/bin/bash

source ~/aoc_envars

event=$1
day_num=$2

day=Day$day_num
day_dir=$event/$day
csproj=$day_dir/$day.csproj

mkdir $day_dir

cat > $csproj << EOF
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>disable</Nullable>
    <PublishSingleFile>True</PublishSingleFile>
    <AssemblyName>day$day_num</AssemblyName>
    <LangVersion>default</LangVersion>
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

cat > $day_dir/Program.cs << EOF
// Advent of Code challenge: https://adventofcode.com/$event/day/$day_num
Console.WriteLine("AoC - Day $day_num\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToList();

    Console.WriteLine($"Part 1:");
    Console.WriteLine($"Part 2:\n");
}
EOF

curl -H "Cookie: session=$AOC__SESSION__TOKEN" "https://adventofcode.com/$event/day/$day_num/input" -o $day_dir/input.txt
touch $day_dir/sample.txt

dotnet sln $event/AoC.$event.sln add $csproj
