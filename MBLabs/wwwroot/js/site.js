$(document).ready(function () {
    var $seuCampoCpf = $("#Input_CPF");
    $seuCampoCpf.mask('000.000.000-00', { reverse: true });

    var $seuCampoPhone = $("#Input_Phone");
    $seuCampoPhone.mask('(00) 00000-0000', { reverse: true });

    var $seuHour = $("#Hour");
    $seuHour.mask('00:00', { reverse: true });

    var $seuPrice = $("#Price");
    $seuPrice.mask('00000000.00', { reverse: true });
});