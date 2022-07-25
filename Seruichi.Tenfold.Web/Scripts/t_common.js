//Start script code of t_reale_Info.cshtml -> Row No. 2 ~ 52
function Bind_Company_Data() {
    //t_reale Info
    if (!$('#reale').hasClass('d-none')) {
        let model = {
            RealECD: common.getUrlParameter('RealECD')
        };

        common.callAjaxWithLoading(_url.get_t_reale_CompanyInfo, model, this, function (result) {
            if (result && result.isOK) {
                Bind_CompanyInfo(model, result.data);
                common.callAjaxWithLoading(_url.get_t_reale_CompanyCountingInfo, model, this, function (result) {
                    if (result && result.isOK) {
                        Bind_CompanyCountingInfo(result.data);
                    }
                })
            }

            if (result && !result.isOK) {
            }
        });
    }
    //t_seller Info
    else if (!$('#seller').hasClass('d-none')) {
        let model = {
            SellerCD: common.getUrlParameter('SellerCD')
        };

        common.callAjaxWithLoading(_url.get_t_seller_Info, model, this, function (result) {
            if (result && result.isOK) {
                Bind_SellerInfo(model, result.data);
            }

            if (result && !result.isOK) {
            }
        });
    }
}
function addNaShi() {
    var html = '\
                    <div class="card m-1 p-0 count-list-no">\
                        <div class="card-body mt-1 pt-1 mb-1 pb-1">\
                            <div class="d-flex justify-content-between">\
                                <div class="d-flex flex-row align-items-center">\
                                    <div class="ms-3">\
                                        <p class="small mb-0" style="color:red;">添付ファイルなし</p>\
                                    </div>\
                                </div>\
                            </div>\
                        </div>\
                    </div>';
    $('#FileList').html('');
    $('#FileList').append(html);
    $('#inputFile').val(null);
}


function Bind_CompanyInfo(model, result) {
    var data = JSON.parse(result);
    if (data.length > 0) {
        if (data[0]['InvalidFLG'] == '有効会員')
            $('#r_InvalidFLG').addClass('text-success');
        else
            $('#r_InvalidFLG').addClass('text-danger');

        $('#r_InvalidFLG').text(data[0]['InvalidFLG']);
        $('#r_REKana').text(data[0]['REKana']);
        $('#r_REName').text(data[0]['REName']);

        $('#r_RealECD').text(data[0]['RealECD']);
        $('#r_Address').text(data[0]['Address']);
        $('#r_HousePhone').text(data[0]['HousePhone']);
        $('#r_Fax').text(data[0]['Fax']);
        $('#r_MailAddress').text(data[0]['MailAddress']);
        $('#r_PICName').text(data[0]['PICName']);
        $('#r_RealName').text(data[0]['REName']);
    }
}

function Bind_CompanyCountingInfo(result) {
    var data = JSON.parse(result);
    if (data.length > 0) {
        $('#r_Constant').text(data[0]['査定数']);
        $('#r_Top5').text(data[0]['Top5']);
        $('#r_Top1').text(data[0]['Top1']);
        $('#r_Customers').text(data[0]['送客数']);
        $('#r_Deals').text(data[0]['商談数']);
        $('#r_Contracts').text(data[0]['成約数']);
        $('#r_Seller_Declines').text(data[0]['売主辞退数']);
        $('#r_Buyer_Declines').text(data[0]['買主辞退数']);
    }
}

function Bind_SellerInfo(model, result) {
    var data = JSON.parse(result);
    if (data.length > 0) {
        if (data[0]['InvalidFLG'] == '有効会員')
            $('#s_InvalidFLG').addClass('text-success');
        else
            $('#s_InvalidFLG').addClass('text-danger');

        $('#s_InvalidFLG').text(data[0]['InvalidFLG']);
        $('#s_SellerKana').text(data[0]['SellerKana']);
        $('#s_SellerName').text(data[0]['SellerName']);
        $('#s_SellerCD').text(data[0]['SellerCD']);

        $('#s_Address').text(data[0]['PrefName'] + data[0]['CityName'] + data[0]['TownName'] + data[0]['Address1'] + data[0]['Address2']);
        $('#s_HousePhone').text(data[0]['HousePhone']);
        $('#s_HandyPhone').text(data[0]['HandyPhone']);
        $('#s_MailAddress').text(data[0]['MailAddress']);

        $('#s_BillingAmt').text(data[0]['今月課金']);
        $('#s_Assessment_constant').text(data[0]['査定数']);
        $('#s_Num_contracts').text(data[0]['成約数']);
        $('#s_Num_seller_declines').text(data[0]['売主辞退数']);
        $('#s_Num_buyer_declines').text(data[0]['買主辞退数']);

        $('#s_InsertDateTime').text(data[0]['会員登録日']);
        $('#s_DeepAssDateTime').text(data[0]['査定依頼日']);
        $('#s_PurchReqDateTime').text(data[0]['買取依頼日']);
        $('#s_LoginDateTime').text(data[0]['最終ログイン日']);
    }
}
//End script code of t_reale_Info.cshtml
function isNullAndUndef(variable) {

    return (variable !== null && variable !== undefined);
}
function Bind_Assess_Guide_View(id, requestInfo) {
    
    $('#req_MansionName').text(requestInfo.split('&')[0]);
    $('#req_RoomNumber').text(' ' + requestInfo.split('&')[1]);
    $('#req_Address').text(requestInfo.split('&')[2]);
    $('#req_Purchase').text('買取依頼日時 ' + requestInfo.split('&')[3]);

    $('#req_ddlstatus').val(requestInfo.split('&')[4]); model_Cache.ProcessKBN = $('#req_ddlstatus').val();
    $('#req_ddlStaff').val(requestInfo.split('&')[5]); model_Cache.TenStaffCD = $('#req_ddlStaff').val();

    $('#req_SendDate').val(requestInfo.split('&')[6]); model_Cache.IntroPlanDate = $('#req_SendDate').val();
    $('#req_SendCompany').val(requestInfo.split('&')[7]);
    $('#req_remarks').val(requestInfo.split('&')[8]); model_Cache.Remark = $('#req_remarks').val();


    if (requestInfo.split('&')[9] === 'null') {
        $('#Chk_req_Name_Kanji').prop('checked', true);
        $('#Chk_req_Name_Kanji').prop('value', 1);
        model_Cache.NameConf = 1;
    }
    else 
        $('#Chk_req_Name_Kanji').prop('checked', false);


    if (requestInfo.split('&')[10] === 'null') {
        $('#Chk_req_pc_Address').prop('checked', true);
        $('#Chk_req_pc_Address').prop('value', 1);
        model_Cache.AddressConf = 1; 
    }
    else
        $('#Chk_req_pc_Address').prop('checked', false);


    if (requestInfo.split('&')[11] === 'null') {
        $('#Chk_req_pc_phone').prop('checked', true);
        $('#Chk_req_pc_phone').prop('value', 1);
        model_Cache.TelConf = 1; 
    }
    else
        $('#Chk_req_pc_phone').prop('checked', false);

    if (requestInfo.split('&')[12] === 'null') {
        $('#Chk_req_pc_mail_address').prop('checked', true);
        $('#Chk_req_pc_mail_address').prop('value', 1);
        model_Cache.TelConf = 1; 
    }
    else
        $('#Chk_req_pc_mail_address').prop('checked', false);

    if (requestInfo.split('&')[13] === 'null') {
        $('#Chk_CopyInfo').prop('checked', true);
        $('#Chk_CopyInfo').prop('value', 1);
        model_Cache.RegisConf = 1;
    }
    else
        $('#Chk_CopyInfo').prop('checked', false);
     
    $('#hdn_AssReID').val((requestInfo.split('&')[14]));
    model_Cache.IntroReqID = $('#hdn_AssReID').val();
    $('#hdn_IsSendOK').val((requestInfo.split('&')[15]));
    //hdn_IsSendOK
    let model1 = {
        IntroReqID: $('#hdn_AssReID').val()
    }
    common.callAjax(_url.get_t_assess_guide_AttachFiles, model1, function (result) {
        if (result && result.isOK) {
            var data = JSON.parse(result.data)
            if (data.length) {
                var html = '';
                for (var i = 0; i < data.length; i++) {
                    var ext = data[i]["AttachFileName"] + data[i]["AttachFileType"];
                    var idDelete = 'attach_' + data[i]["IntroReqID"] + '_' + data[i]["AttachSEQ"] + '_' + data[i]["AttachFileUserCD"];
                    var idDown = 'down_' + data[i]["IntroReqID"] + '_' + data[i]["AttachSEQ"] + '_' + data[i]["AttachFileUserCD"];

                    html += '<div class="card m-1 p-0 count-list-original" >\
                <div class="card-body mt-1 pt-1 mb-1 pb-1">\
                    <div class="d-flex justify-content-between">\
                        <div class="d-flex flex-row align-items-center">\
                            <div class="ms-3">\
                                <a href="#" id="'+ idDown + '" onclick="downloadAttach(this)" class="text-underline">\
                                    <h5>'+ ext + '</h5>\
                                </a>\
                                <p class="small mb-0">アップロード日時：'+ data[i]["AttachFileDateTime1"] + '</p>\
                                <p class="small mb-0"> ('+ data[i]["AttachFileSize"] + ')</p>\
                            </div>\
                        </div>\
                        <div class="d-flex flex-row align-items-center">\
                            <a href="#" class="text-secondary" id="'+ idDelete + '" onclick="CloseTemp(this)"><i class="fa fa-close"></i></a>\
                        </div>\
                    </div>\
                </div></div>';
                }
                $('#list_AttachItem').val('');
                $('#FileList').html('');
                $('#FileList').append(html);
            }
            else {
                addNaShi();
            }
        }
    });



}
//Start script code of t_reale_Modal.cshtml -> Row No. 64 ~ 
function Bind_ModalDetails(id, requestInfo) {
    if (requestInfo) { 
        Bind_Assess_Guide_View(id, requestInfo);
    } 
    
    var DeepAssDateTime = '';
    if (id.split('&')[3] == '') {
    }
    else {
        var Date = id.split('&')[3];
        var Time = id.split('&')[4];
        DeepAssDateTime = Date + ' ' + Time;
    }

    let model = {
        SellerMansionID: id.split('&')[0],
        SellerCD: id.split('&')[1],
        AssReqID: id.split('&')[2]
    };

    $('#pills-home-tab').click();

    common.callAjax(_url.get_Modal_HomeData, model, function (result) {
        if (result && result.isOK) {
            Bind_Modal_HomeData(result.data);
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

    common.callAjax(_url.get_Modal_ProfileData, model, function (result) {
        if (result && result.isOK) {
            Bind_Modal_ProfileData(result.data);
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

    common.callAjax(_url.get_Modal_ContactData, model, function (result) {
        if (result && result.isOK) {
            Bind_Modal_ContactData(result.data);
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

    common.callAjax(_url.get_Modal_DetailData, model, function (result) {
        if (result && result.isOK) {
            Bind_Modal_DetailData(result.data, DeepAssDateTime);
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

function Bind_Modal_HomeData(result) {
    $('#H_MansionInfo').empty();
    $('#ekikoutsu').empty();
    let data = JSON.parse(result);
    let MansionInfo = "";
    let html = "";
    let htmlTemp = "";
    const isEven = data.length % 2 === 0 ? 'Even' : 'Odd';

    if (data.length > 0) {
        //Display Data of MansionInfo
        MansionInfo = '<h4 class="text-center fw-bold">' + data[0]["MansionName"] + ' ' + data[0]["RoomNumber"] + '</h4>\
        <p class="font-monospace small text-start pt-3">'+ data[0]["Address"] + '</p >'
        $('#H_MansionInfo').append(MansionInfo);
    }

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

function Bind_Modal_ProfileData(result) {
    var data = JSON.parse(result);
    if (data.length > 0) {
        $('#Building_structure').text(data[0]['建物構造']);
        $('#Year_of_construction').text(data[0]['築年数']);
        $('#Room_No').text(data[0]['部屋番号']);
        $('#Area').text(data[0]['専有面積']);
        $('#Balcony_area').text(data[0]['バルコニー面積']);
        $('#Sunlighting').text(data[0]['主採光']);
        $('#Number_of_rooms').text(data[0]['間取り']);
        $('#Bathrooms_and_toilets').text(data[0]['バス・トイレ']);
        $('#land_rights').text(data[0]['土地権利']);
        $('#present_state').text(data[0]['現況']);
        $('#management_system').text(data[0]['管理方式']);
        $('#Desired_time_of_sale').text(data[0]['売却希望時期']);
        $('#rent_fee').text(data[0]['家賃'] + '円');
        $('#management_fee').text(data[0]['管理費'] + '円');
        $('#repair_fee').text(data[0]['修繕積立金'] + '円');
        $('#etc_fee').text(data[0]['その他費用'] + '円');
        $('#tax').text(data[0]['固定資産税'] + '円');
    }
}

function Bind_Modal_ContactData(result) {
    var data = JSON.parse(result);
    if (data.length > 0) {
        if (data[0]['有効_無効'] == '有効会員')
            $('#Enable_or_Disable').addClass('text-success');
        else
            $('#Enable_or_Disable').addClass('text-danger');

        $('#Enable_or_Disable').text(data[0]['有効_無効']);
        $('#Name_Katakana').text(data[0]['カナ名']);
        $('#Name_Kanji').text(data[0]['漢字名']);
        $('#pc_SellerCD').text(data[0]['ユーザーCD']);
        $('#pc_Address').text(data[0]['PrefName'] + data[0]['CityName'] + data[0]['TownName'] + data[0]['Address1'] + data[0]['Address2']);
        $('#pc_Phone').text(data[0]['固定電話']);
        $('#pc_Mobile_phone').text(data[0]['携帯電話']);
        $('#pc_mail_address').text(data[0]['メールアドレス']);

        //request
        $('#req_Name_Kanji').text(data[0]['漢字名']);
        $('#req_pc_Address').text(data[0]['PrefName'] + data[0]['CityName'] + data[0]['TownName'] + data[0]['Address1'] + data[0]['Address2']);
        $('#req_pc_Phone').text(data[0]['固定電話']);
        $('#req_pc_Mobile_phone').text(data[0]['携帯電話']);
        $('#req_pc_mail_address').text(data[0]['メールアドレス']);
    }
}

function Bind_Modal_DetailData(result, DeepAssDateTime) {
    var c_html = "", a_html = "";
    $('#CondominiumAssResults tbody').empty();
    $('#AreaAssResults tbody').empty();
    var data = JSON.parse(result);
    $('#DeepAssDateTime').text(DeepAssDateTime);
    for (var i = 0; i < data.length; i++) {
        if (data[i]["Type"] == "1") {
            c_html += '<tr>\
                    <td>'+ data[i]["Rank"] + '</td>\
                    <td>'+ data[i]["REName"] + '</td>\
                    <td class="align-middle text-end"><span class="text-danger">'+ data[i]["AssessAmount"] + '</span>円</td>\
                   </tr>'
        }
        else {
            a_html += '<tr>\
                    <td>'+ data[i]["Rank"] + '</td>\
                    <td>'+ data[i]["REName"] + '</td>\
                    <td class="align-middle text-end"><span class="text-danger">'+ data[i]["AssessAmount"] + '</span>円</td>\
                   </tr>'
        }
    }
    $('#CondominiumAssResults tbody').append(c_html);
    $('#AreaAssResults tbody').append(a_html);
}

//End script code of t_reale_Modal.cshtml

//Start SubMenu
$(function () {
    BindReturnURL();
    Bind_SubMennuURL();
    Bind_Company_Data();         //Bind Company Info Data to the title part of the page
    
    $('ul li a').each(function (idx, a) {
        if ($(a).prop('href') == window.location.href)
            $(this).addClass('active');
        else
            $(this).removeClass('active');
    });

    $(document).on("click", "#btn_t_common_cancle_reload", function (event) {
        window.location.reload();
    });
    $(document).on("click", "#btn_t_common_save_success_popup", function (event) {
        window.location.reload();
    });
})
function BindReturnURL()
{
    $('.page-reale-url').click(function (e) {

        if ($('#_PREVIOUS_REALURL').val() != '') {
            if (document.referrer.trim().length != 0)
            this.href = $('#_PREVIOUS_REALURL').val();
        }
    });
    $('.page-sellerasm-url').click(function (e) {

        if ($('#_PREVIOUS_SELLERURL').val() != '') {
            if (document.referrer.trim().length != 0)
            this.href = $('#_PREVIOUS_SELLERURL').val();
        }
    });
}
function Bind_SubMennuURL() {
    var target;
    //for t_real
    var listItems = $("#subMenu li");

    $('#subMenu li').children('a').removeAttr("href");
    let current = window.location.href;
    listItems.each(function (idx, li) {
        var anchor = $(li).children('a');
        target = current;
        target = target.slice(0, target.lastIndexOf('/'));
        target = target.substring(target.lastIndexOf('/') + 1);
        var newHref = current.replace(target, anchor.attr('name'));
        anchor.prop('href', newHref);
    });
    //debugger
    //for Tseller
    var listItemsSeller = $("#subMenu_Seller li");

    $('#subMenu_Seller li').children('a').removeAttr("href");
    let current_Seller = window.location.href;
    listItemsSeller.each(function (idx, li) {
        var anchor = $(li).children('a');
        target = current_Seller;
        target = target.slice(0, target.lastIndexOf('/'));
        target = target.substring(target.lastIndexOf('/') + 1);
        var newHref = current_Seller.replace(target, anchor.attr('name'));
        anchor.prop('href', newHref);
    });
    SellerAssessMentMenu();

    target = window.location.href;
    if (target.includes('t_reale')) {
        $('#reale').removeClass('d-none');
        $('#submenu_reale').removeClass('d-none');
    }
    else if (target.includes('t_seller')) {
        $('#seller').removeClass('d-none');
        $('#submenu_seller').removeClass('d-none');
    }
    //menu_seller_assess
}
function SellerAssessMentMenu() {
    // tseller_assess
    var listItemsSeller = $("#menu_seller_assess li");

    $('#menu_seller_assess li').children('a').removeAttr("href");
    let current_Seller = window.location.href;
    listItemsSeller.each(function (idx, li) {
        var anchor = $(li).children('a');
        var target = current_Seller;
        target = target.slice(0, target.lastIndexOf('/'));
        target = target.substring(target.lastIndexOf('/') + 1);
        var newHref = current_Seller.replace(target, anchor.attr('name'));
        anchor.prop('href', newHref);
    });
}
//EndSubMenu  subMenu_Seller


//変更取消
function Cancel_Change(id) {
    let cancelModal = "";
    cancelModal = '<div class="modal-dialog modal-lg modal-dialog-centered" role = "document">\
                    <div class="modal-content">\
                        <div class="modal-header">\
                            <h4 class="modal-title">変更取消</h4><button type="button" class="btn-close d-none" data-bs-dismiss="modal" aria-label="Close"></button>\
                        </div>\
                        <div class="modal-body">\
                            <section class="newsletter-subscribe pt-0 p-0">\
                                <div class="container">\
                                    <div class="intro"></div>\
                                    <form class="d-flex justify-content-center flex-wrap mx-auto"  style="text-align: center;">\
                                        <h1 class="cap-icon-h1 border border-danger border-8"><i class="fa fa-exclamation-circle pt-1 pb-1 ps-3 pe-3 cap-icon-i text-danger"></i></h1>\
                                    </form>\
                                </div>\
                                <h4 class="text-center align-self-center" id="PE317"></h4>\
                            </section>\
                        </div>\
                        <div class="modal-footer">\
                            <button type="button" class="btn btn-lg btn-primary" id="btn_t_common_cancle_reload">破棄</button><button type="button" class="btn btn-lg btn-secondary" data-bs-dismiss="modal">閉じる</button>\
                        </div>\
                    </div>\
                </div>'
    $('#' + id).append(cancelModal);
}

//変更保存 
function Save_Change(id) {
    let save_chg = '<div class="modal-dialog modal-lg modal-dialog-centered" role="document">\
        <div class="modal-content">\
                        <div class="modal-header">\
                            <h4 class="modal-title">変更保存</h4><button type="button" class="btn-close d-none" data-bs-dismiss="modal" aria-label="Close"></button>\
                        </div>\
                        <div class="modal-body">\
                            <section class="newsletter-subscribe pt-0 p-0">\
                                <div class="container">\
                                    <div class="intro"></div>\
                                    <form class="d-flex justify-content-center flex-wrap mx-auto" style="text-align: center;">\
                                        <h1 class="cap-icon-h1"><i class="fa fa-exclamation-circle pt-1 pb-1 ps-3 pe-3 cap-icon-i"></i></h1>\
                                    </form>\
                                </div>\
                                <h4 class="text-center align-self-center" id="PE315"></h4>\
                            </section>\
                        </div>\
                        <div class="modal-footer">\
                            <button type="button" class="btn btn-lg btn-primary"  id="btn_t_common_save_change">\
                                保存\
                            </button><button type="button" class="btn btn-lg btn-secondary" data-bs-dismiss="modal">閉じる</button>\
                        </div>\
                    </div >\
                </div >'
    $('#' + id).append(save_chg);

}

function Change_and_Update_Data_Not_Exist(id) {
    let no_data = '<div class="modal-dialog modal-lg modal-dialog-centered" role="document">\
        <div class="modal-content">\
                        <div class="modal-header">\
                            <h4 class="modal-title d-none">変更保存</h4><button type="button" class="btn-close d-none" data-bs-dismiss="modal" aria-label="Close"></button>\
                        </div>\
                        <div class="modal-body">\
                            <section class="newsletter-subscribe pt-0 p-0">\
                                <div class="container">\
                                    <div class="intro"></div>\
                                    <form class="d-flex justify-content-center flex-wrap mx-auto"  style="text-align: center;">\
                                        <h1 class="cap-icon-h1"><i class="fa fa-check cap-icon-i"></i></h1>\
                                    </form>\
                                </div>\
                                <h4 class="text-center align-self-center" id="PE318"></h4>\
                            </section>\
                        </div>\
                        <div class="modal-footer">\
                            <button type="button" class="btn btn-lg btn-secondary" data-bs-dismiss="modal" id="btn_t_common_change_and_update_no_data">閉じる</button>\
                        </div>\
                    </div >\
                </div >'
    $('#' + id).append(no_data);
}

//登録完了
function Save_Successfully(id) {
    let save_suc = '<div class="modal-dialog modal-lg modal-dialog-centered" role="document">\
        <div class="modal-content">\
                        <div class="modal-header">\
                            <h4 class="modal-title">変更保存</h4><button type="button" class="btn-close d-none" data-bs-dismiss="modal" aria-label="Close"></button>\
                        </div>\
                        <div class="modal-body">\
                            <section class="newsletter-subscribe pt-0 p-0">\
                                <div class="container">\
                                    <div class="intro"></div>\
                                    <form class="d-flex justify-content-center flex-wrap mx-auto" style="text-align: center;">\
                                        <h1 class="cap-icon-h1"><i class="fa fa-check cap-icon-i"></i></h1>\
                                    </form>\
                                </div>\
                                <h4 class="text-center align-self-center" id="PE316"></h4>\
                            </section>\
                        </div>\
                        <div class="modal-footer">\
                            <button type="button" class="btn btn-lg btn-secondary" data-bs-dismiss="modal" id="btn_t_common_save_success_popup">閉じる</button>\
                        </div>\
                    </div >\
                </div >'
    $('#' + id).append(save_suc);
}