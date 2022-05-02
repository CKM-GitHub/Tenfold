﻿const _url = {};
$(function () {
    _url.get_issueslist_Data = common.appPath + '/r_issuelist/get_issueslist_Data';

    addEvents();
    $('#realECD').focus();
});

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
        $('#mansiontable tbody').empty();
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        get_issueslist_Data(model, $form);
    });

    $('#btnCSV').on('click', function () {
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);

        common.callAjax(_url.generate_M_SellerMansionCSV, model,
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
                    downloadLink.download = "売主マンションリスト" + dateString + ".csv";

                    document.body.appendChild(downloadLink);
                    downloadLink.click();
                    document.body.removeChild(downloadLink);
                }
                else {
                    alert("There is no data!");
                }
            }
        )
    });
}

function get_issueslist_Data(model, $form) {
    common.callAjaxWithLoading(_url.get_issueslist_Data, model, this, function (result) {
        if (result && result.isOK) {
            Bind_tbody(result.data);
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

function Bind_tbody(result) {
    let data = JSON.parse(result);
    let html = "";
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {

            if (isEmptyOrSpaces(data[i]["ステータス"])) {
                _letter = "";
                _class = "ms-1 ps-1 pe-1 rounded-circle";
                _sort_checkbox = "";
            }
            else {
                _letter = data[i]["ステータス"].charAt(0);
                if (_letter == "新") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-success text-white";
                    _sort_checkbox = "One";
                }
                else if (_letter == "交") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-info txt-dark";
                    _sort_checkbox = "Two";
                }
                else if (_letter == "成") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-secondary";
                    _sort_checkbox = "Three";
                }
                else if (_letter == "辞") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-light text-danger";
                    _sort_checkbox = "Four";
                }
                else if (_letter == "辞") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-dark text-white";
                    _sort_checkbox = "Five";
                }

            }
            html += '<tr>\
            <td class= "text-end"> ' + data[i]["NO"] + '</td>\
            <td class="'+ _sort_checkbox + '"><i class="' + _class + '">' + _letter + '</i><span class="font-semibold"> ' + data[i]["ステータス"] + '</span></td>\
            <td> ' + data[i]["物件CD"] + ' </td>\
            <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" id='+ data[i]["MansionCD"] + '&t_mansion_detail' + '  href="#" onclick="l_logfunction(this.id)">' + data[i]["マンション名"] + '</a><p><small class="text-wrap w-100">' + data[i]["住所"] + '</small></p></td>\
            <td> '+ data[i]["部屋"] + '</td>\
            <td class="text-end">'+ data[i]["階数"] + '</td>\
            <td class="text-end">'+ data[i]["面積"].toFixed(2) + '</td>\
            <td> <a class="text-heading font-semibold text-decoration-underline" href="#" id='+ data[i]["SellerCD"] + '&t_seller_assessment' + ' onclick="l_logfunction(this.id)">' + data[i]["売主名"] + '</a></td>\
            <td class="text-nowrap"> '+ data[i]["登録日時"] + '</td>\
            <td class="text-nowrap"> <a class="text-heading font-semibold text-decoration-underline" href="#" id='+ data[i]["AssReqID"] + '&t_seller_assessment_detail' + ' onclick="l_logfunction(this.id)"> ' + data[i]["詳細査定日時"] + ' </a> </td>\
            <td class="text-nowrap"> '+ data[i]["買取依頼日時"] + ' </td>\
            <td class="text-nowrap">\
            <a class="text-heading font-semibold text-decoration-underline" href="#" id='+ data[i]["GRealECD"] + '&t_seller_assessment_detail_GReal' + ' onclick="l_logfunction(this.id)" >' + data[i]["マンションTop1"] + ' </a><p class="text-end">' + data[i]["マンション金額"] + '</p> </td>\
             <td class="text-nowrap">\
             <a class="text-heading font-semibold text-decoration-underline" href="#" id='+ data[i]["ERealECD"] + '&t_seller_assessment_detail_EReal' + ' onclick="l_logfunction(this.id)"> ' + data[i]["エリアTop1"] + ' </a> <p class="text-end">' + data[i]["エリア金額"] + '</p></td>\
             <td class="text-nowrap"> <a class="text-heading font-semibold text-decoration-underline" href="#" id='+ data[i]["IRealECD"] + '&t_seller_assessment_detail_IRealECD' + ' onclick="l_logfunction(this.id)"> ' + data[i]["買取依頼会社"] + ' </a><p class="text-end">' + data[i]["買取依頼金額"] + '</p> </td>\
             <td class="text-nowrap"> '+ data[i]["送客日時"] + ' </td>\
             <td class="text-nowrap"> '+ data[i]["成約日時"] + ' </td>\
             <td class="text-nowrap"> '+ data[i]["辞退日時"] + '</td>\
            </tr>'
        }

        $('#total_record').text("検索結果：" + data.length + "件")
        $('#total_record_up').text("検索結果：" + data.length + "件")
    }
    else {
        $('#total_record').text("検索結果： 0件")
        $('#total_record_up').text("検索結果： 0件")
    }
    $('#mansiontable tbody').append(html);

    sortTable.getSortingTable("mansiontable");
}