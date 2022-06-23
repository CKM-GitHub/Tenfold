_url = '';

$(function () {
    _url.get_r_statistic_displayData = common.appPath + '/r_statistic/get_r_statistic_displayData';
    setValidation();
    addEvents();
});

function setValidation() {
    $('.form-check-input')
        .addvalidation_errorElement("#CheckBoxError")
        .addvalidation_checkboxlenght(); //E112

    $('#StartDate')
        .addvalidation_errorElement("#errorStartDate")
        .addvalidation_reqired()
        .addvalidation_datecheck() //E108
        .addvalidation_datecompare(); //E111

    $('#EndDate')
        .addvalidation_errorElement("#errorEndDate")
        .addvalidation_reqired()
        .addvalidation_datecheck() //E108
        .addvalidation_datecompare(); //E111
}

function addEvents() {
    common.bindValidationEvent('#form1', '');

    $('#StartDate, #EndDate').on('change', function () {
        const $this = $(this), $start = $('#StartDate').val(), $end = $('#EndDate').val();
        if (!common.checkValidityInput($this)) {
            return false;
        }
        let model = {
            StartDate: $start,
            EndDate: $end
        };
        if (model.StartDate && model.EndDate) {
            if (model.StartDate < model.EndDate) {
                $("#StartDate").hideError();
                $("#EndDate").hideError();
                $("#EndDate").focus();
                return;
            }
        }
    });

    $('.form-check-input').on('change', function () {
        this.value = this.checked ? 1 : 0;
        if ($("input[type=checkbox]:checked").length > 0) {
            $('.form-check-input').hideError();
        }
    }).change();

    $('#btnThisMonth').on('click', function () {
        let today = common.getToday();
        let firstDay = common.getFirstDayofMonth();
        $('#StartDate').val(firstDay);
        $('#EndDate').val(today);
    });
    $('#btnLastMonth').on('click', function () {
        let firstdaypremonth = common.getFirstDayofPreviousMonth();
        let lastdaypremonth = common.getLastDayofPreviousMonth();
        $('#StartDate').val(firstdaypremonth);
        $('#EndDate').val(lastdaypremonth);
    });

    $('#btnProcess').on('click', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        $('#canvas').empty();
        //const fd = new FormData(document.forms.form1);
        //const model = Object.fromEntries(fd);
        var model = {
            dac_route: $('#dac_route').val(),
            dac_apartment: $('#dac_apartment').val(),
            top5_route: $('#top5_route').val(),
            top5_apartment: $('#top5_apartment').val(),
            contracts_route: $('#contracts_route').val(),
            contracts_apartment: $('#contracts_apartment').val(),
            bd_route: $('#bd_route').val(),
            bd_apartment: $('#bd_apartment').val(),
            sd_route: $('#sd_route').val(),
            sd_apartment: $('#sd_apartment').val(),
            ryudo: $('input[name=ryudo]:checked', '#form1').val(),
            StartDate: $('#StartDate').val(),
            EndDate: $('#EndDate').val(),
        }
        get_r_statistic_Data(model, $form);
    });
}

function get_r_statistic_Data(model, $form) {
    common.callAjaxWithLoading(_url.get_r_statistic_displayData, model, this, function (result) {
        if (result && result.isOK) {
            bind_r_statistic_Data(result.data);
        }
        if (result && !result.isOK) {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                $form.getInvalidItems().get(0).focus();
            }
        }
    });
}

function bind_r_statistic_Data(result) {

}