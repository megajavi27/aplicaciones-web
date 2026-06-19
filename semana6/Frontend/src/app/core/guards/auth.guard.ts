import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { map } from 'rxjs';

/**
 * Guard que valida la sesión desde el backend (GET /api/auth/me).
 * Si el servidor responde 401 (sesión expirada o inválida) redirige a /login.
 */
export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.validateSession().pipe(
    map(valid => {
      if (valid) return true;
      router.navigate(['/login']);
      return false;
    })
  );
};
