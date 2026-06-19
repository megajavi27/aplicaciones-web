import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UsuarioService } from '@services/usuario.service';

@Component({
  selector: 'app-usuario-list',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './list.html'
})
export class ListComponent {
  private usuarioService = inject(UsuarioService);

  usuarios = signal<any[]>([]);
  isLoading = signal(true);

  constructor() {
    this.loadUsuarios();
  }

  loadUsuarios() {
    this.isLoading.set(true);
    this.usuarioService.getAll().subscribe({
      next: (data) => {
        this.usuarios.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Error cargando usuarios', err);
        // Fallback for UI demonstration if no API is running
        this.usuarios.set([
          { idUsuario: 1, nombre: 'Admin', apellido: 'Sistema', email: 'admin@hotelbombay.com', activo: true, nombrePerfil: 'Administrador' },
          { idUsuario: 2, nombre: 'Juan', apellido: 'Pérez', email: 'jperez@hotelbombay.com', activo: true, nombrePerfil: 'Recepcionista' },
          { idUsuario: 3, nombre: 'María', apellido: 'Gómez', email: 'mgomez@hotelbombay.com', activo: false, nombrePerfil: 'Gerente' }
        ]);
        this.isLoading.set(false);
      }
    });
  }

  confirmDelete(id: number) {
    if (confirm('¿Está seguro de que desea eliminar este usuario?')) {
      this.usuarioService.delete(id).subscribe({
        next: () => {
          this.loadUsuarios();
        },
        error: (err) => {
          console.error('Error eliminando usuario', err);
          // Fallback UI update
          this.usuarios.update(list => list.filter(u => u.idUsuario !== id));
        }
      });
    }
  }
}
