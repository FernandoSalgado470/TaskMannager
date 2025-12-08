@echo off
echo ============================================
echo   Deteniendo Todos los Microservicios
echo ============================================
echo.

echo Deteniendo procesos de .NET (dotnet)...
taskkill /F /IM dotnet.exe /T 2>nul
if %errorlevel% equ 0 (
    echo - Servicios .NET detenidos
) else (
    echo - No se encontraron servicios .NET corriendo
)

echo.
echo Deteniendo procesos de Node.js (npm)...
taskkill /F /IM node.exe /T 2>nul
if %errorlevel% equ 0 (
    echo - Frontend React detenido
) else (
    echo - No se encontro el frontend corriendo
)

echo.
echo ============================================
echo   Todos los servicios han sido detenidos
echo ============================================
echo.
pause
