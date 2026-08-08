
    document.addEventListener("DOMContentLoaded", function () {

        async function cargarFirmas(solicitudId) {

            const response = await fetch(
                `/Solicitudes/ObtenerFirmas?solicitudId=${solicitudId}`
            );

            const firmas = await response.json();

            const contenedor =
                document.getElementById('contenedorFirmas');

            contenedor.innerHTML = '';

            if (!firmas.length) {
                contenedor.innerHTML =
                    '<div class="text-muted">No existen firmas asignadas.</div>';

                return;
            }

            firmas.forEach(firma => {

                const color = firma.firmada
                    ? 'success'
                    : 'warning';

                const icono = firma.firmada
                    ? 'bi-check-circle-fill'
                    : 'bi-hourglass-split';

                const nombre =
                    firma.firmada
                        ? firma.usuarioFirmante
                        : firma.usuarioRequerido;

                const fecha =
                    firma.firmada
                        ? `<small class="text-muted">${firma.fechaFirma}</small>`
                        : '';

                contenedor.innerHTML += `

            <div class="border rounded-3 p-3 mb-2">

                <span class="badge bg-${color}">
                    <i class="bi ${icono} me-1"></i>
                    ${firma.firmada ? 'Firmada' : 'Pendiente'}
                </span>

                <div class="fw-bold mt-2">
                    ${firma.tipoFirma}
                </div>

                <div class="text-muted">
                    ${nombre}
                </div>

                ${fecha}

            </div>

        `;
            });
        }

    const modalElement = document.getElementById('modalDetalle');
    const bsModal = new bootstrap.Modal(modalElement);

    // ✅ Lightbox elementos (una sola vez)
    const lightbox = document.getElementById('imageLightbox');
    const lightboxImg = document.getElementById('lightboxImg');

    let solicitudIdActual = null;

        document.querySelectorAll('.btn-detalle-solicitud').forEach(card => {
        card.addEventListener('click', function (event) {
            // Medida de seguridad: Evita abrir el modal si el usuario hizo clic en botones internos
            if (event.target.closest('.prevent-modal-trigger')) {
                return;
            }

            // 🔹 Datos Generales
            solicitudIdActual = this.getAttribute('data-id');
            const area = this.getAttribute('data-area');
            const estatus = this.getAttribute('data-estatus');
            const tipo = this.getAttribute('data-tipo');
            const descripcion = this.getAttribute('data-descripcion');
            const razon = this.getAttribute('data-razon');
            const fecha = this.getAttribute('data-fecha');
            const imagen = this.getAttribute('data-imagen');
            const aprobador = this.getAttribute('data-aprobador');
            const fechaRevision = this.getAttribute('data-fecharevision');
            const comentarios = this.getAttribute('data-comentarios');

            // Banderas de validación (Evaluando 'True' proveniente del backend de C#)
            const aplicaResponsable = this.getAttribute('data-aplica-responsable') === 'True';
            const aplicaMandril = this.getAttribute('data-aplica-mandril') === 'True';
            const aplicaPallets = this.getAttribute('data-aplica-pallets') === 'True';
            const aplicaRazon = this.getAttribute('data-aplica-razon') === 'True';

            // Valores textuales
            const responsable = this.getAttribute('data-responsable');
            const mandril = this.getAttribute('data-mandril');
            const pallets = this.getAttribute('data-pallets');
            const razonInv = this.getAttribute('data-razoninv');
            const fechaInicio = this.getAttribute('data-fecha-inicio');
            const fechaFin = this.getAttribute('data-fecha-fin');

            const seccionFirmas =
                document.getElementById('seccionFirmas');

            // ==================================================================
            // 🔹 RENDERIZADO PREMIUM DE INVENTARIO (CON PALOMITAS / N/A)
            // ==================================================================
            const seccionInventario = document.getElementById('seccionInventario');

            // El bloque se muestra si la solicitud está EnProceso o Finalizada (y existen datos técnicos creados o mapeados)
            if (estatus === "EnProceso" || estatus === "Finalizado" || responsable || mandril || pallets || razonInv) {

                // Función auxiliar interna para inyectar las palomitas y formatear los textos correspondientes
                function procesarCeldaTecnica(idBadge, idTexto, aplica, valorReal) {
                    const elBadge = document.getElementById(idBadge);
                    const elTexto = document.getElementById(idTexto);

                    if (aplica) {
                        elBadge.innerHTML = '<span class="badge bg-success-subtle text-success rounded-pill px-2 py-0.5 small shadow-xs"><i class="bi bi-check-lg"></i></span>';
                        elTexto.innerText = (valorReal && valorReal !== "null" && valorReal !== "") ? valorReal : "Sin especificar";
                        elTexto.classList.remove("text-muted", "fst-italic");
                    } else {
                        elBadge.innerHTML = '<span class="badge bg-light text-muted border rounded-pill px-2 py-0.5 small">N/A</span>';
                        elTexto.innerText = "No aplica";
                        elTexto.classList.add("text-muted", "fst-italic");
                    }
                }

                // Procesamos las 6 celdas técnicas de forma simétrica
                procesarCeldaTecnica("badgeResponsable", "modalResponsable", aplicaResponsable, responsable);
                procesarCeldaTecnica("badgeMandril", "modalMandril", aplicaMandril, mandril);
                procesarCeldaTecnica("badgePallets", "modalPallets", aplicaPallets, pallets);
                procesarCeldaTecnica("badgeRazonInv", "modalRazonInv", aplicaRazon, razonInv);

                seccionInventario.classList.remove('d-none');
            } else {
                seccionInventario.classList.add('d-none');
            }

            // 🔹 Inyección del Contenido Base
            document.getElementById('modalTipo').innerText = tipo;
            document.getElementById('modalArea').innerText = area;
            document.getElementById('modalDescripcion').innerText = descripcion;
            document.getElementById('modalRazon').innerText = razon;
            document.getElementById('modalFecha').innerText = fecha;
            document.getElementById('modalFechaInicio').innerText =
                fechaInicio || "N/A";

            document.getElementById('modalFechaFin').innerText =
                fechaFin || "N/A";

            // ==================================================================
            // 🔹 CONTROL MÁQUINA DE ESTADOS (BADGES, PANELES ADMIN Y REVISIÓN)
            // ==================================================================
            const badge = document.getElementById('modalEstatus');
            const seccionRevision = document.getElementById('seccionRevision');

            const bloqueAdmin =
                document.getElementById('bloqueAccionesAdmin');

            const bloquePendiente =
                document.getElementById('bloquePendienteAprobacion');

            const formRevision =
                document.getElementById('seccionFormularioRevision');


            // ============================================
            // NOMBRE AMIGABLE DEL ESTATUS
            // ============================================
            const nombresEstatus = {
                "Pendiente": "Pendiente de Aprobación",
                "EnProceso": "En Proceso",
                "PendienteFirmas": "Pendiente de Firmas",
                "Finalizado": "Finalizada",
                "Rechazado": "Rechazada"
            };

            badge.innerText = nombresEstatus[estatus] || estatus;

            badge.className =
                "badge fs-6 px-3 py-1.5 rounded-pill fw-semibold shadow-xs text-nowrap d-inline-block";


            // ============================================
            // LIMPIEZA GENERAL
            // ============================================

            if (bloqueAdmin) {
                bloqueAdmin.classList.add("d-none");
            }

            if (bloquePendiente) {
                bloquePendiente.classList.add("d-none");
            }

            formRevision.classList.add("d-none");
            seccionFirmas.classList.add("d-none");

            // ============================================
            // PENDIENTE
            // ============================================

            if (estatus === "Pendiente") {

                badge.classList.add("bg-warning", "text-dark");

                if (bloqueAdmin) {
                    bloqueAdmin.classList.remove("d-none");
                }

                if (bloquePendiente) {
                    bloquePendiente.classList.remove("d-none");
                }

                formRevision.classList.remove("d-none");

                document.getElementById('txtComentariosRevision').value = '';

                seccionRevision.classList.add("d-none");
            }


            // ============================================
            // EN PROCESO
            // ============================================

            else if (estatus === "EnProceso") {

                badge.classList.add("bg-info", "text-dark");

                document.getElementById('modalAprobador').innerText = aprobador;
                document.getElementById('modalFechaRevision').innerText = fechaRevision;
                document.getElementById('modalComentarios').innerText = comentarios;

                seccionRevision.classList.remove("d-none");
            }

            // ============================================
            // PENDIENTE FIRMAS
            // ============================================

            else if (estatus === "PendienteFirmas") {

                badge.classList.add("bg-primary", "text-white");

                document.getElementById('modalAprobador').innerText = aprobador;
                document.getElementById('modalFechaRevision').innerText = fechaRevision;
                document.getElementById('modalComentarios').innerText = comentarios;

                seccionRevision.classList.remove("d-none");

                // NUEVO
                seccionFirmas.classList.remove("d-none");

                cargarFirmas(solicitudIdActual);
            }


            // ============================================
            // FINALIZADO
            // ============================================

            else if (estatus === "Finalizado") {

                badge.classList.add("bg-success", "text-white");

                document.getElementById('modalAprobador').innerText = aprobador;
                document.getElementById('modalFechaRevision').innerText = fechaRevision;
                document.getElementById('modalComentarios').innerText = comentarios;

                seccionRevision.classList.remove("d-none");

                // NUEVO
                seccionFirmas.classList.remove("d-none");

                cargarFirmas(solicitudIdActual);
            }


            // ============================================
            // RECHAZADO
            // ============================================

            else if (estatus === "Rechazado") {

                badge.classList.add("bg-danger", "text-white");

                document.getElementById('modalAprobador').innerText = aprobador;
                document.getElementById('modalFechaRevision').innerText = fechaRevision;
                document.getElementById('modalComentarios').innerText = comentarios;

                seccionRevision.classList.remove("d-none");
            }

            // ==================================================================
            // 🔹 GESTIÓN DEL CROQUIS / LAYOUT ADJUNTO
            // ==================================================================
            const seccionCroquis = document.getElementById('seccionCroquis');
            const modalImagen = document.getElementById('modalImagen');

            if (imagen && imagen.trim() !== "") {
                modalImagen.src = imagen;

                // Transfiere la imagen seleccionada al Lightbox global
                modalImagen.onclick = function (e) {
                    e.stopPropagation();
                    lightbox.style.display = "flex";
                    lightboxImg.src = this.src;
                };

                seccionCroquis.classList.remove('d-none');
            } else {
                seccionCroquis.classList.add('d-none');
            }

            // 10. Despliegue en pantalla del modal
            bsModal.show();
        });
        });


    // ✅ cerrar lightbox
    lightbox.onclick = function () {
        lightbox.style.display = "none";
            };

    // 🔹 enviar dictamen
    async function enviarDictamen(nuevoEstatus) {

        const comentarios = document.getElementById('txtComentariosRevision').value;

        let estatusId = 0;

        if (nuevoEstatus === 'Aprobado')
            estatusId = 1; // EnProceso

        if (nuevoEstatus === 'Rechazado')
            estatusId = 4; // Rechazado
    

    const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

    const formData = new FormData();
    formData.append("id", solicitudIdActual);
    formData.append("nuevoEstatus", estatusId);
    formData.append("comentarios", comentarios);

    if (token) {
        formData.append("__RequestVerificationToken", token);
                }

    try {
                    const response = await fetch('/Solicitudes/Evaluar', {
        method: 'POST',
        body: formData
                        });

    if (!response.ok) {
                        throw new Error("Error servidor");
                    }

    const result = await response.json();

    if (result.success) {

                        if (result.redirectUrl) {
        window.location.href = result.redirectUrl;
    return;
                        }

    bsModal.hide();
    window.location.reload();
                    }else {
        alert(result.message);
                    }
                } catch (err) {
        alert("Error procesando solicitud");
    console.error(err);
                }
            }

    const btnAprobar = document.getElementById('btnAprobar');
    const btnRechazar = document.getElementById('btnRechazar');

    if (btnAprobar) {
        btnAprobar.addEventListener('click', () =>
            enviarDictamen('Aprobado'));
        }

    if (btnRechazar) {
        btnRechazar.addEventListener('click', () =>
            enviarDictamen('Rechazado'));
        }

        });
