#!/bin/bash

# Set the working directory to the directory of the script
WORKDIR=$(dirname "$0")

# Display the script directory
echo "Script directory: $WORKDIR"

# Define the path to hfs.exe (assuming Wine is used to run Windows executables on Unix/Linux)
HFS_PATH="$WORKDIR/mac/hfs.exe"

# Change the current directory to the 'mac' folder
cd "$WORKDIR/mac"

# Start hfs.exe using Wine
wine "$HFS_PATH"

# Pause the script to view any output
read -p "Press enter to continue..."
