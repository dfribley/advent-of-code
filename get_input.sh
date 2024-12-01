#!/bin/bash

source ~/aoc_envars
event=$1
day=Day$2

curl -H "Cookie: session=$AOC__SESSION__TOKEN" "https://adventofcode.com/$event/day/$2/input" -o $event/$day/input.txt
