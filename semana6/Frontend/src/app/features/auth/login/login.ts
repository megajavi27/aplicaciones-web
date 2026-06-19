import { Component, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '@services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.html'
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  form: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  isLoading = signal(false);
  errorMessage = signal('');

  onSubmit() {
    if (this.form.valid) {
      this.isLoading.set(true);
      this.errorMessage.set('');

      const { email, password } = this.form.value;

      this.authService.login(email, password).subscribe({
        next: () => {
          this.isLoading.set(false);
          this.router.navigate(['/']);
        },
        error: (err) => {
          this.isLoading.set(false);
          const msg = err?.error?.message ?? 'Credenciales inválidas o error de conexión.';
          this.errorMessage.set(msg);
          console.error(err);
        }
      });
    } else {
      this.form.markAllAsTouched();
    }
  }
}
