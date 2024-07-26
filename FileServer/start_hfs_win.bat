@echo off
setlocal

rem 设置脚本所在目录为工作目录
set "WORKDIR=%~dp0"

rem 显示脚本目录
echo Script directory: %WORKDIR%

rem 定义 hfs.exe 的路径
set "HFS_PATH=%WORKDIR%win\hfs.exe"

rem 设置 hfs.exe 的工作目录为 win 文件夹
cd /d "%WORKDIR%win"

rem 启动 hfs.exe
start "" "%HFS_PATH%"

rem 暂停脚本以查看任何输出
pause

endlocal
