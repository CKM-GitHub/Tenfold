﻿////////const _url = {};

////////$(function () {
////////    setValidation();
////////    _url.get_D_Billing_List = common.appPath + '/r_invoice/Get_D_Billing_List';
////////    addEvents();
////////});


////////function setValidation() {
////////    $('#StartDate')
////////        .addvalidation_errorElement("#errorStartDate")
////////        .addvalidation_datecheck() //E108
////////        .addvalidation_datecompare(); //E111

////////    $('#EndDate')
////////        .addvalidation_errorElement("#errorEndDate")
////////        .addvalidation_datecheck() //E108
////////        .addvalidation_datecompare(); //E111
////////}

////////function addEvents() {
////////    common.bindValidationEvent('#form1', '');

////////    $('#StartDate, #EndDate').on('change', function () {

////////        const $this = $(this), $start = $('#StartDate').val(), $end = $('#EndDate').val()

////////        if (!common.checkValidityInput($this)) {
////////            return false;
////////        }
////////        let model = {
////////            StartDate: $start,
////////            EndDate: $end
////////        };

////////        if (model.StartDate && model.EndDate) {
////////            if (model.StartDate < model.EndDate) {
////////                $("#StartDate").hideError();
////////                $("#EndDate").hideError();
////////                $("#EndDate").focus();
////////                return;
////////            }
////////        }

////////    });


////////    const $Range = $("#Range").val(), $StartDate = $("#StartDate").val(),$EndDate = $("#EndDate").val()

////////    let model = {
////////        Range: $Range,
////////        StartDate: $StartDate,
////////        EndDate: $EndDate,
////////    };
////////    get_D_Billing_List(model, this);
////////    sortTable.getSortingTable("billingtable");

////////    $('#btnDisplay').on('click', function () {
////////        $('#total_record').text("")
////////        $('#total_record_up').text("")
////////        $('#no_record').text("");
////////        $('#billingtable tbody').empty();
////////        $form = $('#form1').hideChildErrors();

////////        if (!common.checkValidityOnSave('#form1')) {
////////            $form.getInvalidItems().get(0).focus();
////////            return false;
////////        }
////////        const fd = new FormData(document.forms.form1);
////////        const model = Object.fromEntries(fd);
////////        get_D_Billing_List(model, $form);

////////    });

////////}


////////function get_D_Billing_List(model, $form) {

////////    common.callAjaxWithLoading(_url.get_D_Billing_List, model, this, function (result) {
////////        if (result && result.isOK) {
////////            Bind_tbody(result.data);
////////        }
////////        if (result && !result.isOK) {
////////            const errors = result.data;
////////            for (key in errors) {
////////                const target = document.getElementById(key);
////////                $(target).showError(errors[key]);
////////                $form.getInvalidItems().get(0).focus();
////////            }
////////        }
////////    });
////////}


////////function Bind_tbody(result) {
////////    let data = JSON.parse(result);
////////    let html = "";
////////    for (var i = 0; i < data.length; i++) {
////////        html += '<tr>\
////////            <td class= "text-end" > ' + data[i]["NO"] + '</td>\
////////            </tr>'
////////    }
////////    if (data.length > 0) {
////////        $('#total_record').text("検索結果：" + data.length + "件")
////////        $('#total_record_up').text("検索結果：" + data.length + "件")
////////    }
////////    else {
////////        $('#total_record').text("検索結果： 0件")
////////        $('#total_record_up').text("検索結果： 0件")
////////        $('#no_record').text("表示可能データがありません");
////////    }
////////    $('#billingtable tbody').append(html);
////////}
