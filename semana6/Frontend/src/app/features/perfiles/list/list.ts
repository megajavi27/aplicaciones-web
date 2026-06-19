import { Component, inject, signal } from '@angular/core';
import { PerfilService } from '@services/perfil.service';

@Component({
  selector: 'app-perfil-list',
  standalone: true,
  templateUrl: './list.html'
})
export class ListComponent {
  private perfilService = inject(PerfilService);

  perfiles = signal<any[]>([]);
  isLoading = signal(true);

  constructor() {
    this.loadPerfiles();
  }

  loadPerfiles() {
    this.isLoading.set(true);
    this.perfilService.getAll().subscribe({
      next: (data) => {
        this.perfiles.set(data);
        this.isLoading.set(false);
      },
      error: () => {
        // Fallback UI
        this.perfiles.set([
          { id: 1, nombre: 'Administrador', descripcion: 'Acceso total al sistema y configuraciones.' },
          { id: 2, nombre: 'Gerente', descripcion: 'Gestión de reservas y reportes generales.' },
          { id: 3, nombre: 'Recepcionista', descripcion: 'Atención al cliente, check-in y check-out.' }
        ]);
        this.isLoading.set(false);
      }
    });
  }
}
