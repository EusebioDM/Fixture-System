#!/bin/bash
echo Copying fixture algorithms
rm -r ./bin/Debug/netcoreapp2.1/FixtureGenerators
mkdir ./bin/Debug/netcoreapp2.1/FixtureGenerators
cp ./../FixtureGenerators/EirinDuran.FixtureGenerators.AllOnce/bin/Debug/netstandard2.0/EirinDuran.FixtureGenerators.AllOnce.dll ./bin/Debug/netcoreapp2.1/FixtureGenerators
cp ./../FixtureGenerators/EirinDuran.FixtureGenerators.RoundRobin/bin/Debug/netstandard2.0/EirinDuran.FixtureGenerators.RoundRobin.dll ./bin/Debug/netcoreapp2.1/FixtureGenerators