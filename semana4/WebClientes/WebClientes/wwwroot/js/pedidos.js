// Helpers para el modal Tailwind (sin Bootstrap)
function mostrarModal() { document.getElementById('modalOverlay').classList.remove('hidden'); }
function cerrarModal()  { document.getElementById('modalOverlay').classList.add('hidden'); }

let detalles = [];
let productosCache = [];

document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('dataForm').addEventListener('submit', guardar);

    document.getElementById('modalOverlay').addEventListener('click', function(e) {
        if (e.target === this) cerrarModal();
    });

    cargarSelects();
    cargarLista();
});

function showAlert(message, type = 'success') {
    const isSuccess = type === 'success';
    const alertHtml = `
        <div class="flex items-center gap-2 px-5 py-3 rounded-xl border ${isSuccess
            ? 'bg-green-50 border-green-200 text-green-800'
            : 'bg-red-50 border-red-200 text-red-800'}">
            <span>${isSuccess ? '✓' : '✕'}</span>
            <span class="flex-1 text-sm font-medium">${message}</span>
            <button onclick="this.parentElement.remove()" class="ml-auto text-lg leading-none opacity-60 hover:opacity-100">✕</button>
        </div>`;
    document.getElementById('alertContainer').innerHTML = alertHtml;
    setTimeout(() => {
        const el = document.getElementById('alertContainer');
        if (el) el.innerHTML = '';
    }, 4000);
}

async function cargarSelects() {
    try {
        const clientes = await fetchApi('/cliente');
        productosCache = await fetchApi('/producto');

        const selectCliente = document.getElementById('ccliente');
        selectCliente.innerHTML = '<option value="">Seleccione Cliente...</option>';
        clientes.forEach(item => {
            selectCliente.innerHTML += `<option value="${item.ccliente}">${item.identificacion} - ${item.nombre} ${item.apellidos}</option>`;
        });

        const selectProducto = document.getElementById('tempProducto');
        selectProducto.innerHTML = '<option value="">Seleccione Producto...</option>';
        productosCache.forEach(item => {
            selectProducto.innerHTML += `<option value="${item.cProducto}" data-precio="${item.precio}">${item.nombre} - $${item.precio}</option>`;
        });
    } catch (e) {
        showAlert('Error al cargar catálogos', 'danger');
    }
}

function estadoBadge(estado) {
    const map = {
        'Pendiente': 'bg-yellow-100 text-yellow-700',
        'Procesado': 'bg-blue-100 text-blue-700',
        'Enviado':   'bg-green-100 text-green-700',
        'Cancelado': 'bg-red-100 text-red-700',
    };
    const cls = map[estado] || 'bg-slate-100 text-slate-600';
    return `<span class="${cls} px-2.5 py-1 rounded-full text-xs font-medium">${estado}</span>`;
}

async function cargarLista() {
    try {
        const data = await fetchApi('/pedido');
        const tbody = document.querySelector('#dataTable tbody');
        tbody.innerHTML = '';

        if (!data || data.length === 0) {
            tbody.innerHTML = `<tr><td colspan="6" class="py-16 text-center">
                <div class="text-5xl mb-3">📭</div>
                <p class="text-slate-300 text-lg font-medium">No hay registros disponibles</p>
                <p class="text-slate-300 text-sm mt-1">Haz clic en Nuevo para agregar</p>
            </td></tr>`;
            return;
        }

        data.forEach(item => {
            const clienteNombre = item.cliente ? `${item.cliente.nombre} ${item.cliente.apellidos}` : '';
            const fecha = new Date(item.fechaPedido).toLocaleDateString();

            const tr = document.createElement('tr');
            tr.className = 'hover:bg-slate-50 border-b border-slate-100 transition-colors';
            tr.innerHTML = `
                <td class="px-6 py-4 font-mono text-slate-400 text-xs">${item.cPedido}</td>
                <td class="px-6 py-4 text-sm text-slate-700 font-medium">${clienteNombre}</td>
                <td class="px-6 py-4 text-sm text-slate-700">${fecha}</td>
                <td class="px-6 py-4 text-sm text-slate-700 font-medium">$${item.total.toFixed(2)}</td>
                <td class="px-6 py-4 text-sm">${estadoBadge(item.estado)}</td>
                <td class="px-6 py-4 text-sm">
                    <button class="border border-blue-400 text-blue-600 hover:bg-blue-50 px-3 py-1.5 rounded-lg text-xs font-medium mr-2 transition-all"
                            onclick="verDetalles(${item.cPedido})">Ver</button>
                    <button class="border border-red-300 text-red-500 hover:bg-red-50 px-3 py-1.5 rounded-lg text-xs font-medium transition-all"
                            onclick="eliminar(${item.cPedido})">Eliminar</button>
                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (e) {
        showAlert('Error al cargar pedidos', 'danger');
    }
}

async function verDetalles(id) {
    try {
        const pedido = await fetchApi(`/pedido/${id}/detalles`);
        if (pedido) {
            abrirModalEditar(pedido);
        }
    } catch (error) {
        showAlert('Error al cargar detalles del pedido', 'danger');
    }
}

function abrirModalCrear() {
    document.getElementById('dataForm').reset();
    document.getElementById('cPedido').value = '';
    document.getElementById('modalTitle').textContent = 'Nuevo Pedido';
    document.getElementById('Estado').value = 'Pendiente';
    detalles = [];
    renderDetalles();
    document.getElementById('btnGuardar').style.display = 'block';
    mostrarModal();
}

function abrirModalEditar(item) {
    document.getElementById('cPedido').value = item.cPedido;
    document.getElementById('ccliente').value = item.ccliente;
    document.getElementById('Estado').value = item.estado;

    detalles = item.detallesPedido ? item.detallesPedido.map(d => ({
        cProducto: d.cProducto,
        nombre: d.producto ? d.producto.nombre : 'Producto ' + d.cProducto,
        cantidad: d.cantidad,
        precioUnitario: d.precioUnitario
    })) : [];

    renderDetalles();
    document.getElementById('modalTitle').textContent = 'Detalles de Pedido';
    document.getElementById('btnGuardar').style.display = 'none';
    mostrarModal();
}

function agregarDetalle() {
    const selectProd = document.getElementById('tempProducto');
    const cantidadInput = document.getElementById('tempCantidad');

    const cProducto = parseInt(selectProd.value);
    const cantidad = parseInt(cantidadInput.value);

    if (!cProducto || isNaN(cantidad) || cantidad <= 0) {
        alert('Seleccione un producto y cantidad válida');
        return;
    }

    const option = selectProd.options[selectProd.selectedIndex];
    const precio = parseFloat(option.getAttribute('data-precio'));
    const nombre = option.text.split(' - ')[0];

    const existente = detalles.find(d => d.cProducto === cProducto);
    if (existente) {
        existente.cantidad += cantidad;
    } else {
        detalles.push({ cProducto, nombre, cantidad, precioUnitario: precio });
    }

    renderDetalles();
    selectProd.value = '';
    cantidadInput.value = '1';
}

function removerDetalle(index) {
    detalles.splice(index, 1);
    renderDetalles();
}

function renderDetalles() {
    const tbody = document.querySelector('#detallesTable tbody');
    tbody.innerHTML = '';

    let total = 0;
    const mostrarBoton = document.getElementById('btnGuardar').style.display !== 'none';

    detalles.forEach((d, index) => {
        const subtotal = d.cantidad * d.precioUnitario;
        total += subtotal;

        tbody.innerHTML += `
            <tr class="border-b border-slate-100">
                <td class="px-3 py-2 text-sm text-slate-700">${d.nombre}</td>
                <td class="px-3 py-2 text-sm text-slate-700">${d.cantidad}</td>
                <td class="px-3 py-2 text-sm text-slate-700">$${d.precioUnitario.toFixed(2)}</td>
                <td class="px-3 py-2 text-sm text-slate-700 font-medium">$${subtotal.toFixed(2)}</td>
                <td class="px-3 py-2 text-sm">
                    ${mostrarBoton
                        ? `<button type="button" class="border border-red-300 text-red-500 hover:bg-red-50 px-2 py-1 rounded text-xs transition-all" onclick="removerDetalle(${index})">✕</button>`
                        : ''}
                </td>
            </tr>
        `;
    });

    document.getElementById('granTotal').textContent = `$${total.toFixed(2)}`;
}

async function guardar(e) {
    e.preventDefault();

    const id = document.getElementById('cPedido').value;
    if (id) {
        cerrarModal();
        return;
    }

    if (detalles.length === 0) {
        alert('Debe agregar al menos un producto al pedido');
        return;
    }

    let total = 0;
    detalles.forEach(d => { total += (d.cantidad * d.precioUnitario); });

    const pedido = {
        ccliente: parseInt(document.getElementById('ccliente').value),
        estado: document.getElementById('Estado').value,
        total: total,
        fechaPedido: new Date().toISOString(),
        detallesPedido: detalles.map(d => ({
            cProducto: d.cProducto,
            cantidad: d.cantidad,
            precioUnitario: d.precioUnitario
        }))
    };

    try {
        await fetchApi('/pedido', 'POST', pedido);
        showAlert('Pedido creado exitosamente');
        cerrarModal();
        cargarLista();
    } catch (error) {
        showAlert('Error al guardar pedido', 'danger');
    }
}

async function eliminar(id) {
    if (confirm('¿Está seguro de eliminar este pedido?')) {
        try {
            await fetchApi(`/pedido/${id}`, 'DELETE');
            showAlert('Pedido eliminado exitosamente');
            cargarLista();
        } catch (error) {
            showAlert('Error al eliminar', 'danger');
        }
    }
}
