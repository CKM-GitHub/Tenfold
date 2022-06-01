//Start script code of t_reale_Info.cshtml -> Row No. 2 ~ 52
function Bind_Company_Data(model, $form) {
    common.callAjaxWithLoading(_url.get_t_reale_purchase_CompanyInfo, model, this, function (result) {
        if (result && result.isOK) {
            Bind_CompanyInfo(model, result.data);
            common.callAjaxWithLoading(_url.get_t_reale_purchase_CompanyCountingInfo, model, this, function (result) {
                if (result && result.isOK) {
                    Bind_CountingInfo(result.data);
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

function Bind_CompanyInfo(model, result) {
    var data = JSON.parse(result);
    if (data.length > 0) {
        if (data[0]['InvalidFLG'] == '有効会員')
            $('#InvalidFLG').addClass('text-success');
        else
            $('#InvalidFLG').addClass('text-danger');

        $('#InvalidFLG').text(data[0]['InvalidFLG']);
        $('#REKana').text(data[0]['REKana']);
        $('#REName').text(data[0]['REName']);
        $('#RealECD').text(data[0]['RealECD']);
        $('#Address').text(data[0]['Address']);
        $('#HousePhone').text(data[0]['HousePhone']);
        $('#Fax').text(data[0]['Fax']);
        $('#MailAddress').text(data[0]['MailAddress']);
        $('#PICName').text(data[0]['PICName']);

        model = {
            REName: data[0]['REName']
        }
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
//End script code of t_reale_Info.cshtml

//Start script code of t_reale_Modal.cshtml -> Row No. 64 ~ 
function Bind_ModalDetails(id) {
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