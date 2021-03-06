const _url = {};
$(function () {
    _url.get_displaydata = common.appPath + '/a_mypage_reglist/get_displaydata';
    _url.InsertL_Log = common.appPath + '/a_mypage_reglist/InsertL_Log';
    _url.InsertD_Assessment = common.appPath + '/a_mypage_reglist/InsertD_Assessment';
    _url.get_t_sellerPossibleTime = common.appPath + '/a_mypage_reglist/get_t_sellerPossibleTime';
    addEvents();
});

function addEvents() {
    let model = {
        SellerID: null
    }
    getSellerList(model, this);

}


function getSellerList(model, $form) {
    common.callAjaxWithLoadingSync(_url.get_displaydata, model, this, function (result) {
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
            var param = "'" + data[i]["AssReqID"] + "','" + data[i]["MansionName"] + "'";
            if (isEmptyOrSpaces(data[i]["ステータス"])) {
                _letter = "";
                _class = "ms-1 ps-1 pe-1 rounded-circle";
                _sort_checkbox = "";
                _enable = "";
            }
            else {
                _letter = data[i]["ステータス"].charAt(0);
                if (_letter == "未") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-primary text-white";
                    _sort_checkbox = "1";
                    _enable = "";
                    
                }
                else if (_letter == "簡") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-info text-white";
                    _sort_checkbox = "2";
                    _enable = "";
                }
                else if (_letter == "詳" ) {
                    _letter = "詳";
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-warning text-white";
                    _sort_checkbox = "3";
                    _enable ="disabled"
                }
                else if (_letter == "送" && data[i]["ステータス"] == "送客依頼") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-success text-white";
                    _sort_checkbox = "4";
                    _enable = "disabled"
                }
                else if (_letter == "交") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-info txt-dark";
                    _sort_checkbox = "5";
                    _enable = "disabled"
                }
                else if (_letter == "成") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-secondary";
                    _sort_checkbox = "6";
                    _enable = "";
                }
                else if (_letter == "売") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-light text-danger";
                    _sort_checkbox = "7";
                    _enable = "disabled"
                } 
                else if (_letter == "買" && data[i]["ステータス"] == "買主辞退") {
                    _letter = "買";
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-dark text-white";
                    _sort_checkbox = "8";
                    _enable = "";
                }
            }
            html +=
            '<tr>\
             <td class="text-truncate text-center" >'+ data[i]["No"] + ' </td >\
             <td class="text-truncate'+ _sort_checkbox + '"><i class="' + _class + '">' + _letter + '</i><span class="font-semibold">' + data[i]["ステータス"] + '</span></td>\
             <td class="text-center">\
            <button type="button"   class="btn btn-primary" data-bs-target="#modal-sateitype"  data-bs-toggle="modal" '+ _enable +'> 査定</button> </a></td>\
             <td class="text-start"><a class="text-heading font-semibold text-decoration-underline" href="'+ common.appPath + '/a_mansion_update/Index?sellerMansionID=' + data[i]["SellerMansionID"] + '" id=' + data[i]["SellerMansionID"] + '&a_mansion_update> ' + data[i]["マンション名＆部屋番号"] + '</a></td>\
             <td class="text-start" >' + data[i]["登録日時"] + ' </td >\
             <td class="text-end"> <a onclick="setIdMansion('+ param + ')" href="#" data-bs-toggle="modal"\
                class="text-decoration-underline"> ' + data[i]["登録日時"] + ' </a>\
            </td>\
             <td class="text-end" >' + data[i]["査定額"] + ' </td >\
             <td class="text-start" >' + data[i]["不動産会社"] + ' </td >\
             <td class="d-none"> '+ data[i]["SellerMansionID"] + '</td>\
             <td class="d-none">'+ _sort_checkbox + '</td>\
            </tr>'
        }
    }
    
    $('#sellermansiontb tbody').append(html);
}
function setIdMansion(id, mansion) {
    $('#exampleModalLabel').text(mansion);
    $('#assessreqid').text(id);
    if (_sort_checkbox == 2) {
        $('#modal-1').modal('show');
    }
    else {
        if (_sort_checkbox == 3) {
            $('#btn_chat').attr('disabled', true);
        }
        else {
            $('#btn').removeAttr('disabled');
        }
        $('#exampleModal').modal('show');
        
    }
}
 
$('#btn_assess').on('click', function () {
    l_logfunction('a_assess_d');
   
});

$('#btn_chat').on('click', function () {
    l_logfunction('a_chat');
});

$('#btn_assessment').on('click', function () {
    let model = {
        
        AssKBN:1
    }
    common.callAjax(_url.InsertD_Assessment, model,
        function (result) {
            if (result && result.isOK) {
                $('#modal-1').modal('show');
            }
        });
});

$('#btn_detailassessment').on('click', function () {
    let model = {
        SellerCD: null
    }
    common.callAjax(_url.get_t_sellerPossibleTime, model,
        function (result) {
            if (result && result.isOK) {
                Bind_zancheckmodal(result.data);
            }
            if (result && !result.isOK) {
                $('#Errorpossible').text("nos");
            }
        });
    
});

$('#btn_plus').on('click', function () {
    window.location.href = common.appPath + '/a_mypage_plus/Index';
});

$('#btn_next').on('click', function () {
    let model = {
        SellerCD: null
    }
    common.callAjax(_url.get_t_sellerPossibleTime, model,
        function (result) {
            if (result && result.isOK) {
                let data = JSON.parse(result.data);
                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        $('#handyphone').val(data[i]["HandyPhone"]);
                        $('#modal-phonenumcheck').modal('show');
                    }
                }
            }
            if (result && !result.isOK) {

            }
        });

});

$('#checkmobile').on('click', function () {
    $('#modal-4').modal('show');
    
});

$('#result').on('click', function () {
    $('#timeover').modal('show');
    let model = {
        AssKBN: 2
    }
    common.callAjax(_url.InsertD_Assessment, model,
        function (result) {
            if (result && result.isOK) {
                window.location.href = common.appPath + '/a_assess_d/Index';
            }
        });
});

function l_logfunction(link) {
    let model = {
        LogDateTime: null,
        LoginKBN: null,
        LoginID: null,
        RealECD: null,
        LoginName: null,
        IPAddress: null,
        Page: null,
        Processing: null,
        Remarks: null,
        SellerCD: null,
        Link: link
    }
    common.callAjax(_url.InsertL_Log, model,
        function (result) {
            if (result && result.isOK) {
                window.location.href = window.location.href.replace('a_mypage_reglist', '') + link + "?AssReqID=" + $('#assessreqid').text();
            }
        });
}


function Bind_zancheckmodal(result) {
    let data = JSON.parse(result);
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {

            $('#possibledata').text(data[i]["PossibleTimes"]);
            $('#zancheck').modal('show');
        }
    }
}



