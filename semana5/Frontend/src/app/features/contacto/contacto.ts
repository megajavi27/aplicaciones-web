import { Component, signal } from '@angular/core';
import { PublicNavbarComponent } from '@shared/layout/public-navbar/public-navbar';
import { PublicFooterComponent } from '@shared/layout/public-footer/public-footer';

@Component({
  selector: 'app-contacto',
  standalone: true,
  imports: [PublicNavbarComponent, PublicFooterComponent],
  templateUrl: './contacto.html'
})
export class ContactoComponent {
  enviado = signal(false);

  onSubmit(event: Event) {
    event.preventDefault();
    this.enviado.set(true);
    (event.target as HTMLFormElement).reset();
  }
}
