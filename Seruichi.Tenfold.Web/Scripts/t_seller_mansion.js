const _url = {};
let _header_mantion_info = "";
$(function () {
     setValidation();
    _url.getM_SellerMansionList = common.appPath + '/t_seller_mansion/GetM_SellerMansionList';
    _url.generate_M_SellerMansionCSV = common.appPath + '/t_seller_mansion/Generate_M_SellerMansionCSV';
    _url.insert_l_log = common.appPath + '/t_seller_mansion/Insert_l_log';
    _url.Get_Pills_Home = common.appPath + '/t_seller_mansion/Get_Pills_Home';
    _url.Get_Pills_Profile = common.appPath + '/t_seller_mansion/Get_Pills_Profile';
    _url.Get_Pills_Contact = common.appPath + '/t_seller_mansion/Get_Pills_Contact';
    addEvents();
    
    $('#navbarDropdownMenuLink').addClass('font-bold active text-underline');
    $('#t_seller_mansion').addClass('font-bold text-underline');
});
function setValidation() {
    $('#MansionName')
        .addvalidation_errorElement("#errorMansionName")
        .addvalidation_maxlengthCheck(50)//E105
        .addvalidation_doublebyte(); //E107

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
function addEvents()
{
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
        $MansionName = $("#MansionName").val(), $Range = $("#Range").val(), $StartDate = $("#StartDate").val(),
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
        MansionName: $MansionName,
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
        $form = $('#form1').hideChildErrors();

        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        $('#mansiontable tbody').empty();
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        getM_SellerMansionList(model, $form);
    });
    $('#btnCSV').on('click', function () {
        $form = $('#form1').hideChildErrors();
        $('#mansiontable tbody').empty();
        getM_SellerMansionList(model, $form)
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
                    downloadLink.download = "売主マンションリスト_" + dateString + ".csv";

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

    $('.btn-close-modal-t-seller-mansion').on('click', function () {
       $('#pills-home-tab').trigger('click');
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
    if (data.length > 0 ) {
    for (var i = 0; i < data.length; i++) {
       
        if (isEmptyOrSpaces(data[i]["ステータス"])) {
            _letter = "";
            _class = "ms-1 ps-1 pe-1 rounded-circle";
            _sort_checkbox = "";
        }
        else {
            _letter = data[i]["ステータス"].charAt(0);            
            if (_letter == "未") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-primary text-white fst-normal fst-normal";              
                _sort_checkbox = "One";
            }
            else if (_letter == "簡") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-info text-white fst-normal";                
                _sort_checkbox = "Two";
            }
            else if (_letter == "査") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-warning text-white fst-normal";               
                _sort_checkbox = "Three";
            }
            else if (_letter == "買" && data[i]["ステータス"] =="買取依頼") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-success text-white fst-normal";               
                _sort_checkbox = "Four";
            }
            else if (_letter == "確") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-warning ext-dark fst-normal";              
                _sort_checkbox = "Five";
            }
            else if (_letter == "交") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-info txt-dark fst-normal";               
                _sort_checkbox = "Six";
            }
            else if (_letter == "成") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-secondary text-dark fst-normal";              
                _sort_checkbox = "Seven";
            }
            else if (_letter == "売" && data[i]["EndStatus"] == 2 && data[i]["ステータス"] == "売主辞退") {
                _letter = "辞";
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-light text-danger fst-normal";               
                _sort_checkbox = "Eight";
            }
            else if (_letter == "売" && data[i]["EndStatus"] == 3 && data[i]["ステータス"] == "売主辞退") {
                _letter = "辞";
                data[i]["ステータス"] = "買主辞退";
                _class = "ms-1 ps-1 pe-1 rounded-circle  bg-dark text-white fst-normal";               
                _sort_checkbox = "Nine";
            }

        }
        html += '<tr>\
            <td class= "text-end"> ' + data[i]["NO"] + '</td>\
            <td class="'+ _sort_checkbox + '"><i class="' + _class + '">' + _letter + '</i><span class="font-semibold"> ' + data[i]["ステータス"] + '</span></td>\
            <td> ' + data[i]["物件CD"] + ' </td>\
            <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" data-bs-toggle="modal" data-bs-target="#mansion" id='+ data[i]["SellerCD"] + '&' + data[i]["物件CD"] + '  href="#" onclick="modal_popup(this.id)">' + data[i]["マンション名"] + '</a><p class="text-wrap w-100">' + data[i]["住所"] + '</p><p class="d-none">' + data[i]["PrefCD"] + '</p></td>\
            <td> '+ data[i]["部屋"] + '</td>\
            <td class="text-end">'+ data[i]["階数"] + '</td>\
            <td class="text-end">'+ data[i]["面積"].toFixed(2) + '</td>\
            <td> <a class="text-heading font-semibold text-decoration-underline" href="#" id='+ data[i]["SellerCD"] + '&t_seller_assessment' + ' onclick="l_logfunction(this.id)">' + data[i]["売主名"] + '</a></td>\
            <td class="text-nowrap"> '+ data[i]["登録日時"] + '</td>\
            <td class="text-nowrap"> <a class="text-heading font-semibold text-decoration-underline" href="#" id='+ data[i]["AssReqID"] +'&t_seller_assessment_detail'+' onclick="l_logfunction(this.id)"> '+ data[i]["詳細査定日時"] + ' </a> </td>\
            <td class="text-nowrap"> '+ data[i]["買取依頼日時"] + ' </td>\
            <td class="text-nowrap">\
            <a class="text-heading font-semibold text-decoration-underline" href="#" id='+ data[i]["GRealECD"] + '&t_seller_assessment_detail_GReal' + ' onclick="l_logfunction(this.id)" >' + data[i]["マンションTop1"] + ' </a><p class="text-end">' + data[i]["マンション金額"] + '</p> </td>\
             <td class="text-nowrap">\
             <a class="text-heading font-semibold text-decoration-underline" href="#" id='+ data[i]["ERealECD"] + '&t_seller_assessment_detail_EReal' + ' onclick="l_logfunction(this.id)"> ' + data[i]["エリアTop1"]+' </a> <p class="text-end">' + data[i]["エリア金額"] + '</p></td>\
             <td class="text-nowrap"> <a class="text-heading font-semibold text-decoration-underline" href="#" id='+ data[i]["IRealECD"] + '&t_seller_assessment_detail_IRealECD' + ' onclick="l_logfunction(this.id)"> ' + data[i]["買取依頼会社"]+' </a><p class="text-end">' + data[i]["買取依頼金額"] + '</p> </td>\
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
        $('#no_record').text("表示可能データがありません");
    }
    $('#mansiontable tbody').append(html);
    
}
function l_logfunction(id) {   
    let model = {
        LoginKBN:null,
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
                if (model.LogStatus == "t_mansion_detail")
                    alert("https://www.seruichi.com/t_mansion_detail?ｍansionCD=" + model.LogId);
                else if (model.LogStatus == "t_seller_assessment")
                   // alert("https://www.seruichi.com/t_seller_assessment?seller=" + model.LogId);
                    window.location.href = common.appPath + '/t_seller_assessment/Index?SellerCD=' + model.LogId;
                else if (model.LogStatus == "t_seller_assessment_detail")
                    alert("https://www.seruichi.com/t_seller_assessment_detail?AssReqID=" + model.LogId);
                else if (model.LogStatus == "t_seller_assessment_detail_GReal")
                    alert("https://www.seruichi.com/t_reale_purchase?realestate=" + model.LogId);
                else if (model.LogStatus == "t_seller_assessment_detail_EReal")
                    alert("https://www.seruichi.com/t_reale_purchase?realestate=" + model.LogId);
                else if (model.LogStatus == "t_seller_assessment_detail_IRealECD")
                    alert("https://www.seruichi.com/t_reale_purchase?realestate=" + model.LogId);
            }
            if (result && !result.isOK) {                
            }
        });
}

function modal_popup(id) {
    var seller_CD = id.split('&')[0];
    var sellerMansion_ID = id.split('&')[1];

    let model = {
        SellerCD: seller_CD,
        SellerMansionID: sellerMansion_ID
    };

    common.callAjax(_url.Get_Pills_Home, model, function (result) {
        if (result && result.isOK) {
            Bind_popup_home(result.data);
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

    common.callAjax(_url.Get_Pills_Profile, model, function (result) {
        if (result && result.isOK) {
            Bind_popup_profile(result.data);
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

    common.callAjax(_url.Get_Pills_Contact, model, function (result) {
        if (result && result.isOK) {
            Bind_popup_contact(result.data);
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

function Bind_popup_home(result) {
    $('#_header_mantion_info_home').empty('');
    $('#ekikoutsu').empty();
    let data = JSON.parse(result);

    let html_HomeList = "";
    let htmlTemp = "";
    const isEven = data.length % 2 === 0 ? 'Even' : 'Odd';

    _header_mantion_info = '<h4 class="text-center fw-bold">' + data[0]["MansionName"] + ' ' + data[0]["RoomNumber"] + '</h4>\
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

    $('#_header_mantion_info_home').append(_header_mantion_info);

    if (isEven == 'Odd') {
        $('#ekikoutsu').append(html_HomeList + htmlTemp);
    }
    else {
        $('#ekikoutsu').append(html_HomeList);
    }
}

function Bind_popup_profile(result) {
    $('#_header_mantion_info_profile').empty('');
    $('#profile-first-column').empty('');
    $('#profile-second-column').empty('');
    $('#profile-third-column').empty('');

    let profile_first_column = "";
    let profile_second_column = "";
    let profile_third_column = "";

    let data = JSON.parse(result);

    for (var i = 0; i < data.length; i++) {

        profile_first_column = '<div class="d-inline-flex pb-1">\
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


        profile_second_column = '<div class="d-inline-flex pb-1">\
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


        profile_third_column = '<div class="d-inline-flex pb-1">\
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
    $('#_header_mantion_info_profile').append(_header_mantion_info);
    $('#profile-first-column').append(profile_first_column);
    $('#profile-second-column').append(profile_second_column);
    $('#profile-third-column').append(profile_third_column);
}

function Bind_popup_contact(result) {
    $('#_header_mantion_info_contact').empty('');
    $('#contact-first').empty('');
    $('#contact-second').empty('');

    let contact_first = "";
    let contact_second = "";


    let data = JSON.parse(result);

    if (data.length > 0) {
        if (data[0]["InvalidFLG"] == "無効会員") {
            _classFlag = "text-danger";
        }
        else {
            _classFlag = "text-success";
        }

        contact_first = '<div class="align-items-center col-12">\
            <div class="p-md-2 p-1" id = "info">\
                <div class="text-muted"><span class='+ _classFlag + '>' + data[0]["InvalidFLG"] + '</span></div>\
                <div class="text-muted">'+ data[0]["SellerKana"] + '</div>\
                <div class="text-muted">\
                    <h2>'+ data[0]["SellerName"] + '</h2>\
                </div>\
                <div class="p-md-1 text-muted">\
                    <span class="fa fa-id-card bg-light p-1 rounded-circle"></span>'+ data[0]["SellerCD"] + '\
                        </div>\
                    </div>\
                </div>'

        contact_second = '<div class="d-flex flex-column col-12" id="info">\
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

        $('#_header_mantion_info_contact').append(_header_mantion_info);
        $('#contact-first').append(contact_first);
        $('#contact-second').append(contact_second);
    }
}
