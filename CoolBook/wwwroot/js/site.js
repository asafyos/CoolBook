let canvas, context, canvaso, contexto;
canvaso = $('#canvas')[0];
context = canvaso.getContext('2d');
context.lineWidth = 2;

context.fillStyle = 'rgba(200, 0, 102, 0.5)';
context.fillRect(10, 10, 50, 50);

context.fillStyle = 'rgba(0, 0, 102, 0.5)';
context.fillRect(40, 40, 25, 25);

context.beginPath();
context.lineWidth = 5;
context.strokeStyle = '#330033'
context.arc(40, 35, 15, Math.PI * 0.4, Math.PI * 1.6, false);
context.stroke();

let tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
let tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl)
})