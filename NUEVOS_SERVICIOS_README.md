# Nuevos Microservicios: LoginService y GradesService

## Resumen

Se han agregado dos nuevos microservicios al proyecto TaskManager:

1. **LoginService** - Servicio de autenticación y gestión de sesiones
2. **GradesService** - Servicio de gestión de calificaciones

Ambos servicios siguen la arquitectura Clean Architecture con 4 capas (Domain, Application, Infrastructure, API) y están completamente integrados con el frontend de React.

---

## LoginService

### Ubicación
`/LoginService/src/`

### Descripción
Microservicio dedicado a la autenticación de usuarios con JWT y gestión de sesiones.

### Entidades
- **User**: Usuarios del sistema
- **LoginAttempt**: Registro de intentos de login (exitosos y fallidos)
- **RefreshToken**: Tokens de refresco para mantener sesión

### Funcionalidades
- ✅ Registro de usuarios con validación
- ✅ Login con email y contraseña
- ✅ Generación de JWT tokens
- ✅ Refresh tokens para renovar sesión
- ✅ Logout y revocación de tokens
- ✅ Protección contra fuerza bruta (máx 5 intentos fallidos en 15 min)
- ✅ Tracking de intentos de login por IP y User Agent

### Endpoints
- `POST /api/auth/register` - Registrar nuevo usuario
- `POST /api/auth/login` - Iniciar sesión
- `POST /api/auth/refresh-token` - Refrescar token
- `POST /api/auth/revoke-token` - Revocar token
- `POST /api/auth/logout` - Cerrar sesión

### Puerto
- HTTP: `5001`
- HTTPS: `7001`

### Base de Datos
`LoginServiceDB_Dev`

---

## GradesService

### Ubicación
`/GradesService/src/`

### Descripción
Microservicio para gestión de calificaciones, categorías y notas finales de estudiantes.

### Entidades
- **Grade**: Calificaciones individuales (exámenes, tareas, etc.)
- **GradeCategory**: Categorías de calificación con pesos porcentuales
- **StudentGrade**: Calificación final por estudiante, materia y período

### Funcionalidades
- ✅ CRUD de calificaciones individuales
- ✅ CRUD de categorías de calificación
- ✅ CRUD de calificaciones finales
- ✅ Cálculo automático de calificación final ponderada
- ✅ Filtrado por estudiante, materia, período
- ✅ Validación de calificaciones
- ✅ Categorías predefinidas (Exámenes 40%, Tareas 30%, Participación 20%, Proyecto 10%)

### Endpoints

#### Calificaciones
- `GET /api/grades` - Obtener todas las calificaciones
- `GET /api/grades/{id}` - Obtener calificación por ID
- `GET /api/grades/student/{studentId}` - Calificaciones de un estudiante
- `GET /api/grades/subject/{subjectId}` - Calificaciones de una materia
- `GET /api/grades/student/{studentId}/subject/{subjectId}` - Calificaciones de estudiante en materia
- `POST /api/grades` - Crear calificación
- `PUT /api/grades/{id}` - Actualizar calificación
- `DELETE /api/grades/{id}` - Eliminar calificación

#### Categorías
- `GET /api/gradecategories` - Obtener todas las categorías
- `GET /api/gradecategories/active` - Obtener categorías activas
- `GET /api/gradecategories/{id}` - Obtener categoría por ID
- `POST /api/gradecategories` - Crear categoría
- `PUT /api/gradecategories/{id}` - Actualizar categoría
- `DELETE /api/gradecategories/{id}` - Eliminar categoría

#### Calificaciones Finales
- `GET /api/studentgrades` - Obtener todas las calificaciones finales
- `GET /api/studentgrades/{id}` - Obtener calificación final por ID
- `GET /api/studentgrades/student/{studentId}` - Calificaciones finales de un estudiante
- `GET /api/studentgrades/subject/{subjectId}` - Calificaciones finales de una materia
- `POST /api/studentgrades` - Crear calificación final
- `PUT /api/studentgrades/{id}` - Actualizar calificación final
- `DELETE /api/studentgrades/{id}` - Eliminar calificación final
- `POST /api/studentgrades/calculate?studentId=X&subjectId=Y&periodId=Z` - Calcular calificación final automáticamente

### Puerto
- HTTP: `5002`
- HTTPS: `7002`

### Base de Datos
`GradesServiceDB_Dev`

---

## Frontend

### Nuevas Páginas

#### 1. Login (`/login`)
- Archivo: `/academic-frontend/src/pages/auth/LoginPage.js`
- Funcionalidad: Formulario de inicio de sesión con validación
- Validaciones: Email válido, contraseña mínimo 6 caracteres

#### 2. Registro (`/register`)
- Archivo: `/academic-frontend/src/pages/auth/RegisterPage.js`
- Funcionalidad: Formulario de registro de usuario
- Validaciones: Username (min 3 chars), email válido, contraseña (min 6 chars), nombre completo

#### 3. Calificaciones (`/grades`)
- Archivo: `/academic-frontend/src/pages/Grades.js`
- Funcionalidad: Gestión completa de calificaciones
- Features:
  - Tabla de calificaciones
  - Crear/Editar/Eliminar calificaciones
  - Selección de categoría
  - Cálculo automático de porcentaje
  - Indicadores visuales (verde/rojo según aprobación)

### Servicios API

#### 1. loginService
- Archivo: `/academic-frontend/src/services/loginService.js`
- URL Base: `http://localhost:5001/api`
- Funciones: register, login, logout, refreshToken, getCurrentUser, isAuthenticated

#### 2. gradesService
- Archivo: `/academic-frontend/src/services/gradesService.js`
- URL Base: `http://localhost:5002/api`
- Funciones: Métodos para grades, categories y student grades

### Navegación
Se actualizó el Navbar con enlaces a:
- Calificaciones (`/grades`)
- Login (`/login`)

---

## Configuración de Base de Datos

Ambos servicios están configurados para usar SQL Server en AWS RDS:

```
Server: controldb.cpc2m0c022b3.us-east-2.rds.amazonaws.com
User: admin
Password: ProyectoIS2023
```

### Bases de Datos
- LoginService: `LoginServiceDB_Dev`
- GradesService: `GradesServiceDB_Dev`

---

## Crear Migraciones

### LoginService

```bash
cd LoginService/src/LoginService.API

# Crear migración inicial
dotnet ef migrations add InitialCreate --project ../LoginService.Infrastructure/LoginService.Infrastructure.csproj --startup-project LoginService.API.csproj --output-dir Migrations

# Aplicar migración a la base de datos
dotnet ef database update --project ../LoginService.Infrastructure/LoginService.Infrastructure.csproj --startup-project LoginService.API.csproj
```

### GradesService

```bash
cd GradesService/src/GradesService.API

# Crear migración inicial
dotnet ef migrations add InitialCreate --project ../GradesService.Infrastructure/GradesService.Infrastructure.csproj --startup-project GradesService.API.csproj --output-dir Migrations

# Aplicar migración a la base de datos
dotnet ef database update --project ../GradesService.Infrastructure/GradesService.Infrastructure.csproj --startup-project GradesService.API.csproj
```

---

## Ejecutar los Servicios

### LoginService

```bash
cd LoginService/src/LoginService.API
dotnet run
```

Swagger UI disponible en: `http://localhost:5001/swagger`

### GradesService

```bash
cd GradesService/src/GradesService.API
dotnet run
```

Swagger UI disponible en: `http://localhost:5002/swagger`

### Frontend

```bash
cd academic-frontend
npm install  # Si es primera vez
npm start
```

Aplicación disponible en: `http://localhost:3000`

---

## Arquitectura

Ambos servicios siguen Clean Architecture:

```
Service/
├── Domain/              # Entidades y reglas de negocio
│   ├── Entities/
│   └── Interfaces/
├── Application/         # Lógica de aplicación
│   ├── DTOs/
│   ├── Interfaces/
│   └── Services/
├── Infrastructure/      # Persistencia de datos
│   ├── Data/
│   └── Repositories/
└── API/                # Controllers y configuración
    ├── Controllers/
    ├── Program.cs
    └── appsettings.json
```

---

## Seguridad

### JWT Configuration

**LoginService:**
- Key: `SuperSecretKeyForLoginServiceMicroservice12345`
- Issuer: `LoginServiceAPI`
- Audience: `LoginServiceClient`
- Expiración: 24 horas

**GradesService:**
- Key: `SuperSecretKeyForGradesServiceMicroservice12345`
- Issuer: `GradesServiceAPI`
- Audience: `GradesServiceClient`
- Expiración: 24 horas

### Passwords
Se utiliza BCrypt para hashear contraseñas con salt automático.

---

## Validaciones Implementadas

### LoginService
- Email válido y único
- Username mínimo 3 caracteres y único
- Contraseña mínimo 6 caracteres
- Máximo 5 intentos fallidos en 15 minutos

### GradesService
- Calificación no puede exceder máximo
- Validación de IDs requeridos
- Validación de rangos (0-100)
- Peso porcentual entre 0-100
- Nombres únicos de categorías

---

## Datos Semilla

### GradesService
Se crean automáticamente 4 categorías de calificación:
1. **Exámenes** - 40% de peso
2. **Tareas** - 30% de peso
3. **Participación** - 20% de peso
4. **Proyecto Final** - 10% de peso

---

## Testing

Todos los endpoints están documentados en Swagger y pueden ser probados desde:
- LoginService: `http://localhost:5001/swagger`
- GradesService: `http://localhost:5002/swagger`

---

## Próximos Pasos

1. ✅ Crear las migraciones de base de datos
2. ✅ Aplicar las migraciones a la base de datos
3. Probar los endpoints desde Swagger
4. Probar el flujo completo desde el frontend
5. Integrar autenticación en otras páginas del frontend (opcional)

---

## Notas Importantes

- Los servicios están configurados con CORS habilitado para permitir peticiones desde el frontend
- Todos los endpoints tienen `[AllowAnonymous]` por defecto para facilitar el testing
- El frontend guarda el token en `localStorage`
- Los refresh tokens tienen validez de 7 días
- Las calificaciones se calculan automáticamente con promedio ponderado según categorías

---

## Contacto

Para cualquier duda o problema, consultar la documentación de cada servicio en sus respectivos README.md.
