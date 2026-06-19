import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, catchError, map, of, tap } from 'rxjs';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private apiUrl = environment.apiUrl;

  private loggedIn = new BehaviorSubject<boolean>(this.hasNombre());
  isLoggedIn$ = this.loggedIn.asObservable();

  /**
   * Realiza el login: el backend establece la cookie HttpOnly con el JWT.
   * Solo el nombre del usuario se almacena en localStorage (requerimiento de la tarea).
   */
  login(email: string, password: string) {
    return this.http
      .post<{ nombre: string; perfil: string }>(
        `${this.apiUrl}/auth/login`,
        { email, password },
        { withCredentials: true }
      )
      .pipe(
        tap(response => {
          if (response?.nombre) {
            localStorage.setItem('nombre', response.nombre);
            localStorage.setItem('perfil', response.perfil ?? '');
            this.loggedIn.next(true);
          }
        })
      );
  }

  /**
   * Cierra sesión: el backend elimina las cookies de sesión.
   */
  logout(): Observable<void> {
    return this.http
      .post<void>(`${this.apiUrl}/auth/logout`, {}, { withCredentials: true })
      .pipe(
        tap(() => this.clearSession()),
        catchError(() => {
          this.clearSession();
          return of(undefined);
        })
      );
  }

  /**
   * Valida la sesión consultando el backend (/api/auth/me).
   * El guard llama este método; si el backend responde 401, la sesión expiró.
   */
  validateSession(): Observable<boolean> {
    return this.http
      .get<{ nombre: string; perfil: string }>(`${this.apiUrl}/auth/me`, { withCredentials: true })
      .pipe(
        map(user => {
          if (user?.nombre) {
            localStorage.setItem('nombre', user.nombre);
            localStorage.setItem('perfil', user.perfil ?? '');
            this.loggedIn.next(true);
          }
          return true;
        }),
        catchError(() => {
          this.clearSession();
          return of(false);
        })
      );
  }

  getUserName(): string | null {
    return localStorage.getItem('nombre');
  }

  isLoggedIn(): boolean {
    return this.hasNombre();
  }

  private hasNombre(): boolean {
    return !!localStorage.getItem('nombre');
  }

  private clearSession(): void {
    localStorage.removeItem('nombre');
    localStorage.removeItem('perfil');
    this.loggedIn.next(false);
    this.router.navigate(['/login']);
  }
}
