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
        const data = await fetchApi('/categoria');
        const tbody = document.querySelector('#dataTable tbody');
        tbody.innerHTML = '';

        if (!data || data.length === 0) {
            tbody.innerHTML = `<tr><td colspan="5" class="py-16 text-center">
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
                <td class="px-6 py-4 font-mono text-slate-400 text-xs">${item.cCategoria}</td>
                <td class="px-6 py-4 text-sm text-slate-700 font-medium">${item.nombre}</td>
                <td class="px-6 py-4 text-sm text-slate-700">${item.descripcion || ''}</td>
                <td class="px-6 py-4 text-sm">
                    ${item.estado
                        ? '<span class="bg-green-100 text-green-700 px-2.5 py-1 rounded-full text-xs font-medium">Activo</span>'
                        : '<span class="bg-slate-100 text-slate-500 px-2.5 py-1 rounded-full text-xs font-medium">Inactivo</span>'}
                </td>
                <td class="px-6 py-4 text-sm">
                    <button class="border border-blue-400 text-blue-600 hover:bg-blue-50 px-3 py-1.5 rounded-lg text-xs font-medium mr-2 transition-all"
                            onclick='abrirModalEditar(${JSON.stringify(item)})'>Editar</button>
                    <button class="border border-red-300 text-red-500 hover:bg-red-50 px-3 py-1.5 rounded-lg text-xs font-medium transition-all"
                            onclick="eliminar(${item.cCategoria})">Eliminar</button>
                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (e) {
        showAlert('Error al cargar categorías', 'danger');
    }
}

function abrirModalCrear() {
    document.getElementById('dataForm').reset();
    document.getElementById('cCategoria').value = '';
    document.getElementById('modalTitle').textContent = 'Nueva Categoría';
    document.getElementById('Estado').value = 'true';
    mostrarModal();
}

function abrirModalEditar(item) {
    document.getElementById('cCategoria').value = item.cCategoria;
    document.getElementById('Nombre').value = item.nombre;
    document.getElementById('Descripcion').value = item.descripcion || '';
    document.getElementById('Estado').value = item.estado.toString();

    document.getElementById('modalTitle').textContent = 'Editar Categoría';
    mostrarModal();
}

async function guardar(e) {
    e.preventDefault();

    const id = document.getElementById('cCategoria').value;
    const categoria = {
        nombre: document.getElementById('Nombre').value,
        descripcion: document.getElementById('Descripcion').value,
        estado: document.getElementById('Estado').value === 'true',
        fechaCreacion: new Date().toISOString()
    };

    try {
        if (id) {
            categoria.cCategoria = parseInt(id);
            await fetchApi(`/categoria/${id}`, 'PUT', categoria);
            showAlert('Categoría actualizada exitosamente');
        } else {
            await fetchApi('/categoria', 'POST', categoria);
            showAlert('Categoría creada exitosamente');
        }
        cerrarModal();
        cargarLista();
    } catch (error) {
        showAlert('Error al guardar', 'danger');
    }
}

async function eliminar(id) {
    if (confirm('¿Está seguro de eliminar esta categoría?')) {
        try {
            await fetchApi(`/categoria/${id}`, 'DELETE');
            showAlert('Categoría eliminada exitosamente');
            cargarLista();
        } catch (error) {
            showAlert('Error al eliminar', 'danger');
        }
    }
}
