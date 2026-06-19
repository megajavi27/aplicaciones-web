# Frontend — Angular 19 (Hotel Bombay)

## Requisitos
- Node.js 20+
- Angular CLI 19+

## Instalar dependencias
```bash
cd Frontend
npm install
```

## Correr el proyecto
```bash
ng serve
```
Disponible en `http://localhost:4200`.

> **Importante:** el backend debe estar corriendo en `http://localhost:5093` antes de iniciar el frontend.

## Flujo de autenticación

1. El usuario ingresa credenciales en `/login` (formulario reactivo con validación).
2. Angular llama `POST /api/auth/login` con `withCredentials: true`.
3. El backend valida, genera el JWT y lo deposita en una cookie **HttpOnly** (`auth_token`). También envía la cookie `XSRF-TOKEN` para protección CSRF.
4. El frontend guarda únicamente el **nombre del usuario** en `localStorage` (requerimiento de la tarea).
5. Rutas protegidas pasan por `authGuard`, que llama `GET /api/auth/me` para validar la sesión desde el backend.
6. El logout llama `POST /api/auth/logout`; el backend limpia las cookies y el frontend borra `localStorage`.

## Estructura relevante

```
src/app/
├── core/
│   ├── guards/auth.guard.ts          # Guard async que valida sesión en backend
│   ├── interceptors/auth.interceptor.ts  # withCredentials + manejo 401
│   └── services/auth.service.ts     # login / logout / validateSession
├── features/
│   └── auth/login/                  # Componente de login
└── app.config.ts                    # XSRF configurado con withXsrfConfiguration
```
