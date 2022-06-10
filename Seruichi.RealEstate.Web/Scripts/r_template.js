const _url = {};
$(function () {
    _url.DisplayData = common.appPath + '/r_template/Get_templateData';
    _url.DeleteData = common.appPath + '/r_auto_mes/DeleteData';
    _url.InsertData = common.appPath + '/r_auto_mes/InsertUpdateData'; 
    addEvents();
}); 

function BindData(result,id) {
    let data = JSON.parse(result);
    let html = "";
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {
            var page = '';
            var label = data[i]['作成ページ区分'];//'エリア', '路線', 'マンション';
            if (label == 'エリア')
                page += '<label class="form-check-label" for= "defaultCheck1" > <span class="ps-1 pe-1 rounded bg-success text-white">エリア</span>';
            if (label == '路線')
                page += '<label class="form-check-label" for="defaultCheck1"> <span class="ps-1 pe-1 rounded bg-warning text-white">路線</span> </label>';
            if (label == 'マンション')
                page += '<label class="form-check-label" for="defaultCheck1"> <span class="ps-1 pe-1 rounded bg-primary text-white">マンション</span> </label>';
            if (label == '')
                page += '<label class="form-check-label" for="defaultCheck1" style="background-color: white !important;" > <span style="background-color: white !important;" class="ps-1 pe-1 rounded bg-primary text-white">0</span> </label>';
            html +=
                '<div class="col">\
                <div class="row justify-content-center">\
                    <div class="col-md-12">\
                        <div class="card shadow-0 border rounded-3 h-100">\
                            <div class="card-body">\
                                <div class="row">\
                                    <div class="col-md-12  col-lg-9 col-xl-9">\
                                                '+ page + '\
                                          <h5 class="pb-2">'+ data[i]['TemplateName'] + '</h5>\
                                        <p class="text-dark mb-2 mb-md-0 small min-h-full">\
                                            最終更新日：'+ data[i]['LstEdtDt'] + '\
                                                                    </p>\
                                    </div>\
                                    <div class="col-md-12 col-lg-3 col-xl-3 border-start">\
                                        <div class="d-flex flex-column">\
                                            <button class="btn btn-danger mt-1" type="button" data-bs-toggle="modal" data-bs-target="#message-del">\
                                                削除\
                                                                        </button>\
                                        </div>\
                                    </div>\
                                </div>\
                            </div>\
                        </div>\
                    </div>\
                </div>\
            </div>';
        }
    }
    if (id == 'master-template') {
        $('.master-template').html('');
        $('.master-template').append(html);
    }
    else if (id == 'rate-template') {
        $('.rate-template').html('');
        $('.rate-template').append(html); 
    } else if (id == 'rent-template') {
        $('.rent-template').html('');
        $('.rent-template').append(html);
    } else if (id == 'option-template') {
        $('.option-template').html('');
        $('.option-template').append(html);
    }
}
function addEvents() {  
    let model = { 
    };

    common.callAjaxWithLoading(_url.DisplayData, model, this, function (result) {
        if (result && result.isOK) { 
            BindData(result.data.split('Ʈ')[0],'master-template');
            BindData(result.data.split('Ʈ')[1],'rate-template');
            BindData(result.data.split('Ʈ')[2],'rent-template');
            BindData(result.data.split('Ʈ')[3], 'option-template');
            //$('#message-com').modal('hide');
            //window.location.href = common.appPath + '/r_auto_mes/Index';
        }

    });
     
    $('#btnNew').on('click', function () {
        $mode = "1";
    });
     
    $('#btnConfirm').on('click', function () {

        let model = {
            MessageSEQ: $HiddenSEQ,
            ProcessKBN: "Delete",
            Remarks: "MessageSEQ：" + $HiddenSEQ
        };

        common.callAjax(_url.DeleteData, model,
            function (result) {
                if (result && !result.isOK) {
                    $('#message-delok').modal('hide');
                }
            });

    });

    $('#btnCloseUp').on('click', function () {
        window.location.href = common.appPath + '/r_auto_mes/Index';
    });

    $('#btnSend').on('click', function () {
        const $this = $(this), $HiddenSEQ = $("#MsgSEQ").val(), $TemplateName = $("#TemplateName").val(),
            $TemplateContent = $("#TemplateContent").val(), $chk = $("#ChkFlg").val()

        let model = {
            MessageSEQ: $HiddenSEQ,
            MessageTitle: $TemplateName,
            MessageTEXT: $TemplateContent,
            ValidFlg: $chk,
            Mode: $mode,
            Remarks: "MessageSEQ：" + $HiddenSEQ
        };

        common.callAjaxWithLoading(_url.InsertUpdateData, model, this, function (result) {
            if (result && result.isOK) {
                $('#message-com').modal('hide');
                window.location.href = common.appPath + '/r_auto_mes/Index';
            }

        });

    });
}