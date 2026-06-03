// Helpers para el modal Tailwind (sin Bootstrap)
function mostrarModal() { document.getElementById('modalOverlay').classList.remove('hidden'); }
function cerrarModal()  { document.getElementById('modalOverlay').classList.add('hidden'); }

document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('dataForm').addEventListener('submit', guardar);

    document.getElementById('modalOverlay').addEventListener('click', function(e) {
        if (e.target === this) cerrarModal();
    });

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

async function cargarLista() {
    try {
        const data = await fetchApi('/proveedor');
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
            const tr = document.createElement('tr');
            tr.className = 'hover:bg-slate-50 border-b border-slate-100 transition-colors';
            tr.innerHTML = `
                <td class="px-6 py-4 font-mono text-slate-400 text-xs">${item.cProveedor}</td>
                <td class="px-6 py-4 text-sm text-slate-700 font-medium">${item.razonSocial}</td>
                <td class="px-6 py-4 text-sm text-slate-700">${item.ruc}</td>
                <td class="px-6 py-4 text-sm text-slate-700">${item.telefono || ''}</td>
                <td class="px-6 py-4 text-sm text-slate-700">${item.correo || ''}</td>
                <td class="px-6 py-4 text-sm">
                    <button class="border border-blue-400 text-blue-600 hover:bg-blue-50 px-3 py-1.5 rounded-lg text-xs font-medium mr-2 transition-all"
                            onclick='abrirModalEditar(${JSON.stringify(item)})'>Editar</button>
                    <button class="border border-red-300 text-red-500 hover:bg-red-50 px-3 py-1.5 rounded-lg text-xs font-medium transition-all"
                            onclick="eliminar(${item.cProveedor})">Eliminar</button>
                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (e) {
        showAlert('Error al cargar proveedores', 'danger');
    }
}

function abrirModalCrear() {
    document.getElementById('dataForm').reset();
    document.getElementById('cProveedor').value = '';
    document.getElementById('modalTitle').textContent = 'Nuevo Proveedor';
    mostrarModal();
}

function abrirModalEditar(item) {
    document.getElementById('cProveedor').value = item.cProveedor;
    document.getElementById('RazonSocial').value = item.razonSocial;
    document.getElementById('Ruc').value = item.ruc;
    document.getElementById('Telefono').value = item.telefono || '';
    document.getElementById('Correo').value = item.correo || '';

    document.getElementById('modalTitle').textContent = 'Editar Proveedor';
    mostrarModal();
}

async function guardar(e) {
    e.preventDefault();

    const id = document.getElementById('cProveedor').value;
    const proveedor = {
        razonSocial: document.getElementById('RazonSocial').value,
        ruc: document.getElementById('Ruc').value,
        telefono: document.getElementById('Telefono').value,
        correo: document.getElementById('Correo').value
    };

    try {
        if (id) {
            proveedor.cProveedor = parseInt(id);
            await fetchApi(`/proveedor/${id}`, 'PUT', proveedor);
            showAlert('Proveedor actualizado exitosamente');
        } else {
            await fetchApi('/proveedor', 'POST', proveedor);
            showAlert('Proveedor creado exitosamente');
        }
        cerrarModal();
        cargarLista();
    } catch (error) {
        showAlert('Error al guardar', 'danger');
    }
}

async function eliminar(id) {
    if (confirm('¿Está seguro de eliminar este proveedor?')) {
        try {
            await fetchApi(`/proveedor/${id}`, 'DELETE');
            showAlert('Proveedor eliminado exitosamente');
            cargarLista();
        } catch (error) {
            showAlert('Error al eliminar', 'danger');
        }
    }
}
