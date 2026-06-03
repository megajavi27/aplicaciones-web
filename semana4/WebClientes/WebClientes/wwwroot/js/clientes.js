// Helpers para el modal Tailwind (sin Bootstrap)
function mostrarModal() { document.getElementById('modalOverlay').classList.remove('hidden'); }
function cerrarModal()  { document.getElementById('modalOverlay').classList.add('hidden'); }

document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('clienteForm').addEventListener('submit', guardar);

    // Cerrar modal al hacer click en el overlay (fuera del card)
    document.getElementById('modalOverlay').addEventListener('click', function(e) {
        if (e.target === this) cerrarModal();
    });

    cargarCatalogos();
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

async function cargarCatalogos() {
    try {
        const catalogos = await fetchApi('/cliente/catalogos');
        if (catalogos) {
            llenarSelect('ctipoidentificacion', catalogos.tiposIdentificacion, 'ctipoidentificacion', 'descripcion');
            llenarSelect('cgenero', catalogos.generos, 'cgenero', 'descripcion');
            llenarSelect('cestadocivil', catalogos.estadosCiviles, 'cestadocivil', 'descripcion');
        }
    } catch (e) {
        showAlert('Error al cargar catálogos', 'danger');
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
        const data = await fetchApi('/cliente');
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
            const tr = document.createElement('tr');
            tr.className = 'hover:bg-slate-50 border-b border-slate-100 transition-colors';
            tr.innerHTML = `
                <td class="px-6 py-4 font-mono text-slate-400 text-xs">${item.ccliente}</td>
                <td class="px-6 py-4 text-sm text-slate-700 font-medium">${item.nombre}</td>
                <td class="px-6 py-4 text-sm text-slate-700">${item.apellidos}</td>
                <td class="px-6 py-4 text-sm text-slate-700">${item.identificacion}</td>
                <td class="px-6 py-4 text-sm text-slate-700">${item.telefono || ''}</td>
                <td class="px-6 py-4 text-sm text-slate-700">${item.correo || ''}</td>
                <td class="px-6 py-4 text-sm">
                    <button class="border border-blue-400 text-blue-600 hover:bg-blue-50 px-3 py-1.5 rounded-lg text-xs font-medium mr-2 transition-all"
                            onclick='abrirModalEditar(${JSON.stringify(item)})'>Editar</button>
                    <button class="border border-red-300 text-red-500 hover:bg-red-50 px-3 py-1.5 rounded-lg text-xs font-medium transition-all"
                            onclick="eliminar(${item.ccliente})">Eliminar</button>
                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (e) {
        showAlert('Error al cargar clientes', 'danger');
    }
}

function abrirModalCrear() {
    document.getElementById('clienteForm').reset();
    document.getElementById('ccliente').value = '';
    document.getElementById('modalTitle').textContent = 'Nuevo Cliente';
    mostrarModal();
}

function abrirModalEditar(item) {
    document.getElementById('ccliente').value = item.ccliente;
    document.getElementById('Nombre').value = item.nombre;
    document.getElementById('Apellidos').value = item.apellidos;
    document.getElementById('Identificacion').value = item.identificacion;
    document.getElementById('ctipoidentificacion').value = item.ctipoidentificacion;
    document.getElementById('cgenero').value = item.cgenero;
    document.getElementById('cestadocivil').value = item.cestadocivil;

    const fecha = new Date(item.fechaNacimiento);
    document.getElementById('FechaNacimiento').value = fecha.toISOString().split('T')[0];

    document.getElementById('Direccion').value = item.direccion || '';
    document.getElementById('Telefono').value = item.telefono || '';
    document.getElementById('Correo').value = item.correo || '';

    document.getElementById('modalTitle').textContent = 'Editar Cliente';
    mostrarModal();
}

async function guardar(e) {
    e.preventDefault();

    const id = document.getElementById('ccliente').value;
    const cliente = {
        nombre: document.getElementById('Nombre').value,
        apellidos: document.getElementById('Apellidos').value,
        identificacion: document.getElementById('Identificacion').value,
        ctipoidentificacion: document.getElementById('ctipoidentificacion').value,
        cgenero: document.getElementById('cgenero').value,
        cestadocivil: document.getElementById('cestadocivil').value,
        fechaNacimiento: document.getElementById('FechaNacimiento').value,
        direccion: document.getElementById('Direccion').value,
        telefono: document.getElementById('Telefono').value,
        correo: document.getElementById('Correo').value
    };

    try {
        if (id) {
            cliente.ccliente = parseInt(id);
            await fetchApi(`/cliente/${id}`, 'PUT', cliente);
            showAlert('Cliente actualizado exitosamente');
        } else {
            await fetchApi('/cliente', 'POST', cliente);
            showAlert('Cliente creado exitosamente');
        }
        cerrarModal();
        cargarLista();
    } catch (error) {
        showAlert('Error al guardar', 'danger');
    }
}

async function eliminar(id) {
    if (confirm('¿Está seguro de eliminar este cliente?')) {
        try {
            await fetchApi(`/cliente/${id}`, 'DELETE');
            showAlert('Cliente eliminado exitosamente');
            cargarLista();
        } catch (error) {
            showAlert('Error al eliminar', 'danger');
        }
    }
}
