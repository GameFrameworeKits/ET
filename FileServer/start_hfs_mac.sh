#!/bin/bash

# 将工作目录设置为脚本所在的目录
WORKDIR="$(cd "$(dirname "$0")" && pwd)"

# 显示脚本所在的目录
echo "Script directory: $WORKDIR"

# 定义HFS可执行文件的路径
HFS_PATH="$WORKDIR/mac/hfs"

# 设置HFS文件的执行权限
chmod +x "$HFS_PATH"

# 检查HFS文件是否存在并可执行
if [ ! -x "$HFS_PATH" ]; then
    echo "HFS executable is not found or is not executable. Please check the file permissions."
    exit 1
fi

# Change the current directory to the 'mac' folder
cd "$WORKDIR/mac"

# 运行HFS
"$HFS_PATH"

# 暂停脚本以查看输出
read -p "Press enter to continue..."

