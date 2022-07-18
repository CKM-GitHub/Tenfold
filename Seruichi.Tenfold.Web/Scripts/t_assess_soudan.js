﻿const _url = {};
$(function () {
    _url.get_t_assess_soudan_DisplayData = common.appPath + '/t_assess_soudan/get_t_assess_soudan_DisplayData';
    _url.get_t_assess_soudan_CSVData = common.appPath + '/t_assess_soudan/get_t_assess_soudan_CSVData';
    _url.get_Modal_infotrainData = common.appPath + '/t_reale_purchase/get_Modal_infotrainData';
    _url.insert_l_log = common.appPath + '/t_assess_soudan/Insert_l_log';
    setValidation();
    addEvents();
    $('#FreeWord').focus();
});

function setValidation() {
    $('#StartDate')
        .addvalidation_errorElement("#errorStartDate")
        .addvalidation_datecheck() //E108
        .addvalidation_datecompare(); //E111

    $('#EndDate')
        .addvalidation_errorElement("#errorEndDate")
        .addvalidation_datecheck() //E108
        .addvalidation_datecompare(); //E111

    $('#FreeWord')
        .addvalidation_errorElement("#errorFreeWord")
        .addvalidation_maxlengthCheck(50);

    $('.form-check-input')
        .addvalidation_errorElement("#CheckBoxError")
        .addvalidation_checkboxlenght(); //E112
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

    let model = {
        FreeWord: $('#FreeWord').val(),
        untreated: $('#untreated').val(),
        processing: $('#processing').val(),
        solution: $('#solution').val(),
        Range: $('#ddlRange').val(),
        StartDate: $('#StartDate').val(),
        EndDate: $('#EndDate').val(),
        M_PIC: $('#M_PIC').val()
    };
    get_t_assess_soudan_DisplayData(model, this);

    sortTable.getSortingTable("tblsoudan");

    $('.form-check-input').on('change', function () {
        this.value = this.checked ? 1 : 0;
        if ($("input[type=checkbox]:checked").length > 0) {
            $('.form-check-input').hideError();
        }
    }).change();

    $('#btnToday').on('click', function () {
        let today = common.getToday();
        $('#StartDate').val(today);
        $('#EndDate').val(today);
    });
    $('#btnThisWeek').on('click', function () {
        let start = common.getDayaweekbeforetoday();
        let end = common.getToday();
        $('#StartDate').val(start);
        $('#EndDate').val(end);
    });
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
        $('#tblsoudan tbody').empty();
        $('#type').val('display');
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        get_t_assess_soudan_DisplayData(model, $form);
    });

    $('#btnCSV').on('click', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        $('#tblsoudan tbody').empty();
        $('#type').val('csv');
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        get_t_assess_soudan_DisplayData(model, $form);

        common.callAjax(_url.get_t_assess_soudan_CSVData, model,
            function (result) {
                //sucess
                var table_data = result.data;

                var csv = common.getJSONtoCSV(table_data);
                if (!(csv == "ERROR")) {
                    var downloadLink = document.createElement("a");
                    var blob = new Blob(["\ufeff", csv]);
                    var url = URL.createObjectURL(blob);
                    downloadLink.href = url;
                    let m = new Date();
                    var dateString =
                        m.getUTCFullYear() + "" +
                        ("0" + (m.getUTCMonth() + 1)).slice(-2) + "" +
                        ("0" + m.getUTCDate()).slice(-2) + "_" +
                        ("0" + m.getHours()).slice(-2) + "" +
                        ("0" + m.getMinutes()).slice(-2) + "" +
                        ("0" + m.getSeconds()).slice(-2);
                    downloadLink.download = "処理待ちリスト＿相談_" + dateString + ".csv";

                    document.body.appendChild(downloadLink);
                    downloadLink.click();
                    document.body.removeChild(downloadLink);
                }
                else {
                    $('#csv-error-modal').modal('show');
                }
            }
        )
    });
}

function get_t_assess_soudan_DisplayData(model, type, $form) {
    common.callAjaxWithLoading(_url.get_t_assess_soudan_DisplayData, model, this, function (result) {
        if (result && result.isOK) {
            Bind_DisplayData(result.data);
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

function isEmptyOrSpaces(str) {
    return str === null || str.match(/^ *$/) !== null;
}

function Bind_DisplayData(result) {
    let _letter = "", _class = "", _snbg_color = "", _rnbg_color = "";
    let data = JSON.parse(result);
    let html = "";
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {

            if (isEmptyOrSpaces(data[i]["ステータス"])) {
                _letter = "";
                _class = "ms-1 ps-1 pe-1 rounded-circle";
            }
            else {
                _letter = data[i]["ステータス"].charAt(0);
                if (_letter == "未") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-primary text-white fst-normal fst-normal";
                }
                else if (_letter == "処") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-warning text-white fst-normal fst-normal";
                }
                else if (_letter == "解") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-success text-white fst-normal fst-normal";
                }
            }

            if (data[i]["ConsultFrom"] = '1') {
                _snbg_color = "bg-warning";
                _rnbg_color = "";
            }
            else {
                _snbg_color = "";
                _rnbg_color = "bg-warning";
            }

            html += '<tr>\
            <td class= "text-end"> ' + (i + 1) + '</td>\
            <td><span class="' + _class + '">' + _letter + '</span><span class="font-semibold"> ' + data[i]["ステータス"] + '</span></td>\
            <td class="text-nowrap">'+ data[i]["相談ID"] + '</td>\
            <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" id='+ data[i]["SellerMansionID"] + '&' + data[i]["SellerCD"] + '&' + data[i]["相談ID"] + '&' + data[i]["PurchReqDateTime"] + ' href="#" onclick="Display_Detail(this.id)"><span class="' + _snbg_color + ' pt-1 pb-1 ps-2 pe-2">' + data[i]["売主名"] + '</span></a></td>\
            <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" id='+ data[i]["SellerMansionID"] + '&' + data[i]["SellerCD"] + '&' + data[i]["相談ID"] + '&' + data[i]["PurchReqDateTime"] + ' href="#" onclick="Display_Detail(this.id)"><span class="' + _rnbg_color + ' pt-1 pb-1 ps-2 pe-2">' + data[i]["不動産会社"] + '</span></a></td>\
            <td class="text-nowrap">'+ data[i]["管理担当"] + '</td>\
            <td class="text-nowrap">'+ data[i]["相談発生日時"] + '</td>\
            <td class="text-nowrap"> '+ data[i]["相談解決日時"] + '</td>\
            <td class="text-center"> '+ data[i]["相談区分"] + '</td>\
            </tr>'
        }

        $('#total_record_up').text("検索結果： " + data.length + "件");
        $('#total_record_down').text("検索結果： " + data.length + "件");
        $('#no_record').text("");
    }
    else {
        $('#total_record_up').text("検索結果： 0件 ");
        $('#total_record_down').text("検索結果： 0件");
        $('#no_record').text("表示可能データがありません");
    }
    $('#tblsoudan tbody').append(html);
}

function Display_Detail(id) {
    var PurchReqDateTime = '';
    if (id.split('&')[3] == '') {
    }
    else {
        var Date = id.split('&')[3];
        var Time = id.split('&')[4];
        PurchReqDateTime = Date + ' ' + Time;
    }

    let model = {
        SellerMansionID: id.split('&')[0],
        SellerCD: id.split('&')[1],
        ConsultID: id.split('&')[2]
    };
    Bind_ModalPopupData(model, PurchReqDateTime);

    $('#soudan').modal('show');
}

function Bind_ModalPopupData(model, PurchReqDateTime) {
    common.callAjax(_url.get_Modal_infotrainData, model, function (result) {
        if (result && result.isOK) {
            Bind_Modal_ConsultData(result.data, PurchReqDateTime);
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

function Bind_Modal_ConsultData(result, PurchReqDateTime) {
    let data = JSON.parse(result);
    let MansionInfo = "";
    let html = "";
    let htmlTemp = "";
    const isEven = data.length % 2 === 0 ? 'Even' : 'Odd';

    if (data.length > 0) {
        $('#ConsultID').text(data[0]['ConsultID']);
        $('#ConsultFrom').text(data[0]['ConsultFrom']);
        $('#ConsultDateTime').text(data[0]['ConsultDateTime']);
        $('#ConsultType').text(data[0]['ConsultType']);
        $('#Consultation').text(data[0]['Consultation']);

        //Display Data of MansionInfo
        MansionInfo = '<h4 class="text-center fw-bold">' + data[0]["MansionName"] + '<strong>' + data[0]["RoomNumber"] + '</strong></h4>\
        <p class="font-monospace small text-start pt-3">'+ data[0]["Address"] + '</p >\
        <p class="font-monospace small text-center pt-3">' + PurchReqDateTime + '</p>'
        $('#H_MansionInfo').append(MansionInfo);

        $('#status').val(data[0]['ProgressKBN']);
        $('#pic').val(data[0]['TenStaffName']);

        //Bind Data of ekikoutsu
        for (var i = 0; i < data.length; i++) {
            html += '<div class="card p-1 col-12 col-md-6 shadow-sm">\
                            <div class="card-body p-2 row">\
                                <div class="col-12">\
                                    <h5>'+ data[i]["LineName"] + '</h5>\
                                </div>\
                                <div class="col-12">\
                                    <p class="text-dark">'+ data[i]["StationName"] + '</p>\
                                </div>\
                                <div class="col-12 text-end">\
                                    <p class="text-dark">'+ data[i]["Distance"] + '</p>\
                                </div>\
                            </div>\
                         </div>'
        }
        htmlTemp = '<div class="invisible card p-1 col-12 col-md-6 shadow-sm"> </div>'
        if (isEven == 'Odd') {
            $('#ekikoutsu').append(html + htmlTemp);
        }
        else {
            $('#ekikoutsu').append(html);
        }
    }
}