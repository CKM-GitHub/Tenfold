const _url = {};
$(function () {
    _url.get_t_seller_Info = common.appPath + '/t_seller_memo/get_t_seller_Info';
    _url.get_t_seller_memo_DisplayData = common.appPath + '/t_seller_memo/get_t_seller_memo_DisplayData';
    _url.Insert_L_Log = common.appPath + '/t_seller_memo/Insert_L_Log';
    _url.Modify_MemoText = common.appPath + '/t_seller_memo/Modify_MemoText';
    _url.Delete_MemoText = common.appPath + '/t_seller_memo/Delete_MemoText';
    setValidation();
    addEvents();
    $('#navbarDropdownMenuLink').addClass('font-bold active text-underline');
    $('#t_seller_memo').addClass('font-bold text-underline');
    $('#subMenu_Seller li').children('a').removeClass("active");
    $('#subMenu_Seller li').children('a').eq(4).addClass('active');
});

function setValidation() {
    $('#MemoText')
        .addvalidation_errorElement("#MemoTextError")
        .addvalidation_reqired()
        .addvalidation_singlebyte_doublebyte();
}

function addEvents() {
    common.bindValidationEvent('#message-com', '');

    let model = {
        SellerCD: common.getUrlParameter('SellerCD')
    }
    get_memo_Data(model, this);

    $('#cap-fixed-button').on('click', function () {
        $('textarea#MemoText').val('');
        $('textarea#MemoText').focus();
        $('#btnEdit_MemoText').off("click");
        $('#btnCancel_Edit').off("click");
        $('#btnCancel_Edit').on('click', function () {
            $('#btnEdit_MemoText').off("click");
            $('#message-com').modal('hide');
        });
        $('#btnEdit_MemoText').on('click', function () {
            $form = $('#message-com').hideChildErrors();
            if (!common.checkValidityOnSave('#message-com')) {
                $form.getInvalidItems().get(0).focus();
                return false;
            }
            let model = {
                SellerCD: common.getUrlParameter('SellerCD'),
                ParentChildKBN: 1,
                ParentSEQ: 0,
                MemoText: $('textarea#MemoText').val(),
                Type: 'Add'
            }
            Modify_Memo(model);
        });
        $form = $('#message-com').hideChildErrors();
        $('#message-com').modal('show');
    })

    $('#btnDelete_OK').on('click', function () {
        Modify_Memo(model);
    })
}

function get_memo_Data(model, $form) {
    common.callAjaxWithLoadingSync(_url.get_t_seller_memo_DisplayData, model, this, function (result) {
        if (result && result.isOK) {
            Bind_memo_Data(result.data);
        }
        else {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                $form.getInvalidItems().get(0).focus();
            }
        }
    })
}

function Bind_memo_Data(result) {
    $('#display_area').empty();
    var data = JSON.parse(result);
    var p_html = '', c_html = '';
    var loginID = $('#loginID').text();
    for (var i = 0; i < data.length; i++) {
        if (data[i]['ParentChildKBN'] == '1') {
            p_html =
                '<div id="div_' + data[i]['SellerMemoSEQ'] + '" class="p-md-5 container">\
                <div class="row pb-3 col-md-12">\
                <div class="d-md-flex">\
                <strong>' + data[i]['TenStaffName'] + '</strong>\
                <p class="ps-md-5 text-secondary text-start">' + data[i]['UpdateDateTime'] + '</p>\
                </div>\
                <div class="clearfix"></div>\
                <p id="mt_' + data[i]['SellerMemoSEQ'] + '" ondblclick="Edit_Memo(\'' + data[i]['SellerMemoSEQ'] + '\',\'' + data[i]['InsertOperator'] + '\')">' + data[i]['MemoText'] + '</p>\
                <p>\
                <button type="button" class="float-right btn btn-outline-primary ml-2" onclick="Reply_Memo(\'' + data[i]['SellerMemoSEQ'] + '\')">\
                    <i class="fa fa-reply"></i> コメント </button>\
                <button type="button" id="btnDelete" class="float-right btn text-white btn-danger delete" onclick="Delete_Memo(\'' + data[i]['SellerMemoSEQ'] + '\')">\
                    <i class="fa fa-times"></i> 削除 </button>\
                </p>\
                </div>\
                </div>';

            $('#display_area').append(p_html);
        }
        else if (data[i]['ParentChildKBN'] == '2') {
            c_html =
                '<div class="card-body">\
                <div class="row col-md-12">\
                <div class="d-md-flex">\
                <strong>' + data[i]['TenStaffName'] + '</strong>\
                <p class="ps-md-5 text-secondary text-start">' + data[i]['UpdateDateTime'] + '</p>\
                </div>\
                <p id="mt_' + data[i]['SellerMemoSEQ'] + '" ondblclick="Edit_Memo(\'' + data[i]['SellerMemoSEQ'] + '\',\'' + data[i]['InsertOperator'] + '\')">' + data[i]['MemoText'] + '</p>\
                <p>\
                <button type="button" id="btnDelete" class="float-right btn text-white btn-danger delete" onclick="Delete_Memo(\'' + data[i]['SellerMemoSEQ'] + '\')">\
                    <i class="fa fa-times"></i> 削除 </button>\
                </p>\
                </div>\
                </div>'

            if ($('#c_' + data[i]['ParentSEQ']).length < 1)
                $('#div_' + data[i]['ParentSEQ']).append('<div id="c_' + data[i]['ParentSEQ'] + '" class="card card-inner bg-gray-50 ms-md-24"></div>');  /*for the first child of the parent*/
            else
                $('#c_' + data[i]['ParentSEQ']).append('<hr class="navbar-divider my-5 opacity-20">');  /*to separate child div with space*/

            $('#c_' + data[i]['ParentSEQ']).append(c_html);
        }
    }
    if (loginID !== 'admin') {
        $('.delete').each(function () {
            $(this).addClass('d-none');
        })
    }
}

function Edit_Memo(SellerMemoSEQ, InsertOperator) {
    if (InsertOperator == $('#loginID').text()) {
        $('textarea#MemoText').val($('#mt_' + SellerMemoSEQ).text());
        $('textarea#MemoText').focus();
        $('#btnEdit_MemoText').off("click");
        $('#btnCancel_Edit').off("click");
        $('#btnCancel_Edit').on('click', function () {
            $('#btnEdit_MemoText').off("click");
            $('#message-com').modal('hide');
        });
        $('#btnEdit_MemoText').on('click', function () {
            $form = $('#message-com').hideChildErrors();
            if (!common.checkValidityOnSave('#message-com')) {
                $form.getInvalidItems().get(0).focus();
                return false;
            }
            let model = {
                SellerCD: common.getUrlParameter('SellerCD'),
                MemoText: $('textarea#MemoText').val(),
                SellerMemoSEQ: SellerMemoSEQ,
                Type: 'Update'
            }
            Modify_Memo(model);
        });
        $form = $('#message-com').hideChildErrors();
        $('#message-com').modal('show');
    }
}

function Modify_Memo(model) {
    $('#message-com').modal('hide');
    common.callAjaxWithLoadingSync(_url.Modify_MemoText, model, this, function (result) {
        if (result && result.isOK) {
            get_memo_Data(model, this);
        }
        else {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                this.getInvalidItems().get(0).focus();
            }
        }
    })
}

function Reply_Memo(SellerMemoSEQ) {
    $('textarea#MemoText').val('');
    $('textarea#MemoText').focus();
    $('#btnEdit_MemoText').off("click");
    $('#btnCancel_Edit').off("click");
    $('#btnCancel_Edit').on('click', function () {
        $('#btnEdit_MemoText').off("click");
        $('#message-com').modal('hide');
    });
    $('#btnEdit_MemoText').on('click', function () {
        $form = $('#message-com').hideChildErrors();
        if (!common.checkValidityOnSave('#message-com')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        let model = {
            SellerCD: common.getUrlParameter('SellerCD'),
            ParentChildKBN: '2',
            ParentSEQ: SellerMemoSEQ,
            MemoText: $('textarea#MemoText').val(),
            Type: 'Add'
        }
        Modify_Memo(model);
    });
    $form = $('#message-com').hideChildErrors();
    $('#message-com').modal('show');
}

function Delete_Memo(SellerMemoSEQ) {
    let model = {
        SellerMemoSEQ: SellerMemoSEQ,
    }
    $('#btnCancel_Delete').off("click");
    $('#btnCancel_Delete').on('click', function () {
        $('#btnDelete_MemoText').off("click");
    });
    $('#btnDelete_MemoText').on('click', function () {
        Delete(model);
    });
    $('#message-del').modal('show');
}

function Delete(model) {
    $('#message-del').modal('hide');
    common.callAjaxWithLoadingSync(_url.Delete_MemoText, model, this, function (result) {
        if (result && result.isOK) {
            $('#btnDelete_MemoText').removeAttr('onclick');
            $('#message-delok').modal('show');
        }
        else {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                this.getInvalidItems().get(0).focus();
            }
        }
    })
}