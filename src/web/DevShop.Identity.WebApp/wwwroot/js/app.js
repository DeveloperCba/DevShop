

function showAlert(type, title, message, callbackOK) {
    Swal.fire({
        icon: type,
        title: title,
        html: message,
        showCancelButton: false,
        allowOutsideClick: false,
        buttonsStyling: false,
        customClass: {
            confirmButton: "btn btn-primary mr-1 mt-1 waves-effect waves-float waves-light",
        },
    }).then(callbackOK)
}

function showConfirm(type, title, message, callbackContinue, callbackCancel, supplyLabelCancel, supplyLabelConfirm) {
    Swal.fire({
        width: 600,
        icon: type,
        title: title,
        html: message,
        showCancelButton: true,
        allowOutsideClick: false,
        buttonsStyling: false,
        customClass: {
            confirmButton: "btn btn-primary",
            cancelButton: "btn btn-outline-secondary"
        },
        confirmButtonText: !supplyLabelConfirm ? 'Sim' : supplyLabelConfirm,
        cancelButtonText: !supplyLabelCancel ? 'Cancelar' : supplyLabelCancel
    }).then((result) => {
        if (result.isConfirmed)
            return callbackContinue();
        else
            return callbackCancel();
    });
}

function showLoading(title, message, timer, callbackExecute, callbackAfterExecute) {
    Swal.fire({
        title: (title == undefined || title == '' ? 'Aguarde' : title),
        padding: 50,
        showCancelButton: false,
        showConfirmButton: false,
        timer: (timer == undefined || !timer ? 5500 : timer),
        html: (message == undefined || message == '' ? 'Aguarde, sua solicitação está sendo processada...' : message),
        showClass: {
            popup: 'swal2-noanimation',
            backdrop: 'swal2-noanimation'
        },
        hideClass: {
            popup: '',
            backdrop: ''
        },
        allowOutsideClick: false,
        allowEscapeKey: false,
        customClass: 'animated zoomIn',
        didOpen: function () {
            swal.showLoading();
            if (callbackExecute)
                return callbackExecute();
        }
    }).then(function () {
        if (callbackAfterExecute)
            return callbackAfterExecute();
    }, function (dismiss) {
        if (callbackAfterExecute)
            return callbackAfterExecute();
    });
}

function AjaxRequest(type, url, responseType, parameters, callbackSuccess, callbackError, isAsync) {

    if (!url || url.length == 0)
        return false;
    axios({
        url: url,
        method: (type == undefined || type == '' ? "GET" : type),
        responseType: (responseType == undefined || responseType == '' ? "json" : responseType),
        data: parameters,
        paramsSerializer: params => transformRequest(params),
        maxContentLength: Infinity,
        maxBodyLength: Infinity
    }).then(function (response) {

        if (callbackSuccess)
            return callbackSuccess(response);
    }).catch(function (error) {
        if (callbackError)
            return callbackError(error.data, error.status);
    })
}

function Toast(icon, message, title, redirect = false, url = null) {
    toastr[icon](
        message,
        title,
        {
            closeButton: true,
            tapToDismiss: false,
            rtl: isRtl
        },
    );
    setTimeout(function () {
        if (redirect) {
            if (!url) {
                window.location.reload();
            } else {
                window.location.href = url;
            }
        }
    }, 3000);
}

const transformRequest = params => {

    const parts = [];

    const encode = val => {
        return encodeURIComponent(val).replace(/%3A/gi, ':')
            .replace(/%24/g, '$')
            .replace(/%2C/gi, ',')
            .replace(/%20/g, '+')
            .replace(/%5B/gi, '[')
            .replace(/%5D/gi, ']');
    }

    const convertPart = (key, val) => {
        if (val instanceof Date)
            val = val.toISOString()
        else if (val instanceof Object)
            val = JSON.stringify(val)

        parts.push(encode(key) + '=' + encode(val));
    }

    Object.entries(params).forEach(([key, val]) => {
        if (val === null || typeof val === 'undefined')
            return

        if (Array.isArray(val))
            val.forEach((v, i) => convertPart(`${key}[${i}]`, v))
        else
            convertPart(key, val)
    })
    return parts.join('&')
}

const transformRequestOptions = params => {
    let options = '';
    let x = 0;
    let item;
    let newItem;
    for (const key in params) {
        if (typeof params[key] !== 'object' && params[key]) {
            options += `${key}=${params[key]}&`;
        } else if (typeof params[key] === 'object' && params[key] && params[key].length) {
            params[key].forEach(el => {
                newItem = params[key];
                if (newItem != item)
                    x = 0

                options += `${key}[${[x++]}]=${el}&`
                item = params[key];
            });
        }
    }
    return options ? options.slice(0, -1) : options;
};

function ErrorHandler(mistakes) {
    let element = '</br>'

    element += '<ul>'
    mistakes.map(function (value, index) {
        element += `   <li> ${value['message']} </li>`
    });
    element += '</ul>'

    return element;
}

function converterFloatReal(valor) {
    var inteiro = null, decimal = null, c = null, j = null;
    var aux = new Array();
    valor = "" + valor;
    c = valor.indexOf(".", 0);
    //encontrou o ponto na string
    if (c > 0) {
        //separa as partes em inteiro e decimal
        inteiro = valor.substring(0, c);
        decimal = valor.substring(c + 1, valor.length);
        if (decimal.length === 1) {
            decimal += "0";
        }
    } else {
        inteiro = valor;
    }

    //pega a parte inteiro de 3 em 3 partes
    for (j = inteiro.length, c = 0; j > 0; j -= 3, c++) {
        aux[c] = inteiro.substring(j - 3, j);
    }

    //percorre a string acrescentando os pontos
    inteiro = "";
    for (c = aux.length - 1; c >= 0; c--) {
        inteiro += aux[c] + '.';
    }
    //retirando o ultimo ponto e finalizando a parte inteiro

    inteiro = inteiro.substring(0, inteiro.length - 1);

    decimal = parseInt(decimal);
    if (isNaN(decimal)) {
        decimal = "00";
    } else {
        decimal = "" + decimal;
        if (decimal.length === 1) {
            decimal = "0" + decimal;
        }
    }
    valor = inteiro + "," + decimal;
    return valor;
}

function toDate(selector, split) {
    var from = $(selector).val().split(split)
    var date = new Date(from[2], from[1] - 1, from[0])
    return (isValidDate(date)) ? moment(date).format('YYYY-MM-DD') : null
}

function isValidDate(d) {
    return d instanceof Date && !isNaN(d);
}

function createComboBoxAjax(element, url, isAsync = true) {
    let $element = $(`#${element}`);
    $element.empty();
    axios({
        async: isAsync,
        url: url,
        method: "GET",
        responseType: "JSON"
    }).then(function (response) {
        response.data.map(function (value, index) {
            $element.append(`<option value="${value['value']}">${value['text']}</option>`);
        });
        $element.val($element).trigger("change");
    });
}

function createComboAjaxPagination(element, placeholder, url, modal) {
    $(`#${element}`).select2({
        placeholder: !placeholder ? 'Selecionar' : placeholder,
        dropdownParent: !modal ? null : $(`#modal_${modal}`),
        language: 'pt-BR',
        ajax: {
            url: url,
            dataType: 'json',
            method: 'GET',
            delay: 250,
            data: function (params) {
                return {
                    currentPage: params.page || 0,
                    perPage: params.pageSize || 30,
                    searchParameter: params.term
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    pagination: {
                        more: (params.page * 30) < (data != null) ? data[0].Total : 0
                    },
                    results: $.map(data, function (obj) {
                        return { id: obj.value, text: obj.text };
                    })
                };
            },
            cache: true
        },
        escapeMarkup: function (markup) { return markup; },
        minimumInputLength: 0
    });
}

/**
 * 
 * @param {any} element
 * @param {any} placeholder
 * @param {any} url 
 */
function createComboBoxSelect2Ajax(element, placeholder, url) {
    let $element = $(`#${element}`);
    $element.select2({
        placeholder: placeholder,
        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                let searchParameter = params.term;
                return { searchParameter };
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (obj) {
                        return { id: obj.value, text: obj.text };
                    })
                };
            },
            cache: true
        },
        escapeMarkup: function (markup) { return markup; },
    });
}

function openModal(url, size, target) {
    var url2 = replaceAll(url, "/", "_");

    if (url2.indexOf("?") > 0) {
        url2 = url2.split("?")[0];
    }

    if (target == null) {
        target = "#modal" + url2;
    }

    $(target).remove();

    $('<div class="modal fade text-left" id="' + target.replace("#", "") + '" tabindex="-1" role="dialog" aria-labelledby="myModalLabel16" aria-hidden="true"><div class="modal-dialog" role="document"></div></div>').appendTo('body');

    if (size == null || size == "") {
        $(target).find('.modal-dialog').addClass('modal-lg');
    } else {
        $(target).find('.modal-dialog').attr("style", "max-width: " + size + "");
    }

    $(target).modal("show");
    $(target).find('.modal-dialog').html("");
    $(target).find('.modal-dialog').load(url, function () {
        swal.close();
    });

    $(target).on('hidden.bs.modal', function () {
        $(target).remove();
    });
}

function loadingPage(load) {
    $("#loading-page").css("display", load);
}

function replaceAll(str, needle, replacement) {
    return str.split(needle).join(replacement);
}

function SaleSimulation() {
    openModal('/Venda/Simulacao', null, null);
}


function bootstrapTableFormatter(tipo, valor) {
    switch (tipo) {
        case 'bool':
            return (valor ? 'SIM' : 'N\u00c3O');

        case 'cpfcnpj':
            var input = $('<input/>').val(valor);
            input.maskCpfCnpj();

            return input.val();

        case 'decimal':
        case 'decimal4':
        case 'percentage':
            var digitos = (tipo == 'decimal4') ? 4 : 2;
            var value = parseFloat(valor ? valor : '0').toLocaleString('pt-BR', { minimumFractionDigits: digitos, maximumFractionDigits: digitos });

            if (tipo == 'percentage') return value + '%';
            return value;

        case 'date':
        case 'datetime':
        case 'datetimefull':
            if (!valor) return null;

            var date = new Date(parseInt(valor.substr(6)));
            var data = preencherAEsquerda(date.getDate(), '0', 2) + '/' + preencherAEsquerda(date.getMonth() + 1, '0', 2) + '/' + date.getFullYear();

            if (tipo == 'date') return data;

            var hora = preencherAEsquerda(date.getHours(), '0', 2) + ':' + preencherAEsquerda(date.getMinutes(), '0', 2);
            data = data + ' ' + hora;

            if (tipo == 'datetime') return data;

            return data + ':' + preencherAEsquerda(date.getSeconds(), '0', 2);

        default:
            return valor;
    }
}