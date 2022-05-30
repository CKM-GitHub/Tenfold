const _url = {};
$(function () {
    setValidation();
    _url.get_t_reale_purchase_CompanyInfo = common.appPath + '/t_reale_purchase/get_t_reale_purchase_CompanyInfo';
    _url.get_t_reale_purchase_CompanyCountingInfo = common.appPath + '/t_reale_purchase/get_t_reale_purchase_CompanyCountingInfo';
    _url.get_t_reale_purchase_DisplayData = common.appPath + '/t_reale_purchase/get_t_reale_purchase_DisplayData';
    _url.get_t_reale_purchase_CSVData = common.appPath + '/t_reale_purchase/get_t_reale_purchase_CSVData';
    _url.Insert_L_Log = common.appPath + '/t_reale_purchase/Insert_L_Log';
    _url.Get_Pills_Home = common.appPath + '/t_reale_purchase/Get_Pills_Home';
    _url.Get_Pills_Profile = common.appPath + '/t_reale_purchase/Get_Pills_Profile';
    _url.Get_Pills_Contact = common.appPath + '/t_reale_purchase/Get_Pills_Contact';
    addEvents();
    $('#navbarDropdownMenuLink').addClass('font-bold active text-underline');
    $('#t_reale_purchase').addClass('font-bold text-underline');
});

function setValidation() {
    $('.form-check-input')
        .addvalidation_errorElement("#CheckBoxError")
        .addvalidation_checkboxlenght(); //E112

    $('#StartDate')
        .addvalidation_errorElement("#errorStartDate")
        .addvalidation_datecheck() //E108
        .addvalidation_datecompare(); //E111

    $('#EndDate')
        .addvalidation_errorElement("#errorEndDate")
        .addvalidation_datecheck() //E108
        .addvalidation_datecompare(); //E111
}

function addEvents() {
    common.bindValidationEvent('#form1', '');

    $('.form-check-input').on('change', function () {
        this.value = this.checked ? 1 : 0;
        if ($("input[type=checkbox]:checked").length > 0) {
            $('.form-check-input').hideError();
        }
    }).change();

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

    let model = {
        RealECD: common.getUrlParameter('reale'),
        chk_Purchase: $("#chk_Purchase").val(),
        chk_Checking: $("#chk_Checking").val(),
        chk_Nego: $("#chk_Nego").val(),
        chk_Contract: $("#chk_Contract").val(),
        chk_SellerDeclined: $("#chk_SellerDeclined").val(),
        chk_BuyerDeclined: $("#chk_BuyerDeclined").val(),
        Range: $("#ddlRange").val(),
        StartDate: $("#StartDate").val(),
        EndDate: $("#EndDate").val()
    };
    get_purchase_Data(model, this);

    sortTable.getSortingTable("tblPurchaseDetails");

    $('#btnDisplay').on('click', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        $('#tblPurchaseDetails tbody').empty();
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        get_purchase_Data(model, $form);
    });

    $('#btnCSV').on('click', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        $('#tblPurchaseDetails tbody').empty();
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        get_purchase_Data(model, $form);

        common.callAjax(_url.get_t_reale_purchase_CSVData, model,
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
                    downloadLink.download = "案件一覧_" + dateString + ".csv";

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

function get_purchase_Data(model, $form) {
    common.callAjaxWithLoading(_url.get_t_reale_purchase_CompanyInfo, model, this, function (result) {
        if (result && result.isOK) {
            Bind_CompanyInfo(result.data);
            common.callAjaxWithLoading(_url.get_t_reale_purchase_CompanyCountingInfo, model, this, function (result) {
                if (result && result.isOK) {
                    Bind_CountingInfo(result.data);
                    common.callAjaxWithLoading(_url.get_t_reale_purchase_DisplayData, model, this, function (result) {
                        if (result && result.isOK) {
                            Bind_DisplayData(result.data);
                        }
                    })
                }
            })
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

function Bind_CompanyInfo(result) {
    var data = JSON.parse(result);
    if (data.length > 0) {
        $('#InvalidFLG').text(data[0]['InvalidFLG']);
        $('#REKana').text(data[0]['REKana']);
        $('#REName').text(data[0]['REName']);
        $('#RealECD').text(data[0]['RealECD']);
        $('#Address').text(data[0]['Address']);
        $('#HousePhone').text(data[0]['HousePhone']);
        $('#Fax').text(data[0]['Fax']);
        $('#MailAddress').text(data[0]['MailAddress']);
        $('#PICName').text(data[0]['PICName']);
    }
}

function Bind_CountingInfo(result) {
    var data = JSON.parse(result);
    if (data.length > 0) {
        $('#Constant').text(data[0]['査定数']);
        $('#Top5').text(data[0]['Top5']);
        $('#Top1').text(data[0]['Top1']);
        $('#Customers').text(data[0]['送客数']);
        $('#Deals').text(data[0]['商談数']);
        $('#Contracts').text(data[0]['成約数']);
        $('#Seller_Declines').text(data[0]['売主辞退数']);
        $('#Buyer_Declines').text(data[0]['買主辞退数']);
    }
}

function Bind_DisplayData(result) {
    let _letter = "", _class = "";
    let data = JSON.parse(result);
    let html = "";
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {

            if (isEmptyOrSpaces(data[i]["ステータス"])) {
                _letter = "";
                _class = "ms-1 ps-1 pe-1 rounded-circle";
            }
            else {
                if (data[i]["ステータス"] == "1") {
                    _letter = "買";
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-success text-white";
                }
                else if (data[i]["ステータス"] == "2") {
                    _letter = "確";
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-warning ext-dark";
                }
                else if (data[i]["ステータス"] == "3") {
                    _letter = "交";
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-info txt-dark";
                }
                else if (data[i]["ステータス"] == "4") {
                    _letter = "成";
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-secondary";
                }
                else if (data[i]["ステータス"] == "5") {
                    _letter = "辞";
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-light text-danger";
                }
                else if (data[i]["ステータス"] == "6") {
                    _letter = "辞";
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-dark text-white";
                }
            }

            html += '<tr>\
            <td class= "text-end"> ' + (i + 1) + '</td>\
            <td><span class="' + _class + '">' + _letter + '</span><span class="font-semibold"> ' + data[i]["ステータス名"] + '</span></td>\
            <td>'+ data[i]["ステータス"] + '</td>\
            <td>'+ data[i]["査定依頼ID"] + '</td>\
            <td>'+ data[i]["売主保持物件ID"] + '</td>\
            <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" id='+ data[i]["査定依頼ID"] + '&r_issueslist' + '  href="#" onclick="l_logfunction(this.id)"><span>' + data[i]["物件名"] + '</span><span>' + data[i]["部屋番号"] + '</span></a></td>\
            <td>'+ data[i]["依頼売主CD"] + '</td>\
            <td>'+ data[i]["売主_カナ"] + '</td>\
            <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" data-bs-toggle="modal" data-bs-target="#SellerDetails" id='+ data[i]["依頼売主CD"] + ' href="#" onclick="Bind_SellerDetails_Popup(this)">' + data[i]["お客様名"] + '</a></td>\
            <td>'+ data[i]["売主_住所１"] + data[i]["売主_住所２"] + data[i]["売主_住所３"] + data[i]["売主_住所４"] + data[i]["売主_住所５"] + '</td>\
            <td>'+ data[i]["売主_固定電話番号"] + '</td>\
            <td>'+ data[i]["売主_携帯電話番号"] + '</td>\
            <td>'+ data[i]["売主_メールアドレス"] + '</td>\
            <td class="text-nowrap">'+ data[i]["買取依頼日時"] + '</td>\
            <td class="text-nowrap"> '+ data[i]["終了日時"] + '</td>\
            <td class="text-end text-nowrap"> '+ data[i]["査定価格"] + '</td>\
            <td>'+ data[i]["不動産担当者CD"] + '</td>\
            <td class="text-nowrap"> '+ data[i]["担当者名"] + '</td>\
            </tr>'
        }

        $('#total_record').text("検索結果： " + data.length + "件");
        $('#total_record_up').text("検索結果： " + data.length + "件");
        $('#no_record').text("");
    }
    else {
        $('#total_record').text("検索結果： 0件");
        $('#total_record_up').text("検索結果： 0件 ");
        $('#no_record').text("表示可能データがありません");
    }
    $('#tblPurchaseDetails tbody').append(html);
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
