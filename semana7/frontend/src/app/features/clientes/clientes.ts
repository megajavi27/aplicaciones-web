import { Component, inject, OnInit } from '@angular/core';
import { ClienteServices } from '../../core/services/cliente.services';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { jsPDF } from 'jspdf';

@Component({
  selector: 'app-clientes',
  imports: [CommonModule],
  templateUrl: './clientes.html',
  styleUrl: './clientes.css',
})
export class Clientes implements OnInit {
  listaClientes: any[] = [];

  constructor(private readonly clienteServicio: ClienteServices) {}

  private readonly rutas = inject(Router);

  ngOnInit(): void {
    this.cargaLista();
  }

  async cargaLista() {
    this.clienteServicio.todos().subscribe({
      next: (lista: any[]) => {
        this.listaClientes = lista;
      },
      error: (errores) => {
        console.log(errores);
      },
    });
  }

  generarFactura(id: number) {
    this.clienteServicio.factura(id).subscribe({
      next: (data: any) => {
        const doc = new jsPDF();

        const azul    = '#1a3c6e';
        const gris    = '#f5f5f5';
        const negro   = '#222222';
        const lineGris = '#cccccc';

        doc.setFillColor(azul);
        doc.rect(0, 0, 210, 35, 'F');

        doc.setTextColor('#ffffff');
        doc.setFontSize(22);
        doc.setFont('helvetica', 'bold');
        doc.text('MI EMPRESA S.A.', 14, 16);

        doc.setFontSize(9);
        doc.setFont('helvetica', 'normal');
        doc.text('RUC: 1790000000001', 14, 23);
        doc.text('Quito, Ecuador | info@miempresa.com', 14, 29);

        doc.setTextColor('#ffffff');
        doc.setFontSize(18);
        doc.setFont('helvetica', 'bold');
        doc.text('FACTURA', 196, 16, { align: 'right' });
        doc.setFontSize(10);
        doc.setFont('helvetica', 'normal');
        doc.text(`N° ${data.numero}`, 196, 23, { align: 'right' });
        doc.text(`Fecha: ${data.fecha}`, 196, 29, { align: 'right' });

        doc.setFillColor(gris);
        doc.rect(14, 42, 182, 30, 'F');
        doc.setDrawColor(lineGris);
        doc.rect(14, 42, 182, 30);

        doc.setTextColor(azul);
        doc.setFontSize(10);
        doc.setFont('helvetica', 'bold');
        doc.text('DATOS DEL CLIENTE', 18, 50);

        doc.setTextColor(negro);
        doc.setFont('helvetica', 'normal');
        doc.setFontSize(9);
        doc.text(`Cédula:    ${data.cliente.cedula}`,   18, 57);
        doc.text(`Nombre:    ${data.cliente.nombres}`,  18, 63);
        doc.text(`Dirección: ${data.cliente.direccion}`, 110, 57);
        doc.text(`Teléfono:  ${data.cliente.telefono}`,  110, 63);

        const tablaY = 82;
        const cols   = [14, 90, 130, 160, 196];

        doc.setFillColor(azul);
        doc.rect(14, tablaY, 182, 9, 'F');
        doc.setTextColor('#ffffff');
        doc.setFont('helvetica', 'bold');
        doc.setFontSize(9);
        doc.text('DESCRIPCIÓN',          cols[0] + 2, tablaY + 6.2);
        doc.text('CANT.',                cols[1] + 2, tablaY + 6.2);
        doc.text('P. UNITARIO',          cols[2] + 2, tablaY + 6.2);
        doc.text('TOTAL',                cols[3] + 2, tablaY + 6.2);

        doc.setTextColor(negro);
        doc.setFont('helvetica', 'normal');

        let filaY = tablaY + 9;
        data.items.forEach((item: any, i: number) => {
          if (i % 2 === 0) {
            doc.setFillColor('#eaf0fb');
            doc.rect(14, filaY, 182, 9, 'F');
          }
          doc.setDrawColor(lineGris);
          doc.rect(14, filaY, 182, 9);

          const itemTotal = item.cantidad * item.precio;
          doc.text(item.descripcion,                cols[0] + 2, filaY + 6.2);
          doc.text(String(item.cantidad),           cols[1] + 2, filaY + 6.2);
          doc.text(`$${item.precio.toFixed(2)}`,    cols[2] + 2, filaY + 6.2);
          doc.text(`$${itemTotal.toFixed(2)}`,      cols[3] + 2, filaY + 6.2);
          filaY += 9;
        });

        const totalesX = 130;
        let totY = filaY + 8;

        doc.setDrawColor(lineGris);
        doc.setFont('helvetica', 'normal');
        doc.setFontSize(9);

        doc.text('Subtotal:',  totalesX,       totY);
        doc.text(`$${data.subtotal.toFixed(2)}`, 194, totY, { align: 'right' });
        totY += 7;

        doc.text('IVA (15%):', totalesX,       totY);
        doc.text(`$${data.iva.toFixed(2)}`,    194, totY, { align: 'right' });
        totY += 2;

        doc.setDrawColor(azul);
        doc.setLineWidth(0.5);
        doc.line(totalesX, totY, 196, totY);
        totY += 6;

        doc.setFont('helvetica', 'bold');
        doc.setFontSize(10);
        doc.setTextColor(azul);
        doc.text('TOTAL:',              totalesX, totY);
        doc.text(`$${data.total.toFixed(2)}`, 194, totY, { align: 'right' });

        doc.setFillColor(azul);
        doc.rect(0, 282, 210, 15, 'F');
        doc.setTextColor('#ffffff');
        doc.setFont('helvetica', 'italic');
        doc.setFontSize(8);
        doc.text('Gracias por su preferencia — MI EMPRESA S.A.', 105, 291, { align: 'center' });

        doc.save(`factura-${data.numero}.pdf`);
      },
      error: (err) => {
        console.log(err);
      },
    });
  }
}
