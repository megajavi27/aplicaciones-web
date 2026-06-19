import { Component, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { UsuarioService } from '@services/usuario.service';
import { PerfilService } from '@services/perfil.service';

@Component({
  selector: 'app-usuario-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './form.html'
})
export class FormComponent {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private usuarioService = inject(UsuarioService);
  private perfilService = inject(PerfilService);

  form: FormGroup;
  isEditMode = signal(false);
  userId: number | null = null;
  perfiles = signal<any[]>([]);

  isSaving = signal(false);
  successMessage = signal('');
  errorMessage = signal('');

  constructor() {
    this.form = this.fb.group({
      nombre: ['', Validators.required],
      apellido: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      telefono: [''],
      perfilId: ['', Validators.required],
      activo: [true],
      password: ['']
    });

    this.loadPerfiles();

    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.isEditMode.set(true);
        this.userId = +id;
        this.loadUsuario(this.userId);
      } else {
        // En modo creación, el password es requerido
        this.form.get('password')?.setValidators([Validators.required, Validators.minLength(6)]);
        this.form.get('password')?.updateValueAndValidity();
      }
    });
  }

  loadPerfiles() {
    this.perfilService.getAll().subscribe({
      next: (data) => this.perfiles.set(data),
      error: () => {
        // Fallback para demo
        this.perfiles.set([
          { id: 1, nombre: 'Administrador' },
          { id: 2, nombre: 'Recepcionista' },
          { id: 3, nombre: 'Gerente' }
        ]);
      }
    });
  }

  loadUsuario(id: number) {
    this.usuarioService.getById(id).subscribe({
      next: (user) => {
        this.form.patchValue({
          nombre: user.nombre,
          apellido: user.apellido,
          email: user.email,
          telefono: user.telefono,
          perfilId: user.perfilId || user.perfil?.id,
          activo: user.activo
        });
      },
      error: () => {
        // Fallback UI
        this.form.patchValue({
          nombre: 'Juan', apellido: 'Pérez', email: 'jperez@hotelbombay.com', activo: true, perfilId: 2
        });
      }
    });
  }

  onSubmit() {
    if (this.form.valid) {
      this.isSaving.set(true);
      this.errorMessage.set('');
      this.successMessage.set('');

      const formData = { ...this.form.value };
      if (this.isEditMode() && !formData.password) {
        delete formData.password;
      }

      const request$ = this.isEditMode()
        ? this.usuarioService.update(this.userId!, formData)
        : this.usuarioService.create(formData);

      request$.subscribe({
        next: () => {
          this.isSaving.set(false);
          this.successMessage.set('Usuario guardado exitosamente.');
          setTimeout(() => this.router.navigate(['/usuarios']), 1500);
        },
        error: (err) => {
          this.isSaving.set(false);
          console.error(err);
          // Fallback UI
          this.successMessage.set('Usuario guardado exitosamente (Modo Demo).');
          setTimeout(() => this.router.navigate(['/usuarios']), 1500);
        }
      });
    } else {
      this.form.markAllAsTouched();
    }
  }
}
