const _url = {};
let html_Home_Menu = "";

$(function () {
    setValidation();
    _url.getM_SellerMansionList = common.appPath + '/t_seller_assessment/GetM_SellerMansionList';
    _url.generate_M_SellerMansionCSV = common.appPath + '/t_seller_assessment/Generate_M_SellerMansionCSV';
    _url.insert_l_log = common.appPath + '/t_seller_assessment/Insert_l_log';
    _url.Get_PopupFor_Home = common.appPath + '/t_seller_assessment/Get_PopupFor_Home';
    _url.Get_PopupFor_ResultType_1 = common.appPath + '/t_seller_assessment/Get_PopupFor_ResultType_1';
    _url.Get_PopupFor_ResultType_2 = common.appPath + '/t_seller_assessment/Get_PopupFor_ResultType_2';
    _url.Get_PopupFor_Detail = common.appPath + '/t_seller_assessment/Get_PopupFor_Detail';
    _url.Get_PopupFor_Seller = common.appPath + '/t_seller_assessment/Get_PopupFor_Seller';
    addEvents();
   
    $('#menu_seller_assess li').children('a').removeClass("active");
    $('#menu_seller_assess li').children('a').eq(0).addClass('active');
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


    const $Chk_Mi = $("#Chk_Mi").val(), $Chk_Kan = $("#Chk_Kan").val(), $Chk_Satei = $("#Chk_Satei").val(),
        $Chk_Kaitori = $("#Chk_Kaitori").val(), $Chk_Kakunin = $("#Chk_Kakunin").val(), $Chk_Kosho = $("#Chk_Kosho").val(),
        $Chk_Seiyaku = $("#Chk_Seiyaku").val(), $Chk_Urinushi = $("#Chk_Urinushi").val(), $Chk_Kainushi = $("#Chk_Kainushi").val(),
        $Chk_Sakujo = $("#Chk_Sakujo").val(),
        $Range = $("#Range").val(), $StartDate = $("#StartDate").val(),
        $EndDate = $("#EndDate").val()

    let model = {
        Chk_Mi: $Chk_Mi,
        Chk_Kan: $Chk_Kan,
        Chk_Satei: $Chk_Satei,
        Chk_Kaitori: $Chk_Kaitori,
        Chk_Kakunin: $Chk_Kakunin,
        Chk_Kosho: $Chk_Kosho,
        Chk_Seiyaku: $Chk_Seiyaku,
        Chk_Urinushi: $Chk_Urinushi,
        Chk_Kainushi: $Chk_Kainushi,
        Chk_Sakujo: $Chk_Sakujo,
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
                    downloadLink.download = "売主査定リスト_" + dateString + ".csv";

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


    $('#btnDownClose').on('click', function () {
        //$('#SNameAndRoomNo').empty();
        //$('#ekikoutsu').empty();
        //$('#ResultDate').empty();
        //$('#tableType1 tbody').empty();
        //$('#tableType2 tbody').empty();
        //location.reload();
        $('#pills-home-tab').trigger('click');
    });
    $('#btnUpClose').on('click', function () {
        //$('#SNameAndRoomNo').empty();
        //$('#ekikoutsu').empty();
        //$('#ResultDate').empty();
        //$('#tableType1 tbody').empty();
        //$('#tableType2 tbody').empty();
        // location.reload();
        $('#pills-home-tab').trigger('click');
    });

    $('#pills-home-tab').on('click', function () {
        //$('#SNameAndRoomNo').empty();
        //$('#ekikoutsu').empty();
        //$('#ResultDate').empty();
        //$('#tableType1 tbody').empty();
        //$('#tableType2 tbody').empty();
        //Get_PopupFor_Home(this.id);

    });
    
    $('#pills-profile-tab').on('click', function () {
        $('#header1').empty();
        $('#Detail_1').empty();
        $('#Detail_2').empty();
        $('#Detail_3').empty();
        Get_PopupFor_Detail();
    });

    $('#pills-contact-tab').on('click', function () {
        $('#header2').empty();
        $('#Seller_1').empty();
        $('#Seller_2').empty();
        Get_PopupFor_Seller();

    });
}

function Get_PopupFor_Detail() {
    let model = {};
    common.callAjax(_url.Get_PopupFor_Detail, model, function (result) {
        if (result && result.isOK) {

            Bind_Popup_Detail(result.data);
        }
        if (result && !result.isOK) {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
            }
        }
    });
}

function Get_PopupFor_Seller() {
    let model = {};
    common.callAjax(_url.Get_PopupFor_Seller, model, function (result) {
        if (result && result.isOK) {

            Bind_Popup_Seller(result.data);
        }
        if (result && !result.isOK) {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);

            }
        }
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
            if (_letter == "未") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-primary text-white fst-normal";
                _sort_checkbox = "1";
            }
            else if (_letter == "簡") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-info text-white fst-normal";
                _sort_checkbox = "2";
            }
            else if (_letter == "詳" && data[i]["ステータス"] == "詳細査定") {
                _letter = "査";
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-warning text-white fst-normal";
                _sort_checkbox = "3";
            }
            else if (_letter == "買" && data[i]["ステータス"] == "買取依頼") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-success text-white fst-normal";
                _sort_checkbox = "4";
            }
            else if (_letter == "確") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-warning ext-dark fst-normal";
                _sort_checkbox = "5";
            }
            else if (_letter == "交") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-info txt-dark fst-normal";
                _sort_checkbox = "6";
            }
            else if (_letter == "成") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-secondary fst-normal";
                _sort_checkbox = "7";
            }
            else if (_letter == "売" && data[i]["ステータス"] == "売主辞退") {
                _letter = "辞";
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-light text-danger fst-normal";
                _sort_checkbox = "8";
            }
            else if (_letter == "買" && data[i]["ステータス"] == "買主辞退") {
                _letter = "辞";
                data[i]["ステータス"] = "買主辞退";
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-dark text-white fst-normal fst-normal";
                _sort_checkbox = "9";
            }
            else if (_letter == "削") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-danger text-white fst-normal fst-normal";
                _sort_checkbox = "10";
            }

        }

        var DeepDatetime = data[i]["詳細査定日時"].replace(" ", "&");
        html += '<tr>\
            <td class= "text-end" > ' + data[i]["NO"] + '</td>\
            <td class="'+ _sort_checkbox + '"><i class="' + _class + '">' + _letter + '</i><span class="font-semibold">' + data[i]["ステータス"] + '</span></td>\
            <td><a class="text-heading font-semibold text-decoration-underline" data-bs-toggle="modal" data-bs-target="#mansion" href="#" id='+ data[i]["SellerMansionID"] + '&' + data[i]["AssReqID"] + '&' + DeepDatetime + ' onclick="Get_PopupFor_Home(this.id)">' + data[i]["マンション名＆部屋番号"] + '</a></td>\
            <td class="text-nowrap"><a class="text-heading font-semibold text-decoration-underline" href="'+ common.appPath + '/t_reale_purchase/Index?RealECD=' + data[i]["RealECD"] + '" id=' + data[i]["RealECD"] + '&t_reale_purchase' + ' onclick="l_logfunction(this.id)"> ' + data[i]["不動産会社"] + '</a></td>\
            <td class="text-nowrap">' + data[i]["登録日時"] + '</td>\
            <td class="text-nowrap">' + data[i]["簡易査定日時"] + '</td>\
            <td class="text-nowrap"> <a class="text-heading font-semibold text-decoration-underline" href="#" id='+ data[i]["AssReqID"] + '&t_seller_assessment_detail' + ' onclick="l_logfunction(this.id)"> ' + data[i]["詳細査定日時"] + ' </a> </td>\
            <td class="text-nowrap"> '+ data[i]["買取依頼日時"] + ' </td>\
            <td class="text-nowrap"> '+ data[i]["送客日時"] + ' </td>\
            <td class="text-nowrap"> '+ data[i]["終了日時"] + '</td>\
            <td class="d-none"> '+ data[i]["SellerMansionID"] + '</td>\
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
        LogStatus: id.split('&')[1]

    };
    common.callAjax(_url.insert_l_log, model,
        function (result) {
            if (result && result.isOK) {
                if (model.LogStatus == "t_reale_purchase") {
                    //alert("https://www.seruichi.com/t_reale_purchase?RealECD=" + model.LogId);
                   //window.location.href = common.appPath + '/t_reale_purchase/Index?RealECD=' + model.LogId;
                }
                else if (model.LogStatus == "t_seller_assessment_detail") {
                    alert(common.appPath +"/t_seller_assessment_detail/Index?AssReqID=" + model.LogId);
                }
            }
            if (result && !result.isOK) {
            }
        });
}


function Get_PopupFor_Home(id) {
   
    var DeepAssDateTime = '';
    if (id.split('&')[2] =='')
    {
    }
    else {
        var Date = id.split('&')[2];
        var Time = id.split('&')[3];
        DeepAssDateTime = Date + ' ' + Time;
    }

    let model = {
        SellerMansionID: id.split('&')[0],
        AssReqID: id.split('&')[1]
    };

    common.callAjax(_url.Get_PopupFor_Home, model,  function (result) {
        if (result && result.isOK) {
            Bind_Popup_Home(result.data);
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

    common.callAjax(_url.Get_PopupFor_ResultType_1, model,  function (result) {
        if (result && result.isOK) {

            Bind_Popup_ResultType_1(result.data, DeepAssDateTime);
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

    common.callAjax(_url.Get_PopupFor_ResultType_2, model,  function (result) {
        if (result && result.isOK) {

            Bind_Popup_ResultType_2(result.data);
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



function Bind_Popup_Home(result) {
    $('#SNameAndRoomNo').empty();
    $('#ekikoutsu').empty();
    let data = JSON.parse(result);
    let html_HomeList = "";
    let htmlTemp = "";
    const isEven = data.length % 2 === 0 ? 'Even' : 'Odd';
   

    html_Home_Menu = '<h4 class="text-center fw-bold">' + data[0]["MansionName"] + ' ' + data[0]["RoomNumber"] + '</h4>\
        <p class="font-monospace small text-start pt-3">'+ data[0]["Address"] + '</p >'
    for (var i = 0; i < data.length; i++) {

        html_HomeList += '<div class="card p-1 col-12 col-md-6 shadow-sm">\
                                                    <div class="card-body p-2 row">\
                                                        <div class="col-12">\
                                                            <h5>'+ data[i]["LineName"] + '</h5>\
                                                        </div>\
                                                        <div class="col-12">\
                                                            <p class="text-dark">'+ data[i]["StationName"] + '</p>\
                                                        </div>\
                                                        <div class="col-12 text-end">\
                                                            <p class="text-dark">徒歩 '+ data[i]["Distance"] + ' 分</p>\
                                                        </div>\
                                                    </div>\
                                                  </div>'
       
    }
    htmlTemp = '<div class="invisible card p-1 col-12 col-md-6 shadow-sm"> </div>'
    $('#SNameAndRoomNo').append(html_Home_Menu);
    if (isEven == 'Odd')
    {
        $('#ekikoutsu').append(html_HomeList + htmlTemp);
    }
    else
    {
        $('#ekikoutsu').append(html_HomeList);
    }
}


function Bind_Popup_ResultType_1(result, DeepAssDateTime) {
    $('#ResultDate').empty();
    $('#tableType1 tbody').empty();
    let data = JSON.parse(result);
    let html_Date = "";
    let html_Type1 = "";
    html_Date = '<h4 class="text-center fw-bold">査定日時</h4>\
                 <p class="font-monospace small text-center pt-0">'+ DeepAssDateTime + '</p >'


    for (var i = 0; i < data.length; i++) {

        html_Type1 += '<tr>\
                      <td>'+ data[i]["Rank"] + '</td>\
                      <td>'+ data[i]["REName"] + '</td>\
                      <td class="align-middle text-end">\
                      <span class="text-danger">'+ data[i]["AssessAmount"] + '</span>円\
                       </td>\
                       </tr>'
    }
    $('#ResultDate').append(html_Date);
    $('#tableType1 tbody').append(html_Type1);
}


function Bind_Popup_ResultType_2(result) {
    $('#tableType2 tbody').empty();
    let data = JSON.parse(result);
    let html_Type2 = "";
    for (var i = 0; i < data.length; i++) {

        html_Type2 += '<tr>\
                      <td>'+ data[i]["Rank"] + '</td>\
                      <td>'+ data[i]["REName"] + '</td>\
                      <td class="align-middle text-end">\
                      <span class="text-danger">'+ data[i]["AssessAmount"] + '</span>円\
                       </td>\
                       </tr>'
    }

    $('#tableType2 tbody').append(html_Type2);
}


function Bind_Popup_Detail(result) {
    let data = JSON.parse(result);
    let html_Detail_header = "";
    let html_Detail_1 = "";
    let html_Detail_2 = "";
    let html_Detail_3 = "";

    for (var i = 0; i < data.length; i++) {

        html_Detail_1 = '<div class="d-inline-flex pb-1">\
            <div class="row">\
                <div class="">\
                    <h6 class="align-self-center"><strong>建物構造</strong><br></h6>\
                            </div>\
                <div class=""><span class="align-self-start">'+ data[0]["StructuralKBN"] + '<br></span></div>\
                    </div>\
                </div>\
                <div class="d-inline-flex pb-1">\
                    <div class="row">\
                        <div class="">\
                            <h6 class="align-self-center"><strong>築年月</strong><br></h6>\
                            </div>\
                            <div class=""><span class="align-self-start">'+ data[0]["ConstYYYYMM"] + '<br></span></div>\
                            </div>\
                        </div>\
                        <div class="d-inline-flex pb-1">\
                            <div class="row">\
                                <div class="">\
                                    <h6 class="align-self-center"><strong>部屋番号</strong><br></h6>\
                            </div>\
                                    <div class=""><span class="align-self-start">'+ data[0]["RoomNumber"] + '<br></span></div>\
                                    </div>\
                                </div>\
                                <div class="d-inline-flex pb-1">\
                                    <div class="row">\
                                        <div class="">\
                                            <h6 class="align-self-center"><strong>専有面積</strong><br></h6>\
                            </div>\
                                            <div class=""><span class="align-self-start">'+ data[0]["RoomArea"].toFixed(2) + ' ㎡<br></span></div>\
                                            </div>\
                                        </div>\
                                        <div class="d-inline-flex pb-1">\
                                            <div class="row">\
                                                <div class="">\
                                                    <h6 class="align-self-center"><strong>バルコニー面積　</strong><br></h6>\
                            </div>\
                                                    <div class=""><span class="align-self-start">'+ data[0]["BalconyArea"].toFixed(2) + ' ㎡<br></span></div>\
                                                    </div>\
                                                </div>\
                                                <div class="d-inline-flex pb-1">\
                                                    <div class="row">\
                                                        <div class="">\
                                                            <h6 class="align-self-center"><strong>主採光</strong><br></h6>\
                            </div>\
                                                            <div class=""><span class="align-self-start">'+ data[0]["Direction"] + '<br></span></div>\
                                                            </div>\
                                                        </div>'


        html_Detail_2 = '<div class="d-inline-flex pb-1">\
            <div class="row">\
                <div class="">\
                    <h6 class="align-self-center"><strong>部屋数</strong><br></h6>\
                            </div>\
                    <div class=""><span class="align-self-start">'+ data[0]["FloorType"] + '部屋<br></span></div>\
                    </div>\
                </div>\
                <div class="d-inline-flex pb-1">\
                    <div class="row">\
                        <div class="">\
                            <h6 class="align-self-center"><strong>バス・トイレ　</strong><br></h6>\
                            </div>\
                            <div class=""><span class="align-self-start">'+ data[0]["BathKBN"] + '<br></span></div>\
                            </div>\
                        </div>\
                        <div class="d-inline-flex pb-1">\
                            <div class="row">\
                                <div class="">\
                                    <h6 class="align-self-center"><strong>土地権利　</strong><br></h6>\
                            </div>\
                             <div class=""><span class="align-self-start">'+ data[0]["RightKBN"] + '<br></span></div>\
                                    </div>\
                                </div>\
                                <div class="d-inline-flex pb-1">\
                                    <div class="row">\
                                        <div class="">\
                                            <h6 class="align-self-center"><strong>現況</strong><br></h6>\
                            </div>\
                                            <div class=""><span class="align-self-start">'+ data[0]["CurrentKBN"] + '<br></span></div>\
                                            </div>\
                                        </div>\
                                        <div class="d-inline-flex pb-1">\
                                            <div class="row">\
                                                <div class="">\
                                                    <h6 class="align-self-center"><strong>管理方式</strong><br></h6>\
                            </div>\
                                                    <div class=""><span class="align-self-start">'+ data[0]["ManagementKBN"] + '<br></span></div>\
                                                    </div>\
                                                </div>\
                                                <div class="d-inline-flex pb-1">\
                                                    <div class="row">\
                                                        <div class="">\
                                                            <h6 class="align-self-center"><strong>売却希望時期　</strong><br></h6>\
                            </div>\
                                                            <div class=""><span class="align-self-start">'+ data[0]["DesiredTime"] + '<br></span></div>\
                                                            </div>\
                                                        </div>'


        html_Detail_3 = '<div class="d-inline-flex pb-1">\
            <div class="row">\
                <div class="">\
                    <h6 class="align-self-center"><strong>家賃</strong><br></h6>\
                            </div>\
                    <div class=""><span class="align-self-start">'+ data[0]["RentFee"] + '円<br></span></div>\
                    </div>\
                </div>\
                <div class="d-inline-flex pb-1">\
                    <div class="row">\
                        <div class="">\
                            <h6 class="align-self-center"><strong>管理賃</strong><br></h6>\
                            </div>\
                            <div class=""><span class="align-self-start">'+ data[0]["ManagementFee"] + '円<br></span></div>\
                            </div>\
                        </div>\
                        <div class="d-inline-flex pb-1">\
                            <div class="row">\
                                <div class="">\
                                    <h6 class="align-self-center"><strong>修繕積立金</strong><br></h6>\
                            </div>\
                                    <div class=""><span class="align-self-start">'+ data[0]["RepairFee"] + '円<br></span></div>\
                                    </div>\
                                </div>\
                                <div class="d-inline-flex pb-1">\
                                    <div class="row">\
                                        <div class="">\
                                            <h6 class="align-self-center"><strong>その他費用</strong><br></h6>\
                            </div>\
                                            <div class=""><span class="align-self-start">'+ data[0]["ExtraFee"] + '円<br></span></div>\
                                            </div>\
                                        </div>\
                                        <div class="d-inline-flex pb-1">\
                                            <div class="row">\
                                                <div class="">\
                                                    <h6 class="align-self-center"><strong>固定資産税</strong><br></h6>\
                            </div>\
                                                    <div class=""><span class="align-self-start">'+ data[0]["PropertyTax"] + '円<br></span></div>\
                                                    </div>\
                                                </div>'
    }
    $('#header1').append(html_Home_Menu);
    $('#Detail_1').append(html_Detail_1);
    $('#Detail_2').append(html_Detail_2);
    $('#Detail_3').append(html_Detail_3);
}

function Bind_Popup_Seller(result) {
    let data = JSON.parse(result);

    let html_Seller_1 = "";
    let html_Seller_2 = "";


    if (data[0]["InvalidFLG"] =="無効会員")
    {
        _classFlag = "text-danger";
    }
    else
    {
        _classFlag = "text-success";
    }

    for (var i = 0; i < data.length; i++) {

        html_Seller_1 = '<div class="align-items-center col-12">\
            <div class="p-md-2 p-1" id = "info">\
                <div class="text-muted"><span class='+ _classFlag+'>'+ data[0]["InvalidFLG"] + '</span></div>\
                <div class="text-muted">'+ data[0]["SellerKana"] + '</div>\
                <div class="text-muted">\
                    <h2>'+ data[0]["SellerName"] + '</h2>\
                </div>\
                <div class="p-md-1 text-muted">\
                    <span class="fa fa-id-card bg-light p-1 rounded-circle"></span>'+ data[0]["SellerCD"] + '\
                        </div>\
                    </div>\
                </div>'

        html_Seller_2 = '<div class="d-flex flex-column col-12" id="info">\
            <div class="p-md-1 text-muted cap-fon-form-w500 ">\
                <span class="fa fa-home bg-light p-1 rounded-circle"></span>'+ data[0]["Address"] + '\
                    </div>\
            <div class="p-md-1 pt-sm-1 text-muted">\
                <span class="fa fa-phone bg-light p-1 rounded-circle"></span> '+ data[0]["Phone"] + '\
                    </div>\
            <div class="p-md-1 pt-sm-1 text-muted">\
                <span class="fa fa-mobile bg-light p-1 ps-2 pe-2 rounded-circle"></span> '+ data[0]["Mobile_Phone"] + '\
                    </div>\
            <div class="p-md-1 text-muted">\
                <span class="fa fa-envelope-o bg-light p-1 rounded-circle"></span>'+ data[0]["MailAddress"] + '\
                    </div>\
                </div>'

    }

    $('#header2').append(html_Home_Menu);
    $('#Seller_1').append(html_Seller_1);
    $('#Seller_2').append(html_Seller_2);
}





