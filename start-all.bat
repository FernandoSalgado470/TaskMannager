@echo off
echo ============================================
echo   Iniciando Todos los Microservicios
echo ============================================
echo.

echo [1/6] Iniciando UserService (Puerto 54894)...
start "UserService" cmd /k "cd /d ""D:\Ing. Software II\Proyecto IS\UserService\src\UserService.API"" && dotnet run"
timeout /t 3 /nobreak >nul

echo [2/6] Iniciando TaskService (Puerto 5000)...
start "TaskService" cmd /k "cd /d ""D:\Ing. Software II\Proyecto IS\TaskService\src\TaskService.API"" && dotnet run"
timeout /t 3 /nobreak >nul

echo [3/6] Iniciando LoginService (Puerto 5001)...
start "LoginService" cmd /k "cd /d ""D:\Ing. Software II\Proyecto IS\LoginService\src\LoginService.API"" && dotnet run"
timeout /t 3 /nobreak >nul

echo [4/6] Iniciando GradesService (Puerto 5002)...
start "GradesService" cmd /k "cd /d ""D:\Ing. Software II\Proyecto IS\GradesService\src\GradesService.API"" && dotnet run"
timeout /t 3 /nobreak >nul

echo [5/6] Iniciando AcademicService (Puerto 5003)...
start "AcademicService" cmd /k "cd /d ""D:\Ing. Software II\Proyecto IS\AcademicService\src\AcademicService.API"" && dotnet run"
timeout /t 5 /nobreak >nul

echo [6/6] Iniciando Frontend React (Puerto 3000)...
start "Frontend React" cmd /k "cd /d ""D:\Ing. Software II\Proyecto IS\academic-frontend"" && npm start"

echo.
echo ============================================
echo   Todos los servicios estan iniciando...
echo ============================================
echo.
echo Servicios disponibles:
echo - UserService:     http://localhost:54894/swagger
echo - TaskService:     http://localhost:5000/swagger
echo - LoginService:    http://localhost:5001/swagger
echo - GradesService:   http://localhost:5002/swagger
echo - AcademicService: http://localhost:5003/swagger
echo - Frontend:        http://localhost:3000
echo.
echo Presiona cualquier tecla para cerrar esta ventana...
pause >nul
