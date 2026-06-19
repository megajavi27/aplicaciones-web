# Backend — .NET 10 Web API (Hotel Bombay)

## Requisitos
- .NET 10 SDK
- MySQL 8+

## Variables de entorno
Crea/edita el archivo `Backend/Backend/.env`:

```env
DB_SERVER=127.0.0.1
DB_PORT=3306
DB_NAME=semana5
DB_USER=root
DB_PASSWORD=123456
DB_CHARSET=utf8mb4

JWT_KEY=HBSecretKey_2026!!
JWT_ISSUER=semana5-api
JWT_AUDIENCE=semana5-client
JWT_EXPIRES_MINUTES=60
```

## Correr el proyecto
```bash
cd Backend/Backend
dotnet run
```
La API queda disponible en `http://localhost:5093`.  
Swagger UI: `http://localhost:5093/directory`

## Endpoints de autenticación

| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| POST | `/api/auth/login` | Login; establece cookie `auth_token` (HttpOnly) y `XSRF-TOKEN` | No |
| POST | `/api/auth/logout` | Limpia las cookies de sesión | Sí |
| GET | `/api/auth/me` | Valida sesión y devuelve datos del usuario | Sí |

## Seguridad de sesión
- **JWT** almacenado en cookie `auth_token` con flags `HttpOnly`, `Secure` (producción) y `SameSite=Lax`.
- **XSRF-TOKEN** cookie legible por JavaScript; Angular la lee y envía el header `X-XSRF-TOKEN` en cada petición mutante.
- **CORS** configurado para `http://localhost:4200` con `AllowCredentials`.

## Pruebas HTTP
Usa el archivo `Backend/Backend.http` en Visual Studio o JetBrains para probar los endpoints directamente.
