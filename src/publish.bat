@echo off
setlocal EnableDelayedExpansion

if [%1]==[] goto BLANK

rem dotnet restore BaGet
rem dotnet build BaGet -c Release -o .\app\%1
dotnet publish BaGet -c Release -o .\app\%1 --self-contained -r %1

GOTO DONE

:BLANK

ECHO No Parameter

GOTO DONE

:DONE
