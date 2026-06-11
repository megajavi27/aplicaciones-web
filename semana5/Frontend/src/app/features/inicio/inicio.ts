import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { PublicNavbarComponent } from '@shared/layout/public-navbar/public-navbar';
import { PublicFooterComponent } from '@shared/layout/public-footer/public-footer';

@Component({
  selector: 'app-inicio',
  standalone: true,
  imports: [RouterLink, PublicNavbarComponent, PublicFooterComponent],
  templateUrl: './inicio.html'
})
export class InicioComponent {
  rooms = [
    {
      nombre: 'Suite Vista al Mar',
      metros: 45,
      cama: '1 Cama King',
      precio: '$320',
      destacado: true,
      imagen: 'https://images.unsplash.com/photo-1591088398332-8a7791972843?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80'
    },
    {
      nombre: 'Habitación Deluxe',
      metros: 32,
      cama: '1 Cama Queen',
      precio: '$210',
      destacado: false,
      imagen: 'https://images.unsplash.com/photo-1611892440504-42a792e24d32?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80'
    },
    {
      nombre: 'Villa con Piscina Privada',
      metros: 70,
      cama: '2 Camas King',
      precio: '$540',
      destacado: false,
      imagen: 'https://images.unsplash.com/photo-1602002418816-5c0aeef426aa?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80'
    }
  ];

  experiencias = [
    {
      icono: 'spa',
      titulo: 'Spa & Bienestar',
      descripcion: 'Tratamientos exclusivos diseñados para restaurar el equilibrio entre cuerpo y mente.'
    },
    {
      icono: 'restaurant',
      titulo: 'Gastronomía Local',
      descripcion: 'Sabores frescos de la región preparados por nuestro equipo de chefs galardonados.'
    },
    {
      icono: 'pool',
      titulo: 'Piscinas Infinitas',
      descripcion: 'Espacios serenos con vistas panorámicas, perfectos para relajarse durante todo el día.'
    }
  ];
}
