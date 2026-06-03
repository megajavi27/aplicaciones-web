// Helpers para el modal Tailwind (sin Bootstrap)
function mostrarModal() { document.getElementById('modalOverlay').classList.remove('hidden'); }
function cerrarModal()  { document.getElementById('modalOverlay').classList.add('hidden'); }

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
        const categorias = await fetchApi('/categoria');
        const proveedores = await fetchApi('/proveedor');

        llenarSelect('cCategoria', categorias, 'cCategoria', 'nombre');
        llenarSelect('cProveedor', proveedores, 'cProveedor', 'razonSocial');
    } catch (e) {
        showAlert('Error al cargar catálogos de producto', 'danger');
    }
}

function llenarSelect(selectId, data, valueField, textField) {
    const select = document.getElementById(selectId);
    select.innerHTML = '<option value="">Seleccione...</option>';
    data.forEach(item => {
        select.innerHTML += `<option value="${item[valueField]}">${item[textField]}</option>`;
    });
}

async function cargarLista() {
    try {
        const data = await fetchApi('/producto');
        const tbody = document.querySelector('#dataTable tbody');
        tbody.innerHTML = '';

        if (!data || data.length === 0) {
            tbody.innerHTML = `<tr><td colspan="7" class="py-16 text-center">
                <div class="text-5xl mb-3">📭</div>
                <p class="text-slate-300 text-lg font-medium">No hay registros disponibles</p>
                <p class="text-slate-300 text-sm mt-1">Haz clic en Nuevo para agregar</p>
            </td></tr>`;
            return;
        }

        data.forEach(item => {
            const categoriaNombre = item.categoria ? item.categoria.nombre : '';
            const proveedorNombre = item.proveedor ? item.proveedor.razonSocial : '';

            // Badge de stock
            let stockBadge;
            if (item.stock === 0) {
                stockBadge = `<span class="bg-red-100 text-red-700 px-2.5 py-1 rounded-full text-xs font-medium">${item.stock}</span>`;
            } else if (item.stock <= 5) {
                stockBadge = `<span class="bg-yellow-100 text-yellow-700 px-2.5 py-1 rounded-full text-xs font-medium">${item.stock}</span>`;
            } else {
                stockBadge = `<span class="bg-green-100 text-green-700 px-2.5 py-1 rounded-full text-xs font-medium">${item.stock}</span>`;
            }

            const tr = document.createElement('tr');
            tr.className = 'hover:bg-slate-50 border-b border-slate-100 transition-colors';
            tr.innerHTML = `
                <td class="px-6 py-4 font-mono text-slate-400 text-xs">${item.cProducto}</td>
                <td class="px-6 py-4 text-sm text-slate-700 font-medium">${item.nombre}</td>
                <td class="px-6 py-4 text-sm text-slate-700">$${item.precio.toFixed(2)}</td>
                <td class="px-6 py-4 text-sm">${stockBadge}</td>
                <td class="px-6 py-4 text-sm text-slate-700">${categoriaNombre}</td>
                <td class="px-6 py-4 text-sm text-slate-700">${proveedorNombre}</td>
                <td class="px-6 py-4 text-sm">
                    <button class="border border-blue-400 text-blue-600 hover:bg-blue-50 px-3 py-1.5 rounded-lg text-xs font-medium mr-2 transition-all"
                            onclick='abrirModalEditar(${JSON.stringify(item)})'>Editar</button>
                    <button class="border border-red-300 text-red-500 hover:bg-red-50 px-3 py-1.5 rounded-lg text-xs font-medium transition-all"
                            onclick="eliminar(${item.cProducto})">Eliminar</button>
                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (e) {
        showAlert('Error al cargar productos', 'danger');
    }
}

function abrirModalCrear() {
    document.getElementById('dataForm').reset();
    document.getElementById('cProducto').value = '';
    document.getElementById('modalTitle').textContent = 'Nuevo Producto';
    mostrarModal();
}

function abrirModalEditar(item) {
    document.getElementById('cProducto').value = item.cProducto;
    document.getElementById('Nombre').value = item.nombre;
    document.getElementById('Precio').value = item.precio;
    document.getElementById('Stock').value = item.stock;
    document.getElementById('cCategoria').value = item.cCategoria;
    document.getElementById('cProveedor').value = item.cProveedor || '';

    document.getElementById('modalTitle').textContent = 'Editar Producto';
    mostrarModal();
}

async function guardar(e) {
    e.preventDefault();

    const id = document.getElementById('cProducto').value;
    const producto = {
        nombre: document.getElementById('Nombre').value,
        precio: parseFloat(document.getElementById('Precio').value),
        stock: parseInt(document.getElementById('Stock').value),
        cCategoria: parseInt(document.getElementById('cCategoria').value),
        cProveedor: document.getElementById('cProveedor').value ? parseInt(document.getElementById('cProveedor').value) : null
    };

    try {
        if (id) {
            producto.cProducto = parseInt(id);
            await fetchApi(`/producto/${id}`, 'PUT', producto);
            showAlert('Producto actualizado exitosamente');
        } else {
            await fetchApi('/producto', 'POST', producto);
            showAlert('Producto creado exitosamente');
        }
        cerrarModal();
        cargarLista();
    } catch (error) {
        showAlert('Error al guardar', 'danger');
    }
}

async function eliminar(id) {
    if (confirm('¿Está seguro de eliminar este producto?')) {
        try {
            await fetchApi(`/producto/${id}`, 'DELETE');
            showAlert('Producto eliminado exitosamente');
            cargarLista();
        } catch (error) {
            showAlert('Error al eliminar', 'danger');
        }
    }
}
