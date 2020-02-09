@echo off
setlocal EnableDelayedExpansion

if [%1]==[] goto BLANK

dotnet publish BaGet -c Release -o .\app\%1 --self-contained -r %1

GOTO DONE

:BLANK

ECHO No Parameter

GOTO DONE

:DONE
