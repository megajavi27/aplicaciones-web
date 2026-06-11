import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { PublicNavbarComponent } from '@shared/layout/public-navbar/public-navbar';
import { PublicFooterComponent } from '@shared/layout/public-footer/public-footer';

@Component({
  selector: 'app-habitaciones',
  standalone: true,
  imports: [RouterLink, PublicNavbarComponent, PublicFooterComponent],
  templateUrl: './habitaciones.html'
})
export class HabitacionesComponent {
  rooms = [
    {
      nombre: 'Suite Vista al Mar',
      metros: 45,
      cama: '1 Cama King',
      huespedes: 2,
      precio: '$320',
      destacado: true,
      descripcion: 'Amplios ventanales con vista panorámica al océano, terraza privada y baño de mármol con tina independiente.',
      imagen: 'https://images.unsplash.com/photo-1591088398332-8a7791972843?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80'
    },
    {
      nombre: 'Habitación Deluxe',
      metros: 32,
      cama: '1 Cama Queen',
      huespedes: 2,
      precio: '$210',
      destacado: false,
      descripcion: 'Espacio luminoso con diseño contemporáneo, escritorio de trabajo y ducha de lluvia.',
      imagen: 'https://images.unsplash.com/photo-1611892440504-42a792e24d32?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80'
    },
    {
      nombre: 'Villa con Piscina Privada',
      metros: 70,
      cama: '2 Camas King',
      huespedes: 4,
      precio: '$540',
      destacado: false,
      descripcion: 'Villa independiente con piscina privada, sala de estar y jardín exclusivo, ideal para familias.',
      imagen: 'https://images.unsplash.com/photo-1602002418816-5c0aeef426aa?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80'
    },
    {
      nombre: 'Habitación Estándar',
      metros: 26,
      cama: '1 Cama Queen',
      huespedes: 2,
      precio: '$165',
      destacado: false,
      descripcion: 'Confort esencial con todas las comodidades necesarias para una estancia placentera.',
      imagen: 'https://images.unsplash.com/photo-1505693416388-ac5ce068fe85?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80'
    },
    {
      nombre: 'Suite Junior',
      metros: 38,
      cama: '1 Cama King',
      huespedes: 3,
      precio: '$255',
      destacado: false,
      descripcion: 'Sala de estar separada, balcón privado y vistas a los jardines del resort.',
      imagen: 'https://images.unsplash.com/photo-1611892440504-42a792e24d32?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80'
    },
    {
      nombre: 'Penthouse Presidencial',
      metros: 110,
      cama: '3 Camas King',
      huespedes: 6,
      precio: '$890',
      destacado: false,
      descripcion: 'El máximo lujo: terraza panorámica, jacuzzi privado, mayordomo personal y vistas de 360°.',
      imagen: 'https://images.unsplash.com/photo-1564501049412-61c2a3083791?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80'
    }
  ];
}
