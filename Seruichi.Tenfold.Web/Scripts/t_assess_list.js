const _url = {};
$(function () {
    _url.getM_SellerMansionList = common.appPath + '/t_assess_list/GetM_SellerMansionList';
    _url.generate_M_SellerMansionCSV = common.appPath + '/t_assess_list/Generate_M_SellerMansionCSV';
    _url.insert_l_log = common.appPath + '/t_assess_list/Insert_l_log';
    setValidation();
    addEvents();
    $('#navbarDropdownMenuLink1').addClass('font-bold active text-underline');
    $('#t_assess_list').addClass('font-bold text-underline');
});

function setValidation()
{
    $('#StartDate')
        .addvalidation_errorElement("#errorStartDate")
        .addvalidation_datecheck() //E108
        .addvalidation_datecompare(); //E111

    $('#EndDate')
        .addvalidation_errorElement("#errorEndDate")
        .addvalidation_datecheck() //E108
        .addvalidation_datecompare(); //E111

    $('.form-check-input')
        .addvalidation_errorElement("#CheckBoxError")
        .addvalidation_checkboxlenght(); //E112
    
}


function addEvents() {
    common.bindValidationEvent('#form1', '');

    $('#StartDate, #EndDate').on('change', function () {

        const $this = $(this), $start = $('#StartDate').val(), $end = $('#EndDate').val()

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


    const $Chk_Shinki = $("#Chk_Shinki").val(), $Chk_Kosho = $("#Chk_Kosho").val(), $Chk_Seiyaku = $("#Chk_Seiyaku").val(),
        $Chk_Urinushi = $("#Chk_Urinushi").val(), $Chk_Kainushi = $("#Chk_Kainushi").val(), $Chk_Hide = $("#Chk_Hide").val(),

        $Range = $("#Range").val(),
        $StartDate = $("#StartDate").val(),
        $EndDate = $("#EndDate").val()

    let model = {
        Chk_Shinki: $Chk_Shinki,
        Chk_Kosho: $Chk_Kosho,
        Chk_Seiyaku: $Chk_Seiyaku,
        Chk_Urinushi: $Chk_Urinushi,
        Chk_Kainushi: $Chk_Kainushi,
        Chk_Hide: $Chk_Hide,
        Range: $Range,
        StartDate: $StartDate,
        EndDate: $EndDate,
    };
    getM_SellerMansionList(model, this);
    sortTable.getSortingTable("mansiontable");

    $('.form-check-input').on('change', function () {
        this.value = this.checked ? 1 : 0;
        if ($("input[type=checkbox]:checked").length > 0) {
            $('.form-check-input').hideError();
        }
    }).change();

    //$('.hide').on('change', function () {
    //    this.value = this.checked ? 1 : 0;
    //    if ($("input[type=checkbox]:checked").length > 0) {
    //        $('.form-check-input').hideError();
    //    }
    //    if ($(this).is(':checked')) {
    //        document.getElementById("Hide").setAttribute("class", "ms-1 ps-1 pe-1 rounded-circle bg-danger text-white");
    //    }
    //    else {
    //        document.getElementById("Hide").setAttribute("class", "ms-1 ps-1 pe-1 rounded-circle bg-black text-white");
    //    }
    //}).change();

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
        $('#total_record').text("")
        $('#total_record_up').text("")
        $('#no_record').text("");
        $('#mansiontable tbody').empty();
        $form = $('#form1').hideChildErrors();

        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        getM_SellerMansionList(model, $form);

    });
    $('#btnCSV').on('click', function () {
        $('#total_record').text("")
        $('#total_record_up').text("")
        $('#no_record').text("");
        $('#mansiontable tbody').empty();
        $form = $('#form1').hideChildErrors();
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        getM_SellerMansionList(model, $form)
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
                    downloadLink.download = "査定依頼一覧リスト＿相談_" + dateString + ".csv";

                    document.body.appendChild(downloadLink);
                    downloadLink.click();
                    document.body.removeChild(downloadLink);
                }
                else {
                    $('#site-error-modal').modal('show');
                }
            }
        )
    });
}

function getM_SellerMansionList(model, $form) {

    common.callAjaxWithLoading(_url.getM_SellerMansionList, model, this, function (result) {
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
    for (var i = 0; i < data.length; i++) {

        if (isEmptyOrSpaces(data[i]["ステータス"])) {
            _letter = "";
            _class = "ms-1 ps-1 pe-1 rounded-circle";
            _sort_checkbox = "";
        }
        else {
            _letter = data[i]["ステータス"].charAt(0);
           
            if (_letter == "新") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-warning text-white fst-normal";
                _sort_checkbox = "1";
            }
            else if (_letter == "交") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-info txt-dark fst-normal";
                _sort_checkbox = "2";
            }
            else if (_letter == "成") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-secondary fst-normal";
                _sort_checkbox = "3";
            }
            else if (_letter == "売" && data[i]["ステータス"] == "売主辞退") {
                _letter = "辞";
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-light text-danger fst-normal";
                _sort_checkbox = "4";
            }
            else if (_letter == "買" && data[i]["ステータス"] == "買主辞退") {
                _letter = "辞";
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-dark text-white fst-normal";
                _sort_checkbox = "5";
            }
            else if (_letter == "非") {
               
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-danger text-white fst-normal";
                _sort_checkbox = "6";
            }
        }
        html += '<tr>\
            <td class= "text-end">' + data[i]["NO"] + '</td>\
            <td class="'+ _sort_checkbox + '"><i class="' + _class + '">' + _letter + '</i><span class="font-semibold">' + data[i]["ステータス"] + '</span></td>\
            <td class="text-nowrap">'+ data[i]["査定ID"] +'</td>\
            <td class="text-nowrap"><a class="text-heading font-semibold text-decoration-underline" href="#" id='+ data[i]["AssReqID"] + '&t_chat&' + data[i]["AssReqID"] + ' onclick="l_logfunction(this.id)"> ' + data[i]["マンション名＆部屋番号"] +'</a> </td>\
            <td class="text-nowrap"><a class="text-heading font-semibold text-decoration-underline" href="#" id='+ data[i]["SellerCD"] + '&t_seller_assessment&' + data[i]["売主名"] + ' onclick="l_logfunction(this.id)"> ' + data[i]["売主名"] +'</a> </td>\
            <td class="text-nowrap"><a class="text-heading font-semibold text-decoration-underline" href="#" id='+ data[i]["RealECD"] + '&t_reale_purchase&' + data[i]["不動産会社"] + ' onclick="l_logfunction(this.id)"> ' + data[i]["不動産会社"] +'</a> </td>\
            <td class="text-nowrap">'+ data[i]["買取依頼日時"] +'</td>\
            <td class="text-nowrap">'+ data[i]["送客日時"] +'</td>\
            <td class="text-nowrap">'+ data[i]["成約日時"] +'</td>\
            <td class="text-nowrap">'+ data[i]["辞退日時"] +'</td>\
            <td class="d-none">'+ data[i]["AssID"] + '</td>\
            <td class="d-none">'+ _sort_checkbox + '</td>\
            </tr>'
    }
    if (data.length > 0) {
        $('#total_record').text("検索結果：" + data.length + "件")
        $('#total_record_up').text("検索結果：" + data.length + "件")
    }
    else {
        $('#total_record').text("検索結果： 0件")
        $('#total_record_up').text("検索結果： 0件")
        $('#no_record').text("表示可能データがありません");
    }
    $('#mansiontable tbody').append(html);
}

function l_logfunction(id) {
    let model = {
        LoginKBN: null,
        LoginID: null,
        RealECD: null,
        LoginName: null,
        IPAddress: null,
        Page: null,
        Processing: null,
        Remarks: null,
        LogId: id.split('&')[0],
        LogStatus: id.split('&')[1],
        LogRemarks: id.split('&')[2],

    };
    common.callAjax(_url.insert_l_log, model,
        function (result) {
            if (result && result.isOK) {
                if (model.LogStatus == "t_chat") {
                    //alert("https://www.seruichi.com/t_reale_purchase?RealECD=" + model.LogId);
                    //window.location.href = common.appPath + "/t_chat/Index?AssReqID=" + model.LogId;
                    alert(common.appPath + "/t_chat/Index?AssReqID=" + model.LogId);
                }
                else if (model.LogStatus == "t_seller_assessment") {
                    window.location.href = common.appPath + "/t_seller_assessment/Index?SellerCD=" + model.LogId;
                }
                else if (model.LogStatus == "t_reale_purchase") {
                    window.location.href = common.appPath + "/t_reale_purchase/Index?RealECD=" + model.LogId;
                }
            }
            if (result && !result.isOK) {
            }
        });
}

