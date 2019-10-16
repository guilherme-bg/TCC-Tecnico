// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function confimarDeletar(uniqueId, foiConfirmado) {
    var deletar = 'deletar_' + uniqueId;
    var confirmarDeletar = 'confirmarDeletar_' + uniqueId;

    if (foiConfirmado) {
        $('#' + deletar).hide();
        $('#' + confirmarDeletar).show();
    } else {
        $('#' + deletar).show();
        $('#' + confirmarDeletar).hide();
    }

}