import { Component } from '@angular/core';
import { PublicNavbarComponent } from '@shared/layout/public-navbar/public-navbar';
import { PublicFooterComponent } from '@shared/layout/public-footer/public-footer';

@Component({
  selector: 'app-experiencias',
  standalone: true,
  imports: [PublicNavbarComponent, PublicFooterComponent],
  templateUrl: './experiencias.html'
})
export class ExperienciasComponent {
  experiencias = [
    {
      icono: 'spa',
      titulo: 'Spa & Bienestar',
      descripcion: 'Tratamientos exclusivos con terapeutas certificados, diseñados para restaurar el equilibrio entre cuerpo y mente en un entorno de total tranquilidad.'
    },
    {
      icono: 'restaurant',
      titulo: 'Gastronomía Local',
      descripcion: 'Sabores frescos de la región preparados por nuestro equipo de chefs galardonados, con menús de temporada e ingredientes locales.'
    },
    {
      icono: 'pool',
      titulo: 'Piscinas Infinitas',
      descripcion: 'Espacios serenos con vistas panorámicas, perfectos para relajarse durante todo el día junto al servicio de bar de piscina.'
    },
    {
      icono: 'fitness_center',
      titulo: 'Centro de Fitness',
      descripcion: 'Gimnasio totalmente equipado con clases guiadas de yoga, pilates y entrenamiento funcional al amanecer.'
    },
    {
      icono: 'sailing',
      titulo: 'Deportes Acuáticos',
      descripcion: 'Kayak, paddle board y snorkel disponibles para explorar la costa con instructores experimentados.'
    },
    {
      icono: 'self_improvement',
      titulo: 'Yoga al Amanecer',
      descripcion: 'Sesiones guiadas frente al mar para comenzar el día con calma, equilibrio y energía renovada.'
    },
    {
      icono: 'local_bar',
      titulo: 'Bar y Coctelería',
      descripcion: 'Cócteles de autor elaborados con ingredientes locales, en un ambiente relajado con vistas al atardecer.'
    },
    {
      icono: 'directions_walk',
      titulo: 'Tours Guiados',
      descripcion: 'Excursiones a sitios cercanos de interés natural y cultural, organizadas por nuestro equipo de concierge.'
    },
    {
      icono: 'child_friendly',
      titulo: 'Actividades Familiares',
      descripcion: 'Programación especial para los más pequeños y espacios diseñados para disfrutar en familia.'
    }
  ];
}
