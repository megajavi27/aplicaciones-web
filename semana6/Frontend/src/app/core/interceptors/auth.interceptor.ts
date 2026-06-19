import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { catchError, throwError } from 'rxjs';

/**
 * Interceptor que:
 * 1. Añade withCredentials a todas las peticiones (envía cookies de sesión).
 * 2. Captura respuestas 401 y fuerza logout (sesión expirada).
 *
 * El token XSRF lo gestiona Angular automáticamente leyendo la cookie
 * XSRF-TOKEN y enviando el header X-XSRF-TOKEN (configurado en app.config.ts).
 */
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  const authReq = req.clone({ withCredentials: true });

  return next(authReq).pipe(
    catchError(error => {
      // Evitar bucle: no interceptar 401 en rutas de auth
      const isAuthUrl = req.url.includes('/auth/');
      if (error.status === 401 && !isAuthUrl) {
        authService.logout().subscribe();
      }
      return throwError(() => error);
    })
  );
};
