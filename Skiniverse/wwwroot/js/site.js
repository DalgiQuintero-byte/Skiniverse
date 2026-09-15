// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let carrito = JSON.parse(localStorage.getItem('carrito')) || [];

function agregarAlCarrito(id, nombre, precio) {
    const existe = carrito.find(p => p.id === id);
    if (existe) {
        existe.cantidad++;
    } else {
        carrito.push({ id, nombre, precio, cantidad: 1 });
    }
    guardarYActualizar();
}

function guardarYActualizar() {
    localStorage.setItem('carrito', JSON.stringify(carrito));
    renderizarCarrito();
}

function renderizarCarrito() {
    const lista = document.getElementById('lista-carrito');
    const totalEl = document.getElementById('total-carrito');
    const countEl = document.getElementById('cart-count');

    if (!lista) return;

    lista.innerHTML = '';
    let total = 0;
    let itemsTotales = 0;

    carrito.forEach(prod => {
        total += prod.precio * prod.cantidad;
        itemsTotales += prod.cantidad;
        lista.innerHTML += `
            <li class="list-group-item d-flex justify-content-between align-items-center">
                <div>
                    <h6>${prod.nombre}</h6>
                    <small>$${prod.precio.toLocaleString()} x ${prod.cantidad}</small>
                </div>
                <button class="btn btn-sm btn-outline-danger" onclick="eliminarDelCarrito(${prod.id})">✕</button>
            </li>
        `;
    });

    totalEl.innerText = `$${total.toLocaleString()}`;
    countEl.innerText = itemsTotales;
}

function eliminarDelCarrito(id) {
    carrito = carrito.filter(p => p.id !== id);
    guardarYActualizar();
}

document.addEventListener('DOMContentLoaded', renderizarCarrito);