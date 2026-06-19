import { Routes } from '@angular/router';
import { authGuard } from '@guards/auth.guard';
import { MainLayoutComponent } from './shared/layout/main-layout/main-layout';

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./features/auth/login/login').then(m => m.LoginComponent) },
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: 'usuarios', loadChildren: () => import('./features/usuarios/routes').then(m => m.usuariosRoutes) },
      { path: 'perfiles', loadChildren: () => import('./features/perfiles/routes').then(m => m.perfilesRoutes) },
      { path: '', redirectTo: 'usuarios', pathMatch: 'full' }
    ]
  },
  { path: '**', redirectTo: '' }
];
